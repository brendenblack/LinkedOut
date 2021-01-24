using FluentResults;
using LinkedOut.Domain.Common;
using LinkedOut.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedOut.Domain.Entities
{
    public class JobApplication : AuditableEntity
    {
        private JobApplication() { }

        public JobApplication(JobSearch parentSearch, string organization, string title)
        {
            ParentSearchId = parentSearch.Id;
            ParentSearch = parentSearch;
            OrganizationName = organization;
            JobTitle = title;

            var creation = new StatusTransition(this, ApplicationStatuses.CREATED, ApplicationStatuses.INPROGRESS, DateTime.Now, ApplicationResolutions.UNRESOLVED);
            ((List<StatusTransition>)Transitions).Add(creation);
        }

        #region Basic details
        public int Id { get; set; }

        public int ParentSearchId { get; private set; }

        public virtual JobSearch ParentSearch { get; private set; }

        private Location _location;
        public Location Location
        {
            get
            {
                return _location ??  Location.PartsUnknown;
            }
            set
            {
                _location = value;
            }
        }

        public string OrganizationName { get; set; }

        public string JobTitle { get; set; }

        public string Resume { get; set; }

        public Formats ResumeFormat = Formats.PLAINTEXT;

        public string CoverLetter { get; set; }

        /// <summary>
        /// A detailed description of the duties and responsibilities of the position, as well as the requirements for the role.
        /// </summary>
        public string JobDescription { get; set; }

        public Formats JobDescriptionFormat { get; set; } = Formats.PLAINTEXT;

        /// <summary>
        /// Where this job opportunity was discovered.
        /// </summary>
        public string Source { get; set; }
        #endregion

        #region Status & notes
        public ApplicationStatuses CurrentStatus
        {
            get
            {
                var latest = Transitions
                    .OrderByDescending(t => t.Timestamp)
                    .FirstOrDefault();

                return latest?.TransitionTo ?? ApplicationStatuses.UNKNOWN;
            }
        }

        public ApplicationResolutions Resolution
        {
            get
            {
                var latest = Transitions
                    .OrderByDescending(t => t.Timestamp)
                    .FirstOrDefault();

                return latest?.Resolution ?? ApplicationResolutions.UNRESOLVED;
            }
        }

        public bool IsClosed => CurrentStatus == ApplicationStatuses.CLOSED;

        public virtual IReadOnlyCollection<StatusTransition> Transitions { get; } = new List<StatusTransition>();

        public bool CanSubmit => CurrentStatus == ApplicationStatuses.INPROGRESS;

        public Result<StatusTransition> Submit(DateTime effectiveAsOf, string resume, Formats resumeFormat)
        {
            var transitionResult = Submit(effectiveAsOf);
            if (transitionResult.IsFailed)
            {
                return transitionResult;
            }

            Resume = resume;
            ResumeFormat = resumeFormat;

            return Result.Ok(transitionResult.Value);
        }

        public Result<StatusTransition> Submit(DateTime effectiveAsOf)
        {
            if (CurrentStatus != ApplicationStatuses.INPROGRESS)
            {
                return Result.Fail($"An application can only be submitted when it is {ApplicationStatuses.INPROGRESS}, but it is currently {CurrentStatus}"); // TODO
            }

            var creation = new StatusTransition(this, ApplicationStatuses.INPROGRESS, ApplicationStatuses.SUBMITTED, effectiveAsOf, ApplicationResolutions.UNRESOLVED);
            ((List<StatusTransition>)Transitions).Add(creation);

            return Result.Ok(creation);
        }        

        /// <summary>
        /// Transition this application from one status to another.
        /// </summary>
        /// <remarks>
        /// This method is agnostic about workflows, and will only enforce that you cannot transition from one statu to the same status.
        /// Use an appropriate class that derives from <see cref="StatusWrapper"/> (e.g. <see cref="SubmittedApplication"/>) as helpers to more
        /// effective manage the status.
        /// </remarks>
        /// <param name="transitionTo"></param>
        /// <param name="effectiveAsOf"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public Result<StatusTransition> Transition(ApplicationStatuses transitionTo, DateTime effectiveAsOf, ApplicationResolutions resolution = ApplicationResolutions.UNRESOLVED)
        {
            // CAUTION: if the inner logic of this method changes, you may have to revisit the first transition created
            // in the ctor

            if (CurrentStatus == transitionTo)
            {
                return Result.Fail("Unable to transition to the current status.");
            }

            if (transitionTo != ApplicationStatuses.CLOSED)
            {
                resolution = ApplicationResolutions.UNRESOLVED;
            }

            var transition = new StatusTransition(this, CurrentStatus, transitionTo, effectiveAsOf, resolution);
            ((List<StatusTransition>)Transitions).Add(transition);
            return Result.Ok(transition);
        }



        /// <summary>
        /// Adds an interview record to this job application.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="interviewMedium">What medium the interview took place over. See <see cref="InterviewTypes"/> for standard mediums.</param>
        /// <param name="interviewers"></param>
        public Result<Note> ScheduleInterview(DateTime start, string interviewMedium, IEnumerable<string> interviewers, TimeSpan duration, string description = "")
        {
            var interview = new Note(this)
            {
                Timestamp = start,
                Author = OrganizationName,
                Contents = $"Interview<br/>Start time: ${start}<br />Duration: ${duration}<br />Medium: ${interviewMedium}<br />Interviewers: ${string.Join(',', interviewers)}",
            };

            if (!string.IsNullOrWhiteSpace(description))
            {
                interview.Contents += $"<br />{description}";
            }

            ((List<Note>)Notes).Add(interview);

            return Result.Ok(interview);
        }
        #endregion

        public bool HasOffer => Offer != null;
        public virtual Offer Offer { get; private set; }

        public Result<Offer> AddOffer(DateTime extended, string details)
        {
            Offer = new Offer(this)
            {
                Extended = extended,
                Expires = extended.AddDays(30),
                Details = details,
            };

            return Result.Ok(Offer);
        }

        public Result RemoveNote(int id)
        {
            var applicationEvent = Notes.FirstOrDefault(e => e.Id == id);
            if (applicationEvent != null)
            {
                return RemoveNote(applicationEvent);
            }

            return Result.Fail("");
        }

        public Result RemoveNote(Note applicationEvent)
        {
            var removed = ((List<Note>)Notes).Remove(applicationEvent);
            return Result.FailIf(!removed, "");
        }

        /// <summary>
        /// Records a note about this application.
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        public Result<Note> AddNote(string contents)
        {
            var note = new Note(this)
            {
                Author = Note.SelfAuthoredAuthor,
                Timestamp = DateTime.Now,
                Contents = contents
            };

            ((List<Note>)Notes).Add(note);

            return Result.Ok(note);
        }

        /// <summary>
        /// Creates a record of contact from the potential employer.
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="author"></param>
        /// <param name="received">When this contact took place.</param>
        /// <returns></returns>
        public Result<Note> RecordEmployerContact(string contents, string author = "", string subject = "", DateTime? received = null)
        {
            var note = new Note(this)
            {
                Author = (string.IsNullOrWhiteSpace(author)) ? OrganizationName : author,
                Timestamp = received ?? DateTime.Now,
                Contents = contents
            };

            ((List<Note>)Notes).Add(note);

            return Result.Ok(note);
        }

        public IReadOnlyCollection<Note> Notes { get; private set; } = new List<Note>();
    }
}

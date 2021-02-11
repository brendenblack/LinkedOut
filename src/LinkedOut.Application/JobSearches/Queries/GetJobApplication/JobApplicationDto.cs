using AutoMapper;
using LinkedOut.Application.Common.Mappings;
using LinkedOut.Domain;
using LinkedOut.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkedOut.Application.JobSearches.Queries.GetJobApplication
{
    public class JobApplicationDto : IMapFrom<JobApplication>
    {
        public int Id { get; set; }

        public int OpportunityId { get; set; }

        public int ParentSearchId { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }

        public ApplicationStatuses CurrentStatus { get; set; }

        public ApplicationResolutions Resolution { get; set; }

        public string Resume { get; set; }

        public Formats ResumeFormat { get; set; }

        public string CoverLetter { get; set; }

        public Formats CoverLetterFormat => Formats.HTML;

        public bool IsClosed { get; set; }

        public List<NoteDto> Notes { get; set; }

        public List<TransitionDto> Transitions { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<JobApplication, JobApplicationDto>()
                .ForMember(
                    d => d.SubmittedOn,
                    o => o.MapFrom(
                        s => s.Transitions
                            .Where(t => t.TransitionTo == ApplicationStatuses.SUBMITTED)
                            .Select(t => t.Timestamp)
                            .FirstOrDefault()))
                .ForMember(d => d.OpportunityId, o => o.MapFrom(s => s.Id));
        }
    }
}
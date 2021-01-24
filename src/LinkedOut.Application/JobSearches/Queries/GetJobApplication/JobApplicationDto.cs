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

        public int ParentSearchId { get; set; }

        public string ParentSearchTitle { get; set; }

        public string OrganizationName { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }

        public ApplicationStatuses CurrentStatus { get; set; }

        public ApplicationResolutions Resolution { get; set; }

        public string LocationCityName { get; set; }

        public string LocationProvince { get; set; }

        public string Resume { get; set; }

        public string CoverLetter { get; set; }

        public string JobTitle { get; set; }

        public bool IsClosed { get; set; }

        public string JobDescription { get; set; }

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
                            .FirstOrDefault()));
        }
    }
}
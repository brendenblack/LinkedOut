using AutoMapper;
using LinkedOut.Application.Common.Mappings;
using LinkedOut.Domain.Entities;
using LinkedOut.Domain.ValueObjects;
using System;
using System.Linq;

namespace LinkedOut.Application.JobSearches.Queries.GetJobSearch
{
    public class JobSearchOpportunityDto : IMapFrom<JobApplication>
    {
        public int Id { get; set; }

        public string OrganizationName { get; set; }

        public string JobTitle { get; set; }

        public DateTime Created { get; set; }

        public DateTime? AppliedOn { get; set; }

        public bool DidApply => AppliedOn.HasValue && AppliedOn.Value != DateTime.MinValue;

        public ApplicationStatuses CurrentStatus { get; set; }

        public ApplicationResolutions Resolution { get; set; }

        public Location Location { get; set; }
    
        public bool HasContact { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<JobApplication, JobSearchOpportunityDto>()
                .ForMember(d => d.DidApply, o => o.Ignore())
                .ForMember(
                    d => d.AppliedOn,
                    o => o.MapFrom(
                        s => s.Transitions
                            .Where(tx => tx.TransitionTo == ApplicationStatuses.SUBMITTED)
                            .Select(tx => tx.Timestamp)
                            .FirstOrDefault()))
                .ForMember(d => d.HasContact, o => o.MapFrom(s => s.Notes.Any(n => !n.IsSelfAuthored)));
        }

    }
}

using AutoMapper;
using LinkedOut.Application.Common.Mappings;
using LinkedOut.Domain;
using LinkedOut.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkedOut.Application.JobSearches.Queries.GetJobOpportunity
{
    public class JobOpportunityDto : IMapFrom<JobApplication>
    {
        public int Id { get; set; }

        public int ParentSearchId { get; set; }

        public string ParentSearchTitle { get; set; }

        public string OrganizationName { get; set; }

        public string JobTitle { get; set; }

        public string JobDescription { get; set; }

        public Formats JobDescriptionFormat { get; set; } = Formats.PLAINTEXT;

        public DateTime Created { get; set; }

        public List<TransitionDto> Transitions { get; set; } = new();

        public DateTime? AppliedOn =>
            Transitions.Where(t => t.TransitionTo == ApplicationStatuses.SUBMITTED)
                .Select(t => t.Timestamp)
                .FirstOrDefault();

        public bool DidApply =>
            AppliedOn.HasValue && AppliedOn.Value != DateTime.MinValue;

        public ApplicationStatuses CurrentStatus { get; set; }

        public ApplicationResolutions Resolution { get; set; }

        public string Location { get; set; }

        //public bool HasContact => 
        //        Notes.Any(n => !n.IsSelfAuthored);

        public List<NoteDto> Notes { get; set; } = new();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<JobApplication, JobOpportunityDto>()
                .ForMember(d => d.Location, o => o.MapFrom(s => s.Location == null ? Domain.ValueObjects.Location.PartsUnknown.ToString() : s.Location.ToString()));
                //.ForMember(d => d.DidApply, o => o.Ignore())
                //.ForMember(d => d.AppliedOn, o => o.Ignore())
                //o => o.MapFrom(
                //    s => s.Transitions
                //        .Where(tx => tx.TransitionTo == ApplicationStatuses.SUBMITTED)
                //        .Select(tx => tx.Timestamp)
                //        .FirstOrDefault()))
                //.ForMember(d => d.Location, o => o.MapFrom(s => s.Location.ToString() ?? ""))
                //.ForMember(d => d.HasContact, o => o.Ignore());
        }
    }
}
using AutoMapper;
using LinkedOut.Application.Common.Mappings;
using LinkedOut.Domain;
using LinkedOut.Domain.Entities;
using LinkedOut.Domain.ValueObjects;
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

        public Formats JobDescriptionFormat { get; set; } = Formats.HTML;

        public DateTime Created { get; set; }

        public Location Location { get; set; }

        public bool DidApply { get; set; }

        //public void Mapping(Profile profile)
        //{
        //    // TODO: having to map this here is cumbersome, it should really be pushed in to the domain somehow
        //    profile.CreateMap<JobApplication, JobOpportunityDto>()
        //        //.ForMember(d => d.Location, o => o.MapFrom(s => s.Location == null ? Domain.ValueObjects.Location.PartsUnknown.ToString() : s.Location.ToString()));
        //    //.ForMember(d => d.AppliedOn, o => o.Ignore())
        //    //o => o.MapFrom(
        //    //    s => s.Transitions
        //    //        .Where(tx => tx.TransitionTo == ApplicationStatuses.SUBMITTED)
        //    //        .Select(tx => tx.Timestamp)
        //    //        .FirstOrDefault()))
        //    //.ForMember(d => d.Location, o => o.MapFrom(s => s.Location.ToString() ?? ""))
        //    //.ForMember(d => d.HasContact, o => o.Ignore());
        //}
    }
}
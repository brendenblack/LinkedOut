using AutoMapper;
using LinkedOut.Application.Common.Mappings;
using LinkedOut.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Queries.GetJobOpportunity
{
    public class NoteDto : IMapFrom<Note>
    {
        public int Id { get; private set; }

        public DateTime Created { get; set; }

        public DateTime LastModified { get; set; }

        public DateTime Timestamp { get; set; }

        public string Subject { get; set; }

        public string Author { get; set; }

        public string Contents { get; set; }

        public bool IsSelfAuthored { get; set; }

        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<Note, NoteDto>()
        //        .ForMember(s => s.IsSelfAuthored, o => o.MapFrom(d => d.Author == Note.SelfAuthoredAuthor));
        //}
    }
}

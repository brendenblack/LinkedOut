using AutoMapper;

namespace LinkedOut.Application.Common.Mappings
{
    internal interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
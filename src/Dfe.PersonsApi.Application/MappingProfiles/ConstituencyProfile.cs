using AutoMapper;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Domain.Constituencies;
using System.Diagnostics.CodeAnalysis;

namespace Dfe.PersonsApi.Application.MappingProfiles
{
    [ExcludeFromCodeCoverage]
    public class ConstituencyProfile : Profile
    {
        public ConstituencyProfile()
        {
            CreateMap<Constituency, MemberOfParliament>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MemberId.Value))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.NameDetails.NameListAs.Split(",", StringSplitOptions.None)[1].Trim()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.NameDetails.NameListAs.Split(",", StringSplitOptions.None)[0].Trim()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.MemberContactDetails.Email))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.NameDetails.NameDisplayAs))
                .ForMember(dest => dest.DisplayNameWithTitle, opt => opt.MapFrom(src => src.NameDetails.NameFullTitle))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => new List<string> { "Member of Parliament" }))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.LastRefresh))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.MemberContactDetails.Phone))
                .ForMember(dest => dest.ConstituencyName, opt => opt.MapFrom(src => src.ConstituencyName))
                .ForMember(dest => dest.ConstituencyPartyName, opt => opt.MapFrom(src => src.PartyName));
        }
    }
}
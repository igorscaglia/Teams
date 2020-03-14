using AutoMapper;
using Teams.API.Dtos;
using Teams.API.Model;
using System.Linq;

namespace Teams.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Aqui nós vamos mapear as nossas entidades do modelo de domínio para as DTOs
            // Nunca devemos expor nosso modelo de domínio!

            // Propriedades com o mesmo nome e tipo são automaticamente mapeadas
            CreateMap<Team, TeamForList>();

            CreateMap<Team, TeamForDetail>()
                .ForMember(dest => dest.members,
                        opt => opt.MapFrom(src => src.Members
                            .Select(x => $"{x.FirstName} {x.LastName}")
                            .Aggregate((a, b) => $"{a}, {b}")));

            CreateMap<TeamForNew, Team>();
            CreateMap<TeamForList, TeamForUpdate>();

            CreateMap<Member, MemberForList>()
                .ForMember(dest => dest.fullname,
                        opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<Member, MemberForDetail>();
        }
    }
}

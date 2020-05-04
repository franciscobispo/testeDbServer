using AutoMapper;

namespace ContaCorrenteService.AutoMap
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Models.Usuario, Entities.Entities.Usuario>();
            CreateMap<Entities.Entities.ContaCorrenteLancamentos, Models.ContaCorrenteLancamentos>().ForMember(o => o.idContaCorrente, b => b.MapFrom(z => z.ContaCorrente.Id));
        }
    }
}

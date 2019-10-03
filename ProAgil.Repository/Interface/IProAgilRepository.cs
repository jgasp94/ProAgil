using System.Threading.Tasks;
using ProAgil.Domain.Entities;

namespace ProAgil.Repository.Interface
{
    public interface IProAgilRepository
    {
        //Entity Framework Generic
         void Add<T> (T Entity) where T : class;
         void Update<T> (T Entity) where T : class;
         void Delete<T> (T Entity) where T : class;
        Task<bool> SaveChangeAsync();

        //Eventos
        Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes);
        Task<Evento[]> GetAllEventoAsync(bool includePalestrantes);
        Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes);

        Task<Palestrante[]> GetAllPalestranteAsyn(bool includeEventos);
        Task<Palestrante[]> GetAllPalestranteAsyncByName(string name, bool includeEventos);
        Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool includeEventos);
    }
}
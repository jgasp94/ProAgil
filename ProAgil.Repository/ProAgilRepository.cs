using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain.Entities;
using ProAgil.Repository.Data;
using ProAgil.Repository.Interface;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        public readonly ProAgilContext _context;

        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public void Add<T>(T Entity) where T : class
        {
            _context.Add(Entity);
        }

        public void Delete<T>(T Entity) where T : class
        {
            _context.Remove(Entity);
        }

        public void Update<T>(T Entity) where T : class
        {
            _context.Update(Entity);
        }
        public async Task<bool> SaveChangeAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include( c=> c.Lotes)
                .Include (c => c.RedeSociais);

           
            if(includePalestrantes)
            {
                query = query.Include( p => p.PalestranteEventos)
                .ThenInclude(p =>p.Palestrante);
            }
            
            query = query.AsNoTracking()
                        .OrderBy(c => c.Id);

            return await query.ToArrayAsync();

        }

        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include( c=> c.Lotes)
                .Include (c => c.RedeSociais);

            if(includePalestrantes)
            {
                query = query.Include( p => p.PalestranteEventos)
                            .ThenInclude(p =>p.Palestrante);
            }
                
            
            query = query.AsNoTracking()
                .OrderByDescending (c => c.DataEvento)
                .Where(c=>c.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestranteAsyncByName(string name, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(c=>c.Nome)
                .Include(c=>c.RedeSociais);

            if(includeEventos)
            {
                query = query.Include(c=>c.PalestranteEventos)
                            .ThenInclude( e=>e.Evento);
            } 

            query = query.Where( e=> e.Nome.ToLower().Contains(name.ToLower()));
            
            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                                        .Include( c=> c.Lotes)
                                        .Include( c=> c.RedeSociais);
            if(includePalestrantes)
            {
                query = query.Include(c=>c.PalestranteEventos)
                            .ThenInclude(p=>p.Palestrante);
            }

            query = query.OrderByDescending(e=>e.DataEvento)
                        .Where(e=>e.Id == EventoId);
            
            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                                                .Include(c => c.Nome)
                                                .Include(c => c.RedeSociais);

            if(includeEventos)
            {
                query = query.Include(c => c.PalestranteEventos)
                            .ThenInclude(e => e.Evento);
            }

            query = query.OrderBy(p => p.Nome)
                        .Where(p => p.Id == PalestranteId);
            
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante[]> GetAllPalestranteAsyn(bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                                            .Include(c=>c.Nome)
                                            .Include(c=>c.RedeSociais);
            
            if(includeEventos)
                query = query.Include(c=>c.PalestranteEventos)
                            .ThenInclude(e=>e.Evento);
            
            return await query.ToArrayAsync();
        }   
    }
}
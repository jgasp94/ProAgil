using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain.Entities;
using ProAgil.Repository.Interface;

namespace ProAgil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalestranteController : ControllerBase
    {
        private readonly IProAgilRepository repository;

        public PalestranteController(IProAgilRepository Repository)
        {
            this.repository = Repository;
        }
        
        #region GETS

        [HttpGet]
        public async Task<IActionResult> Get(bool includeEventos = true)
        {
            try
            {
                return Ok(await repository.GetAllPalestranteAsyn(includeEventos));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status502BadGateway, "Erro no banco de dados");
            }
        }

        [HttpGet("getByName/{NomePalestrante}/{includeEventos}")]
        public async Task<IActionResult> GetByName(string NomePalestrante, bool includeEventos)
        {
            try
            {
                var response = await repository.GetAllPalestranteAsyncByName(NomePalestrante, includeEventos);
                return Ok(Response);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status502BadGateway, "Erro no Banco de dados");
            }
        }
        
        [HttpGet("getById/{idPalestrante}/{includeEventos}")]
        public async Task<IActionResult> GetById(int idPalestrante, bool includeEventos)
        {
            try
            {
                return Ok(await repository.GetPalestranteAsyncById(idPalestrante, includeEventos));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status502BadGateway, "Erro no banco de dados");
            }
        }
        #endregion
        
        [HttpPost]
        public async Task<IActionResult> Post(Palestrante palestrante)
        {
            try
            {
                repository.Add(palestrante);

                if(await repository.SaveChangeAsync())
                    return Created($"api/Palestrante/getById/{palestrante.Id}/true", palestrante);

            }
            catch(System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no banco de dados");
            }
            return BadRequest();
        }

        [HttpPut("{PalestranteId}")]
        public async Task<IActionResult> Put(int PalestranteId, Palestrante palestrante)
        {
            try
            {
                if(repository.GetPalestranteAsyncById(PalestranteId, false).Equals(null))
                    return NotFound("O Palestrante não existe");

                repository.Update(palestrante);

                if(await repository.SaveChangeAsync())
                    return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no banco de dados");
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Palestrante palestrante)
        {
            try
            {
                if(repository.GetEventoAsyncById(palestrante.Id,false).Equals(null))
                    return NotFound("Palestrante não existe!");

                repository.Delete(palestrante);

                if(await repository.SaveChangeAsync())
                    return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problemas no banco de Dados");
            }
            return BadRequest();
        }
    }
}
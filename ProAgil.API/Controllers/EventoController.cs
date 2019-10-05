using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain.Entities;
using ProAgil.Repository.Interface;

namespace ProAgil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repository;

        public EventoController(IProAgilRepository repository)
        {
            _repository = repository;
        }
#region Gets

        [HttpGet]
        public async Task <IActionResult> Get()
        {
            try
            {
                var response = await _repository.GetAllEventoAsync(true);
                return Ok(response);                
            }
            catch (System.Exception)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Problemas no banco de dados");
            }
        }

        [HttpGet("{Eventoid}")]
        public async Task<IActionResult> Get(int Eventoid)
        {
            try
            {
                var response = await _repository.GetEventoAsyncById(Eventoid, true);
                return Ok(response);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problemas no banco de dados");
            }
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var response = await _repository.GetAllEventoAsyncByTema(tema, true);
                return Ok(response);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problemas no banco de dados");
            }
        }
        #endregion
//Insert
        [HttpPost]
        public async Task<ActionResult> Post(Evento evento)
        {
            try
            {
                _repository.Add(evento);

                if(await _repository.SaveChangeAsync())                
                    return Created($"api/evento/{evento.Id}", evento);
                
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Hove um erro na requisição");
            }

            return BadRequest();
        }

    //Update
        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId, Evento evento)
        {
            try
            {
                var response = await _repository.GetEventoAsyncById(EventoId, false);
                
                if(response.Equals(null)) return StatusCode(StatusCodes.Status404NotFound, "O evento não existe");

                _repository.Update(evento);
                
                if(await _repository.SaveChangeAsync())
                    return Created($"/api/evento/{evento.Id}", evento);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Problemas no banco de dados");
            }
            return BadRequest();
        }

    //Delete - Bêbado
    [HttpDelete("{EventoId}")]
        public async Task<IActionResult> Delete(int EventoId)
        {
            try
            {
                var response = await _repository.GetEventoAsyncById(EventoId, false);               
                
                if(response.Equals(null))return StatusCode(StatusCodes.Status404NotFound, "Registro não exite");
                
                _repository.Delete(response);       

                if(await _repository.SaveChangeAsync())return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Houve um problema na base de dados");
            }
            return BadRequest();
        }
    }
}
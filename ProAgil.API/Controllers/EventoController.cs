using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.API.Dtos;
using ProAgil.Domain.Entities;
using ProAgil.Repository.Interface;

namespace ProAgil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repository;
        public readonly IMapper _mapper;

        public EventoController(IProAgilRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }
        #region Gets

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _repository.GetAllEventoAsync(true);
                var response = _mapper.Map<IEnumerable<EventoDTO>>(eventos);
                
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
                var evento = await _repository.GetEventoAsyncById(Eventoid, true);
                var response = _mapper.Map<EventoDTO>(evento);
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
                var eventos = await _repository.GetAllEventoAsyncByTema(tema, true);
                var response = _mapper.Map<IEnumerable<EventoDTO>>(eventos);

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
        public async Task<ActionResult> Post(EventoDTO evento)
        {
            try
            {
                var eventoDto = _mapper.Map<Evento>(evento);
                _repository.Add(eventoDto);

                if (await _repository.SaveChangeAsync())
                    return Created($"api/evento/{evento.Id}", _mapper.Map<EventoDTO>(eventoDto));

            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Hove um erro na requisição: {ex.Message}");
            }

            return BadRequest();
        }
        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if(file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, fileName.Replace("\""," ").Trim());

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);

                    }
                }                
                return Ok();
            }
            catch (System.Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Problemas no banco de dados");
            }
            return BadRequest("Erro ao realizar o Upload");
        }
        //Update
        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId, Evento evento)
        {
            try
            {
                var response = await _repository.GetEventoAsyncById(EventoId, false);

                if (response.Equals(null)) return NotFound();

                
                _mapper.Map(response, evento);
                _repository.Update(evento);

                if (await _repository.SaveChangeAsync())
                    return Created($"/api/evento/{evento.Id}", _mapper.Map<EventoDTO>(evento));
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

                if (response.Equals(null)) return StatusCode(StatusCodes.Status404NotFound, "Registro não exite");

                _repository.Delete(response);

                if (await _repository.SaveChangeAsync()) return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Houve um problema na base de dados");
            }
            return BadRequest();
        }
    }
}
using System.Collections.Generic;

namespace ProAgil.API.Dtos
{
    public class PalestranteDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string MiniCurriculum { get; set; }  
        public string ImagemUrl { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public List<RedeSocialDTO> RedeSociais { get; set; }
        public List<EventoDTO> Evento { get; set; }
    }
}
using System.Collections.Generic;

namespace ProAgil.API.Dtos
{
    public class EventoDTO
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string DataEvento { get; set; }
        public string Tema { get; set; }
        public int QtdPessoas { get; set; }
        public string ImagemUrl { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public List<LoteDTO> Lote { get; set; }
        public List<RedeSocialDTO> RedeSociais { get; set; }
        public List<PalestranteDTO> Palestrante{ get; set; }
    }
}
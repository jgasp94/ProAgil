using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.API.Dtos
{
    public class EventoDTO
    {
        public int Id { get; set; }
        [Required]
        public string Local { get; set; }
        public string DataEvento { get; set; }
        public string Tema { get; set; }
        [Range(2, 12000, ErrorMessage="A quantidade deve ser entre 2 a 120 mil pessoas por evento")]
        public int QtdPessoas { get; set; }
        public string ImagemUrl { get; set; }
        public string Telefone { get; set; }
        [EmailAddress]
        [Required(ErrorMessage="O campo {0} é obrigatório")]
        public string Email { get; set; }
        public List<LoteDTO> Lotes { get; set; }
        public List<RedeSocialDTO> RedeSociais { get; set; }
        public List<PalestranteDTO> Palestrante{ get; set; }
    }
}
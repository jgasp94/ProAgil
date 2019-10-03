namespace ProAgil.Domain.Entities
{
    public class PalestranteEvento
    {
        public int PalestranteId { get; set; }
        public Palestrante Palestrante { get; set; }
        public int EventoId { get; set; }
        public Evento Evento { get; }
    }

    /*
        PaletranteId == EventoId
        1                   2
        1                   1
        1                   2
        1                   3
        2                   1   
     */
}
using System.ComponentModel.DataAnnotations;

namespace monitoreoApiNetCore.Entities
{
    public class HistorialPlaza{
        [Required]
        public string Caseta { get; set;}
        public string ListaSql { get; set; }
        public string Extension { get; set; }
        public string ListaServidor { get; set; }
        public string PesoLista { get; set; }
        public System.DateTime? WebService { get; set; }
    }
}
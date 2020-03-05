using System.ComponentModel.DataAnnotations;

namespace monitoreoApiNetCore.Entities
{
    public class HistorialPlaza{
        
        [Required]
        public string Caseta { get; set;}
        public Lista Lista { get; set;}
        public WebService WebService { get; set;}
        public ListaServidor ListaServidor { get; set;}
    }
}
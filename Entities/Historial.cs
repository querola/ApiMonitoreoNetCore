using System;
using System.ComponentModel.DataAnnotations;

namespace monitoreoApiNetCore.Entities
{
    public class Historial
    {
        [Key]
        public string Nombre { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        public short TamañoDeLista { get; set; }
        public string TipoDeLista { get; set; }
        public string Extension { get; set; }
    }
}
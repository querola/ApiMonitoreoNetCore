using System;

namespace monitoreoApiNetCore.Entities
{
    public class PnImportacionWsIndra
    {
        public long IdExp { get; set; }
        public DateTime? DateTransaction { get; set; }
        public string Voie { get; set; }
        public long? EventNumber { get; set; }
        public long? FolioEct { get; set; }
        public int? VersionTarif { get; set; }
        public int? IdPaiement { get; set; }
        public int? TabIdClasse { get; set; }
        public string ClaseMarcada { get; set; }
        public decimal? MontoMarcado { get; set; }
        public int? AcdClass { get; set; }
        public string ClaseDetectada { get; set; }
        public decimal? MontoDetectado { get; set; }
        public string ContenuIso { get; set; }
        public int? CodeGrilleTarif { get; set; }
        public string IdObsMp { get; set; }
        public DateTime? FechaExt { get; set; }
        public int? ShiftNumber { get; set; }
        public int? Plaza { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace monitoreoApiNetCore.Entities
{
    public class pn_importacion_wsIndra
    {
        [Key]
        [Column ("id_exp")]
        public long IdExp { get; set; }
        [Column ("DATE_TRANSACTION")]
        public DateTime? DateTransaction { get; set; }
        [Column ("VOIE")]
        public string Voie { get; set; }
        [Column ("EVENT_NUMBER")]
        public long? EventNumber { get; set; }
        [Column ("FOLIO_ECT")]
        public long? FolioEct { get; set; }
        [Column ("Version_Tarif")]
        public int? VersionTarif { get; set; }
        [Column ("ID_PAIEMENT")]
        public int? IdPaiement { get; set; }
        [Column ("TAB_ID_CLASSE")]
        public int? TabIdClasse { get; set; }
        [Column ("CLASE_MARCADA")]
        public string ClaseMarcada { get; set; }
        [Column ("MONTO_MARCADO")]
        public decimal? MontoMarcado { get; set; }
        [Column ("ACD_CLASS")]
        public int? AcdClass { get; set; }
        [Column ("CLASE_DETECTADA")]
        public string ClaseDetectada { get; set; }
        [Column ("MONTO_DETECTADO")]
        public decimal? MontoDetectado { get; set; }
        [Column ("CONTENU_ISO")]
        public string ContenuIso { get; set; }
        [Column ("CODE_GRILLE_TARIF")]
        public int? CodeGrilleTarif { get; set; }
        [Column ("ID_OBS_MP")]
        public string IdObsMp { get; set; }
        [Column ("fecha_ext")]
        public DateTime? FechaExt { get; set; }
        [Column ("Shift_number")]
        public int? ShiftNumber { get; set; }
        [Column ("PLAZA")]
        public int? Plaza { get; set; }
    }
}
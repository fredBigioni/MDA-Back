using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace WebApi.Entities
{
    public partial class CustomMarketResultVersionHistoric { 
    public int VersionCode { get; set; }

    public int? LineCode { get; set; }

    public string LineDescription { get; set; }

    public int CustomMarketCode { get; set; }

    public string CustomMarketDescription { get; set; }

    public int ProductPresentationCode { get; set; }

    public string ProductPresentationDescription { get; set; }

    public string EAN { get; set; }

    public int? ProductCode { get; set; }

    public string ProductDescription { get; set; }

    public bool? OwnProduct { get; set; }

    public int LabCode { get; set; }

    public string LabAbrev { get; set; }

    public string LabDescription { get; set; }

    public string PPG { get; set; }

    public string MercadoMarca { get; set; }

    public string Marca { get; set; }

    public string TC { get; set; }

    public string TCDescription { get; set; }

    public string FF { get; set; }

    public string FFDewcription { get; set; }

    public string Genero { get; set; }

    public string GeneroDesc { get; set; }

    public DateTime? LanzamientoProd { get; set; }

    public DateTime? LanzamientoPres { get; set; }

    public string Drugs { get; set; }

    public float? Modifier { get; set; }

    public float? INTEModifier { get; set; }

    public float? TAM1_Units { get; set; }
    public float? TAM1_Vals { get; set; }
    public float? TAM2_Units { get; set; }
    public float? TAM2_Vals { get; set; }
    public float? TAM3_Units { get; set; }
    public float? TAM3_Vals { get; set; }
    public float? TAM4_Units { get; set; }
    public float? TAM4_Vals { get; set; }
    public float? TAM5_Units { get; set; }
    public float? TAM5_Vals { get; set; }
    public float? YTD_Units { get; set; }
    public float? YTD_Vals { get; set; }
    public float? YTDAA_Units { get; set; }
    public float? YTDAA_Vals { get; set; }
    public float? TRIM_Units { get; set; }
    public float? TRIM_Vals { get; set; }
    public float? TRIM_IA_Units { get; set; }
    public float? TRIM_IA_Vals { get; set; }
    public float? TRIM_AA_Units { get; set; }
    public float? TRIM_AA_Vals { get; set; }
    public float? M1_Units { get; set; }
    public float? M1_Vals { get; set; }
    public float? M2_Units { get; set; }
    public float? M2_Vals { get; set; }
    public float? M3_Units { get; set; }
    public float? M3_Vals { get; set; }
    public float? M4_Units { get; set; }
    public float? M4_Vals { get; set; }
    public float? M5_Units { get; set; }
    public float? M5_Vals { get; set; }
    public float? M6_Units { get; set; }
    public float? M6_Vals { get; set; }
    public float? M7_Units { get; set; }
    public float? M7_Vals { get; set; }
    public float? M8_Units { get; set; }
    public float? M8_Vals { get; set; }
    public float? M9_Units { get; set; }
    public float? M9_Vals { get; set; }
    public float? M10_Units { get; set; }
    public float? M10_Vals { get; set; }
    public float? M11_Units { get; set; }
    public float? M11_Vals { get; set; }
    public float? M12_Units { get; set; }
    public float? M12_Vals { get; set; }
    public float? M13_Units { get; set; }
    public float? M13_Vals { get; set; }
    public float? M14_Units { get; set; }
    public float? M14_Vals { get; set; }
    public float? M15_Units { get; set; }
    public float? M15_Vals { get; set; }
    public float? M16_Units { get; set; }
    public float? M16_Vals { get; set; }
    public float? M17_Units { get; set; }
    public float? M17_Vals { get; set; }
    public float? M18_Units { get; set; }
    public float? M18_Vals { get; set; }
    public float? M19_Units { get; set; }
    public float? M19_Vals { get; set; }
    public float? M20_Units { get; set; }
    public float? M20_Vals { get; set; }
    public float? M21_Units { get; set; }
    public float? M21_Vals { get; set; }
    public float? M22_Units { get; set; }
    public float? M22_Vals { get; set; }
    public float? M23_Units { get; set; }
    public float? M23_Vals { get; set; }
    public float? M24_Units { get; set; }
    public float? M24_Vals { get; set; }

    public bool? TravelCrm { get; set; }

    public string AdUser { get; set; }

    public string ResponsibleName { get; set; }

    public string ResponsibleLastName { get; set; }
}
}

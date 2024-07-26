using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class CustomMarketDetailRequest
    {        
        public int? CustomMarketGroupCode { get; set; }
        public int? DetailCustomMarketCode { get; set; }
        public int? DrugCode { get; set; }
        public int? DrugGroupCode { get; set; }        
        public bool EnsureVisible { get; set; } 
        public bool Expand { get; set; }
        public bool Graphs { get; set; }
        public string Intemodifier { get; set; }
        public string ItemCondition { get; set; }
        public int? LaboratoryCode { get; set; }
        public int? LaboratoryGroupCode { get; set; }
        public string Modifier { get; set; }
        public int Order { get; set; }
        public bool? OwnProduct { get; set; }
        public string Pattern { get; set; }
        public int? PharmaceuticalFormCode { get; set; }    
        public int? ProductCode { get; set; }
        public int? ProductGroupCode { get; set; }
        public int? ProductPresentationCode { get; set; }
        public int? ProductPresentationGroupCode { get; set; }
        public int? ProductTypeCode { get; set; }
        public string RegExPattern { get; set; } 
        public bool Resume { get; set; }
        public int? TherapeuticalClassCode { get; set; }
    }
}
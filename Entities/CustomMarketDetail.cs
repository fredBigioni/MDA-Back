using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using WebApi.Views;

#nullable disable

namespace WebApi.Entities
{
    public partial class CustomMarketDetail
    {
        public int? ClassCode { get; set; }

        public int CustomMarketCode { get; set; }

        public int? CustomMarketGroupCode { get; set; }

        public int? DetailCustomMarketCode { get; set; }        

        public int? DrugCode { get; set; }

        public int? DrugGroupCode { get; set; }        

        public bool EnsureVisible { get; set; } 

        public bool Expand { get; set; }

        public bool Graphs { get; set; }

        public double Intemodifier { get; set; }

        public double? Intemodifier2 { get; set; }

        public string ItemCondition { get; set; }

        public int? ItemBrand { get; set; }

        public int? LaboratoryCode { get; set; }

        public int? LaboratoryGroupCode { get; set; }

        public double Modifier { get; set; }

        public double? Modifier2 { get; set; }

        public int Order { get; set; }

        public bool? OwnProductsReport { get; set; }

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


        public virtual Class Class { get; set; }

        [JsonIgnore]
        public virtual CustomMarket CustomMarket { get; set; }

        public virtual CustomMarketGroup CustomMarketGroup { get; set; }

        public virtual CustomMarket DetailCustomMarket { get; set; }
        
        public virtual Drug Drug { get; set; }

        public virtual DrugGroup DrugGroup { get; set; }
        
        public virtual Laboratory Laboratory { get; set; }
        
        public virtual LaboratoryGroup LaboratoryGroup { get; set; }

        public virtual PharmaceuticalForm PharmaceuticalForm { get; set; }   
        
        public virtual Product Product { get; set; }
        
        public virtual ProductGroup ProductGroup { get; set; }
        
        public virtual ProductPresentation ProductPresentation { get; set; }
        
        public virtual ProductPresentationGroup ProductPresentationGroup { get; set; }

        public virtual ProductType ProductType { get; set; }
        
        public virtual TherapeuticalClass TherapeuticalClass { get; set; }
    }
}

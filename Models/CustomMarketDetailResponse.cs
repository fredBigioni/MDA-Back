using System.Text.Json.Serialization;
using WebApi.Entities;
using System;
using System.Text.Json;

namespace WebApi.Models
{
    public class CustomMarketDetailResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public int? CustomMarketGroupCode { get; set; }

        public int? DetailCustomMarketCode { get; set; }

        public string DetailCustomMarketDescription { get; set; }

        public int? DrugCode { get; set; }

        public string DrugDescription { get; set; }

        public int? DrugGroupCode { get; set; }        

        public string DrugGroupDescription { get; set; }  

        public bool EnsureVisible { get; set; } 

        public bool Expand { get; set; }

        public bool Graphs { get; set; }

        public string Intemodifier { get; set; }

        public string ItemCondition { get; set; }

        public int? LaboratoryCode { get; set; }

        public string LaboratoryDescription { get; set; }

        public int? LaboratoryGroupCode { get; set; }

        public string LaboratoryGroupDescription { get; set; }

        public string Modifier { get; set; }

        public int Order { get; set; }

        public bool? OwnProductsReport { get; set; }

        public bool? OwnProduct { get; set; }

        public string Pattern { get; set; }

        public int? PharmaceuticalFormCode { get; set; }    

        public string PharmaceuticalFormDescription { get; set; }    

        public int? ProductCode { get; set; }

        public string ProductDescription { get; set; }

        public int? ProductGroupCode { get; set; }

        public string ProductGroupDescription { get; set; }    

        public int? ProductPresentationCode { get; set; }

        public string ProductPresentationDescription { get; set; }

        public int? ProductPresentationGroupCode { get; set; }

        public string ProductPresentationGroupDescription { get; set; }  

        public int? ProductTypeCode { get; set; } 

        public string ProductTypeDescription { get; set; } 

        public string RegExPattern { get; set; } 

        public bool Resume { get; set; }

        public int? TherapeuticalClassCode { get; set; }

        public string TherapeuticalClassDescription { get; set; }

        public CustomMarketDetailResponse(
            CustomMarketDetail customMarketDetail,
            string productDescription, 
            string productPresentationDescription,
            bool? ownProductsReport,
            bool? ownProduct
            )
        {
            CustomMarketGroupCode = customMarketDetail.CustomMarketGroupCode;
            DetailCustomMarketCode = customMarketDetail.DetailCustomMarket?.Code;
            DetailCustomMarketDescription = customMarketDetail.DetailCustomMarket?.Description;
            DrugCode = customMarketDetail.Drug?.Code;
            DrugDescription = customMarketDetail.Drug?.Description;
            DrugGroupCode = customMarketDetail.DrugGroup?.Code;
            DrugGroupDescription = customMarketDetail.DrugGroup?.Description;
            EnsureVisible = customMarketDetail.EnsureVisible;
            Expand = customMarketDetail.Expand;
            Graphs = customMarketDetail.Graphs;
            Intemodifier = customMarketDetail.Intemodifier.ToString();
            ItemCondition = customMarketDetail.ItemCondition;
            LaboratoryCode = customMarketDetail.Laboratory?.Code;
            LaboratoryDescription = customMarketDetail.Laboratory?.Description;
            LaboratoryGroupCode = customMarketDetail.LaboratoryGroup?.Code;
            LaboratoryGroupDescription = customMarketDetail.LaboratoryGroup?.Description;
            Modifier =customMarketDetail.Modifier.ToString();
            Order = customMarketDetail.Order;
            OwnProduct = ownProduct;
            OwnProductsReport = ownProductsReport;
            Pattern = customMarketDetail.Pattern;      
            PharmaceuticalFormCode = customMarketDetail.PharmaceuticalForm?.Code;
            PharmaceuticalFormDescription = customMarketDetail.PharmaceuticalFormCode != null ?customMarketDetail.PharmaceuticalForm?.Imscode + " - " + customMarketDetail.PharmaceuticalForm?.Description : null;
            ProductCode = customMarketDetail.ProductCode;
            ProductDescription = productDescription;
            ProductGroupCode = customMarketDetail.ProductGroupCode;
            ProductGroupDescription = customMarketDetail.ProductGroup?.Description;            
            ProductPresentationCode = customMarketDetail.ProductPresentationCode;
            ProductPresentationDescription = productPresentationDescription;
            ProductPresentationGroupCode = customMarketDetail.ProductPresentationGroupCode;
            ProductPresentationGroupDescription = customMarketDetail.ProductPresentationGroup?.Description;                        
            ProductTypeCode = customMarketDetail.ProductTypeCode;
            ProductTypeDescription = customMarketDetail.ProductType?.Description;
            Resume = customMarketDetail.Resume;
            TherapeuticalClassCode = customMarketDetail.TherapeuticalClass?.Code;
            TherapeuticalClassDescription = customMarketDetail.TherapeuticalClassCode != null ? customMarketDetail.TherapeuticalClass?.Imscode + " - " + customMarketDetail.TherapeuticalClass?.Description : null;
        }        
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using WebApi.Views;

#nullable disable

namespace WebApi.Views
{
    public partial class CustomMarketPreview
    {
        public int Id { get; set; }
        public double Modifier { get; set; } 
        public double Intemodifier { get; set; }
        public bool OwnProduct { get; set; }
        public string EntityDesc { get; set; }    
        public int EntityId { get; set; }
        public string ProductPresentationDescription { get; set; }   
        public int ProductPresentationCode { get; set; }   
        public string ProductDescription { get; set; }   
        public int ProductCode { get; set; }           
        public string LabDescription { get; set; }   
        public int LabCode { get; set; }           
        public string TCDescription { get; set; }   
        public string TCCode { get; set; }           
        public int? ClassCode { get; set; }           
        public string PFCode { get; set; }           
        public string PFDescription { get; set; }           
        public string Pattern { get; set; }           
        public int EnsureVisible { get; set; } 
        public int Resume { get; set; }
        public int Expand { get; set; }
        public int Graph { get; set; }
        public string ItemCondition { get; set; }    
        public int Orden { get; set; }
        public string Drug { get; set; }    
        public string BusinessUnit { get; set; }    
    }
}

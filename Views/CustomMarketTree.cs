using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Views
{
    public partial class CustomMarketTree
    {
        public int? LineGroupCode { get; set; }
        public string LineGroupDescription { get; set; }
        public int? LineCode { get; set; }
        public string LineDescription { get; set; }
        public int? CustomMarketCode { get; set; }
        public string CustomMarketDescription { get; set; }        
        //public int? CustomMarketOrder { get; set; }
        public bool? CustomMarketTest { get; set; }
    }
}
using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Views
{
    public partial class ProductComponent
    {
        public int? ProductCode { get; set; }
        public int? ProductGroupCode { get; set; }
        public string Description { get; set; }
        public string Class { get; set; }
        public string Laboratory { get; set; }
        public bool? OwnProduct { get; set; }
    }
}
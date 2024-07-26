using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Views
{
    public partial class ProductPresentationComponent
    {
        public int? ProductPresentationGroupCode { get; set; }
        public int? Code { get; set; }
        public string Description { get; set; }
        public string Class { get; set; }
        public string Laboratory { get; set; }
        public string TherapeuticalClass { get; set; }
        public int? PharmaceuticalFormCode { get; set; }
    }
}
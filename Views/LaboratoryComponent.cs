using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Views
{
    public partial class LaboratoryComponent
    {
        public int? LaboratoryGroupCode { get; set; }
        public int? LaboratoryCode { get; set; }
        public string Imscode { get; set; }
        public string Description { get; set; }
        public bool? ownLab { get; set; }
    }
}
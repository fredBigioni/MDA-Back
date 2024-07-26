using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Views
{
    public partial class PharmaceuticalFormComponent
    {
        public int Code { get; set; }
        public string Imscode { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public int? ProductCode { get; set; }
        [JsonIgnore]
        public int? ProductGroupCode { get; set; }
        [JsonIgnore]
        public int? ProductPresentationCode { get; set; }
        [JsonIgnore]
        public int? ProductPresentationGroupCode { get; set; }        
    }
}
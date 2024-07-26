using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Entities
{
    public partial class LaboratoryGroupDetail
    {
        [JsonIgnore]
        public int LaboratoryGroupCode { get; set; }
        [JsonIgnore]
        public int LaboratoryCode { get; set; }

        public virtual Laboratory Laboratory { get; set; }
        [JsonIgnore]
        public virtual LaboratoryGroup LaboratoryGroup { get; set; }
    }
}
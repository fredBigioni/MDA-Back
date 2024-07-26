using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Views
{
    public partial class DrugComponent
    {
        public int? DrugGroupCode { get; set; }
        public int? DrugCode { get; set; }
        public string Description { get; set; }
    }
}
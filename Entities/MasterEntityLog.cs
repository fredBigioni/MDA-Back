using System;

#nullable disable

namespace WebApi.Entities
{
    public partial class MasterEntityLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Entity { get; set; }
        public string LogJson { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

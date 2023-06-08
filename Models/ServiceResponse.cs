using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fightAPI.Models
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public DateTime Time { get; set; } = DateTime.UtcNow;
    }
}
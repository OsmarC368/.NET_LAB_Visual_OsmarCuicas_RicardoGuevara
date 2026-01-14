using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBlazor.Data.Models
{
    public class ResponseAPI<TEntity>
    {
        public bool ok { get; set; }
        public string mensaje { get; set; } = string.Empty;
        public Array datos { get; set; } = Array.Empty<TEntity>();

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBlazor.Data
{
    public class Response<TEntity>
    {
        public string statusCode { get; set; } = string.Empty;
        public bool Ok { get; set; }
        public string message { get; set; } = string.Empty;
        public TEntity? Data { get; set; }

    }
}
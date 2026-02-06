using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBlazor.Data.Models.Core;

namespace AppBlazor.Data
{
    public class Response<TEntity>
    {
        public string statusCode { get; set; } = string.Empty;
        public bool Ok { get; set; }
        public string message { get; set; } = string.Empty;
        public TEntity? Data { get; set; }

        public static implicit operator Response<TEntity>(Models.Core.Responses.Response<Measure> v)
        {
            throw new NotImplementedException();
        }
    }
}
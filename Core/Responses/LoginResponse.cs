using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Responses
{
	public class ResponseLogin
	{
		public string jwt { get; set; }
		public int idusersession { get; set; }
	}
}

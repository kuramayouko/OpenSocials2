namespace OpenSocials.App_Code
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

	public class MetaBusiness
	{
		public Facebook FacebookApi { get; set; }
		public Instagram InstagramApi { get; set; }

        private readonly string Token;
		
		public class Facebook
		{

		}

		public class Instagram
		{

		}
		
	}
}

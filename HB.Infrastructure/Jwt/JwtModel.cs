using System;
namespace HB.Service.Engine
{
	public class JwtModel
    {
        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
	}
}


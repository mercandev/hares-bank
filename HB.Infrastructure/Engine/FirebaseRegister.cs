using System;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HB.Infrastructure.Engine
{
    public static class FirebaseRegister
    {
		public static void FirebaseAuthRegister(this IServiceCollection services , IConfiguration configuration)
		{
            var config = new FirebaseAuthConfig
            {
                ApiKey = configuration.GetValue<string>("Firebase:Key"),
                AuthDomain = configuration.GetValue<string>("Firebase:AuthDomain"),
                Providers = new FirebaseAuthProvider[]
                {
                     new EmailProvider()
                }
            };

            services.AddSingleton(config);
        }
	}
}


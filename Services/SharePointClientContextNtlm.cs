using Microsoft.SharePoint.Client;
using System.Net;

namespace net60api.Services
{
    public static class SharePointClientContextNtlmFactoryServiceConfiguration
    {
        public static IServiceCollection AddSharePointContextNtlmFactory(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ISharePointClientContextFactory, SharePointClientContextNtlmFactory>();
            return serviceCollection;
        }

        public static IServiceCollection AddCurrentUserSharePointClientContextNtlm(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSharePointContextNtlmFactory();

            serviceCollection.AddScoped<ClientContext>((services) =>
            {
                var clientContextFactory = services.GetService<ISharePointClientContextFactory>();
                return clientContextFactory.GetClientContext();
            });

            return serviceCollection;
        }
    }

    public class SharePointClientContextNtlmFactory : ISharePointClientContextFactory
    {
        private readonly IConfiguration _configuration;

        public SharePointClientContextNtlmFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private ClientContext GetClientContextInternal(string siteUrl = null, string user = null, string password = null, string domain = null)
        {
            siteUrl ??= _configuration.GetValue<string>("sp:BaseUrl");
            if (string.IsNullOrEmpty(siteUrl))
                throw new Exception("The SharePoint URL is not specified or configured");

            user = user ?? _configuration.GetValue<string>("sp:User");
            password = password ?? _configuration.GetValue<string>("sp:Password");
            domain = domain ?? _configuration.GetValue<string>("sp:Domain");

            var clientContext = new ClientContext(siteUrl);
            clientContext.Credentials = new NetworkCredential(user, password, domain);

            return clientContext;
        }

        public ClientContext GetClientContext(string siteUrl = null)
        {
            return GetClientContextInternal(siteUrl);
        }
    }
}
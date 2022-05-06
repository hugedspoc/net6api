using Microsoft.Identity.Web;
using Microsoft.SharePoint.Client;

namespace net60api.Services
{
    public static class SharePointClientContextFactoryServiceConfiguration
    {
        public static IServiceCollection AddSharePointContextFactory(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ISharePointClientContextFactory, SharePointClientContextFactory>();
            return serviceCollection;
        }

        public static IServiceCollection AddCurrentUserSharePointClientContext(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSharePointContextFactory();

            serviceCollection.AddScoped<ClientContext>((services) =>
            {
                var clientContextFactory = services.GetService<ISharePointClientContextFactory>();
                return clientContextFactory.GetClientContext();
            });

            return serviceCollection;
        }
    }

    public class SharePointClientContextFactory : ISharePointClientContextFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenAcquisition _tokenAcquisition;

        public SharePointClientContextFactory(IConfiguration configuration, ITokenAcquisition tokenAcquisition)
        {
            _configuration = configuration;
            _tokenAcquisition = tokenAcquisition;
        }

        private string GetResourceUri(string siteUrl)
        {
            var uri = new Uri(siteUrl);
            return $"{uri.Scheme}://{uri.DnsSafeHost}";
        }

        private string[] GetSharePointResourceScope(string uri, string[] scopes, bool safeUri = true, bool prefixUri = true)
        {
            string resourceUri = uri;
            
            if(safeUri) resourceUri = GetResourceUri(uri);
            
            var resourceScope = scopes == null
                ? new[] { $"{resourceUri}/.default" }
                : scopes.Select(scope => prefixUri ? $"{resourceUri}/{scope}" : scope).ToArray();

            return resourceScope;
        }

        private ClientContext GetClientContextInternal(string siteUrl, string[] scopes = null)
        {
            var scopesList = _configuration.GetValue<string>("SharePoint:Scopes");
            if (string.IsNullOrEmpty(scopesList))
                throw new Exception("The SharePoint Scopes are not specified or configured");
            scopes = scopesList.Split(' ');

            //var appIdUri = _configuration.GetValue<string>("SharePoint:AppIdUri");
            //if (string.IsNullOrEmpty(appIdUri))
            //    throw new Exception("The SharePoint App ID Uri is not specified or configured");

            siteUrl ??= _configuration.GetValue<string>("SharePoint:BaseUrl");
            if (string.IsNullOrEmpty(siteUrl))
                throw new Exception("The SharePoint URL is not specified or configured");

            // Acquire the access token.
            //string[] effectiveScopes = GetSharePointResourceScope(appIdUri, scopes, false, true);
            var clientContext = new ClientContext(siteUrl);
            clientContext.ExecutingWebRequest += (object sender, WebRequestEventArgs e) =>
            {
                string accessToken = _tokenAcquisition.GetAccessTokenForUserAsync(scopes).GetAwaiter().GetResult();
                e.WebRequestExecutor.RequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            };

            return clientContext;
        }

        public ClientContext GetClientContext(string siteUrl = null)
        {
            return GetClientContextInternal(siteUrl);
        }
    }
}
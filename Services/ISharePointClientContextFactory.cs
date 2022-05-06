using Microsoft.SharePoint.Client;

namespace net60api.Services
{
    public interface ISharePointClientContextFactory
    {
        ClientContext GetClientContext(string siteUrl = null);
    }
}
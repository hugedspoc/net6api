using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using System.Net;
//using Microsoft.Graph;
using net60api.Services;

namespace net60api.Pages
{
    //[Authorize]
    //[AuthorizeForScopes(ScopeKeySection = "MicrosoftGraph:Scopes")]
    //[AuthorizeForScopes(ScopeKeySection = "SharePoint:Scopes")]
    //[AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        //private readonly IDownstreamWebApi _downstreamWebApi;
        //private readonly GraphServiceClient _graphServiceClient;
        private readonly ISharePointClientContextFactory _spClientContextFactory;

        public IndexModel(ILogger<IndexModel> logger, ISharePointClientContextFactory spClientContextFactory) //GraphServiceClient graphServiceClient, IDownstreamWebApi downstreamWebApi)
        {
            _logger = logger;
            _spClientContextFactory = spClientContextFactory;
            //_graphServiceClient = graphServiceClient;
            //_downstreamWebApi = downstreamWebApi;
        }

        public async Task OnGet()
        {
        }
    }
}
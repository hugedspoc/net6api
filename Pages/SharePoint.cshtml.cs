using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using net60api.Services;
using OfficeDevPnP.Core;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.SharePoint.Client;
using System;
using System.Security.Claims;

namespace net60api.Pages
{
    [Authorize]
    public class SharePointModel : PageModel
    {
        private readonly ILogger<SharePointModel> _logger;

        //private readonly IDownstreamWebApi _downstreamWebApi;
        //private readonly GraphServiceClient _graphServiceClient;

        public SharePointModel(ILogger<SharePointModel> logger) //GraphServiceClient graphServiceClient, IDownstreamWebApi downstreamWebApi)
        {
            _logger = logger;
            //_graphServiceClient = graphServiceClient;
            //_downstreamWebApi = downstreamWebApi;
        }

        public async Task OnGet()
        {
            //var user = await _graphServiceClient.Me.Request().GetAsync();
            //ViewData["GraphApiResult"] = user.DisplayName;

            //using (var ctx = _spClientContextFactory.GetClientContext())
            //{
            //    ctx.Load(ctx.Web);
            //    await ctx.ExecuteQueryAsync();
            //    var webTitle = ctx.Web.Title;
            //    ViewData["SPCsomResult"] = $"Nom du site = {webTitle}";
            //}

            //using var response = await _downstreamWebApi.CallWebApiForUserAsync("DownstreamApi").ConfigureAwait(false);
            //if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    var apiResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            //    ViewData["ApiResult"] = apiResult;
            //}
            //else
            //{
            //    var error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            //    throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}: {error}");
            //}
        }
    }
}
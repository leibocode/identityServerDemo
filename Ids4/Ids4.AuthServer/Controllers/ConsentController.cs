using IdentityServer4.Stores;
using IdentityServer4.Services;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ids4.AuthServer.ViewModels;

namespace Ids4.AuthServer.Controllers
{
    public class ConsentController : Controller
    {

        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;


        public ConsentController(IClientStore clientStore, IResourceStore resourceStore,
            IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }

        private async Task<ConsentViewModel> BuildConsentViewModel(string returnUrl)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null)
            {
                return null;
            }

            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);

            return CreateConsentViewModel(request,client,resources);
        }

        private ConsentViewModel CreateConsentViewModel(AuthorizationRequest request,
            Client client, Resources resources)
        {
            var vm = new ConsentViewModel();
            vm.ClientName = client.ClientName;
            vm.ClientLogoUrl = client.LogoUri;
            vm.ClientUrl = client.ClientUri;
            vm.AllowRememberConsent = client.AllowRememberConsent;

            vm.IdentityScopes = resources.IdentityResources.Select(
                i=>CreateScopeViewModel(i));
            vm.ResourceScopes = resources.ApiResources.SelectMany
                (i => i.Scopes).Select(i => CreateScopeViewModel(i));

            return vm;

        }

        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource)
        {
            return new ScopeViewModel
            {
                Name = identityResource.Name,
                DisplayName = identityResource.Description,
                Description = identityResource.Description,
                Required = identityResource.Required,
                Checked = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
        }

        private ScopeViewModel CreateScopeViewModel(Scope scope)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Required = scope.Required,
                Checked = scope.Required,
                Emphasize = scope.Emphasize
            };
        }


        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await BuildConsentViewModel(returnUrl);
            if (model ==null)
            {

            }

            return View(model);
        }
    }
}

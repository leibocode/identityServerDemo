using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using MvcCookieAuthSample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCookieAuthSample.Controllers
{
    public class ConSentController : Controller
    {
        public readonly IClientStore _clientStore;
        public readonly IResourceStore _resourceStore;
        public readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConSentController(IClientStore clientStore, IResourceStore resourceStore, IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }

        private async Task<ConSentViewModel> BuildConsentViewModel(string retrunUrl)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(retrunUrl);

            if (request == null)
            {
                return null;
            }

            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
            var vm = CreateConsentViewModel(request, client, resources);
            vm.ReturnUrl = retrunUrl;
            return vm;

        }

        private ConSentViewModel CreateConsentViewModel(AuthorizationRequest request, Client client, Resources resources)
        {
            var vm = new ConSentViewModel();
            vm.ClientName = client.ClientName;
            vm.ClientLogoUrl = client.LogoUri;
            vm.ClientUrl = client.ClientUri;
            vm.AllowRememberConsent = client.AllowRememberConsent;

            vm.IdentitySceops = resources.IdentityResources.Select(x => createScopeViewModel(x));
            vm.ResourceScopes = resources.ApiResources.SelectMany(i => i.Scopes).Select(i => createScopeViewModel(i));
            return vm;
        }

        private ScopeViewModel createScopeViewModel(IdentityResource identityResource)
        {
            return new ScopeViewModel()
            {
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Checked = identityResource.Required,
                Required = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
        }

        private ScopeViewModel createScopeViewModel(Scope scope)
        {
            return new ScopeViewModel
            {
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Checked = scope.Required,
                Required = scope.Required,
                Emphasize = scope.Emphasize
            };

        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await BuildConsentViewModel(returnUrl);
            if (model == null)
            {

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(InputConSentViewModel viewModel)
        {
            ConsentResponse consentResponse = null;
            if (viewModel.Button == "no")
            {
                consentResponse = ConsentResponse.Denied;
            }
            else if (viewModel.Button=="yes")
            {
                if (viewModel.ScopedConSented !=null && viewModel.ScopedConSented.Any())
                {
                    consentResponse = new ConsentResponse()
                    {
                        RememberConsent = viewModel.RemberConsented,
                        ScopesConsented = viewModel.ScopedConSented
         
                    };
                }
                if (consentResponse !=null)
                {
                    var request = await _identityServerInteractionService.GetAuthorizationContextAsync(viewModel.ReturnUrl);
                    await _identityServerInteractionService.GrantConsentAsync(request, consentResponse);

                }
            }
        }
    }
}

using IdentityServer4.Stores;
using IdentityServer4.Services;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ids4.AuthServer.ViewModels;

namespace Ids4.AuthServer.Services
{
    public class ConsentService
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentService(IClientStore clientStore,
            IResourceStore resourceStore,
            IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }

        #region private Methods
        private ConsentViewModel CreateConsentViewModel(AuthorizationRequest request,Client client,Resources resources,  InputConsentViewModel model)
        {
            var rememberConsent = model?.RememberConsent ?? true;
            //选择的权限
            var selectedScopes = model?.ScopedsConsented ?? Enumerable.Empty<string>();
            var vm = new ConsentViewModel();
            vm.ClientName = client.ClientName;
            vm.ClientLogoUrl = client.LogoUri;
            vm.ClientId = client.ClientId;
            vm.RememberConsent = rememberConsent;

            //vm.IdentityScorpes = resources.IdentityResources.Select(x => CreateScopeViewModel(x,selectedScopes.Contains(x.Name) || model ==null));
            //vm.ResourceScorpes = resources.ApiResources.SelectMany(i => i.Scopes).Select(x => CreateScopeViewModel(x, selectedScopes.Contains(x.Name) || model == null));

            return vm;
        }

        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource,bool check)
        {
            return new ScopeViewModel
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Checked = check || identityResource.Required,
                Required = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
        }

        private ScopeViewModel CreateScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Checked = check || scope.Required,
                Required = scope.Required,
                Emphasize = scope.Emphasize
            };
        }
        #endregion

        public async Task<ConsentViewModel> BuildConsentViewModel(string returnUrl,InputConsentViewModel model = null)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null)
            {
                return null;
            }
            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
            var vm = CreateConsentViewModel(request, client,resources,model);
            vm.ReturnUrl = returnUrl;
            return vm;
        }

        public async Task<ProcessConsentResult> ProcessConsent(InputConsentViewModel viewModel)
        {
            ConsentResponse consentResponse = null;
            var result = new ProcessConsentResult();
            if (viewModel.Button == "no")
            {
                consentResponse = ConsentResponse.Denied;
            }else if (viewModel.Button =="yes")
            {
                if (viewModel.ScopedsConsented !=null && viewModel.ScopedsConsented.Any())
                {
                    consentResponse = new ConsentResponse
                    {
                        RememberConsent = viewModel.RememberConsent,
                        ScopesConsented = viewModel.ScopedsConsented
                    };
                }

                result.ValidationError = "请至少选中一个权限";
            }

            if (consentResponse !=null)
            {
                var consentfViewModel = await BuildConsentViewModel(viewModel.ReturnUrl, viewModel);
                result.ViewModel = consentfViewModel;
            }
            return result;
        }
    }
}

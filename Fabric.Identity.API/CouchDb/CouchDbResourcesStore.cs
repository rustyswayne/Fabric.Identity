﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Fabric.Identity.API.CouchDb
{
    public class CouchDbResourcesStore : IResourceStore
    {
        private readonly IDocumentDbService _documentDbService;
        private const string IdentityResourceDocumentType = "identityresource:";
        private const string ApiResourceDocumentType = "apiresource:";

        public CouchDbResourcesStore(IDocumentDbService documentDbService)
        {
            _documentDbService = documentDbService;
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var identityResources = _documentDbService.GetDocuments<IdentityResource>(IdentityResourceDocumentType).Result;

            var matchingResources = identityResources.Where(r => scopeNames.Contains(r.Name));

            return Task.FromResult(matchingResources);
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var apiResources = _documentDbService.GetDocuments<ApiResource>(ApiResourceDocumentType).Result;

            var apiResourcesForScope = apiResources.Where(a => scopeNames.Contains(a.Name));

            return Task.FromResult(apiResourcesForScope);
        }

        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            return _documentDbService.GetDocument<ApiResource>(name);
        }

        public Task<Resources> GetAllResources()
        {
            var apiResources = _documentDbService.GetDocuments<ApiResource>(ApiResourceDocumentType).Result;
            var identityResources = _documentDbService.GetDocuments<IdentityResource>(IdentityResourceDocumentType).Result;

            var result = new Resources(identityResources, apiResources);
            return Task.FromResult(result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Data.Edm.Library;
using Microsoft.OData.Edm;
using Naveego.Sdk.Logging;
using PluginOData.API.Utility;
using PluginOData.DataContracts;
using PluginOData.Helper;
using Simple.OData.Client;
using V3EdmModelBase = Microsoft.Data.Edm.Library.EdmModelBase;
using V4EdmModelBase = Microsoft.OData.Edm.EdmModelBase;

namespace PluginOData.API.Factory
{
    public class ApiClient: IApiClient
    {

        private static ODataClient Client { get; set; }
        private Settings Settings { get; set; }

        public ApiClient(HttpClient httpClient, Settings settings)
        {
            var oDataSettings = new ODataClientSettings(httpClient)
            {
                BaseUri = new Uri(settings.BaseUrl),
                PayloadFormat = ODataPayloadFormat.Json,
                ReadUntypedAsString = true,
                OnTrace = (x, y) => Console.WriteLine(string.Format(x, y)),
            };

            if (settings.HasAuth())
            {
                var authString =
                    Convert.ToBase64String(Encoding.UTF8.GetBytes($"{settings.Username}:{settings.Password}"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);
            }
            
            Client = new ODataClient(oDataSettings);
            Settings = settings;
        }
        
        public async Task TestConnection()
        {
            try
            {
                await Client.GetMetadataAsync();
            }
            catch (Exception e)
            {
                Logger.Error(e, e.Message);
                throw;
            }
        }

        public async Task<ODataMetadata> GetMetadataAsync()
        {
            if (await Client.GetMetadataAsync() is V3EdmModelBase metadataV3)
            {
                return new ODataMetadata(metadataV3, null);
            }

            if (await Client.GetMetadataAsync() is V4EdmModelBase metadataV4)
            {
                return new ODataMetadata(null, metadataV4);
            }

            throw new Exception("Error getting metadata.");
        }
        
        public async Task<IEnumerable<IDictionary<string,object>>> GetAllEntitiesAsync(string entityName)
        {
            return await Client
                .For(entityName)
                .FindEntriesAsync();
        }
        
        public async Task<int> GetCountAsync(string entityName)
        {
            return await Client
                .For(entityName)
                .Count()
                .FindScalarAsync<int>();
        }
    }
}
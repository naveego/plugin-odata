using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Data.Edm.Library;
using PluginOData.DataContracts;

namespace PluginOData.API.Factory
{
    public interface IApiClient
    {
        Task TestConnection();
        Task<ODataMetadata> GetMetadataAsync();
        Task<IEnumerable<IDictionary<string, object>>> GetAllEntitiesAsync(string entityName);
        Task<int> GetCountAsync(string entityName);
    }
}
using System.Threading.Tasks;
using Naveego.Sdk.Plugins;
using PluginOData.API.Factory;
using PluginOData.API.Utility;

namespace PluginOData.API.Discover
{
    public static partial class Discover
    {
        public static async Task<Count> GetCountOfRecords(IApiClient apiClient, Schema schema)
        {
            return new Count
            {
                Kind = Count.Types.Kind.Exact, 
                Value = await apiClient.GetCountAsync(schema.Id)
            };
        }
    }
}
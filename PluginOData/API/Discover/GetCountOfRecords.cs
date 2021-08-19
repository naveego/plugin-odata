using System;
using System.Threading.Tasks;
using Naveego.Sdk.Logging;
using Naveego.Sdk.Plugins;
using PluginOData.API.Factory;
using PluginOData.API.Utility;

namespace PluginOData.API.Discover
{
    public static partial class Discover
    {
        public static async Task<Count> GetCountOfRecords(IApiClient apiClient, Schema schema)
        {
            try
            {
                return new Count
                {
                    Kind = Count.Types.Kind.Exact, 
                    Value = await apiClient.GetCountAsync(schema.Id),
                };
            }
            catch (Exception e)
            {
                Logger.Error(e, e.Message);
                return new Count
                {
                    Kind = Count.Types.Kind.Unavailable,
                };
            }
        }
    }
}
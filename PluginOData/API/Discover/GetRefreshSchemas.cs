using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using Naveego.Sdk.Plugins;
using PluginOData.API.Factory;
using PluginOData.API.Utility;

namespace PluginOData.API.Discover
{
    public static partial class Discover
    {
        public static async IAsyncEnumerable<Schema> GetRefreshSchemas(IApiClient apiClient,
            RepeatedField<Schema> refreshSchemas, int sampleSize = 5)
        {
            foreach (var schema in refreshSchemas)
            {
                yield return await GetAllSchemas(apiClient, sampleSize, schema.Id).FirstAsync();
            }
        }
    }
}
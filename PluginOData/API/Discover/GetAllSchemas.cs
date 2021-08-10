using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Edm.Library;
using Microsoft.OData.Edm;
using Naveego.Sdk.Plugins;
using Newtonsoft.Json;
using PluginOData.API.Factory;
using PluginOData.API.Utility;
using PluginOData.DataContracts;
using PluginOData.Helper;

using V3EdmModelBase = Microsoft.Data.Edm.Library.EdmModelBase;

using V4EdmModelBase = Microsoft.OData.Edm.EdmModelBase;

namespace PluginOData.API.Discover
{
    public static partial class Discover
    {
        public static async IAsyncEnumerable<Schema> GetAllSchemas(IApiClient apiClient, 
            int sampleSize = 5, string idFilter = "")
        {
            var metadata = await apiClient.GetMetadataAsync();
            var v3Model = metadata.GetV3Model();
            var v4Model = metadata.GetV4Model();

            if (v3Model != null)
            {
                var v3Schemas = GetSchemasForV3Metadata(apiClient, v3Model, sampleSize, idFilter);

                await foreach (var schema in v3Schemas)
                {
                    yield return schema;
                }
            }
            
            if (v4Model != null)
            {
                var v3Schemas = GetSchemasForV4Metadata(apiClient, v4Model, sampleSize, idFilter);

                await foreach (var schema in v3Schemas)
                {
                    yield return schema;
                }
            }
        }

        private static async Task<Schema> AddSampleAndCount(IApiClient apiClient, Schema schema,
            int sampleSize)
        {
            // add sample and count
            var records = Read.Read.ReadRecordsAsync(apiClient, schema).Take(sampleSize);
            schema.Sample.AddRange(await records.ToListAsync());
            schema.Count = await GetCountOfRecords(apiClient, schema);

            return schema;
        }
    }
}
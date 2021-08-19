using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Naveego.Sdk.Logging;
using Naveego.Sdk.Plugins;
using Newtonsoft.Json;
using PluginOData.API.Factory;
using PluginOData.API.Utility;

namespace PluginOData.API.Read
{
    public static partial class Read
    {
        public static async IAsyncEnumerable<Record> ReadRecordsAsync(IApiClient apiClient, Schema schema)
        {
            var data = await apiClient.GetAllEntitiesAsync(schema.Id);

            foreach (var record in data)
            {
                var recordMap = new Dictionary<string, object>();

                foreach (var property in schema.Properties)
                {
                    try
                    {
                        if (record.ContainsKey(property.Id))
                        {
                            switch (property.Type)
                            {
                                case PropertyType.String:
                                case PropertyType.Text:
                                case PropertyType.Decimal:
                                    recordMap[property.Id] = record[property.Id].ToString();
                                    break;
                                default:
                                    recordMap[property.Id] = record[property.Id];
                                    break;
                            }
                        }
                        else
                        {
                            recordMap[property.Id] = null;
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, $"No column with property Id: {property.Id}");
                        Logger.Error(e, e.Message);
                        recordMap[property.Id] = null;
                    }
                }

                yield return new Record
                {
                    Action = Record.Types.Action.Upsert,
                    DataJson = JsonConvert.SerializeObject(recordMap)
                };;
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Naveego.Sdk.Plugins;
using PluginOData.API.Factory;
using V3EdmModelBase = Microsoft.Data.Edm.Library.EdmModelBase;

namespace PluginOData.API.Discover
{
    public static partial class Discover
    {
        public static async IAsyncEnumerable<Schema> GetSchemasForV3Metadata(IApiClient apiClient,
            V3EdmModelBase metadata, int sampleSize = 5, string idFilter = "")
        {
            foreach (IEdmEntityContainer entityContainer in metadata.EntityContainers())
            {
                foreach (IEdmEntityContainerElement item in entityContainer.Elements)
                {
                    if (item is IEdmEntitySet)
                    {
                        var entitySet = (IEdmEntitySet) item;
                        var schemaElement = (IEdmSchemaElement) entitySet.ElementType;
                        var structuredType = (IEdmStructuredType) schemaElement;
                        var entityType = (IEdmEntityType) schemaElement;

                        if (!string.IsNullOrWhiteSpace(idFilter))
                        {
                            if (schemaElement.Name != idFilter)
                            {
                                continue;
                            }
                        }

                        // base schema to be added to
                        var schema = new Schema
                        {
                            Id = entitySet.Name,
                            Name = schemaElement.Name,
                            Description = "",
                            DataFlowDirection = Schema.Types.DataFlowDirection.Read
                        };

                        var properties = new List<Property>();
                        foreach (IEdmProperty property in structuredType.DeclaredProperties)
                        {
                            var id = property.Name ?? $"UNKNOWN";
                            var type = property.Type.FullName() ?? "";
                            var isKey = entityType.DeclaredKey.Contains(property);

                            properties.Add(new Property
                            {
                                Id = id,
                                Name = id,
                                Description = "",
                                Type = GetPropertyType(type),
                                TypeAtSource = type,
                                IsKey = isKey,
                                IsNullable = !isKey,
                                IsCreateCounter = false,
                                IsUpdateCounter = false,
                            });
                        }

                        schema.Properties.Clear();
                        schema.Properties.AddRange(properties);

                        // get sample and count
                        yield return await AddSampleAndCount(apiClient, schema, sampleSize);
                    }
                }
                // if (item is IEdmEntitySet)
                // {
                //     var entitySet = (IEdmEntitySet)item;
                //     var schemaElement = (IEdmSchemaElement)entitySet.ElementType;
                //     var structuredType = (IEdmStructuredType) schemaElement;
                //     var entityType = (IEdmEntityType) schemaElement;
                //     // IEdmStructuredType structuredType = (IEdmStructuredType)schemaElement;
                //
                //     if (structuredType.BaseType == null && structuredType.TypeKind == EdmTypeKind.Entity)
                //     {
                //         // filter to only target schema if filter specified
                //         if (!string.IsNullOrWhiteSpace(idFilter))
                //         {
                //             if (schemaElement.Name != idFilter)
                //             {
                //                 continue;
                //             }
                //         }
                //         
                //         // IEdmEntityType entityType = (IEdmEntityType) schemaElement;
                //         
                //         // base schema to be added to
                //         var schema = new Schema
                //         {
                //             Id = schemaElement.Name,
                //             Name = schemaElement.Name,
                //             Description = "",
                //             DataFlowDirection = Schema.Types.DataFlowDirection.Read
                //         };
                //         
                //         var properties = new List<Property>();
                //         foreach (IEdmProperty property in structuredType.DeclaredProperties)
                //         {
                //             var id = property.Name ?? $"UNKNOWN";
                //             var type = property.Type.FullName() ?? "";
                //             var isKey = entityType.DeclaredKey.Contains(property);
                //             
                //             properties.Add(new Property
                //             {
                //                 Id = id,
                //                 Name = id,
                //                 Description = "",
                //                 Type = GetPropertyType(type),
                //                 TypeAtSource = type,
                //                 IsKey = isKey,
                //                 IsNullable = !isKey,
                //                 IsCreateCounter = false,
                //                 IsUpdateCounter = false,
                //             });
                //         }
                //         
                //         schema.Properties.Clear();
                //         schema.Properties.AddRange(properties);
                //         
                //         // get sample and count
                //         yield return await AddSampleAndCount(apiClient, schema, sampleSize);
                //     }
                // }
            }
        }
    }
}
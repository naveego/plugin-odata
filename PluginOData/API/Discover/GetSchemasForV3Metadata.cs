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
        public static async IAsyncEnumerable<Schema> GetSchemasForV3Metadata(IApiClient apiClient, V3EdmModelBase metadata, int sampleSize = 5, string idFilter = "")
        {
            foreach (IEdmSchemaElement schemaElement in metadata.SchemaElements)
            {
                if (schemaElement is IEdmStructuredType)
                {
                    IEdmStructuredType structuredType = (IEdmStructuredType)schemaElement;

                    if (structuredType.BaseType == null && structuredType.TypeKind == EdmTypeKind.Entity)
                    {
                        // filter to only target schema if filter specified
                        if (!string.IsNullOrWhiteSpace(idFilter))
                        {
                            if (schemaElement.Name != idFilter)
                            {
                                continue;
                            }
                        }
                        
                        IEdmEntityType entityType = (IEdmEntityType) schemaElement;
                        
                        // base schema to be added to
                        var schema = new Schema
                        {
                            Id = schemaElement.Name,
                            Name = schemaElement.Name,
                            Description = "",
                            DataFlowDirection = Schema.Types.DataFlowDirection.Read
                        };
                        
                        var properties = new List<Property>();
                        foreach (IEdmProperty property in structuredType.DeclaredProperties)
                        {
                            properties.Add(new Property
                            {
                                Id = property.Name,
                                Name = property.Name,
                                Description = "",
                                Type = GetPropertyType(property.Type.FullName()),
                                TypeAtSource = property.Type.FullName(),
                                IsKey = entityType.DeclaredKey.Contains(property),
                                IsNullable = !entityType.DeclaredKey.Contains(property),
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
            }
        }
    }
}
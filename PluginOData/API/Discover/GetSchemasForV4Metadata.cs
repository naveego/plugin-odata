using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OData.Edm;
using Naveego.Sdk.Plugins;
using PluginOData.API.Factory;
using V4EdmModelBase = Microsoft.OData.Edm.EdmModelBase;

namespace PluginOData.API.Discover
{
    public static partial class Discover
    {
        public static async IAsyncEnumerable<Schema> GetSchemasForV4Metadata(IApiClient apiClient,
            V4EdmModelBase metadata, int sampleSize = 5, string idFilter = "")
        {
            foreach (var item in metadata.EntityContainer.Elements.Where(e =>
                e.ContainerElementKind == EdmContainerElementKind.EntitySet))
            {
                var entitySet = (IEdmEntitySet) item;
                var schemaElement = (IEdmSchemaElement) entitySet.Type.AsElementType();
                var structuredType = (IEdmStructuredType) schemaElement;
                var entityType = (IEdmEntityType) schemaElement;

                // filter to only target schema if filter specified
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
    }
}
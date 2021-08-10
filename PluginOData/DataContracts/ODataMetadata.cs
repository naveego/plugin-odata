using V3EdmModelBase = Microsoft.Data.Edm.Library.EdmModelBase;
using V4EdmModelBase = Microsoft.OData.Edm.EdmModelBase;

namespace PluginOData.DataContracts
{
    public class ODataMetadata
    {
        private readonly V3EdmModelBase? _v3Model;
        private readonly V4EdmModelBase? _v4Model;
        
        public ODataMetadata(V3EdmModelBase v3Model, V4EdmModelBase v4Model)
        {
            _v3Model = v3Model;
            _v4Model = v4Model;
        }

        public V3EdmModelBase? GetV3Model()
        {
            return _v3Model;
        }
        
        public V4EdmModelBase? GetV4Model()
        {
            return _v4Model;
        }
    }
}
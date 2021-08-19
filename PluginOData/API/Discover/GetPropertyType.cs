using System;
using System.ComponentModel.DataAnnotations;
using Naveego.Sdk.Logging;
using Naveego.Sdk.Plugins;

namespace PluginOData.API.Discover
{
    public static partial class Discover
    {
        public static PropertyType GetPropertyType(string dataType)
        {
            // return PropertyType.String;
            try
            {
                switch (dataType)
                {
                    case "Edm.DateTime":
                        return PropertyType.Datetime;
                    case "Edm.Date":
                        return PropertyType.Date;
                    case "Edm.Int32":
                    case "Edm.Int64":
                        return PropertyType.Integer;
                    case "Edm.Decimal":
                        return PropertyType.Decimal;
                    case "Edm.Float32":
                    case "Edm.Double":
                        return PropertyType.Float;
                    case "Edm.Boolean":
                        return PropertyType.Bool;
                    case "Edm.String":
                        return PropertyType.String;
                    default:
                        return PropertyType.Json;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, e.Message);
                throw;
            }
        }
    }
}
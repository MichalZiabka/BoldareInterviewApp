using BoldareApp.Dto;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace BoldareApp.Infrastructure
{
    public class ODataModel
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<BreweryDto>("breweries");
            return builder.GetEdmModel();
        }
    }
}

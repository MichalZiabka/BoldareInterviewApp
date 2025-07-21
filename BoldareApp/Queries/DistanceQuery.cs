using System.Globalization;

namespace BoldareApp.Queries
{
    public class DistanceQuery : PaginatedQueryBase
    {
        public required double Latitude { get; init; }
        public required double Longitude { get; init; }

        protected override string Path => "?";

        protected override void AddParameters(List<string> parameters)
        {
            parameters.Add($"by_dist={Latitude.ToString(CultureInfo.InvariantCulture)},{Longitude.ToString(CultureInfo.InvariantCulture)}");
        }
    }
}

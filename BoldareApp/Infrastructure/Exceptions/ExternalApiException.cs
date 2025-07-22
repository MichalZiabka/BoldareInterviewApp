namespace BoldareApp.Infrastructure.Exceptions
{
    public class ExternalApiException : Exception
    {
        public ExternalApiException()
            : base("Can not parse data") { }
    }
}

namespace BoldareApp.Infrastructure.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
            : base("Invalid username or password") { }
    }
}

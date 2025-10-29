namespace Domain.Entities
{
    public sealed class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = $"Invalid Email Or Password") 
            : base(message)
        {
            
        }
    }
}

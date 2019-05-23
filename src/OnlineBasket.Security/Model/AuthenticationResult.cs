namespace OnlineBasket.Security.Model
{
    /// <summary>
    /// Wrapper class to possibilitate identification of failure or success
    /// </summary>
    public class AuthenticationResult
    {
        public bool Authenticated { get; set; }

        public AuthenticatedToken Token { get; set; }
    }
}
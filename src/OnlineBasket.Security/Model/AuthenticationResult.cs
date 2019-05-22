namespace OnlineBasket.Security.Model
{
    public class AuthenticationResult
    {
        public bool Authenticated { get; set; }

        public AuthenticatedToken Token { get; set; }
    }
}
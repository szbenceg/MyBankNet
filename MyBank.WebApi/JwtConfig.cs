namespace MyBank.WebApi
{
    public class JwtConfig
    {
        public string JwtKey { get; set; } = null!;

        public string JwtIssuer { get; set; } = null!;

        public int JwtExpireMinutes { get; set; }
    }
}

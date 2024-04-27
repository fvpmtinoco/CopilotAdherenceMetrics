namespace CopilotAdherence.Settings
{
    public record Jwt
    {
        public string Key { get; init; }
        public string Issuer { get; init; }
        public string Audience { get; init; }
    }
}

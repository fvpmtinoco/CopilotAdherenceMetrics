namespace CopilotAdherence.Settings
{
    public record GitHubSettings
    {
        public string PAT { get; init; }
        public string OrganizationName { get; init; }
    }
}

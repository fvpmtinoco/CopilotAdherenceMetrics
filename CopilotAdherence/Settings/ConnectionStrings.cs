namespace CopilotAdherence.Settings
{
    public record ConnectionStrings
    {
        public string MongoDbConnection { get; init; }
        public string DataBaseName { get; init; }
        public string RedisConnection { get; init; }
    }
}

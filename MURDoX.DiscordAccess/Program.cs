namespace MURDoX.DiscordAccess
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Startup startup = new();
            await startup.RunAsync();
        }
    }
}
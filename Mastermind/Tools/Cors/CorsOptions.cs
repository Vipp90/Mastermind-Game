namespace Mastermind.Tools.Cors
{
    public sealed class CorsOptions
    {
        public bool IsEnabled { get; set; }
        public IEnumerable<string> Origins { get; set; }
    }
}

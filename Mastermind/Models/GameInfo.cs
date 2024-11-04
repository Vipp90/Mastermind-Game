namespace Mastermind.Models
{
    public class GameInfo
    {
        public string PlayerName { get; set; }
        public Code? Code { get; set; }
        public Mode GameMode { get; set; }
    }
}

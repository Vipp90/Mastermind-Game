using System.Security.Cryptography.X509Certificates;

namespace Mastermind.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public string PlayerName { get; set; }
        public Code Code { get; set; }
        public Mode GameMode { get; set; }
        public TimeOnly GameTime { get; set; }
    }
    public enum Mode
    {
        ManualSet,
        RandomSet
    }
}

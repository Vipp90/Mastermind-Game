using Mastermind.Models;

namespace Mastermind.ResponseModels
{
    public class CheckCodeResponse
    {
        public bool Guessed { get; set; }
        public Hint? Hint { get; set; }
        public bool? SaveGame { get; set; }
        public Code? HiddenCode { get; set; }
    }

    public class Hint
    {
        public int CorrectPlace { get; set; }
        public int WrongPlace { get; set; }
        public int NotOccur { get; set; }
    }
}

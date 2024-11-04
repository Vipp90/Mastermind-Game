namespace Mastermind.Models
{
    public class Code
    {
        public Colors FirstColor { get; set; }
        public Colors SecondColor { get; set; }
        public Colors ThirdColor { get; set; }
        public Colors FourthColor { get; set; }

        public enum Colors
        {
            Blue,
            Red,
            Green,
            Yellow,
            Brown,
            Orange
        }
    }
}

using System.Runtime.CompilerServices;

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
        public override bool Equals(object obj)
        {
            if (obj is Code other)
            {
                return FirstColor == other.FirstColor &&
                       SecondColor == other.SecondColor &&
                       ThirdColor == other.ThirdColor &&
                       FourthColor == other.FourthColor;
            }
            return false;
        }

        public bool IsEmpty()
        {
            return !Enum.IsDefined(typeof(Colors), FirstColor) ||
           !Enum.IsDefined(typeof(Colors), SecondColor) ||
           !Enum.IsDefined(typeof(Colors), ThirdColor) ||
           !Enum.IsDefined(typeof(Colors), FourthColor);
        }
    }
}

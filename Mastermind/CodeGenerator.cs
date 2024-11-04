using Mastermind.Models;
using static Mastermind.Models.Code;

namespace Mastermind
{
    public static class CodeGenerator
    {
        public static Code GenerateCode()
        {
            var random = new Random();
            var code = new Code();
            code.FirstColor = GetRandomColor(random);
            code.SecondColor = GetRandomColor(random);
            code.ThirdColor = GetRandomColor(random);
            code.FourthColor = GetRandomColor(random);
            return code;
        }

        private static Colors GetRandomColor(Random random)
        {
            Array values = Enum.GetValues(typeof(Colors));
            return (Colors)values.GetValue(random.Next(values.Length));
        }
    }
}

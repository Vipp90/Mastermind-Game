using Mastermind.Models;

namespace Mastermind.RequestModels
{
    record CheckCodeRequest(Code UserCode, int Chances);
}

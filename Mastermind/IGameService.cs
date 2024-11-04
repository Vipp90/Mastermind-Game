using Mastermind.Models;
using System.Runtime.InteropServices;

namespace Mastermind
{
    public interface IGameService
    {
        public Game CreateGame(GameInfo game);
    }
}

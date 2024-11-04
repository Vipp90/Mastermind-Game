using Mastermind.Models;
using Mastermind.ResponseModels;
using System.Runtime.InteropServices;

namespace Mastermind
{
    public interface IGameService
    {
        public Game? CreateGame(GameInfo game);
        public CheckCodeResponse? CheckCode(Guid gameId, Code userCode);
        public Game? GetGame(Guid gameId);
        public Task<ResponseModel> SaveScore(Guid gameId, int score);
    }
}

using Mastermind.Models;
using Mastermind.ResponseModels;
using System.Runtime.InteropServices;

namespace Mastermind
{
    public interface IGameService
    {
        public ServiceResult<string> CreateGame(GameInfo game);
        public ServiceResult<CheckCodeResponse> CheckCode(Guid gameId, Code userCode);
        public Game? GetGame(Guid gameId);
        public Task<ServiceResult<string>> SaveScore(Guid gameId, int score);
        public Task<ServiceResult<List<HighscoresBoard>>> GetHighscores();
    }
}

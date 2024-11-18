using Mastermind.Models;
using Mastermind.Repository;
using Mastermind.ResponseModels;
using System;
using System.CodeDom.Compiler;
using System.Drawing;
using static Mastermind.Models.Code;

namespace Mastermind
{
    public class GameService : IGameService
    {
        private readonly IRepository _gameRepository;
        private static Dictionary<Guid, Game> CurrentGames = new Dictionary<Guid, Game>();
        public GameService(IRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public ServiceResult<string> CreateGame(GameInfo gameInfo)
        {
            var code = gameInfo.GameMode.Equals(Mode.RandomSet) ? CodeGenerator.GenerateCode() : gameInfo.Code;
            if (!(code != null) || code.IsEmpty() == true) return new ServiceResult<string>(false, "Code is not set", null);
            if (gameInfo.GameMode == Mode.RandomSet && String.IsNullOrEmpty(gameInfo.PlayerName))
            {
                return new ServiceResult<string>(false, "Player name in random game cannot be empty", null);
            }
            var game = new Game();
            game.Id = Guid.NewGuid();
            game.PlayerName = gameInfo.PlayerName;
            game.GameMode = gameInfo.GameMode;
            game.Code = code;
            CurrentGames[game.Id] = game;
            return new ServiceResult<string>(true, "", game.Id.ToString());
        }

        public ServiceResult<CheckCodeResponse> CheckCode(Guid gameId, Code userCode)
        {
            var game = GetGame(gameId);
            if (game == null)
            {
                return new ServiceResult<CheckCodeResponse>(false, "Game not found or already finished", null);
            }
            var gameCode = game.Code;
            var response = new CheckCodeResponse();
            if (gameCode.Equals(userCode))
            {
                response.Guessed = true;
                return new ServiceResult<CheckCodeResponse>(true, "", response); ;
            }
            response.Hint = new Hint();
            List<Colors> correctColors = new List<Colors> { gameCode.FirstColor, gameCode.SecondColor, gameCode.ThirdColor, gameCode.FourthColor };
            List<Colors> userColors = new List<Colors> { userCode.FirstColor, userCode.SecondColor, userCode.ThirdColor, userCode.FourthColor };
            for (int i = correctColors.Count - 1; i >= 0; i--)
            {
                if (correctColors[i] == userColors[i])
                {
                    response.Hint.CorrectPlace += 1;
                    correctColors.Remove(correctColors[i]);
                    userColors.Remove(userColors[i]);
                }
            }

            for (int j = userColors.Count - 1; j >= 0; j--)
            {
                var index = correctColors.IndexOf(userColors[j]);
                if (index != -1)
                {
                    response.Hint.WrongPlace += 1;
                    correctColors.Remove(correctColors[index]);
                }
                else
                {
                    response.Hint.NotOccur += 1;
                }
            }
            response.Guessed = false;
            return new ServiceResult<CheckCodeResponse>(true, "", response);
        }

        public Game? GetGame(Guid gameId)
        {
            if (!CurrentGames.TryGetValue(gameId, out var game))
            {
                return null;
            }
            return game;
        }

        public async Task<ServiceResult<string>> SaveScore(Guid gameId, int score)
        {
            var result = true;
            var game = GetGame(gameId);
            if (game == null)
            {
                return new ServiceResult<string>(false, "Game not found or already finished", null);
            }
            if (game.GameMode.Equals(Mode.ManualSet)) return new ServiceResult<string>(true, "", null);

            var playerHighscore = await _gameRepository.PlayerHighscore(game.PlayerName);
            if (playerHighscore == 0)
            {
                result = await _gameRepository.AddScore(game.PlayerName, score);
            }

            if (score < playerHighscore)  // Score is a the number of seconds, the smaller is better 
            {
                result = await _gameRepository.UpdateScore(game.PlayerName, score);
            }
            return result ? new ServiceResult<string>(true, "", game.Id.ToString()) : new ServiceResult<string>(false, "An error occurred while adding a score.", null);
        }

        public async Task<ServiceResult<List<HighscoresBoard>>> GetHighscores()
        {
            var result = await _gameRepository.Highscores();
            return result is null ? new ServiceResult<List<HighscoresBoard>>(false, "An error occurred while retrieving the highscore board.", null) : new ServiceResult<List<HighscoresBoard>>(true, "", result);
        }
    }
}

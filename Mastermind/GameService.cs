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

        public Game CreateGame(GameInfo gameInfo)
        {
            var code = gameInfo.Equals(Mode.RandomSet) ? CodeGenerator.GenerateCode() : gameInfo.Code;
            if (code == null) return null;
            var game = new Game();
            game.Id = Guid.NewGuid();
            game.PlayerName = gameInfo.PlayerName;
            game.GameMode = gameInfo.GameMode;
            game.Code = code;
            CurrentGames[game.Id] = game;
            return game;
        }

        public CheckCodeResponse? CheckCode(Guid gameId, Code userCode)
        {
            var game = GetGame(gameId);
            if (game == null)
            {
                return null;
            }
            var gameCode = game.Code;
            var response = new CheckCodeResponse();
            if (gameCode.Equals(userCode))
            {
                response.Guessed = true;
                return response;
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
            return response;
        }

        public Game? GetGame(Guid gameId)
        {
            if (!CurrentGames.TryGetValue(gameId, out var game))
            {
                return null;
            }
            return game;
        }

        public async Task<ResponseModel> SaveScore(Guid gameId, int score)
        {
            var result = true;
            var game = GetGame(gameId);
            if (game == null)
            {
                return new ResponseModel(false, "Game not found or already finished");
            }
            if (game.GameMode.Equals(Mode.ManualSet)) return new ResponseModel(true, "");

            var playerHighscore = await _gameRepository.PlayerHighscore(game.PlayerName);
            if (playerHighscore == 0)
            {
                result = await _gameRepository.AddScore(game.PlayerName, score);
            }

            if (score < playerHighscore)  // Score is a the number of seconds, the smaller is better 
            {
                result = await _gameRepository.UpdateScore(game.PlayerName, score);
            }
            return result ? new ResponseModel(true, "") : new ResponseModel(false, "An error occurred while adding a score.");
        }
    }
}

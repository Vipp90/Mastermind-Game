using Mastermind.Models;
using System;
using System.CodeDom.Compiler;
using System.Drawing;
using static Mastermind.Models.Code;

namespace Mastermind
{
    public class GameService : IGameService
    {
        public Game CreateGame(GameInfo gameInfo)
        {
            var game = new Game();
            game.Id = Guid.NewGuid();
            game.PlayerName = gameInfo.PlayerName;
            game.GameMode = gameInfo.GameMode;
            game.Code = CodeGenerator.GenerateCode();
            return game;
        }

        public ResponseModel CheckCode(Code gameCode, Code userCode)
        {
            var response = new ResponseModel();
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
    }
}

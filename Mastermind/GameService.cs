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
    }
}

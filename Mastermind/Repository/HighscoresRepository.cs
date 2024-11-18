using Mastermind.Models;
using Mastermind.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace Mastermind.Repository
{
    public class HighscoresRepository : IRepository
    {
        private readonly Database _context;
        private readonly ILogger<HighscoresRepository> _logger;
        public HighscoresRepository(Database context, ILogger<HighscoresRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> PlayerHighscore(string playerName)
        {
            var result = await _context.HighScores.SingleOrDefaultAsync(x => x.PlayerName == playerName);
            return result is null ? 0 : result.Score;
        }

        public async Task<bool> AddScore(string playerName, int score)
        {
            var highscore = new Highscores();
            highscore.PlayerName = playerName;
            highscore.Score = score;
            try
            {
                await _context.HighScores.AddAsync(highscore);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding a score.");
                return false;
            }

        }

        public async Task<bool> UpdateScore(string playerName, int score)
        {
            var playerHighscore = await _context.HighScores.SingleAsync(x => x.PlayerName == playerName);
            playerHighscore.Score = score;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating a score.");
                return false;
            }
            return true;
        }

        public async Task<List<HighscoresBoard>> Highscores()
        {
            var i = 0;
            var highScoresBoard = new List<HighscoresBoard>();
            try
            {
                var result = await _context.HighScores.OrderBy(x => x.Score).ToListAsync();
                if (result.Any())
                {
                    foreach (var highscore in result)
                    {
                        i++;
                        highScoresBoard.Add(new HighscoresBoard { PlayerName = highscore.PlayerName, Score = ConvertTimeToString(highscore.Score) });
                    }
                }
                return highScoresBoard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the highscore board");
                return null;
            }

        }

        private string ConvertTimeToString(int miliseconds)
        {
            TimeSpan time = TimeSpan.FromMilliseconds(miliseconds);
            string result = string.Format("{0:D2}m:{1:D2}s:{2:D2}mili",
                            time.Minutes,
                            time.Seconds,
                            time.Milliseconds);
            return result;
        }
    }
}

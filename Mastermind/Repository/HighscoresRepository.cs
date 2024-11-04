using Mastermind.Models;
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
    }
}

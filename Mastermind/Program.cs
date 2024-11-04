using Mastermind;
using Mastermind.Models;
using Mastermind.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IRepository, HighscoresRepository>();
builder.Services.AddDbContext<Database>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

app.MapPost("/game", (GameInfo gameInfo, IGameService _gameService) =>
{
    var game = _gameService.CreateGame(gameInfo);
    if (game != null) return Results.Ok(game.Id);
    return Results.BadRequest("No code provided");
});

app.MapPost("/game/{id}/guess", (Guid id, Code userCode, IGameService _gameService) =>
{
    var result = _gameService.CheckCode(id, userCode);
    if (result == null)
    {
        return Results.NotFound("Game not found or already finished");
    }
    return Results.Ok(result);
});

app.MapPost("/game/score", async (Guid gameId, int score, IGameService _gameService) =>
{
    var result = await _gameService.SaveScore(gameId, score);
    return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Run();

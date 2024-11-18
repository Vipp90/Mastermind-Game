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
    var result = _gameService.CreateGame(gameInfo);
    return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
});

app.MapPost("/game/{id}/guess", (Guid id, Code userCode, IGameService _gameService) =>
{
    var result = _gameService.CheckCode(id, userCode);
    return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
});

app.MapPost("/game/score", async (Guid gameId, int score, IGameService _gameService) =>
{
    var result = await _gameService.SaveScore(gameId, score);
    return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
});

app.MapGet("/game/score", async (IGameService _gameService) =>
{
    var result = await _gameService.GetHighscores();
    return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

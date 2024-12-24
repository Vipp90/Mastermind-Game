using Mastermind;
using Mastermind.Models;
using Mastermind.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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

app.MapPost("/game/{gameId}/guess", (Guid gameId, Code userCode, IGameService _gameService) =>
{
    var result = _gameService.CheckCode(gameId, userCode);
    return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
});

app.MapPost("/game/{gameId}/score", async (Guid gameId, [FromBody] int score, IGameService _gameService) =>
{
    var result = await _gameService.SaveScore(gameId, score);
    return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
});

app.MapGet("/game/score", async (IGameService _gameService) =>
{
    var result = await _gameService.GetHighscores();
    return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
});

app.MapDelete("/game/{gameId}", (Guid gameId, IGameService _gameService) =>
{
    _gameService.DeleteGame(gameId);
    return Results.Ok();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

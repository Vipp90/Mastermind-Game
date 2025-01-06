using Mastermind;
using Mastermind.Models;
using Mastermind.Repository;
using Mastermind.Tools.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Mastermind.RequestModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(builder.Configuration);
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IRepository, HighscoresRepository>();
builder.Services.AddDbContext<Database>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();
app.UseCors(builder.Configuration);
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<Database>();
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.MapPost("/game", (GameInfo gameInfo, IGameService _gameService) =>
{
    var result = _gameService.CreateGame(gameInfo);
    return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
});

app.MapPost("/game/{gameId}/guess", (Guid gameId, CheckCodeRequest request, IGameService _gameService) =>
{
    var result = _gameService.CheckCode(gameId, request.UserCode, request.lastScore);
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
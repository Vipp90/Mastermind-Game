using Mastermind;
using Mastermind.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGameService, GameService>();

var app = builder.Build();

var currentGames = new Dictionary<Guid, Game>();

app.MapPost("/game", (GameInfo gameInfo, IGameService _gameService) =>
{
    var game = _gameService.CreateGame(gameInfo);
    currentGames[game.Id] = game;
    return Results.Ok(game.Id);
});

app.MapPost("/game/{id}/guess", (Guid id, Code userCode, IGameService _gameService) =>
{
    if (!currentGames.TryGetValue(id, out var game))
    {
        return Results.NotFound("Game not found or already finished");
    }
    var result = _gameService.CheckCode(game.Code, userCode);
    return Results.Ok(result);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Run();

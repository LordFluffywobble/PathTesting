using Scalar.AspNetCore;
using Testing.PathFinder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSingleton<PathFinders>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("With Pathfinder API documentation")
            .WithTheme(ScalarTheme.DeepSpace);
    });
}

app.UseHttpsRedirection();

app.MapPost("/api/path/add", (Coordinates coord, PathFinders pathFinder) =>
{
    pathFinder.AddCoordinates(coord.X, coord.Y, coord.Z);

    return Results.Ok(new {
        Message = "Coordinates added", 
        CurrentHistory = pathFinder.PathHistory
    });
});

app.MapGet("/api/path/history", (PathFinders pathFinder) =>
{
    return Results.Ok(pathFinder.PathHistory);
});


app.MapPut("/api/path/update", (Coordinates coord, PathFinders pathFinder) =>
{
    if (pathFinder.PathHistory.Count == 0)
    {
        return Results.BadRequest("No history");
    }
    pathFinder.UpdateCoordinates(coord.X, coord.Y, coord.Z);
    
    return Results.Ok(new { 
        Message = "Coordinates updated",
        CurrentHistory = pathFinder.PathHistory
    });    
});

app.MapGet("/api/path/{id}", (PathFinders pathFinder, int id) =>
{
    if (id < 0 || id >= pathFinder.PathHistory.Count)
    {
        return Results.NotFound("No such Id");
    }   
    return Results.Ok(pathFinder.PathHistory[id]);
    
});

app.Run();


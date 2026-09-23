using Microsoft.AspNetCore.Authorization.Infrastructure;
using Scalar.AspNetCore;
using Testing.PathFinder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSingleton<PathFinders>();

var app = builder.Build();


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

app.MapGet("/api/path/history", (PathFinders pathFinder) => {
    return Results.Ok(pathFinder.PathHistory);
});


app.MapPut("/api/path/update", (Coordinates coord, PathFinders pathFinder, int id) =>
{
    if (pathFinder.PathHistory.Count == 0){
        return Results.BadRequest("No history");
    }
    
    pathFinder.UpdateCoordinates(coord.X, coord.Y, coord.Z, id);
    
    return Results.Ok(new { 
        Message = "Coordinates updated",
        CurrentHistory = pathFinder.PathHistory
    });    
});

app.MapGet("/api/path/{id}", (PathFinders pathFinder, int id) =>
{
    if (id < 0 || id >= pathFinder.PathHistory.Count){
        return Results.NotFound("No such Id");
    }   
    
    return Results.Ok(pathFinder.PathHistory[id]);
    
});

app.MapGet("/path/api/shortestpath", (PathFinders pathFinder, int id1, int id2) => 
{
    
    if (pathFinder.PathHistory.Count < 2){
        return Results.BadRequest("Error: To few coordinates");
    }
    if (id1 < 0 || id1 > pathFinder.PathHistory.Count){
        return Results.BadRequest("Error: No such ID exists for path 1");
    }
    if (id2 < 0 || id2 > pathFinder.PathHistory.Count){
        return Results.BadRequest("Error: No such ID exists for path 2");
    }

    var path1 = pathFinder.PathHistory[id1];
    var path2 = pathFinder.PathHistory[id2];
    var sPath = pathFinder.ShortestPath(id1, id2);

    return Results.Ok(new {ShortesPathIs = sPath});

});

app.MapDelete("/path/api/delete/{id}", (PathFinders pathFinder, int id) =>
{
    if (id < 0 || id >= pathFinder.PathHistory.Count){
        return Results.NotFound($"Path with ID {id} does not exists");
    }
    
    var deletedPath = pathFinder.PathHistory[id];
    pathFinder.DeleteTaskId(id);
    
    return Results.Ok(new {
        message = $"Removed path with ID {id}",
        deletedData = deletedPath  
    });
});

app.Run();


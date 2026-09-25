using Aspire.Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Scalar.AspNetCore;
using Testing.PathFinder;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var connectionString = builder.Configuration.GetConnectionString("PathFinderDb");

builder.AddNpgsqlDbContext<PathFinderDbContext>("PathFinderDb");

builder.Services.AddOpenApi();

builder.Services.AddSingleton<PathFinders>();

var app = builder.Build();

app.MapDefaultEndpoints();

//if (app.Environment.IsDevelopment())
//{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("With Pathfinder API documentation")
            .WithTheme(ScalarTheme.DeepSpace);
    });
//}

//app.UseHttpsRedirection();

app.MapPost("/api/path/add", async (Coordinates coord, PathFinderDbContext db) =>{
    var newCoord = new Coordinates(coord.X, coord.Y, coord.Z){
        Id = 0 // Postgres genererer en ny automatisk
    };

    db.Coordinates.Add(newCoord);
    await db.SaveChangesAsync();

    return Results.Ok(new {
        Message = "Coordinates added to database", 
        Data = newCoord
    });
});

app.MapGet("/api/path/history", async (PathFinderDbContext db) => {
    var history = await db.Coordinates.ToListAsync();
    return Results.Ok(history);
});


app.MapPut("/api/path/update/{id}", async (Coordinates coord, PathFinderDbContext db, int id) =>{   
    var foundCoord = await db.Coordinates.FindAsync(id);
    
    if (foundCoord == null){
        return Results.NotFound($"No such coordinates with ID {id}");      
    }

    var updatedCoord = foundCoord with {X = coord.X, Y = coord.Y, Z = coord.Z};

    db.Entry(foundCoord).CurrentValues.SetValues(updatedCoord);
    await db.SaveChangesAsync();

    return Results.Ok(new { 
        Message = "Coordinates updated",
        Data = updatedCoord
    });    
});

app.MapGet("/api/path/{id}", async (PathFinderDbContext db, int id) =>{
    var identity = await db.Coordinates.FindAsync(id);

    if ( identity == null){
        return Results.NotFound(new {
            Message = $"Found no Coordinates with the ID {id}"});       
    }
    return Results.Ok(identity);
    
});

app.MapGet("/path/api/shortestpath", async (PathFinderDbContext db, int id1, int id2) =>{  
    var path1 = await db.Coordinates.FindAsync(id1);
    var path2 = await db.Coordinates.FindAsync(id2);
    
    double dx = path2.X - path1.X;
    double dy = path2.Y - path1.Y;
    double dz = path2.Z - path1.Z;

    var sPath = Math.Sqrt(dx*dx + dy*dy + dz*dz);

    return Results.Ok(new {ShortestPathIs = sPath});

});

app.MapPut("/path/api/incrementcoordinates/{id}", async (PathFinderDbContext db,Coordinates coord, int id) => {
    var baseCoord = await db.Coordinates.FindAsync(id);
    
    if ( baseCoord == null){
        return Results.BadRequest($"Error: No such ID exists");        
    }

    var newCoordinate = new Coordinates(
        baseCoord.X + coord.X,
        baseCoord.Y + coord.Y,
        baseCoord.Z + coord.Z
    )
    {
        Id = 0
    };

    db.Coordinates.Add(newCoordinate);
    await db.SaveChangesAsync();

    return Results.Ok(new {
        Message = $"Incremented the coordinates at ID {id} and created a new one",
        Path = newCoordinate
    });
});

app.MapDelete("/path/api/delete/{id}", async (PathFinderDbContext db, int id) =>{
    var coord = await db.Coordinates.FindAsync(id);
    if (coord == null){
        return Results.NotFound($"ID {id} does not exist");
    }
    db.Coordinates.Remove(coord);
    await db.SaveChangesAsync();
    
    return Results.Ok(new {
        Message = $"Removed path with ID {id}"
    });
});


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PathFinderDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}

app.Run();


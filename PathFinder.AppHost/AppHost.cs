var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
                      .WithPgAdmin();

var pathFinderDb = postgres.AddDatabase("PathFinderDb");


builder.AddProject<Projects.PathFinder_Api>("pathfinder-api")
        .WithReference(pathFinderDb);

builder.Build().Run();

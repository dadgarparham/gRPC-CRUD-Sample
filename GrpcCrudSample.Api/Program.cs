using GrpcCrudSample.Api.Repositories;
using GrpcCrudSample.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddSingleton<IPersonRepository, InMemoryPersonRepository>();

var app = builder.Build();

app.MapGrpcService<PersonGrpcService>();
app.MapGet("/", () => "This service uses gRPC. Use a gRPC client to communicate.");

app.Run();

using GrpcCrudSample.Api.Repositories;
using GrpcCrudSample.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddSingleton<IPersonRepository, InMemoryPersonRepository>();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger().UseSwaggerUI(c => c.DocumentTitle = "Parham");
app.UseHsts();
app.UseHttpsRedirection();

app.MapGrpcService<PersonGrpcService>();
app.MapGet("/", () => "This service uses gRPC. Use a gRPC client to communicate.");

app.Run();

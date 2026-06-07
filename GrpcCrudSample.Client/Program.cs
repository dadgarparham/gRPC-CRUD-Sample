using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcCrudSample.Api;

Console.WriteLine("gRPC Client Started...");

using var channel = GrpcChannel.ForAddress("https://localhost:7243");
var client = new PersonService.PersonServiceClient(channel);

try
{
    var allBefore = await client.GetAllAsync(new Empty());
    Console.WriteLine("Initial persons:");
    foreach (var person in allBefore.Persons)
    {
        PrintPerson(person);
    }

    var created = await client.CreateAsync(new CreatePersonRequest
    {
        Name = "Parham",
        LastName = "Dadgar",
        PersonalCode = "4133103955",
        BirthDay = Timestamp.FromDateTime(new DateTime(1986, 10, 23, 0, 0, 0, DateTimeKind.Utc))
    });

    Console.WriteLine("Created person:");
    PrintPerson(created);

    var fetched = await client.GetByIdAsync(new GetPersonByIdRequest { Id = created.Id });
    Console.WriteLine("Fetched by id:");
    PrintPerson(fetched);

    var updated = await client.UpdateAsync(new UpdatePersonRequest
    {
        Id = created.Id,
        Name = "Parham",
        LastName = "Dadgar Updated",
        PersonalCode = "4133103955",
        BirthDay = Timestamp.FromDateTime(new DateTime(1986, 10, 23, 0, 0, 0, DateTimeKind.Utc))
    });

    Console.WriteLine("Updated person: ");

    PrintPerson(updated);

    var deleteResult = await client.DeleteAsync(new DeletePersonRequest { Id = created.Id });
    Console.WriteLine($"Delete result: {deleteResult.Success} - {deleteResult.Message}");

    var allAfter = await client.GetAllAsync(new Empty());
    Console.WriteLine("Final persons: ");
    foreach (var person in allAfter.Persons)
    {
        PrintPerson(person);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.WriteLine("gRPC Client is Finished!");
Console.ReadKey();

static void PrintPerson(PersonResponse person)
{
    Console.WriteLine($"Id: {person.Id}, Name: {person.Name}, LastName: {person.LastName}, PersonalCode: {person.PersonalCode}, birthDay: {person.BirthDay.ToDateTime():yyyy-MM-dd}");
}

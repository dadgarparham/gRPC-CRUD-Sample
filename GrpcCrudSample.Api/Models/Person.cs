namespace GrpcCrudSample.Api.Models;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PersonalCode { get; set; } = string.Empty;
    public DateTime BirthDay { get; set; }
}

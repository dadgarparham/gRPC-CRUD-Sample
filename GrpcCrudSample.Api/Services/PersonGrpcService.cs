using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcCrudSample.Api.Models;
using GrpcCrudSample.Api.Repositories;

namespace GrpcCrudSample.Api.Services;

public class PersonGrpcService : PersonService.PersonServiceBase
{
    private readonly IPersonRepository _repository;

    public PersonGrpcService(IPersonRepository repository)
    {
        _repository = repository;
    }

    public override async Task<PersonListResponse> GetAll(Empty request, ServerCallContext context)
    {
        var persons = await _repository.GetAllAsync();
        var response = new PersonListResponse();
        response.Persons.AddRange(persons.Select(MapToMessage));
        return response;
    }

    public override async Task<PersonResponse> GetById(GetPersonByIdRequest request, ServerCallContext context)
    {
        var person = await _repository.GetByIdAsync(request.Id);
        if (person is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Person with id '{request.Id}' was not found."));
        }

        return MapToMessage(person);
    }

    public override async Task<PersonResponse> Create(CreatePersonRequest request, ServerCallContext context)
    {
        ValidateCreateOrUpdateRequest(request.Name, request.LastName, request.PersonalCode, request.BirthDay);

        if (await _repository.ExistsByPersonalCodeAsync(request.PersonalCode))
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "A person with this personal code already exists."));
        }

        var entity = new Person
        {
            Name = request.Name,
            LastName = request.LastName,
            PersonalCode = request.PersonalCode,
            BirthDay = request.BirthDay.ToDateTime()
        };

        var created = await _repository.CreateAsync(entity);
        return MapToMessage(created);
    }

    public override async Task<PersonResponse> Update(UpdatePersonRequest request, ServerCallContext context)
    {
        if (request.Id <= 0)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Id must be greater than zero."));
        }

        ValidateCreateOrUpdateRequest(request.Name, request.LastName, request.PersonalCode, request.BirthDay);

        if (await _repository.ExistsByPersonalCodeAsync(request.PersonalCode, request.Id))
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "A person with this personal code already exists."));
        }

        var entity = new Person
        {
            Id = request.Id,
            Name = request.Name,
            LastName = request.LastName,
            PersonalCode = request.PersonalCode,
            BirthDay = request.BirthDay.ToDateTime()
        };

        var updated = await _repository.UpdateAsync(entity);
        if (updated is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Person with id '{request.Id}' was not found."));
        }

        return MapToMessage(updated);
    }

    public override async Task<DeletePersonResponse> Delete(DeletePersonRequest request, ServerCallContext context)
    {
        if (request.Id <= 0)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Id must be greater than zero."));
        }

        var deleted = await _repository.DeleteAsync(request.Id);
        if (!deleted)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Person with id '{request.Id}' was not found."));
        }

        return new DeletePersonResponse
        {
            Success = true,
            Message = "Person deleted successfully."
        };
    }

    private static void ValidateCreateOrUpdateRequest(string name, string lastName, string personalCode, Timestamp? birthDay)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Name is required."));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "LastName is required."));

        if (string.IsNullOrWhiteSpace(personalCode))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "PersonalCode is required."));

        if (birthDay is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "birthDay is required."));

        var BirthDay = birthDay.ToDateTime();
        if (BirthDay > DateTime.UtcNow)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "birthDay cannot be in the future."));
    }

    private static PersonResponse MapToMessage(Person person)
    {
        return new PersonResponse
        {
            Id = person.Id,
            Name = person.Name,
            LastName = person.LastName,
            PersonalCode = person.PersonalCode,
            BirthDay = Timestamp.FromDateTime(DateTime.SpecifyKind(person.BirthDay, DateTimeKind.Utc))
        };
    }
}

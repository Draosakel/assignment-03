using Assignment3.Core;
using System.Collections.ObjectModel;
using Assignment3;

namespace Assignment3.Entities;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new List<User>();
    public IReadOnlyCollection<User> Users => _users;

    public (Response Response, int UserId) Create(UserCreateDTO user)
    {
        User u = new()
        {
            Name = user.Name,
            Email = user.Email
        };

        var uniqueEmail = EmailIsUnique(user.Email, _users);
        if (!uniqueEmail.Item1) return (Response.Conflict, uniqueEmail.Item2);

        //Id serialization
        int id = 1;
        id = _users.OrderByDescending(a => a.Id).Select(a => a.Id).First() + 1;
        u.Id = id;

        //Users list?
        _users.Add(u);

        return (Response.Created, u.Id);
    }

    public Response Delete(int userId, bool force = false)
    {
        bool exists = _users.Where(a => a.Id == userId).ToList().Count > 0;
        if (exists) _users.Remove(_users.Where(a => a.Id == userId).First());
        return (exists ? Response.Deleted : Response.NotFound);
    }

    public UserDTO Read(int userId)
    {
        var user = _users.Where(a => a.Id == userId).FirstOrDefault();
        if (user == null) return null!;
        return new UserDTO(user.Id, user.Name, user.Email);
    }

    public IReadOnlyCollection<UserDTO> ReadAll()
    {
        var list = new List<UserDTO>();
        foreach (var user in _users)
        {
            list.Add(new UserDTO(user.Id, user.Name, user.Email));
        }
        return new ReadOnlyCollection<UserDTO>(list);
    }

    public Response Update(UserUpdateDTO user)
    {
        var u = _users.Where(a => a.Id == user.Id).First();
        if (u == null) return Response.NotFound;

        var uniqueEmail = EmailIsUnique(user.Email, _users);
        if (!uniqueEmail.Item1) return (Response.Conflict);

        u = new User()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };

        return Response.Updated;
    }

    private (bool, int) EmailIsUnique(string email, IEnumerable<User> users)
    {
        var isUnique = true;
        var takenId = -1;
        foreach (var user in users)
        {
            if (user.Email == email)
            {
                isUnique = false;
                takenId = user.Id;
                break;
            }
        }
        return (isUnique, takenId);
    }
}

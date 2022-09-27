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
        var u = _users.FirstOrDefault(a => a.Id == userId);
        bool exists = u != null;
        bool canBeDeleted = true;
        if (u.Tasks.Count > 0 && force == false) canBeDeleted = false;
        if (exists)
        {
            if (canBeDeleted) _users.Remove(_users.Where(a => a.Id == userId).First());
            else return Response.Conflict;
        }
        return (exists ? Response.Deleted : Response.NotFound);
    }

    public UserDTO Read(int userId)
    {
        var user = _users.FirstOrDefault(a => a.Id == userId);
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

        u.Name = user.Name;
        u.Email = user.Email;

        return Response.Updated;
    }
}

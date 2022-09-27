// See https://aka.ms/new-console-template for more information
using Assignment3.Entities;

Console.WriteLine("Hello, World!");

var factory = new KanbanContextFactory();
using var context = factory.CreateDbContext(args);

var user = new User
{
    Name = "Name",
    Email = "Email@Gmail.com",
    Id = 1
};

context.Users.Add(user);
context.SaveChanges();
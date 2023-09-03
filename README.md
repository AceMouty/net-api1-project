# .NET Server Side Routing and Data Repositories

## Introduction

Use `.NET` to build an API that performs _CRUD_ operations on `blog posts`.
Completion of this exercise shows demonstration and understanding of...

- Repository Pattern
- Use of .NET Dependency Injection
- Library registration using `IServiceCollection`
- Extension Methods
- Modeling Requests, Responses and Domain level data
- Controller based routing
- Mapping pattern converting between domain and external representation of data

## Prerequisites
.NET 7 SDK

## Instructions

### Project Setup

Create a new solution in the project root using `dotnet new sln -n <solution_name>` replacing `<solution_name>`
with the name of the project like `PostsService`.

Create two new projects
- `Posts.Api`: Web API Project, this is where Controllers, Mapping Extension methods and API Contracts will live.
- `Posts.Application`: Class Library, this is where our Models and Repositories will live along with Extension Methods
  for project registration.

Create a project reference where `Posts.Api` refers to `Posts.Application`

#### Posts.Application Nuget Packages

- Microsoft.Extensions.DependencyInjection.Abstractions: required for being able to register the `Posts.Application` properly

### Task 1: Minimum Viable Product

- Add the code necessary to `Posts.Api` and `Posts.Application` to implement the endpoints listed below.

- Configure the API to handle to the following routes. Some of these endpoints might require more than one call to the 
  Post repository in `Post.Application/Repositories/Post.cs`.
  

| N   | Method | Endpoint                | Description                                                                                                                     |
| --- | ------ | ----------------------- | ------------------------------------------------------------------------------------------------------------------------------- |
| 1   | GET    | /api/posts              | Returns **an array of all the post objects** contained in the database                                                          |
| 2   | GET    | /api/posts/:id          | Returns **the post object with the specified id**                                                                               |
| 3   | POST   | /api/posts              | Creates a post using the information sent inside the request body and returns **the newly created post object**                 |
| 4   | PUT    | /api/posts/:id          | Updates the post with the specified id using data from the request body and **returns the modified document**, not the original |
| 5   | DELETE | /api/posts/:id          | Removes the post with the specified id and returns the **deleted post object**                                                  |
| 6   | GET    | /api/posts/:id/comments | Returns an **array of all the comment objects** associated with the post with the specified id                                  |

#### 2 [GET] /api/posts/:id

- If the _post_ with the specified `id` is not found:

  - return HTTP status code `404` (Not Found).
  - return the following JSON: `{ message: "The post with the specified ID does not exist" }`.

#### 3 [POST] /api/posts

- save the new _post_ the the database.
- return HTTP status code `201` (Created).
- return the newly created _post_.

#### 4 [PUT] /api/posts/:id

- If the _post_ with the specified `id` is not found:

  - return HTTP status code `404` (Not Found).
  - return the following JSON: `{ message: "The post with the specified ID does not exist" }`.

- If the post is found:

  - update the post using the new information sent in the `request body`.
  - return HTTP status code `200` (OK).
  - return the newly updated _post_.

#### 5 [DELETE] /api/posts/:id

- If the _post_ with the specified `id` is not found:

  - return HTTP status code `404` (Not Found).
  - return the following JSON: `{ message: "The post with the specified ID does not exist" }`.

#### 6 [GET] /api/posts/:id/comments

- If the _post_ with the specified `id` is not found:

  - return HTTP status code `404` (Not Found).
  - return the following JSON: `{ message: "The post with the specified ID does not exist" }`.
  
### Repository API

In the `Posts.Application` class library create a folder called `Repositories`, in the `Repositories` folder create a
`IPostRepository` interface and also a `PostRepository` class.

The `IPostRepository` should declare the following methods...

- `InitDb`: Returns a Task of type boolean and will setup dummy data in the PostRepository. More on this in the **Important Notes** section
- `GetAllAsync`: Returns a Task of IEnumerable of type Post, that will get all posts contained in the in-memory DB
- `CreateAsync`: calling `CreateAsync` and providing a domain Post object will add a post to the in-memory DB that returns a Task of type boolean indicating the post was successfully added.
- `UpdateAsync`: calling `UpdateAsync` and providing a domain Post object will update a post in the in-memory DB that returns a Task of type boolean indicating the post was successfully updated.
- `GetByIdAsync`: takes a `id` as input that represents a post id returning a Task of type Post where post is a post matching the provided id or null if a post with the provided id does not exist
- `DeleteByIdAsync`: takes a `id` as input that represents a post id returning a Task of type boolean returning true if a post was removed or false if there was no item to be deleted
- `GetCommentsByPostIdAsync`: to find Comments associated to a Post, take in a `Post Id` and get all comments where the comment `Post Id` matches the provided post id returning a Task of type IEnumerable of type Comment

The `PostRepository` should then _implement_ the `IPostRepository`, and then added to the .NET Dependency Injection container 
example of this in the **Important Notes** section.

### Blog Post Schema

A Blog Post in the database has the following structure:

```
{
  id: "9d9ecdbf-cad0-4111-a688-b7d796bf31d1", // Guid, required
  title: "The post title", // String, required
  contents: "The post contents", // String, required
  created_at: Mon Aug 14 2017 12:50:16 GMT-0700 (PDT) // DateTime, defaults to current date and time
  updated_at: Mon Aug 14 2017 12:50:16 GMT-0700 (PDT) // DateTime, defaults to current date and time
}
```

### Comment Schema

A Comment in the database has the following structure:

```
{
  text: "The text of the comment", // String, required
  post_id: "The id of the associated post", // Integer, required, must match the id of a post entry in the database
  created_at: Mon Aug 14 2017 12:50:16 GMT-0700 (PDT) // Date, defaults to current date
  updated_at: Mon Aug 14 2017 12:50:16 GMT-0700 (PDT) // Date, defaults to current date
}
```

#### Important Notes

##### Disable Https Locally

You can run into some strange behavior locally when using Https so we can disable it
```csharp
// Program.cs
var app = builder.Build();

// Other items...

// Disable HTTPs locally
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
```

##### Add Application and Init In-Memory DB

In order to get the `Posts.Application` in-memory database setup correctly we will need to do the following...

- Setup the repository class
```csharp
// Posts.Application/Repositories/PostRepository.cs
public class PostRepository
{
    // in memory DB
    private readonly List<Post> _posts = new();
    private readonly List<Comment> _comments = new();

    public Task<bool> InitDb()
    {
        var p = new Post
        {
            Id = Guid.NewGuid(),
            Title = ".NET Is The Best",
            Contents = ".Net is the best...that is all",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var c = new Comment
        {
            CommentId = Guid.NewGuid(),
            Text = "This was awesome!",
            PostId = p.Id
        };
        
        _posts.Add(p);
        _comments.Add(c);
        
        return Task.FromResult(true);
    }
}
```

- Setup an extension method to run `InitDb`

```csharp
using Microsoft.Extensions.DependencyInjection;
using Posts.Application.Repositories;

// Posts.ApplicationServiceCollectionExtensions
public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPostRepository, PostRepository>();
        return services;
    }

    public static IServiceProvider AddDatabase(this IServiceProvider services)
    {
        var repository = services.GetRequiredService<IPostRepository>();
        repository.InitDb();

        return services;
    }
}
```

- Register the `Post.Application` project and add the DB
```csharp
// Program.cs

var builder = WebApplication.CreateBuilder(args);

// Other code...

// Custom Registration
builder.Services.AddApplication();

var app = builder.Build();

// Init in-memory DB
app.Services.AddDatabase();
```

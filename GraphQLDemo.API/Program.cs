using GraphQLDemo.API.DataLoaders;
using GraphQLDemo.API.Schema;
using GraphQLDemo.API.Schema.Mutations;
using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using GraphQLDemo.API.Services.Instructors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddFiltering()
    .AddSorting();

builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<InstructorsRepository>();
builder.Services.AddScoped<InstructorDataLoader>();

builder.Services.AddInMemorySubscriptions();

builder.Services.AddPooledDbContextFactory<SchoolDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("default")).LogTo(Console.WriteLine));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    IDbContextFactory<SchoolDbContext> contextFactory = services.GetRequiredService<IDbContextFactory<SchoolDbContext>>();
    
    using (SchoolDbContext context = contextFactory.CreateDbContext())
    {
        context.Database.Migrate();
    }
}

app.MapGraphQL();

app.UseRouting();

app.UseWebSockets();

app.MapGet("/", () => "Hello World!");

app.Run();
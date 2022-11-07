using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FirebaseAdminAuthentication.DependencyInjection.Models;
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
    .AddType<CourseType>()
    .AddType<InstructorType>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization();

builder.Services.AddSingleton(FirebaseApp.Create());
builder.Services.AddFirebaseAuthentication();
builder.Services.AddAuthorization(o => o.AddPolicy("IsAdmin", p => p.RequireClaim(FirebaseUserClaimType.EMAIL, "kidakupe@gmail.com")));

builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<InstructorsRepository>();
builder.Services.AddScoped<InstructorDataLoader>();
builder.Services.AddScoped<UserDataLoader>();

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
app.UseAuthentication();

app.UseWebSockets();

app.MapGet("/", () => "Hello World!");

app.Run();
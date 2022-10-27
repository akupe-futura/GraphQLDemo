using GraphQLDemo.API.Schema;
using GraphQLDemo.API.Schema.Mutations;
using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>();

builder.Services.AddInMemorySubscriptions();

builder.Services.AddPooledDbContextFactory<SchoolDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("default")));

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
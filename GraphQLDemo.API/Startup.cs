//
// using HotChocolate;
//
// namespace GraphQLDemo.API;
//
// public class Startup
// {
//     private IConfiguration _configuration { get; }
//     private IWebHostEnvironment _environment { get; }
//
//     public Startup(IConfiguration configuration, IWebHostEnvironment environment)
//     {
//         _configuration = configuration;
//         _environment = environment;
//     }
//
//     public void ConfigureServices(IServiceCollection services)
//     {
//         services.AddGraphQL();
//     }
//
//     public void Configure(IApplicationBuilder app)
//     {
//         var isDevelopment = _environment.IsDevelopment();
//         if (isDevelopment)
//         {
//             app.UseDeveloperExceptionPage();
//         }
//         
//         app.UseRouting();
//         
//         app.UseEndpoints(endpoints =>
//         {
//             //endpoints.MapGet("/", () => "Hello World!");
//             endpoints.MapGraphQL();
//         });
//     }
// }

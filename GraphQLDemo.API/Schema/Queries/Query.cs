using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Schema.Filter;
using GraphQLDemo.API.Schema.Sorters;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Schema.Queries;

public class Query
{

    [UseDbContext(typeof(SchoolDbContext))]
    public async Task<IEnumerable<ISearchResultType>> Search(string term, [ScopedService] SchoolDbContext context)
    {
        IEnumerable<CourseType> courses = await context.Courses
            .Where(c => c.Name.Contains(term))
            .Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
                CreatorId = c.CreatorId,
            }).ToListAsync();

        IEnumerable<InstructorType> instructors = await context.Instructors
            .Where(i => i.Firstname.Contains(term)|| i.Lastname.Contains(term))
            .Select(i => new InstructorType()
            {
                Id = i.Id,
                Firstname = i.Firstname,
                Lastname = i.Lastname,
                Salary = i.Salary,
                
            }).ToListAsync();

        return new List<ISearchResultType>().Concat(courses).Concat(instructors);
    }
    
    [GraphQLDeprecated("Test")]
    public string Instructions => "New in GraphQL";
}
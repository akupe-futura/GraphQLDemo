using Bogus;
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
    private readonly CoursesRepository _coursesRepository;

    public Query(CoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }

    public async Task<IEnumerable<CourseType>> GetOffsetCourses()
    {
        IEnumerable<CourseDTO> courseDTOs = await _coursesRepository.GetAll();

        return courseDTOs.Select(c => new CourseType()
        {
            Id = c.Id,
            Name = c.Name,
            Subject = c.Subject,
            InstructorId = c.InstructorId,
        });
    }

    //Case when pagigation is affecting directly query in db
    //order does matter
    [UseDbContext(typeof(SchoolDbContext))]
    [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
    //[UseProjection]
    //remove property from filter funx
    [UseFiltering(typeof(CourseFilterType))]
    //[UseSorting(typeof(CourseSortType))]
    public IQueryable<CourseType> GetCourses([ScopedService] SchoolDbContext context)
    {

        return context.Courses.Select(c => new CourseType()
        {
            Id = c.Id,
            Name = c.Name,
            Subject = c.Subject,
            InstructorId = c.InstructorId,
            CreatorId = c.CreatorId,
        });
    }

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

    public async Task<CourseType> GetCourseByIdAsync(Guid id)
    {
        CourseDTO courseDTO = await _coursesRepository.GetById(id);

        return new CourseType()
        {
            Id = courseDTO.Id,
            Name = courseDTO.Name,
            Subject = courseDTO.Subject,
            InstructorId = courseDTO.InstructorId,
        };
    }
    
    [GraphQLDeprecated("Test")]
    public string Instructions => "New in GraphQL";
}
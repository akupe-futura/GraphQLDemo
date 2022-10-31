using Bogus;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Schema.Filter;
using GraphQLDemo.API.Schema.Sorters;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;

namespace GraphQLDemo.API.Schema.Queries;

public class Query
{
    private readonly CoursesRepository _coursesRepository;

    public Query(CoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }

    //Case when first we select the data from db and then do the filter/pagination/filter
    [UseSorting]
    public async Task<IEnumerable<CourseType>> GetCourses()
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
    //remove property from filter funx
    [UseFiltering(typeof(CourseFilterType))]
    [UseSorting(typeof(CourseSortType))]
    public IQueryable<CourseType> GetPaginatedCourses([ScopedService] SchoolDbContext context)
    {

        return context.Courses.Select(c => new CourseType()
        {
            Id = c.Id,
            Name = c.Name,
            Subject = c.Subject,
            InstructorId = c.InstructorId,
        });
    }

    public async Task<CourseType> GetCourseById(Guid id)
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
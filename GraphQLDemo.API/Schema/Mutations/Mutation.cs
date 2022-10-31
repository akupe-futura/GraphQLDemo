using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services.Courses;
using HotChocolate.Subscriptions;

namespace GraphQLDemo.API.Schema.Mutations;

public class Mutation
{
    private readonly CoursesRepository _coursesRepository;

    public Mutation(CoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }

    public async Task<CourseResult> CreateCourse(CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender)
    {
        CourseDTO courseDto = new CourseDTO()
        {
            Name = courseInputType.Name,
            Subject = courseInputType.Subject,
            InstructorId = courseInputType.InstructorId
        };
        
        courseDto = await _coursesRepository.Create(courseDto);
        
        CourseResult course = new CourseResult()
        {
            Id = courseDto.Id,
            Name = courseDto.Name,
            Subject = courseDto.Subject,
            InstructorId = courseDto.InstructorId
            
        };

        await topicEventSender.SendAsync(nameof( Subscription.CourseCreated), course);
        
        return course;
    }

    public async Task<CourseResult> UpdateCourse(Guid id, CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender)
    {
        CourseDTO courseDto = new CourseDTO()
        {
            Id=id,
            Name = courseInputType.Name,
            Subject = courseInputType.Subject,
            InstructorId = courseInputType.InstructorId
        };
        
        courseDto = await _coursesRepository.Update(courseDto);
        
        CourseResult course = new CourseResult()
        {
            Id = courseDto.Id,
            Name = courseDto.Name,
            Subject = courseDto.Subject,
            InstructorId = courseDto.InstructorId
            
        };
        
        string updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdate)}";
        
        await topicEventSender.SendAsync(updateCourseTopic, course);
        
        return course;
    }

    public async Task<bool> DeleteCourse(Guid id)
    {
        try
        {
            return await _coursesRepository.Delete(id);
        }
        catch (Exception)
        {

            return false;
        }
        
    }
}
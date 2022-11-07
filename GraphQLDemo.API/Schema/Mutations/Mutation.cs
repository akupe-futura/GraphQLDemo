using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services.Courses;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Subscriptions;
using System.Security.Claims;

namespace GraphQLDemo.API.Schema.Mutations;

public class Mutation
{
    private readonly CoursesRepository _coursesRepository;

    public Mutation(CoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }

    [Authorize]
    public async Task<CourseResult> CreateCourse(CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender, ClaimsPrincipal claimsPrincipal)
    {
        string userId = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID);

        CourseDTO courseDto = new CourseDTO()
        {
            Name = courseInputType.Name,
            Subject = courseInputType.Subject,
            InstructorId = courseInputType.InstructorId,
            CreatorId = userId
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

    [Authorize]
    public async Task<CourseResult> UpdateCourse(Guid id, CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender, ClaimsPrincipal claimsPrincipal)
    {
        string userId = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID);

        CourseDTO courseDto = await _coursesRepository.GetById(id);


        if (courseDto.CreatorId == null)
        {
            throw new GraphQLException(new Error("Course not found.", "COURSE_NOT_FOUND"));
        }

        if (courseDto.CreatorId != userId)
        {
            throw new GraphQLException(new Error("You do not have permission to update this course.", "INVALID_PERMISSION"));
        }


        courseDto.Name = courseInputType.Name;
        courseDto.Subject = courseInputType.Subject;
        courseDto.InstructorId = courseInputType.InstructorId;
        
        
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

    [Authorize(Policy = "IsAdmin")]
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
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Schema.Queries;

namespace GraphQLDemo.API.Schema;

public class CourseResult
{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }
}
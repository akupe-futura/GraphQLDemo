namespace GraphQLDemo.API.DTOs;

public class StudentDTO
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public double GPA { get; set; }
    
    public IEnumerable<CourseDTO> Courses { get; set; }
}
namespace GraphQLDemo.API.DTOs;

public class InstructorDTO
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public double Salary { get; set; }
    public IEnumerable<CourseDTO> Courses { get; set; }
}
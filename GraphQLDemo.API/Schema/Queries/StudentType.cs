namespace GraphQLDemo.API.Schema.Queries;

public class StudentType
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    
    [GraphQLName("gpa")]
    public double GPA { get; set; }
}
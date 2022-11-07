namespace GraphQLDemo.API.Schema.Queries;

public class InstructorType :ISearchResultType
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public double Salary { get; set; }
}
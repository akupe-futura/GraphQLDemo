using GraphQLDemo.API.Schema.Queries;
using HotChocolate.Data.Filters;

namespace GraphQLDemo.API.Schema.Filter
{
    public class CourseFilterType: FilterInputType<CourseType>
    {
        protected override void Configure(IFilterInputTypeDescriptor<CourseType> descriptor)
        {
            descriptor.Ignore(c => c.Students);
            base.Configure(descriptor);
        }
    }
}

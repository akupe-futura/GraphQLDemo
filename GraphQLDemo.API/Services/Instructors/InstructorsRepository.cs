﻿using GraphQLDemo.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Services.Instructors
{
    public class InstructorsRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> _contextFactory;

        public InstructorsRepository(IDbContextFactory<SchoolDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }


        public async Task<InstructorDTO> GetById(Guid instructorId)
        {
            using (SchoolDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Instructors
                    .FirstOrDefaultAsync(c => c.Id == instructorId);
            }
        }

        public async Task<IEnumerable<InstructorDTO>> GETManyByIds(IReadOnlyList<Guid> instructorIds)
        {
            using (SchoolDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Instructors
                    .Where(i => instructorIds.Contains(i.Id)).ToListAsync();
            }
        }
    }
}

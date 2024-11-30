using Microsoft.EntityFrameworkCore;
using PlateauMed.Core;
using PlateauMed.Core.Entities;
using PlateauMed.Infrastructure.Exceptions;
using PlateauMed.Infrastructure.Interfaces.Repositories;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataDbContext _dbContext;

        public UserRepository(DataDbContext dbContext) => _dbContext = dbContext;

        public async Task<UserModel> CreateUser(UserModel model)
        {
            var entity = model.Map();
            await _dbContext.Set<User>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Map();
        }

        public async Task<UserModel> GetUser(string email)
        {
            var record = await _dbContext.Set<User>()
                                         .AsNoTracking()
                                         .Where(x=>x.Email == email)
                                         .Select(x=>x.Map())
                                         .FirstOrDefaultAsync() ??
                         throw new NotFoundException("User not found");
            return record;
        }

        public async Task<UserModel> GetUser(Guid userId)
        {
            var record = await _dbContext.Set<User>()
                                         .AsNoTracking()
                                         .Where(x => x.Id == userId)
                                         .Select(x => x.Map())
                                         .FirstOrDefaultAsync() ??
                         throw new NotFoundException("User not found");
            return record;
        }

        public async Task<(UserModel, string)> GetUserWithPasswordHash(string email)
        {
            var record = await _dbContext.Set<User>()
                                      .AsNoTracking()
                                      .Where(x => x.Email == email)
                                      .FirstOrDefaultAsync() ??
                      throw new NotFoundException("User not found");
            return (record.Map(), record.PasswordHash);
        }

        public async Task<bool> IsUserExists(string email)
        {
            return await _dbContext.Set<User>()
                                   .AsNoTracking()
                                   .AnyAsync(x => x.Email == email);
        }
    }
}

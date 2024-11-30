using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> GetUser(string email);
        Task<(UserModel, string)> GetUserWithPasswordHash(string email);
        Task<bool> IsUserExists(string email);
        Task<UserModel> GetUser(Guid userId);
        Task<UserModel> CreateUser(UserModel model);
    }
}

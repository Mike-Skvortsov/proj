using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataBaseContext _context;

        public UserRepository(DataBaseContext context)
        {
            this._context = context;
        }

        public async Task<ICollection<User>> GetAllAsync()
        {
            return await this._context.Users.Include(x => x.Cards).ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await this._context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(User user)
        {
            await this._context.Users.AddAsync(user);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            this._context.Users.Update(user);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            this._context.Users.Remove(user);
            await this._context.SaveChangesAsync();
        }
    }
}

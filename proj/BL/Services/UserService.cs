using DataAccess;
using DataAccess.Repositories;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            this._repository = repository;
        }

        public Task<ICollection<User>> GetAllAsync()
        {
            var userFromRepository = this._repository.GetAllAsync();
            return userFromRepository;
        }

        public Task<User> GetByIdAsync(int id)
            => this._repository.GetByIdAsync(id);

        public Task AddAsync(User user)
            => this._repository.AddAsync(user);

        public Task UpdateAsync(User user)
            => this._repository.UpdateAsync(user);

        public async Task<bool> TryUpdateAsync(User user, int id)
        {
            var userToUpdate = await this._repository.GetByIdAsync(id);
            if (userToUpdate != null)
            {
                userToUpdate.Cards = user.Cards;
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                userToUpdate.Id = user.Id;


                await this._repository.UpdateAsync(userToUpdate);

                return true;
            }

            return false;
        }

        public Task DeleteAsync(User user)
            => this._repository.DeleteAsync(user);
    }
}

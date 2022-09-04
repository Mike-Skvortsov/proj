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
    public class OperationService : IOperationService
    {
        private readonly IOperationRepository _repository;
        private readonly ICardRepository _cardRepository;

        public OperationService(IOperationRepository repository, ICardRepository cardRepository)
        {
            this._repository = repository;
            _cardRepository = cardRepository;
        }
        public Task<ICollection<Operation>> GetAllAsync()
        {
            var operationFromRepository = this._repository.GetAllAsync();
            return operationFromRepository;
        }

        public Task<Operation> GetByIdAsync(int id)
            => this._repository.GetByIdAsync(id);

        public async Task AddAsync(Operation operation)
        {
            Card card = await _cardRepository.GetByIdAsync(operation.CardId);
            if ((operation.Type == OperationType.Enrollment && operation.Sum >= 0) || operation.Type == OperationType.WritingOff && operation.Sum < 0)
            {
                card.CardAmount += operation.Sum;
            }
            if (operation.Type == OperationType.WritingOff && operation.Sum >= 0)
            {
                card.CardAmount -= operation.Sum;
            }
            operation.Card = card;
            await this._repository.AddAsync(operation);
        }

        public Task UpdateAsync(Operation operation)
            => this._repository.UpdateAsync(operation);

        public async Task<bool> TryUpdateAsync(int id, Operation operation)
        {
            var operationToUpdate = await this._repository.GetByIdAsync(id);

            if (operationToUpdate != null)
            {
                operationToUpdate.Sum = operation.Sum;
                operationToUpdate.Name = operation.Name;
                operationToUpdate.CardId = operation.CardId;
                operationToUpdate.Type = operation.Type;
                await this._repository.UpdateAsync(operationToUpdate);

                return true;
            }

            return false;
        }

        public Task DeleteAsync(Operation operation)
            => this._repository.DeleteAsync(operation);
    }
}


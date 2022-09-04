using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly DataBaseContext _context;

        public CardRepository(DataBaseContext context)
        {
            this._context = context;
        }

        public async Task<ICollection<Card>> GetAllAsync()
        {
            return await this._context.Cards.Include(x => x.Operations).Include(x => x.User).ToListAsync();
        }

        public async Task<Card> GetByIdAsync(int id)
        {
            return await this._context.Cards.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Card card)
        {
            await this._context.Cards.AddAsync(card);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Card card)
        {
            this._context.Cards.Update(card);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Card card)
        {
            this._context.Cards.Remove(card);
            await this._context.SaveChangesAsync();
        }
    }
}

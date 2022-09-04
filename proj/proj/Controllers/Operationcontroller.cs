using AutoMapper;
using BL.Services;
using DataAccess;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proj.Models;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace proj.Controllers
{
    
    [ApiController]
    [Route("operation")]
    public class Operationcontroller : ControllerBase
    {
        private readonly IOperationService _operationService;
        private readonly ICardService _cardService;
        private readonly IMapper _mapper;

        public Operationcontroller(IOperationService operationService, ICardService cardService, IMapper mapper)
        {
            this._operationService = operationService;
            this._cardService = cardService;
            this._mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var operations = await this._operationService.GetAllAsync();
            return this.Ok(operations);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var operations = await this._operationService.GetByIdAsync(id);
            var model = this._mapper.Map<OperationModel>(operations);
            return this.Ok(model);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] OperationModel model)
        {
            var operation = this._mapper.Map<Operation>(model);
            
            await _operationService.AddAsync(operation);
            return this.Ok();
        }

        [HttpPut("{id:int}/edit")]
        public async Task<IActionResult> Update([FromBody] OperationModel model, [FromRoute] int id)
        {
            var operation = this._mapper.Map<Operation>(model);
            var oldOperation = await _operationService.GetByIdAsync(id);
            var card = await _cardService.GetByIdAsync(operation.CardId);
            if(card == null)
            {
                return this.NotFound();
            }
            card.CardAmount -= oldOperation.Sum;
            card.CardAmount += model.Sum;
            var result = await this._operationService.TryUpdateAsync(id, operation);

            if (result)
            {
                return this.Ok();
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpDelete("{id:int}/delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var operation = await this._operationService.GetByIdAsync(id);
            var card = await _cardService.GetByIdAsync(operation.CardId);
            card.CardAmount -= operation.Sum;
            if (operation != null)
            {
                await this._operationService.DeleteAsync(operation);
                return this.Ok();
            }

            return this.NotFound();
        }
    }
}

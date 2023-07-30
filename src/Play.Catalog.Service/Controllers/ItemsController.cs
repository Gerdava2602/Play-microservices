using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{
    [ApiController] //Attribute to enable features for dev experience
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> itemsRepository;

        //Constructor
        public ItemsController(IRepository<Item> itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDTO());
            return items;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid Id)
        {
            var item = (await itemsRepository.GetAsync(Id)).AsDTO();

            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        //ActionResult is used when you want to notify that an action was executed
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto itemDto)
        {
            var item = new Item
            {
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price,
                CreatedDate = DateTime.UtcNow
            };

            await itemsRepository.CreateAsync(item);

            //Will say, the item has been created and you can find it at the location
            return CreatedAtAction(nameof(GetByIdAsync), new { Id = item.Id }, item);
        }

        [HttpPut("{Id}")]
        //Iaction is used when we want to perform an action but we don't need to return anything
        public async Task<IActionResult> PutAsync(Guid Id, UpdateItemDto updateItemDto)
        {
            var existingEntity = await itemsRepository.GetAsync(Id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            existingEntity.Name = updateItemDto.Name;
            existingEntity.Description = updateItemDto.Description;
            existingEntity.Price = updateItemDto.Price;

            await itemsRepository.UpdateAsync(existingEntity);

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync(Guid Id)
        {
            var item = await itemsRepository.GetAsync(Id);

            if (item == null)
            {
                return NotFound();
            }

            await itemsRepository.RemoveAsync(item.Id);

            return NoContent();
        }
    }
}
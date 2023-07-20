using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController] //Attribute to enable features for dev experience
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTime.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures potion", 5, DateTime.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deals a small amount of damage", 5, DateTime.UtcNow),
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        [HttpGet("{Id}")]
        public ActionResult<ItemDto> GetById(Guid Id)
        {
            var item = items.Where(item => item.Id == Id).SingleOrDefault();

            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        //ActionResult is used when you want to notify that an action was executed
        public ActionResult<ItemDto> Post(CreateItemDto itemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), itemDto.Name, itemDto.Description, itemDto.Price, DateTime.UtcNow);
            items.Add(item);
            //Will say, the item has been created and you can find it at the location
            return CreatedAtAction(nameof(GetById), new { Id = item.Id }, item);
        }

        [HttpPut("{Id}")]
        //Iaction is used when we want to perform an action but we don't need to return anything
        public IActionResult Put(Guid Id, UpdateItemDto updateItemDto)
        {
            var existingItem = items.Where(item => item.Id == Id).SingleOrDefault();

            if (existingItem == null)
            {
                return NotFound();
            }

            //Creates a clone of the existingItem using the new attributes
            var updatedItem = existingItem with
            {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price
            };

            var index = items.FindIndex(existingItem => existingItem.Id == Id);
            items[index] = updatedItem;

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(Guid Id)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == Id);

            if (index < 0)
            {
                return NotFound();
            }

            items.RemoveAt(index);
            return NoContent();
        }
    }
}
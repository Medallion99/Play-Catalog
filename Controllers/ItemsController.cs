using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repository;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ItemsController : ControllerBase
    {
        // private readonly ItemsRepository itemsRepository = new ItemsRepository();
        private readonly IItemsRepository _itemsRepository;

        public ItemsController(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await _itemsRepository.GetAllAsync()).Select(item => item.AsDto());
            return items;   
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemByIdAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);

            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Post(CreateItemDto model)
        {
            var item = new Item()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _itemsRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetItemByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto model)
        {
            var existingItem = await _itemsRepository.GetAsync(id);

            if (existingItem is not null)
            {
                existingItem.Name = model.Name;
                existingItem.Description = model.Description;
                existingItem.Price = model.Price;
            }

            await _itemsRepository.UpdateAsync(existingItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);

            if (item is not null)
            {
                await _itemsRepository.RemoveAsync(item.Id);
                return Ok();
            }
            return BadRequest();
        }
    }
}
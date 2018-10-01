using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly DataContext _context;

        public ShoppingController(DataContext context)
        {
            _context = context;

            if (!_context.ShoppingItems.Any())
            {
                _context.ShoppingItems.AddRange(
                    new ShoppingItem { Id = 1, Name = "Orange Juice", Manufacturer = "Orange Tree", Price = 18.00m },
                    new ShoppingItem { Id = 2, Name = "Orange", Manufacturer = "Orange Tree", Price = 19.15m },
                    new ShoppingItem { Id = 3, Name = "Diary Milk", Manufacturer = "Cow", Price = 22.50m },
                    new ShoppingItem { Id = 4, Name = "Cheese", Manufacturer = "Cow", Price = 21.00m },
                    new ShoppingItem { Id = 5, Name = "butter", Manufacturer = "Cow", Price = 30.00m },
                    new ShoppingItem { Id = 6, Name = "wool", Manufacturer = "Cow", Price = 40.00m },
                    new ShoppingItem { Id = 7, Name = "Frozen Pizza", Manufacturer = "Uncle Mickey", Price = 26.25m },
                    new ShoppingItem { Id = 8, Name = "Social Pizza", Manufacturer = "Dominos", Price = 32.50m },
                    new ShoppingItem { Id = 9, Name = "Pan Pizza", Manufacturer = "Pizza Hut", Price = 35.75m },
                    new ShoppingItem { Id = 10, Name = "Chichen Pizza", Manufacturer = "KFC", Price = 42.95m });
                _context.SaveChanges();
            }
        }

        // GET api/values
        [HttpGet]
        public ActionResult<List<ShoppingItem>> Get()
        {
            return Ok(_context.ShoppingItems.ToList());
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetShoppingItem")]
        public ActionResult<ShoppingItem> Get(int id)
        {
            var shoppingItem = _context.ShoppingItems.FirstOrDefault(p => p.Id == id);

            if (shoppingItem == null)
                return NotFound();

            return Ok(shoppingItem);
        }
        
        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]ShoppingItem shoppingItem)
        {
            if (shoppingItem == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.ShoppingItems.Add(shoppingItem);
            _context.SaveChanges();

            return CreatedAtRoute("GetShoppingItem", new { id = shoppingItem.Id }, shoppingItem);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ShoppingItem shoppingItem)
        {
            if (shoppingItem == null || shoppingItem.Id != id || !ModelState.IsValid)
                return BadRequest();

            var existingShoppingItem = _context.ShoppingItems.FirstOrDefault(p => p.Id == id);

            if (existingShoppingItem == null)
                return NotFound();

            existingShoppingItem.Name = shoppingItem.Name;
            existingShoppingItem.Manufacturer = shoppingItem.Manufacturer;
            existingShoppingItem.Price = shoppingItem.Price;

            _context.ShoppingItems.Update(existingShoppingItem);
            _context.SaveChanges();

            return Ok(shoppingItem);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var shoppingItem = _context.ShoppingItems.FirstOrDefault(p => p.Id == id);

            if (shoppingItem != null)
            {
                _context.ShoppingItems.Remove(shoppingItem);
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }

            return new NoContentResult();
        }
    }
}

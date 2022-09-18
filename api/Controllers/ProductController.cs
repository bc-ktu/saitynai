using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly StoreContext context;

        public ProductController(StoreContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await context.Products.FindAsync(id);

            if (product == null) 
                return NotFound();

            return product;
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await context.Products
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (product == null) 
                return NotFound();

            context.Products.Remove(product);
            var result = await context.SaveChangesAsync() > 0;

            if(result)
                return Ok();
            
            return BadRequest(error: "Problem with deleting the product" );
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(Product newProduct)
        {
            var existingProduct = await context.Products.FindAsync(newProduct.Id);
            if(existingProduct != null)
                return BadRequest(error: "Such product exists");

            /*var product = new Product {
                
            };*/

            context.Products.Add(newProduct);
            var result = await context.SaveChangesAsync() > 0;
            if(result)
                return Ok();
            
            return BadRequest(error: "Problem with creating the product" );
        }

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product updatedProduct)
        {
            if (id != updatedProduct.Id)
                return BadRequest();

            var existingProduct = await context.Products.FindAsync(updatedProduct.Id);
            if(existingProduct == null)
                return NotFound();

            existingProduct.Title = updatedProduct.Title;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.IsNew = updatedProduct.IsNew ;
            existingProduct.Type = updatedProduct.Type;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Quantity = updatedProduct.Quantity;
            
            var result = await context.SaveChangesAsync() > 0;
            if(result)
                return Ok();
            
            return BadRequest(error: "Problem with updating the product" );

            /*var newProduct = new Product {
                Title = existingProduct.Title,
                Description = existingProduct.Description,
                IsNew = existingProduct
            };*/
        }
    }
}
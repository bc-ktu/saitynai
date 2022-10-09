using api.Data;
using api.DTOs;
using api.Entities;
using api.Models;
using api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class ProductController : ControllerBase
    {
        //private readonly StoreContext context; 
        private readonly IProductService service;
        private readonly IMapper mapper;
       /* private Mapper mapp;*/

        //private MapperConfiguration configProductToDto = new MapperConfiguration(ctg =>
        //{
        //    ctg.AddProfile<ProductDataMappingProfile>();
        //    //ctg.AllowNullCollections = true;
        //    ctg.CreateMap<Product, ProductDto>();//.Include<Comment, CommentDto>();
        //});
        //private MapperConfiguration configDtoToProduct = new MapperConfiguration (ctg => ctg.CreateMap<ProductDto, Product>());

        //public ProductController(StoreContext context)
        //{
        //    this.context = context;
        //}

        public ProductController(IProductService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
            /*mapp = InitializeAutomapper();*/
        }

        /*static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Product, ProductDto>();
            });
            var mapper = new Mapper(config);
            return mapper;
        }*/

        [HttpGet]
       // [Route("api/Order/{OrderId}/Products")]
        public async Task<ActionResult<List<ProductDto>>> Get()
        {
            var products = await service.GetAllProducts();
            List<ProductDto> result = mapper.Map<List<Product>, List<ProductDto>>(products);
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
       // [Route("api/Order/{OrderId}/Products/{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await service.GetProduct(id);
            if (product == null)
                return NotFound($"Produktas su Id {id} nerastas.");
            var productDto = mapper.Map<Product, ProductDto>(product);
            return Ok(productDto);
        }

        [HttpPut]
        [Route("{id}")]
        // [Route("api/Order/{OrderId}/Products/{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, ProductDto updatedProduct)
        {           
            var product = await service.GetProduct(id);
            if (product == null)
                return NotFound($"Produktas su Id {id} nerastas.");

            var productFromDto = mapper.Map<ProductDto, Product>(updatedProduct, product);
            try
            {
                await service.UpdateProduct(id, productFromDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko atnaujinti produkto. Klaida: {ex.Message}");
            }
            return StatusCode(204);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto newProduct)
        {
            var mapDtoToProduct = mapper.Map<ProductDto, Product>(newProduct);
            try
            {
                await service.CreateProduct(mapDtoToProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko sukurti produkto. Klaida: {ex.Message}");
            }
            return StatusCode(201);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await service.GetProduct(id);
            if (product == null)
                return NotFound($"Produktas su Id {id} nerastas.");
            try
            {
                await service.DeleteProduct(product);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko pašalinti produkto. Klaida: {ex.Message}");
            }
            return Ok(); //TODO: check code
        }

        //[HttpGet(Name = "GetAll")]
        //public async Task<ActionResult<List<Product>>> GetProducts()
        //{
        //    var products = await context.Products.ToListAsync();
        //    return await context.Products
        //        .Include(c => c.Comments)
        //        .ToListAsync();
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Product>> GetProduct(int id)
        //{
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteProduct(int id)
        //{
        //    //remove related comments
        //    var comments = await context.Comments
        //        .Where(p => p.ProductId == id)
        //        .ToListAsync();

        //    if (comments != null)
        //        context.Comments.RemoveRange(comments);

        //    //remove the product
        //    var product = await context.Products
        //        .Where(p => p.Id == id)
        //        .FirstOrDefaultAsync();

        //    if (product == null) 
        //        return NotFound();

        //    context.Products.Remove(product);

        //    var result = await context.SaveChangesAsync() > 0;

        //    if(result)
        //        return Ok();

        //    return BadRequest(error: "Problem with deleting the product" );
        //}

        //[HttpPost]
        //public async Task<ActionResult> CreateProduct(Product newProduct)
        //{
        //    var existingProduct = await context.Products.FindAsync(newProduct.Id);
        //    if(existingProduct != null)
        //        return BadRequest(error: "Such product exists");

        //    /*var product = new Product {

        //    };*/

        //    context.Products.Add(newProduct);
        //    var result = await context.SaveChangesAsync() > 0;
        //    if(result)
        //        return Ok();

        //    return BadRequest(error: "Problem with creating the product" );
        //}

        //[HttpPut] //dto
        //public async Task<ActionResult<List<Product>>> UpdateProduct(int id, Product updatedProduct)
        //{
        //    if (id != updatedProduct.Id)
        //        return BadRequest();

        //    var existingProduct = await context.Products.FindAsync(updatedProduct.Id);
        //    if(existingProduct == null)
        //        return NotFound();

        //    existingProduct.Title = updatedProduct.Title;
        //    existingProduct.Description = updatedProduct.Description;
        //    existingProduct.IsNew = updatedProduct.IsNew;
        //    existingProduct.Type = updatedProduct.Type;
        //    existingProduct.Price = updatedProduct.Price;
        //    existingProduct.Quantity = updatedProduct.Quantity;

        //    var result = await context.SaveChangesAsync() > 0;
        //    if(result)
        //        return await context.Products
        //        .Where(c => c.Id == id)
        //        .Include(c => c.Comments)
        //        .ToListAsync();

        //    return BadRequest(error: "Problem with updating the product" );

        //    /*var newProduct = new Product {
        //        Title = existingProduct.Title,
        //        Description = existingProduct.Description,
        //        IsNew = existingProduct
        //    };*/
        //}

        //private ProductDto MapProductToDto(Product product)
        //{
        //    return new ProductDto
        //    {
        //        Title = product.Title,
        //        Price = (decimal)product.Price,
        //        Type = product.Type,
        //        Description = product.Description,
        //        Quantity = product.Quantity,
        //        IsNew = product.isNew,
        //        Comments = product.Comments.Select(item => new Comment
        //        {
        //            Id = item.Id,
        //            DateCreated = item.DateCreated,
        //            Title = item.Title,
        //            Text = item.Text,
        //            isDeleted = item.isDeleted,
        //            DateEditted = item.DateEditted
        //        }).ToList()
        //    };
        //}
    }
}
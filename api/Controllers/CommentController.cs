using api.Data;
using api.Data.Services;
using api.DTOs;
using api.Entities;
using api.Services;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/Products/{productId}/[controller]s")]
   // [Route("api/Order/{orderId}/Products/{productId}/[controller]s")]

    public class CommentController : ControllerBase
    {
        private readonly ICommentService service;
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public CommentController(ICommentService service, IProductService productService, IMapper mapper)
        {
            this.service = service;
            this.productService = productService;
            this.mapper = mapper;
        }


        [HttpGet]
        [Route("api/Products/{productId}/[controller]s")]
        public async Task<ActionResult<List<CommentDto>>> Get(int productId, int orderId = -1)
        {
            var product = await productService.GetProduct(productId);
            if (product == null)
                return NotFound($"Produktas (Id={productId}) nerastas.");

            var comments = await service.GetAllComments(productId);
            if (comments.Count == 0)
            {
                return NotFound(string.Format($"Produktas (Id={productId}) komentarų neturi."));
            }
            List<CommentDto> result = mapper.Map<List<Comment>, List<CommentDto>>(comments);
            return Ok(result);
        }

        [HttpGet]
        [Route("api/Products/{productId}/[controller]s/{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(int productId, int id)
        {
            var product = await productService.GetProduct(productId);
            if (product == null)
                return NotFound($"Produktas (Id={productId}) nerastas.");

            var comment = await service.GetComment(productId, id);
            if (comment == null)
                return NotFound($"Produktas (Id={productId}) komentaro (Id={id}) neturi.");

            var CommentDto = mapper.Map<Comment, CommentDto>(comment);
            return Ok(CommentDto);
        }

        [HttpPut]
        [Route("api/Products/{productId}/[controller]s/{id}")]
        public async Task<ActionResult<CommentDto>> UpdateComment(int productId, int id, UpdateCommentDto updatedComment)
        {
            var product = await productService.GetProduct(productId);
            if (product == null)
                return NotFound($"Produktas (Id={productId}) nerastas.");

            var comment = await service.GetComment(productId, id);
            if (comment == null)
                return NotFound($"Komentaras (Id={id}) nerastas.");

            var commentFromDto = mapper.Map<UpdateCommentDto, Comment>(updatedComment, comment);
            commentFromDto.Product = product;
            commentFromDto.DateEditted = DateTime.UtcNow;
            commentFromDto.IsEditted = true;
            try
            {
                await service.UpdateComment(productId, id, commentFromDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko atnaujinti komentaro. Klaida: {ex.Message}");
            }
            return StatusCode(204);
        }

        [HttpPost]
        [Route("api/Products/{productId}/[controller]s")]
        public async Task<IActionResult> CreateComment(int productId, CreateCommentDto newComment)
        {
            var product = await productService.GetProduct(productId);
            if (product == null)
                return NotFound($"Produktas (Id={productId}) nerastas.");

            var mapDtoToComment = mapper.Map<CreateCommentDto, Comment>(newComment);
            mapDtoToComment.Product = product;
            mapDtoToComment.DateCreated = DateTime.UtcNow;
            try
            {
                await service.CreateComment(mapDtoToComment);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko sukurti komentaro. Klaida: {ex.Message}");
            }
            return StatusCode(201);
        }

        [HttpDelete]
        [Route("api/Products/{productId}/[controller]s/{id}")]
        public async Task<IActionResult> DeleteComment(int productId, int id)
        {
            var product = await productService.GetProduct(productId);
            if (product == null)
                return NotFound($"Produktas (Id={productId}) nerastas.");

            var Comment = await service.GetComment(productId, id);
            if (Comment == null)
                return NotFound($"Komentaras (Id={id}) nerastas.");
            try
            {
                await service.DeleteComment(Comment);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko pašalinti komentaro. Klaida: {ex.Message}");
            }
            return Ok(); //TODO: check code
        }

        //[HttpDelete]
        //public async Task<IActionResult> DeleteComments(int productId)
        //{
        //    var product = await productService.GetProduct(productId);
        //    if (product == null)
        //        return NotFound($"Produktas (Id={productId}) nerastas.");

        //    var comments = await service.GetAllComments(productId);
        //    if (comments == null)
        //        return NotFound($"Produktas (Id={productId}) neturi komentarų.");
        //    try
        //    {
        //        await service.DeleteComments(comments);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Nepavyko pašalinti komentarų. Klaida: {ex.Message}");
        //    }
        //    return Ok(); //TODO: check code
        //}

        //[HttpGet(Name = "Products")]
        //public async Task<ActionResult<Comment>> GetComments()
        //{
        //    var comments = await context.Comments.ToListAsync();
        //    return Ok(comments);
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Comment>> GetComment(int id)
        //{
        //    var comment = await context.Comments.FindAsync(id);

        //    if (comment == null) 
        //        return NotFound();

        //    return comment;
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteComment(int id)
        //{
        //    var comment = await context.Comments
        //        .Where(p => p.Id == id)
        //        .Include(c => c.Product)
        //        .FirstOrDefaultAsync();

        //    if (comment == null) 
        //        return NotFound();

        //    context.Comments.Remove(comment);
        //    var result = await context.SaveChangesAsync() > 0;

        //    if(result)
        //        return StatusCode(204, "Sekmingai istrintas komentaras.");

        //    return BadRequest(error: "Problem with deleting the comment" );
        //}

        //[HttpPost] //veikia
        //public async Task<ActionResult> CreateComment(CommentDto newComment)
        //{
        //    var product = await context.Products
        //        .Where(c => c.Id == newComment.ProductId)
        //        .Include(c => c.Comments)
        //        .FirstOrDefaultAsync();

        //    if (product == null)
        //        return NotFound();

        //    var existingComment = await context.Comments.FindAsync(newComment.Id);
        //    if(existingComment != null)
        //        return BadRequest(error: "Such comment exists");

        //    var comment = new Comment {
        //        Title = newComment.Title,
        //        Text = newComment.Text,
        //        IsEditted = false,
        //        isDeleted = false,
        //        IsFeatured = newComment.isFeatured,
        //        DateCreated = DateTime.Now,
        //        ProductId = product.Id,
        //        Product = product
        //    };

        //    context.Comments.Add(comment);
        //    var result = await context.SaveChangesAsync() > 0;
        //    if(result)
        //        return Ok();

        //    return BadRequest(error: "Problem with creating the comment" );
        //}

        //[HttpPut]
        //public async Task<ActionResult<Comment>> UpdateComment(int id, Comment updatedComment)
        //{
        //    if (id != updatedComment.Id)
        //        return BadRequest();

        //    var existingComment = await context.Comments.FindAsync(updatedComment.Id);
        //    if(existingComment == null)
        //        return NotFound();

        //    existingComment.Title = updatedComment.Title;
        //    existingComment.Text = updatedComment.Text;
        //    existingComment.IsEditted = true;
        //    existingComment.isDeleted = updatedComment.isDeleted;
        //    existingComment.IsFeatured = updatedComment.IsFeatured;
        //    existingComment.DateEditted = DateTime.Now;

        //    var result = await context.SaveChangesAsync() > 0;
        //    if(result)
        //        return Ok();

        //    return BadRequest(error: "Problem with updating the comment" );

        //    /*var newProduct = new Product {
        //        Title = existingProduct.Title,
        //        Description = existingProduct.Description,
        //        IsNew = existingProduct
        //    };*/
        //}
    }
}
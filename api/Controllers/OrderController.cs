using api.Data.Services;
using api.DTOs;
using api.Entities;
using api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly IOrderService service;
       // private readonly IProductService orderService;
        private readonly IMapper mapper;

        public OrderController(IOrderService service, IMapper mapper)
        {
            this.service = service;
            //this.orderService = orderService;
            this.mapper = mapper;
        }/*

        [HttpGet]
        public async Task<ActionResult<List<CommentDto>>> Get(int productId)
        {
            var order = await orderService.GetOrder(productId);
            if (order == null)
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
        [Route("{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(int productId, int id)
        {
            var product = await orderService.GetProduct(productId);
            if (product == null)
                return NotFound($"Produktas (Id={productId}) nerastas.");

            var comment = await service.GetComment(productId, id);
            if (comment == null)
                return NotFound($"Produktas (Id={productId}) komentaro (Id={id}) neturi.");

            var CommentDto = mapper.Map<Comment, CommentDto>(comment);
            return Ok(CommentDto);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<CommentDto>> UpdateComment(int productId, int id, UpdateCommentDto updatedComment)
        {
            var product = await orderService.GetProduct(productId);
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
        public async Task<IActionResult> CreateComment(int productId, CreateCommentDto newComment)
        {
            var product = await orderService.GetProduct(productId);
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
        [Route("{id}")]
        public async Task<IActionResult> DeleteComment(int productId, int id)
        {
            var product = await orderService.GetProduct(productId);
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

        [HttpDelete]
        public async Task<IActionResult> DeleteComments(int productId)
        {
            var product = await orderService.GetProduct(productId);
            if (product == null)
                return NotFound($"Produktas (Id={productId}) nerastas.");

            var comments = await service.GetAllComments(productId);
            if (comments == null)
                return NotFound($"Produktas (Id={productId}) neturi komentarų.");
            try
            {
                await service.DeleteComments(comments);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko pašalinti komentarų. Klaida: {ex.Message}");
            }
            return Ok(); //TODO: check code
        }
*/
    }
}

using api.Data;
using api.Data.Entities;
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
    public class CommentController : ControllerBase
    {
        private readonly ICommentService service;
        private readonly IProductService productService;
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public CommentController(ICommentService service, IProductService productService, IOrderService orderService, IMapper mapper)
        {
            this.service = service;
            this.productService = productService;
            this.mapper = mapper;
            this.orderService = orderService;
        }

        [HttpGet]
        [Route("api/Products/{productId}/[controller]s")]
        public async Task<ActionResult<List<CommentDto>>> Get(int productId)
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
        [Route("api/Orders/{orderId}/Products/{productId}/[controller]s")]
        public async Task<ActionResult<List<CommentDto>>> Get(int productId, int orderId)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            var product = await productService.GetProduct(productId);
            if (product == null)
                return NotFound($"Produktas (Id={productId}) nerastas.");

            var comments = await service.GetAllComments(productId, orderId);
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

        [HttpGet]
        [Route("api/Orders/{orderId}/Products/{productId}/[controller]s/{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(int productId, int id, int orderId)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            var product = await productService.GetProduct(productId);
            if (product == null)
                return NotFound($"Produktas (Id={productId}) nerastas.");

            var comment = await service.GetComment(productId, id, orderId);
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

            if (comment.IsDeleted)
                return BadRequest($"Komentaras (Id={id}) pažymėtas kaip ištrintas.");

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

        [HttpPut]
        [Route("api/Orders/{orderId}/Products/{productId}/[controller]s/{id}")]
        public async Task<ActionResult<CommentDto>> UpdateComment(int orderId, int productId, int id, UpdateCommentDto updatedComment)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            var product = await productService.GetProduct(productId);
            if (product == null)
                return NotFound($"Produktas (Id={productId}) nerastas.");

            var comment = await service.GetComment(productId, id);
            if (comment == null)
                return NotFound($"Komentaras (Id={id}) nerastas.");

            if (comment.IsDeleted)
                return BadRequest($"Komentaras (Id={id}) pažymėtas kaip ištrintas.");

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

        [HttpPost]
        [Route("api/Orders/{orderId}/Products/{productId}/[controller]s")]
        public async Task<IActionResult> CreateComment(int orderId, int productId, CreateCommentDto newComment)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            var product = await productService.GetProduct(productId);
            if (product == null)
                return NotFound($"Produktas (Id={productId}) nerastas.");

            if (order.Status != OrderStatuses.Atliktas && !product.IsDisplayed)
                return BadRequest($"Netinkamas užsakymo statusas - {order.Status.ToString().ToLower()}. Komentarą galima palikti, kai statusas yra {OrderStatuses.Atliktas.ToString().ToLower()}");

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

            var comment = await service.GetComment(productId, id);
            if (comment == null)
                return NotFound($"Komentaras (Id={id}) nerastas.");
            comment.IsDeleted = true;
            try
            {
                await service.DeleteComment(comment);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko pašalinti komentaro. Klaida: {ex.Message}");
            }
            return StatusCode(204);
        }

        [HttpDelete]
        [Route("api/Orders/{orderId}/Products/{productId}/[controller]s/{id}")]
        public async Task<IActionResult> DeleteComment(int orderId, int productId, int id)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            var product = await productService.GetProduct(productId);
            if (product == null)
                return NotFound($"Produktas (Id={productId}) nerastas.");

            var comment = await service.GetComment(productId, id);
            if (comment == null)
                return NotFound($"Komentaras (Id={id}) nerastas.");
            comment.IsDeleted = true;
            try
            {
                await service.DeleteComment(comment);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko pašalinti komentaro. Klaida: {ex.Message}");
            }
            return StatusCode(204);
        }
    }
}
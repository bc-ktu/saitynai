using api.Authorization.Model;
using api.Data.DTOs;
using api.Data.Entities;
using api.Data.Services;
using api.DTOs;
using api.Entities;
using api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService service;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;

        public OrderController(IOrderService service, IMapper mapper, IAuthorizationService authorizationService)
        {
            this.service = service;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.RegisteredUser)]
        public async Task<ActionResult<List<OrderDto>>> Get()
        {
            List<Order> orders = new List<Order>();

            if (User.IsInRole(Roles.Admin))
                orders = await service.GetAllOrders();
            else if (User.IsInRole(Roles.RegisteredUser))
                orders = await service.GetAllUsersOrders(User.FindFirstValue(ClaimTypes.NameIdentifier));

            List<OrderDto> result = mapper.Map<List<Order>, List<OrderDto>>(orders);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.RegisteredUser)]
        public async Task<ActionResult<CommentDto>> GetOrder(int id)
        {
            var order = await service.GetOrder(id);
            if (order == null)
                return NotFound($"Užsakymas (Id={id}) nerastas.");

            var authorizationResult = await authorizationService.AuthorizeAsync(User, order, PolicyNames.ResourceOwner);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var OrderDto = mapper.Map<Order, OrderDto>(order);
            return Ok(OrderDto);
        }

        //Order created, products are added to order with Put method
        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.RegisteredUser)]
        public async Task<IActionResult> CreateOrder()
        {
            var order = new Order();
            order.Status = OrderStatuses.Sukurtas;
            order.DateCreated = DateTime.UtcNow;
            order.OrdererId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            order.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);//User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await service.CreateOrder(order);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko sukurti užsakymo. Klaida: {ex.Message}");
            }
            return StatusCode(201);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.RegisteredUser)]
        public async Task<ActionResult<OrderDto>> UpdateOrder(int id, UpdateOrderDto updatedOrder)
        {
            var order = await service.GetOrder(id);
            if (order == null)
                return NotFound($"Užsakymas (Id={id}) nerastas.");

            var authorizationResult = await authorizationService.AuthorizeAsync(User, order, PolicyNames.ResourceOwner);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            if (!Enum.IsDefined(typeof(OrderStatuses), updatedOrder.Status))
                return BadRequest($"Netinkamas užsakymo statusas.");

            var orderDto = mapper.Map<UpdateOrderDto, Order>(updatedOrder, order);
            orderDto.DateEditted = DateTime.UtcNow;
            try
            {
                await service.UpdateOrder(id, orderDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko atnaujinti užsakymo. Klaida: {ex.Message}");
            }
            return StatusCode(204);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = Roles.RegisteredUser)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await service.GetOrder(id);
            if (order == null)
                return NotFound($"Užsakymas (Id={id}) nerastas.");

            var authorizationResult = await authorizationService.AuthorizeAsync(User, order, PolicyNames.ResourceOwner);
            if(!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            if (order.Status != OrderStatuses.Sukurtas && order.Status != OrderStatuses.Pateiktas)
            {
                return BadRequest($"Negalima pašalinti užsakymo dėl statuso. Statusas - {order.Status.ToString().ToLower()}");
            }

            try
            {
                await service.DeleteOrder(order);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko pašalinti užsakymo. Klaida: {ex.Message}");
            }
            return StatusCode(204);
        }
    }
}

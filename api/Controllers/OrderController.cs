using api.Data.DTOs;
using api.Data.Entities;
using api.Data.Services;
using api.DTOs;
using api.Entities;
using api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService service;
        private readonly IMapper mapper;

        public OrderController(IOrderService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> Get()
        {
            var orders = await service.GetAllOrders();
            List<OrderDto> result = mapper.Map<List<Order>, List<OrderDto>>(orders);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CommentDto>> GetOrder(int id)
        {
            var order = await service.GetOrder(id);
            if (order == null)
                return NotFound($"Užsakymas (Id={id}) nerastas.");

            var OrderDto = mapper.Map<Order, OrderDto>(order);
            return Ok(OrderDto);
        }
        //Order created, products are added to order with Put method
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto newOrder)
        {
            var mapDtoToOrder = mapper.Map<CreateOrderDto, Order>(newOrder);
            mapDtoToOrder.Status = OrderStatuses.Sukurtas;
            mapDtoToOrder.DateCreated = DateTime.UtcNow;
            try
            {
                await service.CreateOrder(mapDtoToOrder);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko sukurti užsakymo. Klaida: {ex.Message}");
            }
            return StatusCode(201);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<OrderDto>> UpdateOrder(int id, UpdateOrderDto updatedOrder)
        {
            var order = await service.GetOrder(id);
            if (order == null)
                return NotFound($"Užsakymas (Id={id}) nerastas.");

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
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await service.GetOrder(id);
            if (order == null)
                return NotFound($"Užsakymas (Id={id}) nerastas.");

            if (order.Status != OrderStatuses.Sukurtas && order.Status != OrderStatuses.Pateiktas)
            {
                return BadRequest($"Negalima pašalinti užsakymo dėl statuso. Statusas - {order.Status}");
            }

            try
            {
                await service.DeleteOrder(order);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko pašalinti užsakymo. Klaida: {ex.Message}");
            }
            return Ok();
        }
    }
}

using api.Data;
using api.Data.Entities;
using api.Data.Services;
using api.DTOs;
using api.Entities;
using api.Models;
using api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

//id ir role (token viduje)
//nice to have: refresh token

namespace api.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService service;
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public ProductController(IProductService service, IOrderService orderService, IMapper mapper)
        {
            this.service = service;
            this.orderService = orderService;
            this.mapper = mapper;
        }
        
        [HttpGet]
        [Route("api/[controller]s")]
        public async Task<ActionResult<List<ProductDto>>> Get()
        {
            var products = await service.GetAllProducts();
            List<ProductDto> result = mapper.Map<List<Product>, List<ProductDto>>(products);
            return Ok(result);
        }

        [HttpGet]
        [Route("api/Orders/{orderId}/[controller]s")]
        public async Task<ActionResult<List<ProductDto>>> Get(int orderId)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            var products = await service.GetAllProducts(orderId);
            List<ProductDto> result = mapper.Map<List<Product>, List<ProductDto>>(products);
            return Ok(result);
        }

        [HttpGet]
        [Route("api/[controller]s/{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await service.GetProduct(id);
            if (product == null)
                return NotFound($"Produktas su Id {id} nerastas.");
            var productDto = mapper.Map<Product, ProductDto>(product);
            return Ok(productDto);
        }

        [HttpGet]
        [Route("api/Orders/{orderId}/[controller]s/{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id, int orderId)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            var product = await service.GetProduct(id, orderId);
            if (product == null)
                return NotFound($"Produktas su Id {id} nerastas.");
            var productDto = mapper.Map<Product, ProductDto>(product);
            return Ok(productDto);
        }

        [HttpPut]
        [Route("api/[controller]s/{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, ProductDto updatedProduct)
        {           
            var product = await service.GetProduct(id);
            if (product == null)
                return NotFound($"Produktas (Id={id}) nerastas.");

            var productFromDto = mapper.Map<ProductDto, Product>(updatedProduct, product);
            try
            {
                await service.UpdateProduct(id, productFromDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko atnaujinti produkto. Klaida: {ex.Message}.");
            }
            return StatusCode(204);
        }

        [HttpPut]
        [Route("api/Orders/{orderId}/[controller]s/{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, ProductDto updatedProduct, int orderId)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            var product = await service.GetProduct(id);
            if (product == null)
                return NotFound($"Produktas (Id={id}) nerastas.");

            if (order.Status != OrderStatuses.Sukurtas && order.Status != OrderStatuses.Pateiktas)
                return BadRequest($"Negalima atnaujinti produkto prie užsakymo dėl statuso. Statusas - {order.Status}.");

            if (updatedProduct.Price <= 0)
                return BadRequest($"Kaina turi būti teigiama.");

            if (updatedProduct.Quantity <= 0)
                return BadRequest($"Netinkamas produkto kiekis: {updatedProduct.Quantity}.");

            var productFromDto = mapper.Map<ProductDto, Product>(updatedProduct, product);
            productFromDto.OrderId = orderId;
            product.CanBeBought = false;
            if (productFromDto.Price != product.Price || productFromDto.Quantity != product.Quantity)
            {
                order.Subtotal -= (decimal) product.Price * product.Quantity;
                order.Subtotal += (decimal) productFromDto.Price * productFromDto.Quantity;
                order.Total = order.Subtotal + 5; // TODO Investigate
            }
            try
            {
                await service.UpdateProduct(id, productFromDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko atnaujinti produkto. Klaida: {ex.Message}.");
            }
            return StatusCode(204);
        }

        [HttpPost]
        [Route("api/[controller]s")]
        public async Task<IActionResult> CreateProduct(CreateProductDto newProduct)
        {
            var mapDtoToProduct = mapper.Map<CreateProductDto, Product>(newProduct);

            if (mapDtoToProduct.CanBeBought && (mapDtoToProduct.Price == null || mapDtoToProduct.Price <= 0))
                return BadRequest($"Kainos laukelis turi būti užpildytas arba prekė turi būti neparduodama.");

            if (newProduct.Quantity <= 0)
            {
                return BadRequest($"Netinkamas produkto kiekis: {newProduct.Quantity}.");
            }
            try
            {
                await service.CreateProduct(mapDtoToProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko sukurti produkto. Klaida: {ex.Message}.");
            }
            return StatusCode(201);
        }

        [HttpPost]
        [Route("api/Orders/{orderId}/[controller]s")]
        public async Task<IActionResult> CreateProduct(CreateProductDto newProduct, int orderId)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            if (order.Status != OrderStatuses.Sukurtas && order.Status != OrderStatuses.Pateiktas)
                return BadRequest($"Negalima sukurti produkto prie užsakymo dėl statuso. Statusas - {order.Status.ToString().ToLower()}.");

            if (newProduct.Price == null || newProduct.Price <= 0)
                return BadRequest($"Kainos laukelis turi būti užpildytas.");

            if (newProduct.Quantity <= 0)
                return BadRequest($"Netinkamas produkto kiekis: {newProduct.Quantity}.");

            var mapDtoToProduct = mapper.Map<CreateProductDto, Product>(newProduct);
            mapDtoToProduct.OrderId = orderId;
            mapDtoToProduct.CanBeBought = false;
            mapDtoToProduct.IsDisplayed = false;
            mapDtoToProduct.Creator = order.Orderer;
            order.Subtotal += (decimal) mapDtoToProduct.Price * mapDtoToProduct.Quantity;
            order.DateEditted = DateTime.UtcNow;
            order.Total = order.Subtotal + 5;
            try
            {
                await service.CreateProduct(mapDtoToProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko sukurti produkto. Klaida: {ex.Message}.");
            }
            return StatusCode(201);
        }

        [HttpDelete]
        [Route("api/[controller]s/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await service.GetProduct(id);
            if (product == null)
                return NotFound($"Produktas su Id {id} nerastas.");

            if (product.OrderId != null)
                return NotFound($"Produkto (Id={id}) negalima ištrinti, pašalinkite produktą iš užsakymo (Id={product.OrderId}).");

            try
            {
                await service.DeleteProduct(product);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko pašalinti produkto. Klaida: {ex.Message}.");
            }
            return StatusCode(204);
        }

        [HttpDelete]
        [Route("api/Orders/{orderId}/[controller]s/{id}")]
        public async Task<IActionResult> DeleteProduct(int id, int orderId)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            var product = await service.GetProduct(id);
            if (product == null)
                return NotFound($"Produktas su Id {id} nerastas.");

            if (order.Status != OrderStatuses.Sukurtas && order.Status != OrderStatuses.Pateiktas)
                return BadRequest($"Negalima ištrinti produkto dėl statuso. Statusas - {order.Status.ToString().ToLower()}.");

            order.DateEditted = DateTime.UtcNow;
            order.Subtotal -= (decimal)product.Price * product.Quantity;
            order.Total = order.Subtotal >= 0 ? order.Subtotal + 5 : 0;
            try
            {
                await service.DeleteProduct(product);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko pašalinti produkto. Klaida: {ex.Message}.");
            }
            return StatusCode(204);
        }

        [HttpPatch]
        [Route("api/Orders/{orderId}/[controller]s/{id}")] //??? perkelti i put, parasyti metoda kuris pereitu per visus products ir isimtu is order, jei orderid = null
        public async Task<IActionResult> RemoveProductFromOrder(int id, int orderId)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            var product = await service.GetProduct(id);
            if (product == null)
                return NotFound($"Produktas su Id {id} nerastas.");

            if (order.Status != OrderStatuses.Sukurtas && order.Status != OrderStatuses.Pateiktas)
                return BadRequest($"Negalima pašalinti produkto iš užsakymo dėl statuso. Statusas - {order.Status.ToString().ToLower()}.");

            order.Subtotal -= (decimal)product.Price * product.Quantity;
            order.Total = order.Subtotal >= 0 ? order.Subtotal + 5 : 0;
            order.DateEditted = DateTime.UtcNow;
            product.CanBeBought = true;
            try
            {
                await service.RemoveProductFromOrder(product.Id);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko produkto pašalinti produkto iš užsakymo. Klaida: {ex.Message}.");
            }
            return StatusCode(204);
        }

        [HttpPatch]
        [Route("api/Orders/{orderId}/[controller]s/{id}/add")] // ???
        public async Task<IActionResult> AddExistingProductToOrder(int id, int orderId)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            var product = await service.GetProduct(id);
            if (product == null)
                return NotFound($"Produktas su Id {id} nerastas.");

            if (order.Status != OrderStatuses.Sukurtas && order.Status != OrderStatuses.Pateiktas)
                return BadRequest($"Negalima pridėti produkto prie užsakymo dėl statuso. Statusas - {order.Status.ToString().ToLower()}.");

            if (!product.CanBeBought)
                return BadRequest($"Produktas (Id={id}) neparduodamas.");

            if (product.Price == null || product.Price <= 0)
                return BadRequest($"Kainos laukelis turi būti užpildytas.");

            if (product.Quantity <= 0)
                return BadRequest($"Netinkamas produkto kiekis (Produkto Id={id}).");

            order.Subtotal += (decimal)product.Price * product.Quantity;
            order.Total = order.Subtotal + 5;
            product.CanBeBought = false;
            product.OrderId = orderId;
            order.DateEditted = DateTime.UtcNow;
            try
            {
                await service.AddExistingProductToOrder(product);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko produkto pridėti prie užsakymo. Klaida: {ex.Message}.");
            }
            return StatusCode(204);
        }
    }
}
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

        [HttpPut]
        [Route("api/Orders/{orderId}/[controller]s/{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, ProductDto updatedProduct, int orderId)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            var product = await service.GetProduct(id);
            if (product == null)
                return NotFound($"Produktas su Id {id} nerastas.");

            if (order.Status != OrderStatuses.Sukurtas && order.Status != OrderStatuses.Pateiktas)
            {
                return BadRequest($"Negalima atnaujinti produkto prie užsakymo dėl statuso. Statusas - {order.Status}");
            }

            var productFromDto = mapper.Map<ProductDto, Product>(updatedProduct, product);
            productFromDto.OrderId = orderId;
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
        [Route("api/[controller]s")]
        public async Task<IActionResult> CreateProduct(CreateProductDto newProduct)
        {
            var mapDtoToProduct = mapper.Map<CreateProductDto, Product>(newProduct);
            mapDtoToProduct.isNew = true;
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

        [HttpPost]
        [Route("api/Orders/{orderId}/[controller]s")]
        public async Task<IActionResult> CreateProduct(CreateProductDto newProduct, int orderId)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            if (order.Status != OrderStatuses.Sukurtas && order.Status != OrderStatuses.Pateiktas)
            {
                return BadRequest($"Negalima sukurti produkto prie užsakymo dėl statuso. Statusas - {order.Status}");
            }

            var mapDtoToProduct = mapper.Map<CreateProductDto, Product>(newProduct);
            mapDtoToProduct.OrderId = orderId;
            mapDtoToProduct.isNew = true;
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
                return BadRequest($"Nepavyko pašalinti produkto. Klaida: {ex.Message}");
            }
            return Ok(); //TODO: check code
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
            {
                return BadRequest($"Negalima ištrinti produkto dėl statuso. Statusas - {order.Status}");
            }

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

        [HttpPatch]
        [Route("api/Orders/{orderId}/[controller]s/{id}")]
        public async Task<IActionResult> RemoveProductFromOrder(int id, int orderId)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            var product = await service.GetProduct(id);
            if (product == null)
                return NotFound($"Produktas su Id {id} nerastas.");

            if (order.Status != OrderStatuses.Sukurtas && order.Status != OrderStatuses.Pateiktas)
            {
                return BadRequest($"Negalima pašalinti produkto iš užsakymo dėl statuso. Statusas - {order.Status}");
            }

            try
            {
                await service.RemoveProductFromOrder(product.Id);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko produkto pašalinti produkto iš užsakymo. Klaida: {ex.Message}");
            }
            return Ok(); //TODO: check code
        }

        [HttpPatch]
        [Route("api/Orders/{orderId}/[controller]s/{id}")]
        public async Task<IActionResult> AddExistingProductToOrder(int id, int orderId)
        {
            var order = await orderService.GetOrder(orderId);
            if (order == null)
                return NotFound($"Užsakymas (Id={orderId}) nerastas.");

            var product = await service.GetProduct(id);
            if (product == null)
                return NotFound($"Produktas su Id {id} nerastas.");

            if (order.Status != OrderStatuses.Sukurtas && order.Status != OrderStatuses.Pateiktas)
            {
                return BadRequest($"Negalima pridėti produkto iš užsakymo dėl statuso. Statusas - {order.Status}");
            }
            product.OrderId = orderId;
            try
            {
                await service.AddExistingProductToOrder(product);
            }
            catch (Exception ex)
            {
                return BadRequest($"Nepavyko produkto . Klaida: {ex.Message}");
            }
            return Ok(); //TODO: check code
        }
    }
}
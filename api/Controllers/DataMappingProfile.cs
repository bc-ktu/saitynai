using api.Data.DTOs;
using api.DTOs;
using api.Entities;
using api.Models;
using AutoMapper;

namespace api.Controllers
{
    internal class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<UpdateProductQuantityDto, ProductDto>();

            CreateMap<Comment, CommentDto>();
            CreateMap<CommentDto, Comment>();
            CreateMap<CreateCommentDto, Comment>();
            CreateMap<UpdateCommentDto, Comment>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<CreateOrderDto, Order>();
            CreateMap<UpdateOrderDto, Order>();
            CreateMap<UpdateOrderStatusDto, Order>();
        }
    }

}
using api.Data.DTOs;
using api.Data.Entities;
using api.DTOs;
using api.Entities;
using api.Models;
using AutoMapper;
using System.Collections.Generic;

namespace api.Controllers
{
    internal class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<CreateProductDto, Product>();

            CreateMap<Comment, CommentDto>();
            CreateMap<Comment, CommentWithAuthorDto>();
            CreateMap<CommentDto, Comment>();
            CreateMap<CreateCommentDto, Comment>();
            CreateMap<UpdateCommentDto, Comment>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
           // CreateMap<CreateOrderDto, Order>();
            CreateMap<UpdateOrderDto, Order>();

            CreateMap<CreateUserDto, RegisteredUser>();
            CreateMap<RegisteredUser, CreateUserDto>();
            CreateMap<RegisteredUser, UserDto>();

            CreateMap<RegisteredUser, SuccessfulLoginResponseDto>();
        }
    }

}
using AutoMapper;
using Business.Implementations.Queries.Books.Dtos;
using Business.Implementations.Queries.Users.Dtos;
using Business.Implementations.Queries.Requests.Dtos;
using Business.Implementations.Queries.Reservations.Dtos;
using DataAccess.Entities;
using Azure.Core;
using Request = DataAccess.Entities.Request;

namespace Business.Implementations.MapperProfiles;
public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserDto>()
            ;
        CreateMap<Request, RequestDto>()
            ;
        CreateMap<Reservation, ReservationDto>()
            ;
        CreateMap<Book, BookDto>()
           ;
    }
}

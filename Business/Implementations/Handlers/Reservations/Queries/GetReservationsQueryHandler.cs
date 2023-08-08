using AutoMapper;
using Business.Abstracts;
using Business.Implementations.Queries.Reservations.Dtos;
using Business.Implementations.Queries.Reservations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;

namespace Business.Implementations.Handlers.Reservations.Queries
{
    public class GetReservationsQueryHandler : IRequestHandler<GetReservationsQuery, IEnumerable<ReservationDto>>
    {
        private readonly IReservationManager _ReservationManager;
        private readonly IMapper _mapper;
        

        public GetReservationsQueryHandler(IReservationManager ReservationManager, IMapper mapper)
        {
            
            _ReservationManager = ReservationManager;
            _mapper = mapper;
           
        }



        public async Task<IEnumerable<ReservationDto>> Handle(GetReservationsQuery Reservation, CancellationToken cancellationToken)
        {
            
            var Reservations = await _ReservationManager.GetReservationsAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ReservationDto>>(Reservations);
        }
    }
}

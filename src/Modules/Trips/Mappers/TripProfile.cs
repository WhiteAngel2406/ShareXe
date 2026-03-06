using AutoMapper;

using ShareXe.Modules.Trips.Entities;
using ShareXe.src.Modules.Trips.Dtos;

namespace ShareXe.src.Modules.Trips.Mappers
{
    public class TripsProfile : Profile
    {
        public TripsProfile()
        {
            // Ánh xạ từ request đầu vào (CreateTripDto) sang Entity (Trip) để lưu vào DB
            // Các trường như DriverProfileId hay Status sẽ được gán thủ công trong Service sau
            CreateMap<CreateTripDto, Trip>();

            // Ánh xạ từ Entity (Trip) trong DB sang DTO (TripDto) để trả về cho Client
            CreateMap<Trip, TripDto>();
        }
    }
}

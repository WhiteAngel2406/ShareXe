using AutoMapper;

namespace ShareXe.Base.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Đây là nơi chứa các quy tắc Map. 
            // Hiện tại chưa có Entity cụ thể 

            // Ví dụ: Khi có Db, bạn sẽ bỏ comment dòng dưới
            // CreateMap<Car, CarDto>().ReverseMap();
            // CreateMap<Trip, TripDto>().ReverseMap();

            // Nguyên tắc: CreateMap<Source, Destination>();
        }
    }
}

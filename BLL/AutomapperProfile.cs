using AutoMapper;
using BLL.Models;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AutomapperProfile:Profile
    {
        public AutomapperProfile()
        {
            CreateMap<BookDto, Book>()
                .ReverseMap();

            CreateMap<Book, BookFullInfo>()
                .ForMember(b => b.Name, r => r.MapFrom(x => x.Customer.Name))
                .ForMember(b => b.Surname, r => r.MapFrom(x => x.Customer.Surname))
                .ForMember(b => b.Email, r => r.MapFrom(x => x.Customer.Email))
                .ForMember(b => b.Category, r => r.MapFrom(x => x.Room.Category))
                .ForMember(b => b.VisitorsNumber, r => r.MapFrom(x => x.Room.VisitorsNumber))
                .ForMember(b => b.Description, r => r.MapFrom(x => x.Room.Description))
                .ForMember(b => b.imgName, r => r.MapFrom(x => x.Room.ImgName))
                .ReverseMap();

            CreateMap<BookFullInfo, Customer>()
                .ReverseMap();

            CreateMap<BookFullInfo, BookCreateModel>()
                .ReverseMap();

            CreateMap<BookCreateModel, Customer>()
                .ReverseMap();

            CreateMap<Room, RoomDto>();

            CreateMap<RoomDto, Room>()
                .ForMember(x => x.ImgName, opt => opt.Condition(src => (src.imgName != null)));

            CreateMap<RoomFullInfo, Room>()
                .ReverseMap();

            CreateMap<CustomerDto, Customer>()
                .ReverseMap();

            CreateMap<RoomWithImages, Room>()
                .ForMember(m=>m.ImgName, x => x.Condition((src, dest, sourceValue) => sourceValue != null));

            CreateMap<Room, RoomWithImages>();

        }
    }
}

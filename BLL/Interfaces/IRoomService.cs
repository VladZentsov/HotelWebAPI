using BLL.Models;
using Business.Interfaces;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IRoomService:ICrud<RoomDto>
    {
        Task<RoomWithImages> GetByIdWithDetailsAsync(string id);
        Task<IEnumerable<RoomFullInfo>> GetBookedRoomsWithDetails();
        Task<IEnumerable<IEnumerable<RoomsSettlement>>> GetRoomsSettlement();
        Task<RoomFullInfo> GetBookedRoomWithDetailsById(string id);

    }
}

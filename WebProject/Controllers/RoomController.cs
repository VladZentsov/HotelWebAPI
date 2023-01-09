using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[TradeMarketExceptionFilterAttribute]
    public class RoomController: ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAll()
        {
            return Content(JsonConvert.SerializeObject(await _roomService.GetAllAsync()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<RoomWithImages>>> GetRoomById(string id)
        {
            return Content(JsonConvert.SerializeObject(await _roomService.GetByIdAsync(id)));
        }

        [HttpGet("roomDetailed/{id}")]
        public async Task<ActionResult<IEnumerable<RoomWithImages>>> GetRoomDetailedById(string id)
        {
            return Content(JsonConvert.SerializeObject(await _roomService.GetByIdWithDetailsAsync(id)));
        }

        [HttpGet("roomFullInfo/{id}")]
        public async Task<ActionResult<RoomFullInfo>> GetRoomFullInfoById(string id)
        {
            return Content(JsonConvert.SerializeObject(await _roomService.GetBookedRoomWithDetailsById(id)));
        }

        [HttpGet("roomFullInfos")]
        public async Task<ActionResult<IEnumerable<RoomFullInfo>>> GetRoomsFullInfo()
        {
            return Content(JsonConvert.SerializeObject(await _roomService.GetBookedRoomsWithDetails()));
        }

        [HttpGet("roomsSettlement")]
        public async Task<ActionResult<IEnumerable<IEnumerable<RoomsSettlement>>>> GetRoomsSettlement()
        {
            return Content(JsonConvert.SerializeObject(await _roomService.GetRoomsSettlement()));
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] RoomWithImages value)
        {
            await _roomService.AddAsync(value);

            return Ok(value);
        }

        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] RoomWithImages value)
        {
            var result = await _roomService.UpdateAsync(value);

            return Content(JsonConvert.SerializeObject(result));
        }

        [HttpGet("roomFilter")]
        public async Task<ActionResult> FilteredRooms([FromBody] RoomFilter value)
        {
            return Content(JsonConvert.SerializeObject(await _roomService.RoomsFilterSearch(value)));
        }



        //// POST: api/customers
        //[HttpGet]
        //public async Task<ActionResult> Add([FromBody]  value)
        //{
        //    await _customerService.AddAsync(value);

        //    return Ok(value);
        //}

    }
}

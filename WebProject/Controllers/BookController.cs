using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebProject.Filters;

namespace WebProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAll()
        {
            return Content(JsonConvert.SerializeObject(await _bookService.GetAllAsync()));
        }

        // POST: api/customers
        [HttpGet("freeBooksForRoom/{roomId}")]
        public async Task<ActionResult> GetFreeBookDatesForRoom(string roomId)
        {
            var result = JsonConvert.SerializeObject(await _bookService.GetFreeBookDates(roomId));
            return Content(result);
        }

        [HttpGet("getBook/{id}")]
        public async Task<ActionResult> GetBookById(string id)
        {
            return Content(JsonConvert.SerializeObject(await _bookService.GetByIdAsync(id)));
        }

        [HttpPost("createBook")]
        public async Task<ActionResult> CreateBook([FromBody] BookCreateModel value)
         
        {

            DateTime startDate = value.StartDate ?? throw new Exception();
            value.StartDate = startDate.AddDays(1);

            DateTime endDate = value.EndDate ?? throw new Exception();
            value.EndDate = endDate.AddDays(1);

            await _bookService.CreateBook(value);

            return Ok(value);
        }

        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] BookDto value)
        {
            var result = await _bookService.UpdateAsync(value);

            return Content(JsonConvert.SerializeObject(result));
        }

    }
}

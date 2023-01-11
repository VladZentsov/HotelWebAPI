using BLL.Models;
using FluentAssertions;
using HotelTests.DALTests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelTests.IntegrationTests
{
    public class RoomIntegrationTest
    {
        private HotelWebApplicationFactory _factory;
        private HttpClient _client;
        private const string RequestUri = "api/Room/";

        [SetUp]
        public void Init()
        {
            _factory = new HotelWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task RoomController_GetAll_ReturnsAllFromDb()
        {
            //arrange
            var expected = UnitTestHelper.RoomsDto;

            // act
            var httpResponse = await _client.GetAsync(RequestUri);

            // assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<RoomDto>>(stringResponse).ToList();

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("1")]
        [TestCase("2")]
        public async Task RoomController_GetRoomById_ReturnsAllFromDb(string id)
        {
            //arrange
            var expected = UnitTestHelper.GetRoomDto(id);

            // act
            var httpResponse = await _client.GetAsync(RequestUri+id);

            // assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<RoomDto>(stringResponse);

            actual.Should().BeEquivalentTo(expected);
        }
        //[TestCase("1")]
        //[TestCase("2")]
        //public async Task RoomController_GetRoomDetailedById_ReturnsAllFromDb(string id)
        //{
        //    //arrange
        //    var expected = UnitTestHelper.GetRoomDto(id);

        //    // act
        //    var httpResponse = await _client.GetAsync(RequestUri + "roomDetailed/"+ id);

        //    // assert
        //    httpResponse.EnsureSuccessStatusCode();
        //    var stringResponse = await httpResponse.Content.ReadAsStringAsync();
        //    var actual = JsonConvert.DeserializeObject<RoomDto>(stringResponse);

        //    actual.Should().BeEquivalentTo(expected);
        //}


    }
}

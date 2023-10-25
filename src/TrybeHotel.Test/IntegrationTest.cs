namespace TrybeHotel.Test;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Diagnostics;
using System.Xml;
using System.IO;
using FluentAssertions;
using TrybeHotel.Dto;
using System.Net;
using System.Text;

[Collection("Integration Test Collection")]
public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    public HttpClient _clientTest;

    public IntegrationTest(WebApplicationFactory<Program> factory)
    {
        //_factory = factory;
        _clientTest = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TrybeHotelContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ContextTest>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTestDatabase");
                });
                services.AddScoped<ITrybeHotelContext, ContextTest>();
                services.AddScoped<ICityRepository, CityRepository>();
                services.AddScoped<IHotelRepository, HotelRepository>();
                services.AddScoped<IRoomRepository, RoomRepository>();
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                using (var appContext = scope.ServiceProvider.GetRequiredService<ContextTest>())
                {
                    appContext.Database.EnsureCreated();
                    appContext.Database.EnsureDeleted();
                    appContext.Database.EnsureCreated();
                    appContext.Cities.Add(new City { CityId = 1, Name = "Manaus" });
                    appContext.Cities.Add(new City { CityId = 2, Name = "Palmas" });
                    appContext.SaveChanges();
                    appContext.Hotels.Add(new Hotel { HotelId = 1, Name = "Trybe Hotel Manaus", Address = "Address 1", CityId = 1 });
                    appContext.Hotels.Add(new Hotel { HotelId = 2, Name = "Trybe Hotel Palmas", Address = "Address 2", CityId = 2 });
                    appContext.Hotels.Add(new Hotel { HotelId = 3, Name = "Trybe Hotel Ponta Negra", Address = "Addres 3", CityId = 1 });
                    appContext.SaveChanges();
                    appContext.Rooms.Add(new Room { RoomId = 1, Name = "Room 1", Capacity = 2, Image = "Image 1", HotelId = 1 });
                    appContext.Rooms.Add(new Room { RoomId = 2, Name = "Room 2", Capacity = 3, Image = "Image 2", HotelId = 1 });
                    appContext.Rooms.Add(new Room { RoomId = 3, Name = "Room 3", Capacity = 4, Image = "Image 3", HotelId = 1 });
                    appContext.Rooms.Add(new Room { RoomId = 4, Name = "Room 4", Capacity = 2, Image = "Image 4", HotelId = 2 });
                    appContext.Rooms.Add(new Room { RoomId = 5, Name = "Room 5", Capacity = 3, Image = "Image 5", HotelId = 2 });
                    appContext.Rooms.Add(new Room { RoomId = 6, Name = "Room 6", Capacity = 4, Image = "Image 6", HotelId = 2 });
                    appContext.Rooms.Add(new Room { RoomId = 7, Name = "Room 7", Capacity = 2, Image = "Image 7", HotelId = 3 });
                    appContext.Rooms.Add(new Room { RoomId = 8, Name = "Room 8", Capacity = 3, Image = "Image 8", HotelId = 3 });
                    appContext.Rooms.Add(new Room { RoomId = 9, Name = "Room 9", Capacity = 4, Image = "Image 9", HotelId = 3 });
                    appContext.SaveChanges();
                }
            });
        }).CreateClient();
    }

    [Trait("Category", "City Endpoints")]
    [Theory(DisplayName = "GET /city should return a 200 status")]
    [InlineData("/city")]
    public async Task GetCityShouldReturn200StatusCode(string url)
    {
        HttpResponseMessage response = await _clientTest.GetAsync(url);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Trait("Category", "City Endpoints")]
    [Theory(DisplayName = "GET /city should return a list of cities")]
    [InlineData("/city")]
    public async Task GetCityShouldReturnListOfCities(string url)
    {
        HttpResponseMessage response = await _clientTest.GetAsync(url);
        var expectedCities = new List<CityDto>
        {
            new CityDto { CityId = 1, Name = "Manaus" },
            new CityDto { CityId = 2, Name = "Palmas" }
        };

        var cities = JsonConvert.DeserializeObject<List<CityDto>>(await response.Content.ReadAsStringAsync());

        cities.Should().BeEquivalentTo(expectedCities);
    }

    [Trait("Category", "City Endpoints")]
    [Theory(DisplayName = "POST /city should return a 201 status")]
    [InlineData("/city")]
    public async Task PostCityShouldReturn201StatusCode(string url)
    {
        var newCity = new { Name = "Rio de Janeiro" };
        var requestBody = new StringContent(JsonConvert.SerializeObject(newCity), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _clientTest.PostAsync(url, requestBody);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Trait("Category", "City Endpoints")]
    [Theory(DisplayName = "POST /city should return the correct response content")]
    [InlineData("/city")]
    public async Task PostCityShouldReturnCorrectResponseContent(string url)
    {
        var newCity = new { Name = "Rio de Janeiro" };
        var requestBody = new StringContent(JsonConvert.SerializeObject(newCity), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _clientTest.PostAsync(url, requestBody);
        var expectedResponse = new CityDto { CityId = 3, Name = "Rio de Janeiro" };

        var city = JsonConvert.DeserializeObject<CityDto>(await response.Content.ReadAsStringAsync());

        city.Should().BeEquivalentTo(expectedResponse);
    }

    [Trait("Category", "Hotel Endpoints")]
    [Theory(DisplayName = "GET /hotel should return a 200 status")]
    [InlineData("/hotel")]
    public async Task GetHotelShouldReturn200StatusCode(string url)
    {
        HttpResponseMessage response = await _clientTest.GetAsync(url);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Trait("Category", "Hotel Endpoints")]
    [Theory(DisplayName = "GET /hotel should return a list of hotels")]
    [InlineData("/hotel")]
    public async Task GetHotelShouldReturnListOfHotels(string url)
    {
        HttpResponseMessage response = await _clientTest.GetAsync(url);
        var expectedHotels = new List<HotelDto>
        {
            new HotelDto { HotelId = 2, Name = "Trybe Hotel Palmas", Address = "Address 2", CityId = 2, CityName = "Palmas" },
            new HotelDto { HotelId = 1, Name = "Trybe Hotel Manaus", Address = "Address 1", CityId = 1, CityName = "Manaus" },
            new HotelDto { HotelId = 3, Name = "Trybe Hotel Ponta Negra", Address = "Addres 3", CityId = 1, CityName = "Manaus" }
        };

        var hotels = JsonConvert.DeserializeObject<List<HotelDto>>(await response.Content.ReadAsStringAsync());

        hotels.Should().BeEquivalentTo(expectedHotels);
    }

    [Trait("Category", "Hotel Endpoints")]
    [Theory(DisplayName = "POST /hotel should return a 201 status")]
    [InlineData("/hotel")]
    public async Task PostHotelShouldReturn201StatusCode(string url)
    {
        var newHotel = new { Name = "Trybe Hotel RJ", Address = "Address 4", CityId = 2 };
        var requestBody = new StringContent(JsonConvert.SerializeObject(newHotel), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _clientTest.PostAsync(url, requestBody);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Trait("Category", "Hotel Endpoints")]
    [Theory(DisplayName = "POST /hotel should return the correct response content")]
    [InlineData("/hotel")]
    public async Task PostHotelShouldReturnCorrectResponseContent(string url)
    {
        var newHotel = new { Name = "Trybe Hotel RJ", Address = "Address 4", CityId = 2 };
        var requestBody = new StringContent(JsonConvert.SerializeObject(newHotel), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _clientTest.PostAsync(url, requestBody);
        var expectedResponse = new HotelDto { HotelId = 4, Name = "Trybe Hotel RJ", Address = "Address 4", CityId = 2, CityName = "Palmas" };

        var hotel = JsonConvert.DeserializeObject<HotelDto>(await response.Content.ReadAsStringAsync());

        hotel.Should().BeEquivalentTo(expectedResponse);
    }

    [Trait("Category", "Room Endpoints")]
    [Theory(DisplayName = "GET /room should return a 200 status")]
    [InlineData("/room/1")]
    public async Task GetRoomShouldReturn200StatusCode(string url)
    {
        HttpResponseMessage response = await _clientTest.GetAsync(url);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Trait("Category", "Room Endpoints")]
    [Theory(DisplayName = "GET /room should return a list of rooms")]
    [InlineData("/room/1")]
    public async Task GetRoomShouldReturnListOfRooms(string url)
    {
        HttpResponseMessage response = await _clientTest.GetAsync(url);
        var expectedRooms = new List<RoomDto>
    {
        new RoomDto { RoomId = 3, Name = "Room 3", Capacity = 4, Image = "Image 3", Hotel = new HotelDto { HotelId = 1, Name = "Trybe Hotel Manaus", Address = "Address 1", CityId = 1, CityName = "Manaus" } },
        new RoomDto { RoomId = 2, Name = "Room 2", Capacity = 3, Image = "Image 2", Hotel = new HotelDto { HotelId = 1, Name = "Trybe Hotel Manaus", Address = "Address 1", CityId = 1, CityName = "Manaus" } },
        new RoomDto { RoomId = 1, Name = "Room 1", Capacity = 2, Image = "Image 1", Hotel = new HotelDto { HotelId = 1, Name = "Trybe Hotel Manaus", Address = "Address 1", CityId = 1, CityName = "Manaus" } }
    };

        var rooms = JsonConvert.DeserializeObject<List<RoomDto>>(await response.Content.ReadAsStringAsync());

        rooms.Should().BeEquivalentTo(expectedRooms);
    }

    [Trait("Category", "Room Endpoints")]
    [Theory(DisplayName = "POST /room should return a 201 status")]
    [InlineData("/room")]
    public async Task PostRoomShouldReturn201StatusCode(string url)
    {
        var newRoom = new { Name = "Room 4", Capacity = 2, Image = "Image 4", HotelId = 2 };
        var requestBody = new StringContent(JsonConvert.SerializeObject(newRoom), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _clientTest.PostAsync(url, requestBody);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Trait("Category", "Room Endpoints")]
    [Theory(DisplayName = "POST /room should return the correct response content")]
    [InlineData("/room")]
    public async Task PostRoomShouldReturnCorrectResponseContent(string url)
    {
        var newRoom = new { Name = "Room 4", Capacity = 2, Image = "Image 4", HotelId = 2 };
        var requestBody = new StringContent(JsonConvert.SerializeObject(newRoom), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _clientTest.PostAsync(url, requestBody);
        var expectedResponse = new RoomDto { RoomId = 10, Name = "Room 4", Capacity = 2, Image = "Image 4", Hotel = new HotelDto { HotelId = 2, Name = "Trybe Hotel Palmas", Address = "Address 2", CityId = 2, CityName = "Palmas" } };

        var room = JsonConvert.DeserializeObject<RoomDto>(await response.Content.ReadAsStringAsync());

        room.Should().BeEquivalentTo(expectedResponse);
    }

    [Trait("Category", "Room Endpoints")]
    [Theory(DisplayName = "DELETE /room should return a 204 status")]
    [InlineData("/room/1")]
    public async Task DeleteRoomShouldReturn204StatusCode(string url)
    {
        HttpResponseMessage response = await _clientTest.DeleteAsync(url);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

}
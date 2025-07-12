using System.Net;
using System.Net.Http.Json;
using internship_entry_task.Models.Game.Request;
using internship_entry_task.Models.Game.Response;

namespace internship_entry_task.Tests;

[Collection("IntegrationTests")]
public class GamesControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<ProgramPlaceholder>>
{
    private readonly HttpClient _client;

    public GamesControllerIntegrationTests(CustomWebApplicationFactory<ProgramPlaceholder> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
        public async Task Create_ReturnsOkAndValidResponse()
        {
            var response = await _client.PostAsync("/v1/games/Create", null);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<GameCreateResponse>();

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.True(result.N > 0);
            Assert.True(result.TurnPlayer == 'X' || result.TurnPlayer == 'O');
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenGameDoesNotExist()
        {
            var nonExistingId = Guid.NewGuid();
            var response = await _client.GetAsync($"/v1/games/{nonExistingId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateAndGet_ReturnsCreatedGame()
        {
            // Создаем игру
            var createResponse = await _client.PostAsync("/v1/games/Create", null);
            var game = await createResponse.Content.ReadFromJsonAsync<GameCreateResponse>();

            // Получаем игру
            var getResponse = await _client.GetAsync($"/v1/games/{game.Id}");

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var fetchedGame = await getResponse.Content.ReadFromJsonAsync<internship_entry_task.Domain.Entities.DbGame>();

            Assert.NotNull(fetchedGame);
            Assert.Equal(game.Id, fetchedGame.Id);
            Assert.Equal(game.N, fetchedGame.N);
        }

        [Fact]
        public async Task MakeMove_ReturnsOk_AndSetsETag()
        {
            // Создаем игру
            var createResponse = await _client.PostAsync("/v1/games/Create", null);
            var game = await createResponse.Content.ReadFromJsonAsync<GameCreateResponse>();

            var moveRequest = new MoveRequest
            {
                Row = 0,
                Col = 0
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"/v1/games/{game.Id}/move")
            {
                Content = JsonContent.Create(moveRequest)
            };

            var moveResponse = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, moveResponse.StatusCode);
            Assert.True(moveResponse.Headers.ETag != null);

            var updatedGame = await moveResponse.Content.ReadFromJsonAsync<internship_entry_task.Domain.Entities.DbGame>();
            Assert.Equal(game.Id, updatedGame.Id);
        }

        [Fact]
        public async Task MakeMove_ReturnsBadRequest_WhenCellAlreadyOccupied()
        {
            // Создаем игру
            var createResponse = await _client.PostAsync("/v1/games/Create", null);
            var game = await createResponse.Content.ReadFromJsonAsync<GameCreateResponse>();

            var moveRequest = new MoveRequest { Row = 0, Col = 0 };

            // Первый ход
            await _client.PostAsJsonAsync($"/v1/games/{game.Id}/move", moveRequest);

            // Повторный ход в ту же ячейку
            var secondResponse = await _client.PostAsJsonAsync($"/v1/games/{game.Id}/move", moveRequest);

            Assert.Equal(HttpStatusCode.BadRequest, secondResponse.StatusCode);
        }
}
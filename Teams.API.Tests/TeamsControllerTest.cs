using System.Collections.Generic;
using System.Linq;
using Auto.VehicleCatalog.API.Tests;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Teams.API.Controllers;
using Xunit;
using Teams.API.Dtos;
using System;

namespace Teams.API.Tests
{
    public class TeamsControllerTest : IClassFixture<DependencyFixture>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TeamsController> _logger;
        private readonly FakeMemoryRepositoryHelper _memoryRepositoryHelper;

        public TeamsControllerTest(DependencyFixture dependencyFixture)
        {
            _mapper = dependencyFixture.ServiceProvider.GetService<IMapper>();
            _logger = Mock.Of<ILogger<TeamsController>>();
            _memoryRepositoryHelper = dependencyFixture.ServiceProvider.GetService<FakeMemoryRepositoryHelper>();
        }

        // Poucos testes negativos dá para se fazer quando 
        // estamos retornando uma lista sem parâmetros ;-)
        [Fact]
        public async void QueryTeamList_ReturnsCorrectTeams()
        {
            // 1A
            TeamsController controller = new TeamsController(_memoryRepositoryHelper
                .RepoWith5Teams(), _mapper, _logger);

            // 2A
            var result = await controller.GetAllTeams();

            // 3A
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TeamForList>>(okResult.Value);

            Assert.Equal(5, returnValue.Count());
        }

        [Fact]
        public async void GetNonExistentTeam_ReturnsNotFound()
        {
            // 1A
            TeamsController controller = new TeamsController(_memoryRepositoryHelper
                .RepoWith5Teams(), _mapper, _logger);
            var id = Guid.NewGuid();

            // 2A
            var result = await controller.GetTeam(id);

            // 3A
            var okResult = Assert.IsType<NotFoundObjectResult>(result);
            dynamic returnValue = okResult.Value;

            Assert.Contains($"Team with id {id} not found.", returnValue.ToString());
        }

        [Fact]
        public async void CreateTeam_AddsTeamToList()
        {
            // 1A
            TeamsController controller = new TeamsController(_memoryRepositoryHelper
                .RepoWith5Teams(), _mapper, _logger);

            var okResult = Assert.IsType<OkObjectResult>(await controller.GetAllTeams());
            var teams = Assert.IsType<List<TeamForList>>(okResult.Value);
            TeamForNew t1 = new TeamForNew()
            {
                name = "fake6",
                id = Guid.Parse(FakeMemoryRepositoryHelper.NOT_IN_USE_1)
            };

            // 2A
            var result = await controller.Post(t1);

            // 3A
            var okResult1 = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<string>(okResult1.Value);
            Assert.Equal("Team created.", returnValue);

            var okResult2 = Assert.IsType<OkObjectResult>(await controller.GetAllTeams());
            var newTeams = Assert.IsType<List<TeamForList>>(okResult2.Value);

            Assert.Equal(newTeams.Count(), teams.Count() + 1);
            Assert.NotNull(newTeams.FirstOrDefault(x => x.id.Equals(Guid.Parse(FakeMemoryRepositoryHelper.NOT_IN_USE_1))));
        }

        [Fact]
        public async void UpdateTeam_ModifiesTeamToList()
        {
            // 1A
            TeamsController controller = new TeamsController(_memoryRepositoryHelper
                .RepoWith5Teams(), _mapper, _logger);

            var okResult = Assert.IsType<OkObjectResult>(await controller.GetAllTeams());
            var teams = Assert.IsType<List<TeamForList>>(okResult.Value);
            var team = teams.First();
            var teamForUpdate = _mapper.Map<TeamForUpdate>(team);
            teamForUpdate.name = "fake99";

            // 2A
            var result = await controller.Put(team.id, teamForUpdate);

            // 3A
            var okResult1 = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<string>(okResult1.Value);
            Assert.Equal("Team updated.", returnValue);

            var okResult2 = Assert.IsType<OkObjectResult>(await controller.GetAllTeams());
            var newTeams = Assert.IsType<List<TeamForList>>(okResult2.Value);

            var teamUpdated = newTeams.First(x => x.id == team.id);
            Assert.Equal("fake99", teamUpdated.name);
        }

        [Fact]
        public async void UpdateNonExistentTeam_ReturnsNotFound()
        {
            // 1A
            TeamsController controller = new TeamsController(_memoryRepositoryHelper
                .RepoWith5Teams(), _mapper, _logger);
            var id = Guid.NewGuid();

            // 2A
            var result = await controller.Put(id, new TeamForUpdate());

            // 3A
            var okResult = Assert.IsType<NotFoundObjectResult>(result);
            dynamic returnValue = okResult.Value;

            Assert.Contains($"Team with id {id} not found.", returnValue.ToString());
        }

        [Fact]
        public async void DeleteTeam_RemovesTeamToList()
        {
            // 1A
            TeamsController controller = new TeamsController(_memoryRepositoryHelper
                .RepoWith5Teams(), _mapper, _logger);

            var okResult = Assert.IsType<OkObjectResult>(await controller.GetAllTeams());
            var teams = Assert.IsType<List<TeamForList>>(okResult.Value);
            var team = teams.First();
            var id = team.id;
            
            // 2A
            var result = await controller.Delete(id);

            // 3A
            var okResult1 = Assert.IsType<NoContentResult>(result);

            var okResult2 = Assert.IsType<OkObjectResult>(await controller.GetAllTeams());
            var newTeams = Assert.IsType<List<TeamForList>>(okResult2.Value);

            Assert.Equal(newTeams.Count(), teams.Count() -1);
        }

         [Fact]
        public async void DeleteNonExistentTeam_ReturnsNotFound()
        {
            // 1A
            TeamsController controller = new TeamsController(_memoryRepositoryHelper
                .RepoWith5Teams(), _mapper, _logger);
            var id = Guid.NewGuid();

            // 2A
            var result = await controller.Delete(id);

            // 3A
            var okResult = Assert.IsType<NotFoundObjectResult>(result);
            dynamic returnValue = okResult.Value;

            Assert.Contains($"Team with id {id} not found.", returnValue.ToString());
        }
    }
}

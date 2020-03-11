using Auto.VehicleCatalog.API.Tests;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Teams.API.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Teams.API.Dtos;

namespace Teams.API.Tests
{
    public class MembersControllerTest : IClassFixture<DependencyFixture>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<MembersController> _logger;
        private readonly FakeMemoryRepositoryHelper _memoryRepositoryHelper;

        public MembersControllerTest(DependencyFixture dependencyFixture)
        {
            _mapper = dependencyFixture.ServiceProvider.GetService<IMapper>();
            _logger = Mock.Of<ILogger<MembersController>>();
            _memoryRepositoryHelper = dependencyFixture.ServiceProvider.GetService<FakeMemoryRepositoryHelper>();
        }

        [Fact]
        public async void QueryAllTeamMembersList_ReturnsCorrectMembers()
        {
            // 1A
            MembersController controller = new MembersController(_memoryRepositoryHelper
                .RepoWith5Teams(), _mapper, _logger);
            Guid teamId = new Guid(FakeMemoryRepositoryHelper.TEAM_1_ID);

            // 2A
            var result = await controller.GetAllMembers(teamId);

            // 3A
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<MemberForList>>(okResult.Value);

            Assert.Equal(2, returnValue.Count());
        }
    }
}

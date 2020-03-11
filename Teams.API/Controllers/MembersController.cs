using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Teams.API.Dtos;
using Teams.API.Persistence;

namespace Teams.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/teams/{teamId}/[controller]")] // Rota básica
    public class MembersController: ControllerBase
    {
        private readonly ITeamRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<MembersController> _logger;

        public MembersController(ITeamRepository repository, IMapper mapper, ILogger<MembersController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMembers(Guid teamId)
        {
            // recuperar o time
            var team = await _repository.GetTeam(teamId);

            // mapear os membros para o nosso DTO
            var result = _mapper.Map<IEnumerable<MemberForList>>(team.Members);

            // retornar o resultado
            return this.Ok(result);
        }
    }
}

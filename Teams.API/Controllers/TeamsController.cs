using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Teams.API.Dtos;
using Teams.API.Model;
using Teams.API.Persistence;

namespace Teams.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TeamsController> _logger;

        public TeamsController(ITeamRepository repository, IMapper mapper, ILogger<TeamsController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async virtual Task<IActionResult> GetAllTeams()
        {
            // recuperar todos os times
            var teams = await _repository.GetTeams();

            // mapear os times para o nosso DTO
            var result = _mapper.Map<IEnumerable<TeamForList>>(teams);

            // retornar o resultado
            return this.Ok(result);
        }

        [HttpGet("{id:guid}", Name = nameof(GetTeam))]
        public async Task<IActionResult> GetTeam(Guid id)
        {
            // recuperar o time pelo id
            var team = await _repository.GetTeam(id);

            // Se for nulo então não existe um time com o id informado
            if (team == null)
            {
                return NotFound(new { Message = $"Team with id {id} not found." });
            }
            else
            {
                // mapear o time para o DTO
                var result = _mapper.Map<TeamForDetail>(team);

                // retornar o resultado
                return Ok(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(TeamForNew team)
        {
            // mapear o DTO para o modelo
            var teamMapped = _mapper.Map<Team>(team);

            // Se não foi passado um guid então criamos um
            if (Guid.Empty.Equals(teamMapped))
            {
                teamMapped.Id = Guid.NewGuid();
            }

            // Adiciona o novo time
            _repository.AddTeam(teamMapped);

            // Salva o time no bd
            if (await _repository.SaveAll())
            {
                return CreatedAtRoute(nameof(GetTeam), new { id = teamMapped.Id }, "Team created.");
            }
            else
            {
                string errorMsg = "Failed adding team on server.";
                _logger.LogError(errorMsg);
                throw new Exception(errorMsg);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, TeamForUpdate teamForUpdate)
        {
            var team = await _repository.GetTeam(id);

            // Se for nulo então não existe um time com o id informado
            if (team == null)
            {
                return NotFound(new { Message = $"Team with id {id} not found." });
            }
            else
            {
                // Atualiza o nome do time
                team.Name = teamForUpdate.name;

                // Salva o banco de dados
                if (await _repository.SaveAll())
                {
                    return CreatedAtRoute(nameof(GetTeam), new { id = team.Id }, "Team updated.");
                }
                else
                {
                    string errorMsg = "Failed updating team on server.";
                    _logger.LogError(errorMsg);
                    throw new Exception(errorMsg);
                }
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var team = await _repository.GetTeam(id);

            // Se for nulo então não existe um time com o id informado
            if (team == null)
            {
                return NotFound(new { Message = $"Team with id {id} not found." });
            }
            else
            {
                // Efetua a remoção do time
                _repository.RemoveTeam(team);

                // Salva o banco de dados
                if (await _repository.SaveAll())
                {
                    return NoContent();
                }
                else
                {
                    string errorMsg = "Failed deleting team on server";
                    _logger.LogError(errorMsg);
                    throw new Exception(errorMsg);
                }
            }
        }
    }
}

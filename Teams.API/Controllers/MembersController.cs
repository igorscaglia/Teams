using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class MembersController : ControllerBase
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

        [HttpGet("api/teams/{teamId:guid}/[controller]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllMembers(Guid teamId)
        {
            // recuperar o time
            var team = await _repository.GetTeam(teamId);

            if (team == null)
            {
                return NotFound();
            }
            else
            {
                // mapear os membros para o nosso DTO
                var result = _mapper.Map<IEnumerable<MemberForList>>(team.Members);

                // retornar o resultado
                return this.Ok(result);
            }
        }

        [HttpGet("api/teams/{teamId:guid}/[controller]/{memberId:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetMember(Guid teamId, Guid memberId)
        {
            // recuperar o time
            var team = await _repository.GetTeam(teamId);

            if (team == null)
            {
                return NotFound();
            }
            else
            {
                var member = team.Members.FirstOrDefault(p => p.Id == memberId);

                if (member == null)
                {
                    return NotFound();
                }
                else
                {
                    // mapear o membro para o nosso DTO
                    var result = _mapper.Map<MemberForDetail>(member);

                    // retornar o resultado
                    return this.Ok(result);
                }
            }
        }

        [HttpGet("api/members/{memberId:guid}/team")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTeamForMember(Guid memberId)
        {
            var teamId = await GetTeamIdForMember(memberId);

            if (teamId != Guid.Empty)
            {
                return this.Ok(new
                {
                    TeamID = teamId
                });
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpPost("api/teams/{teamId:guid}/[controller]")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> PostMember(Guid teamId, MemberForNew memberForNew)
        {
            var team = await _repository.GetTeam(teamId);

            if (team == null)
            {
                return NotFound(new { Message = $"Team with id {teamId} not found." });
            }
            else
            {
                var member = _mapper.Map<Member>(memberForNew);

                // Se não foi passado um guid então criamos um
                if (Guid.Empty.Equals(member.Id))
                {
                    member.Id = Guid.NewGuid();
                }

                team.Members.Add(member);

                // Salva o membro no bd
                if (await _repository.SaveAll())
                {
                    return CreatedAtAction(nameof(GetMember), new { teamId = team.Id, memberId = member.Id }, "Member created.");
                }
                else
                {
                    string errorMsg = "Failed adding member on server.";
                    _logger.LogError(errorMsg);
                    return this.NotFound(errorMsg);
                }
            }
        }

        [HttpPut("api/teams/{teamId:guid}/[controller]/{memberId:guid}")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> PutMember(Guid teamId, Guid memberId, MemberForUpdate updatedMember)
        {
            var team = await _repository.GetTeam(teamId);

            if (team == null)
            {
                return NotFound(new { Message = $"Team with id {teamId} not found." });
            }
            else
            {
                var member = team.Members.FirstOrDefault(m => m.Id == memberId);

                if (member == null)
                {
                    return NotFound(new { Message = $"Member with id {memberId} not found." });
                }
                else
                {
                    // Atualiza o membro
                    _mapper.Map(updatedMember, member);

                    // Salva o banco de dados
                    if (await _repository.SaveAll())
                    {
                        return CreatedAtAction(nameof(GetMember), new { teamId = team.Id, memberId = member.Id }, "Member updated.");
                    }
                    else
                    {
                        string errorMsg = "Failed updating member on server.";
                        _logger.LogError(errorMsg);
                        return this.NotFound(errorMsg);
                    }
                }
            }
        }

        [HttpDelete("api/teams/{teamId:guid}/[controller]/{memberId:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteMember(Guid teamId, Guid memberId)
        {
            var team = await _repository.GetTeam(teamId);

            if (team == null)
            {
                return NotFound(new { Message = $"Team with id {teamId} not found." });
            }
            else
            {
                var member = team.Members.FirstOrDefault(m => m.Id == memberId);

                if (member == null)
                {
                    return NotFound(new { Message = $"Member with id {memberId} not found." });
                }
                else
                {
                    // Efetua a remoção do membro
                    team.Members.Remove(member);

                    // Salva o banco de dados
                    if (await _repository.SaveAll())
                    {
                        return NoContent();
                    }
                    else
                    {
                        string errorMsg = "Failed deleting member on server.";
                        _logger.LogError(errorMsg);
                        return this.NotFound(errorMsg);
                    }
                }
            }
        }

        private async Task<Guid> GetTeamIdForMember(Guid memberId)
        {
            foreach (var team in await _repository.GetTeams())
            {
                var member = team.Members.FirstOrDefault(m => m.Id == memberId);
                if (member != null)
                {
                    return team.Id;
                }
            }
            return Guid.Empty;
        }
    }
}

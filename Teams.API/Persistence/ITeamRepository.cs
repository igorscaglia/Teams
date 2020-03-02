using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teams.API.Model;

namespace Teams.API.Persistence
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetTeams();
        Task<Team> GetTeam(Guid id);
        void AddTeam(Team team);
        void RemoveTeam(Team team);
        Task<bool> SaveAll();
    }
}

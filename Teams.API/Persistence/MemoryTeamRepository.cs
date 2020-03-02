using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teams.API.Model;
using System.Linq;

namespace Teams.API.Persistence
{
    // Repositório utilizado essencialmente para testes
    public class MemoryTeamRepository : ITeamRepository
    {
        private ICollection<Team> _teams;
  
        public MemoryTeamRepository()
        {
            if (_teams == null)
                _teams = new List<Team>();
        }

        public MemoryTeamRepository(ICollection<Team> teams) => _teams = teams;

        public void AddTeam(Team team)
        {
            _teams.Add(team);
        }

        public void RemoveTeam(Team team)
        {
            _teams.Remove(team);
        }

        public async Task<IEnumerable<Team>> GetTeams()
        {
            return await Task.Run(() =>
            {
                return _teams;
            });
        }

        public async Task<Team> GetTeam(Guid id)
        {
            return await Task.Run(() =>
            {
                return _teams.FirstOrDefault(x => x.Id == id);
            });
        }

        public async Task<bool> SaveAll()
        {
            return await Task.Run(() =>
            {
                return true;
            });
        }
    }
}

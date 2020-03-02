using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Teams.API.Model;
using Teams.API.Persistence;

namespace Teams.API.Setup
{
    // Apenas para semear o nosso repositório quando estivermos em Development mode
    public class SeedDatabase
    {
        public static void SeedDomain(ITeamRepository repository)
        {
            var teamsData = File.ReadAllText("Setup/Teams.json");
            var teamsObjs = JsonConvert.DeserializeObject<List<Team>>(teamsData);

            foreach (var team in teamsObjs)
            {
                repository.AddTeam(team);
            }

            repository.SaveAll().Wait();
        }
    }
}

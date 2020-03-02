using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teams.API.Dtos
{
    public class TeamForUpdate
    {
        [Required]
        public string name { get; set; }
    }
}
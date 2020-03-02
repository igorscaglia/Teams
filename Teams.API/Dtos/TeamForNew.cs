using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teams.API.Dtos
{
    public class TeamForNew
    {
        public Guid? id { get; set; }

        [Required]
        public string name { get; set; }
    }
}

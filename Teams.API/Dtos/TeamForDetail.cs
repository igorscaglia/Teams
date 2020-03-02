using System;

namespace Teams.API.Dtos
{
    public class TeamForDetail
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public string members { get; set; }
    }
}
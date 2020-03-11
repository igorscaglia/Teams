using System;

namespace Teams.API.Dtos
{
    public class MemberForDetail
    {
        public Guid id { get; set; }
        
        public string firstname { get; set; }

        public string lastname { get; set; }

        public string teamname { get; set; }
    }
}
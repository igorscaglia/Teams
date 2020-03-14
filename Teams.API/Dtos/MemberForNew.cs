using System;

namespace Teams.API.Dtos
{
    public class MemberForNew
    {
        public Guid id { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }
    }
}
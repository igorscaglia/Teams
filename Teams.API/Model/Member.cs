using System;

namespace Teams.API.Model
{
    public class Member
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Member() { }

        public Member(Guid id) : this()
        {
            this.Id = id;
        }

        public Member(Guid id, string firstName, string lastName) : this(id)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public override string ToString()
        {
            return this.LastName;
        }
    }
}
using System;
using System.Collections.Generic;
using Teams.API.Model;
using Teams.API.Persistence;

namespace Teams.API.Tests
{
    public class FakeMemoryRepositoryHelper
    {
        public const string TEAM_1_ID = "AF12ED8B-37F3-44EA-9E6A-D104B440DC51";
        public const string TEAM_2_ID = "0584EFC6-0F3E-453E-8623-86E780266FE5";
        public const string TEAM_3_ID = "9B11E119-FB83-42F9-93DE-F25423CBD115";
        public const string TEAM_4_ID = "013B89BC-1304-47A9-B090-4DBB83BE15D1";
        public const string TEAM_5_ID = "07E89D5B-4246-4A75-BA1E-ED36F6BDAF3E";

        public const string MEMBER_1_ID = "AE88116A-BDD2-487E-AF2D-54795E25AE09";
        public const string MEMBER_2_ID = "C44B83DA-919E-4C65-882F-D58CE24513C0";
        public const string MEMBER_3_ID = "C959E715-6591-4137-98B5-CAD38CD0D883";
        public const string MEMBER_4_ID = "C485BD5E-741B-466F-9913-D6C384DF60C5";
        public const string MEMBER_5_ID = "5AD4A96F-C1DB-4B99-AAFE-CE51BB340FE7";
        public const string MEMBER_6_ID = "AE6517DD-4EB9-4B5F-8A66-1A4855850943";
        public const string MEMBER_7_ID = "F3DE7A8E-3EFB-456D-911F-AD312414B6FB";
        public const string MEMBER_8_ID = "F9A80A75-98E2-4E24-B64E-1BD63FFE62B2";
        public const string MEMBER_9_ID = "340CCF45-23F3-4A42-BFD7-EA805B014F7E";
        public const string MEMBER_10_ID = "9E022B48-CE11-49D4-959E-1B58B561DBB4";

        public const string NOT_IN_USE_1 = "AB5B8EE0-C6E3-4588-BC7E-5AA474BDB5F4";
        public const string NOT_IN_USE_2 = "F1B3DEF1-C2BA-4B7A-86CF-718B94488266";
        public const string NOT_IN_USE_3 = "4DFF811F-89FD-4EBA-8B74-DAD406FC0B0D";

        public ITeamRepository RepoWith5Teams()
        {
            ITeamRepository _memoryRepository = new MemoryTeamRepository();

            _memoryRepository.AddTeam(new Team()
            {
                Name = "fake1",
                Id = Guid.Parse(TEAM_1_ID),
                Members = new List<Member>()
                {
                    new Member(Guid.Parse(MEMBER_1_ID), "fake1", "fake1"),
                    new Member(Guid.Parse(MEMBER_2_ID), "fake2", "fake2")
                }
            });
            _memoryRepository.AddTeam(new Team()
            {
                Name = "fake2",
                Id = Guid.Parse(TEAM_2_ID),
                Members = new List<Member>()
                {
                    new Member(Guid.Parse(MEMBER_3_ID), "fake3", "fake3"),
                    new Member(Guid.Parse(MEMBER_4_ID), "fake4", "fake4")
                }
            });
            _memoryRepository.AddTeam(new Team()
            {
                Name = "fake3",
                Id = Guid.Parse(TEAM_3_ID),
                Members = new List<Member>()
                {
                    new Member(Guid.Parse(MEMBER_5_ID), "fake5", "fake5"),
                    new Member(Guid.Parse(MEMBER_6_ID), "fake6", "fake6")
                }
            });
            _memoryRepository.AddTeam(new Team()
            {
                Name = "fake4",
                Id = Guid.Parse(TEAM_4_ID),
                Members = new List<Member>()
                {
                    new Member(Guid.Parse(MEMBER_7_ID), "fake7", "fake7"),
                    new Member(Guid.Parse(MEMBER_8_ID), "fake8", "fake8")
                }
            });
            _memoryRepository.AddTeam(new Team()
            {
                Name = "fake5",
                Id = Guid.Parse(TEAM_5_ID),
                Members = new List<Member>()
                {
                    new Member(Guid.Parse(MEMBER_9_ID), "fake9", "fake9"),
                    new Member(Guid.Parse(MEMBER_10_ID), "fake10", "fake10")
                }
            });

            return _memoryRepository;
        }
    }
}

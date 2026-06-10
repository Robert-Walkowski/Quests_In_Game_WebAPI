using QuestsInGame_WebAPI_Project.Models;
using Raven.Client.Documents.Indexes;

namespace QuestsInGame_WebAPI_Project.StaticIndexes
{
    public class Characters_ByExpirienceScore : AbstractIndexCreationTask<CharacterModel, Characters_ByExpirienceScore.Result>
    {
        public class Result
        {
            public string Name { get; set; }
            public string GameClass { get; set; }
            public int Level { get; set; }
            public int ExperienceScore { get; set; }
            public string? Tier { get; set; }
        }

        public Characters_ByExpirienceScore()
        {
            Map = characters => from c in characters
                                select new Result
                                {
                                    Name = c.Name,
                                    GameClass = c.GameClass,
                                    Level = c.Level,
                                    ExperienceScore = c.Level * (c.GameClass == "Warrior" ? 15 :
                                                             c.GameClass == "Mag" ? 20 : 10),
                                    Tier = c.Level >= 50 ? "Gold" :
                                       c.Level >= 25 ? "Silver" : "Bronze"
                                };

            Store(x => x.ExperienceScore, FieldStorage.Yes);
            Store(x => x.Tier, FieldStorage.Yes);
        }
    }
}

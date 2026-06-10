using QuestsInGame_WebAPI_Project.Models;
using Raven.Client.Documents.Indexes;

namespace QuestsInGame_WebAPI_Project.StaticIndexes
{
    public class Characters_WithGuildName : AbstractIndexCreationTask<CharacterModel, Characters_WithGuildName.Result>
    {
        public class Result
        {
            public string Name { get; set; }
            public string GameClass { get; set; }
            public int Level { get; set; }
            public string GuildName { get; set; }
        }

        public Characters_WithGuildName()
        {
            Map = characters => from c in characters
                                let guild = LoadDocument<GuildModel>(c.CharacterGuildId)
                                select new Result
                                {
                                    Name = c.Name,
                                    GameClass = c.GameClass,
                                    Level = c.Level,
                                    GuildName = guild != null ? guild.GuildName : "Brak gildii"
                                };

            Store(x => x.GuildName, FieldStorage.Yes);
        }
    }
}

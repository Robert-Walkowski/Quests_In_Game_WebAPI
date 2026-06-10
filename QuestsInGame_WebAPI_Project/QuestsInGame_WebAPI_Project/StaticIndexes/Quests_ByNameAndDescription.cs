using QuestsInGame_WebAPI_Project.Models;
using Raven.Client.Documents.Indexes;

namespace QuestsInGame_WebAPI_Project.StaticIndexes
{
    public class Quests_ByNameAndDescription : AbstractIndexCreationTask<QuestModel, Quests_ByNameAndDescription.Result>
    {
        public class Result
        {
            public string QuestData { get; set; }
        }

        public Quests_ByNameAndDescription()
        {
            Map = quests => from quest in quests
                            select new
                            {
                                QuestData = new object[]
                                {
                                    quest.QuestTitle,
                                    quest.QuestDescription,
                                }
                            };

            Index(x => x.QuestData, FieldIndexing.Search);
        }
    }
}

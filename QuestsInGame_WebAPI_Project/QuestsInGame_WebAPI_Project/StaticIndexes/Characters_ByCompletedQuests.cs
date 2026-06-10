using QuestsInGame_WebAPI_Project.Models;
using Raven.Client.Documents.Indexes;

namespace QuestsInGame_WebAPI_Project.StaticIndexes
{
    public class QuestCompletions_ByCharacter : AbstractIndexCreationTask<QuestCompletionModel, QuestCompletions_ByCharacter.Result>
    {
        public class Result
        {
            public string CharacterId { get; set; }
            public int QuestCount { get; set; }
        }

        public QuestCompletions_ByCharacter()
        {
            Map = completions => from c in completions
                                 select new Result
                                 {
                                     CharacterId = c.CharacterId,
                                     QuestCount = 1
                                 };

            Reduce = results => from r in results
                                group r by r.CharacterId into g
                                select new Result
                                {
                                    CharacterId = g.Key,
                                    QuestCount = g.Sum(x => x.QuestCount)
                                };
        }
    }
}

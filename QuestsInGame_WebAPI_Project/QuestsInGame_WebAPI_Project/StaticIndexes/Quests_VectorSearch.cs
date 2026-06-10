using QuestsInGame_WebAPI_Project.Models;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.Vector;
using System.Collections;

namespace QuestsInGame_WebAPI_Project.StaticIndexes
{
    public class Quests_VectorSearch : AbstractIndexCreationTask<QuestModel, Quests_VectorSearch.Result>
    {
        public class Result
        {
            public string QuestTitle { get; set; }
            public object Vector { get; set; }
        }

        public Quests_VectorSearch()
        {
            Map = quests => from q in quests
                            select new Result
                            {
                                QuestTitle = q.QuestTitle,
                                Vector = CreateVector(q.QuestTitle + " " + q.QuestDescription)
                            };

            VectorIndexes.Add(x => x.Vector,
                new VectorOptions
                {
                    SourceEmbeddingType = VectorEmbeddingType.Text,
                    DestinationEmbeddingType = VectorEmbeddingType.Single
                }
            );
        }
    }
}

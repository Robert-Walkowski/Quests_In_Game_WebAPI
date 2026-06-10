using Raven.Client.Documents;

namespace QuestsInGame_WebAPI_Project.Database
{
    public static class DocumentStoreHolder
    {
        private static readonly Lazy<IDocumentStore> LazyStore =
            new Lazy<IDocumentStore>(() =>
            {
                var store = new DocumentStore
                {
                    Urls = new[] {
                        "http://localhost:8889",
                    },
                    Database = "GameQuests"
                };

                return store.Initialize();
            });

        public static IDocumentStore Store =>
            LazyStore.Value;
    }
}

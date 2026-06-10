namespace QuestsInGame_WebAPI_Project.Enums
{
    public enum CharacterStatus
    {
        SUCCESS = 0,
        EMPTY_NAME = 1,
        EMPTY_GAMECLASS = 2,
        LEVEL_BELOW_MINIMUM = 3,
        CHARACTER_NOT_FOUND = 4,
        INCORRECT_GAMECLASS = 5,
        GUILD_NOT_EXISTS = 6,
        AUTO_INDEX_FAIL = 7,
        RAVEN_ERROR = 99
    }

    public static class CharacterStatusClass
    {
        public static string ToMessage(this CharacterStatus status)
        {
            switch (status)
            {
                case CharacterStatus.SUCCESS:
                    return "Operacja zakończona sukcesem.";
                case CharacterStatus.EMPTY_NAME:
                    return "Podałeś pustą nazwę użytkownika.";
                case CharacterStatus.EMPTY_GAMECLASS:
                    return "Nazwa klasy postaci/gracza nie może być pusta.";
                case CharacterStatus.LEVEL_BELOW_MINIMUM:
                    return "Poziom postaci jest poniżej zera lub pusty.";
                case CharacterStatus.CHARACTER_NOT_FOUND:
                    return "Postać o tym 'id' nie istnieje.";
                case CharacterStatus.INCORRECT_GAMECLASS:
                    return "Podano nazwę klasy, która nie jest dostępna w tej grze.";
                case CharacterStatus.GUILD_NOT_EXISTS:
                    return "Podany id gildii jest niepoprawny (nie istnieje gildia o takim id).";
                case CharacterStatus.AUTO_INDEX_FAIL:
                    return "Przy autoindeksie wystąpił błąd (najpewniej przez to, że nie ma takich rekordów w bazie).";
                case CharacterStatus.RAVEN_ERROR:
                    return "Wystąpił problem związany z ravenem.";
                default:
                    return "Niezarejestrowany błąd związany z postacią.";
            }
        }
    }
}

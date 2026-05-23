namespace QuestsInGame_WebAPI_Project.Enums
{
    public enum CharacterStatus
    {
        SUCCESS = 0,
        EMPTY_NAME = 1,
        EMPTY_GAMECLASS = 2,
        LEVEL_BELOW_MINIMUM = 3,
        CHARACTER_NOT_FOUND = 4,
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
                case CharacterStatus.RAVEN_ERROR:
                    return "Wystąpił problem związany z ravenem.";
                default:
                    return "Niezarejestrowany błąd związany z charakterem.";
            }
        }
    }
}

namespace QuestsInGame_WebAPI_Project.Enums
{
    public enum QuestStatus
    {
        SUCCESS = 0,
        EMPTY_QUEST_TITLE = 1,
        INCORRECT_QUEST_LEVEL = 2,
        QUEST_NOT_FOUND = 3,
        RAVEN_ERROR = 99
    }

    public static class QuestStatusClass
    {
        public static string ToMessage(this QuestStatus status)
        {
            switch (status)
            {
                case QuestStatus.SUCCESS:
                    return "Operacja zakończona sukcesem.";
                case QuestStatus.EMPTY_QUEST_TITLE:
                    return "Tytuł questa nie może być pusty.";
                case QuestStatus.INCORRECT_QUEST_LEVEL:
                    return "Poziom questa nie może być mniejszy bądź równy 0.";
                case QuestStatus.QUEST_NOT_FOUND:
                    return "Nie znaleziono questa o zadanym id.";
                case QuestStatus.RAVEN_ERROR:
                    return "Wystąpił problem związany z ravenem.";
                default:
                    return "Niezarejestrowany błąd związany z questem.";
            }
        }
    }
}

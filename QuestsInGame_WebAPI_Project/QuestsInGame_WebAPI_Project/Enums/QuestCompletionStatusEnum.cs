namespace QuestsInGame_WebAPI_Project.Enums
{
    public enum QuestCompletionStatusEnum
    {
        SUCCESS = 0,
        CHARACTER_NOT_FOUND = 1,
        QUEST_NOT_FOUND = 2,
        QUEST_GRADE_TOO_HIGH = 3,
        QUEST_ALREADY_COMPLETED = 4,
        RAVEN_ERROR = 99
    }

    public static class QuestCompletionStatusEnumClass
    {
        public static string ToMessage(this QuestCompletionStatusEnum status)
        {
            switch (status)
            {
                case QuestCompletionStatusEnum.SUCCESS:
                    return "Operacja zakończona sukcesem.";
                case QuestCompletionStatusEnum.CHARACTER_NOT_FOUND:
                    return "Gracz/Postać o podanym id nie istnieje, bądź podałeś puste pole 'characterId'.";
                case QuestCompletionStatusEnum.QUEST_NOT_FOUND:
                    return "Quest o podanym id nie istnieje, bądź podałeś puste pole 'questId'.";
                case QuestCompletionStatusEnum.QUEST_GRADE_TOO_HIGH:
                    return "Podałeś za dużą/za niską ocenę wykonania Questa (skala: 1-10)";
                case QuestCompletionStatusEnum.QUEST_ALREADY_COMPLETED:
                    return "Nie możesz wziąć/wykonać questa, który jest już skończony (ma status 'COMPLETED').";
                case QuestCompletionStatusEnum.RAVEN_ERROR:
                    return "Wystąpił problem związany z ravenem.";
                default:
                    return "Niezarejestrowany błąd związany z questem.";
            }
        }
    }
}

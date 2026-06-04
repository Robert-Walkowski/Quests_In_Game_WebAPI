using System.Runtime.CompilerServices;

namespace QuestsInGame_WebAPI_Project.Enums
{
    public enum GuildStatus
    {
        SUCCESS = 0,
        EMPTY_GUILD_NAME = 1,
        GUILD_NOT_FOUND = 2,
        RAVEN_ERROR = 99
    }

    public static class GuildStatusClass
    {
        public static string ToMessage(this GuildStatus status)
        {
            switch (status)
            {
                case GuildStatus.SUCCESS:
                    return "Operacja zakończona sukcesem.";
                case GuildStatus.EMPTY_GUILD_NAME:
                    return "Nazwa gildii nie może być pusta.";
                case GuildStatus.GUILD_NOT_FOUND:
                    return "Gildia o podanym 'id' nie istnieje.";
                case GuildStatus.RAVEN_ERROR:
                        return "Wystąpił problem związany z ravenem.";
                default:
                    return "Niezarejestrowany błąd związany z gildią.";
            }
        }
    }
}

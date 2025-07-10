using System.Collections.Generic;

public static class GameState
{
    // Собранные ключи
    public static HashSet<string> collectedKeys = new HashSet<string>();

    // Метод для сброса всех ключей (например при начале новой игры или загрузке новой главы)
    public static void ResetKeys()
    {
        collectedKeys.Clear();
    }
}
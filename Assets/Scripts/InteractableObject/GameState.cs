using System.Collections.Generic;

public static class GameState
{
    // ��������� �����
    public static HashSet<string> collectedKeys = new HashSet<string>();

    // ����� ��� ������ ���� ������ (�������� ��� ������ ����� ���� ��� �������� ����� �����)
    public static void ResetKeys()
    {
        collectedKeys.Clear();
    }
}
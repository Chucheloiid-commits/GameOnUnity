using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public Sprite characterPortrait;  // ������� ���������
    [TextArea(2, 5)]
    public string dialogueText;       // ����� �������
}

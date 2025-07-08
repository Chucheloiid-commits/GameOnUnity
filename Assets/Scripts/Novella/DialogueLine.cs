using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public Sprite characterPortrait;  // Портрет персонажа
    [TextArea(2, 5)]
    public string dialogueText;       // Текст диалога
}

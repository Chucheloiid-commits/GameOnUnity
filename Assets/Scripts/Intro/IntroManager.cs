using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections; // Добавляем для работы с корутинами
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class DialogEntry
{
    public Sprite characterPortrait;    // Портрет персонажа
    [TextArea(2, 5)]
    public string dialogueText;         // Текст диалога
}

public class IntroManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image frameImage;             // Фон
    public Image characterPortrait;      // Портрет персонажа
    public GameObject dialogPanel;       // Панель диалога
    public TextMeshProUGUI dialogText; // Текст диалога

    [Header("Content")]
    public List<Sprite> frames;               // Фоновые кадры
    public List<DialogEntry> dialogueLines;   // Диалоги с портретами

    [Header("Text Reveal Settings")] // Новые настройки для прорисовки текста
    public float textRevealSpeed = 0.05f; // Скорость прорисовки (время между буквами)
    private Coroutine currentRevealCoroutine; // Для отслеживания текущей корутины прорисовки

    private int currentIndex = 0;
    private enum State { ShowFrameOnly, ShowDialog, RevealingText } // Добавляем состояние для прорисовки
    private State currentState = State.ShowFrameOnly;

    void Start()
    {
        dialogPanel.SetActive(false);
        characterPortrait.gameObject.SetActive(false);
        ShowFrame(currentIndex);
    }

    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            Advance();
        }
    }

    void Advance()
    {
        // Если текст прорисовывается, при клике мы хотим немедленно показать весь текст
        if (currentState == State.RevealingText)
        {
            if (currentRevealCoroutine != null)
            {
                StopCoroutine(currentRevealCoroutine); // Останавливаем текущую корутину
            }
            dialogText.maxVisibleCharacters = dialogText.text.Length; // Показываем весь текст сразу
            currentState = State.ShowDialog; // Переходим в состояние полного отображения текста
            return; // Завершаем функцию, чтобы не переходить к следующему диалогу сразу
        }

        if (currentIndex >= frames.Count || currentIndex >= dialogueLines.Count)
        {
            SceneManager.LoadScene("House");
            return;
        }

        if (currentState == State.ShowFrameOnly)
        {
            dialogPanel.SetActive(true);
            characterPortrait.gameObject.SetActive(true);

            // ЗАПУСКАЕМ КОРУТИНУ ДЛЯ ПРОРИСОВКИ ТЕКСТА
            currentRevealCoroutine = StartCoroutine(RevealText(dialogueLines[currentIndex].dialogueText));
            SetPortraitPreserveAspect(dialogueLines[currentIndex].characterPortrait);

            currentState = State.RevealingText; // Устанавливаем новое состояние
        }
        else // currentState == State.ShowDialog
        {
            // Этот блок выполняется, когда текст полностью показан и мы переходим к следующему кадру/диалогу
            dialogPanel.SetActive(false);
            characterPortrait.gameObject.SetActive(false);

            currentIndex++;
            if (currentIndex < frames.Count && currentIndex < dialogueLines.Count)
            {
                ShowFrame(currentIndex);
                currentState = State.ShowFrameOnly;
            }
            else
            {
                SceneManager.LoadScene("House");
            }
        }
    }

    // НОВАЯ КОРУТИНА ДЛЯ ПРОРИСОВКИ ТЕКСТА
    IEnumerator RevealText(string textToReveal)
    {
        dialogText.text = textToReveal; // Устанавливаем весь текст
        dialogText.maxVisibleCharacters = 0; // Изначально делаем текст невидимым

        // Проходимся по каждой букве и постепенно увеличиваем количество видимых символов
        for (int i = 0; i <= textToReveal.Length; i++)
        {
            dialogText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(textRevealSpeed); // Ждем заданное время
        }

        currentState = State.ShowDialog; // После прорисовки переходим в состояние ShowDialog
    }

    void ShowFrame(int index)
    {
        if (index < frames.Count)
        {
            frameImage.sprite = frames[index];
        }
    }

    void SetPortraitPreserveAspect(Sprite sprite)
    {
        if (characterPortrait == null || sprite == null) return;

        characterPortrait.sprite = sprite;

        RectTransform rt = characterPortrait.GetComponent<RectTransform>();

        float containerWidth = 250f;
        float containerHeight = 350f;

        float spriteRatio = sprite.rect.width / sprite.rect.height;
        float containerRatio = containerWidth / containerHeight;

        float width, height;

        if (spriteRatio > containerRatio)
        {
            width = containerWidth;
            height = width / spriteRatio;
        }
        else
        {
            height = containerHeight;
            width = height * spriteRatio;
        }

        rt.sizeDelta = new Vector2(width, height);
    }
}
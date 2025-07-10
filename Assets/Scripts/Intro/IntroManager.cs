using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class DialogEntry
{
    public Sprite characterPortrait;     // Портрет персонажа
    public string characterName;         // Имя персонажа
    [TextArea(2, 5)]
    public string dialogueText;          // Текст диалога
}

public class IntroManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image frameImage;                    // Фоновое изображение
    public Image characterPortrait;             // Портрет персонажа
    public GameObject dialogPanel;              // Панель диалога
    public TextMeshProUGUI dialogText;          // Текст диалога
    public TextMeshProUGUI characterNameText;   // Имя персонажа

    [Header("Content")]
    public List<Sprite> frames;                 // Слайды/фоновые кадры
    public List<DialogEntry> dialogueLines;     // Диалоги (портрет, имя, текст)

    [Header("Text Reveal Settings")]
    public float textRevealSpeed = 0.05f;       // Скорость появления текста

    [Header("Scene Settings")]
    public string nextSceneName = "House";      // Название сцены для перехода

    [Header("Audio")]
    public AudioClip backgroundMusic;           // Фоновая музыка
    private AudioSource musicSource;

    private Coroutine currentRevealCoroutine;
    private int currentIndex = 0;

    private enum State { ShowFrameOnly, ShowDialog, RevealingText }
    private State currentState = State.ShowFrameOnly;

    void Start()
    {
        // Воспроизведение фоновой музыки
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0.5f;
        musicSource.Play();

        dialogPanel.SetActive(false);
        characterPortrait.gameObject.SetActive(false);
        characterNameText.gameObject.SetActive(false);

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
        if (currentState == State.RevealingText)
        {
            if (currentRevealCoroutine != null)
            {
                StopCoroutine(currentRevealCoroutine);
            }

            dialogText.maxVisibleCharacters = dialogText.text.Length;
            currentState = State.ShowDialog;
            return;
        }

        // Переход на другую сцену, если закончились кадры или диалоги
        if (currentIndex >= frames.Count || currentIndex >= dialogueLines.Count)
        {
            StopMusicAndLoadScene();
            return;
        }

        if (currentState == State.ShowFrameOnly)
        {
            ShowDialog();
        }
        else // currentState == State.ShowDialog
        {
            HideDialog();
            currentIndex++;

            if (currentIndex < frames.Count && currentIndex < dialogueLines.Count)
            {
                ShowFrame(currentIndex);
                currentState = State.ShowFrameOnly;
            }
            else
            {
                StopMusicAndLoadScene();
            }
        }
    }

    void ShowDialog()
    {
        dialogPanel.SetActive(true);
        characterPortrait.gameObject.SetActive(true);
        characterNameText.gameObject.SetActive(true);

        characterPortrait.sprite = dialogueLines[currentIndex].characterPortrait;
        characterNameText.text = dialogueLines[currentIndex].characterName;

        currentRevealCoroutine = StartCoroutine(RevealText(dialogueLines[currentIndex].dialogueText));
        SetPortraitPreserveAspect(dialogueLines[currentIndex].characterPortrait);

        currentState = State.RevealingText;
    }

    void HideDialog()
    {
        dialogPanel.SetActive(false);
        characterPortrait.gameObject.SetActive(false);
        characterNameText.gameObject.SetActive(false);
    }

    IEnumerator RevealText(string textToReveal)
    {
        dialogText.text = textToReveal;
        dialogText.maxVisibleCharacters = 0;

        for (int i = 0; i <= textToReveal.Length; i++)
        {
            dialogText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(textRevealSpeed);
        }

        currentState = State.ShowDialog;
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

    void StopMusicAndLoadScene()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }

        SceneManager.LoadScene(nextSceneName);
    }
}

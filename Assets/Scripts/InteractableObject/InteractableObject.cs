using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    public GameObject questionMarkPrefab;
    private GameObject questionMarkInstance;

    public GameObject dialogCanvas;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI characterNameText;

    private bool isPlayerNearby = false;
    private bool hasInteracted = false;
    private bool isDialogOpen = false;

    private movePlayer playerMovement;

    public Image characterPortrait;
    public Sprite portraitSprite;

    [TextArea]
    public string dialogMessage = "Привет! Это мой диалог.";

    // Статусы взаимодействия
    private static bool isAnyDialogOpen = false;
    private static InteractableObject lastInteractedObject = null;

    private static bool waitForNextPress = false;

    void Update()
    {
        if (hasInteracted) return;

        if (isDialogOpen)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                CloseDialog();
                waitForNextPress = true; // Ждём отпускания и повторного нажатия
            }
            return;
        }

        if (isPlayerNearby)
        {
            // Показать вопросик
            if (questionMarkInstance == null)
            {
                Vector3 pos = transform.position + Vector3.up * 0.4f;
                questionMarkInstance = Instantiate(questionMarkPrefab, pos, Quaternion.identity);
            }

            // Проверяем, можно ли открыть диалог
            if (!isAnyDialogOpen &&
                !waitForNextPress &&
                Keyboard.current.eKey.wasPressedThisFrame)
            {
                ShowDialog();
            }
        }
        else
        {
            // Убрать вопросик, если игрок ушёл
            if (questionMarkInstance != null)
            {
                Destroy(questionMarkInstance);
                questionMarkInstance = null;
            }
        }

        // Сброс ожидания нажатия E, когда клавиша отпущена
        if (waitForNextPress && !Keyboard.current.eKey.isPressed)
        {
            waitForNextPress = false;
        }
    }

    private void ShowDialog()
    {
        dialogCanvas.SetActive(true);
        dialogText.text = dialogMessage;
        isDialogOpen = true;
        isAnyDialogOpen = true;
        lastInteractedObject = this;

        if (characterPortrait != null && portraitSprite != null)
        {
            characterPortrait.sprite = portraitSprite;
            characterPortrait.gameObject.SetActive(true);
        }

        if (characterNameText != null)
        {
            characterNameText.text = "Юна";
            characterNameText.gameObject.SetActive(true);
        }

        if (playerMovement != null)
            playerMovement.SetMovementEnabled(false);
    }

    private void CloseDialog()
    {
        dialogCanvas.SetActive(false);
        isDialogOpen = false;
        hasInteracted = true;
        isAnyDialogOpen = false;

        if (questionMarkInstance != null)
        {
            Destroy(questionMarkInstance);
            questionMarkInstance = null;
        }

        if (characterPortrait != null)
            characterPortrait.gameObject.SetActive(false);

        if (characterNameText != null)
            characterNameText.gameObject.SetActive(false);

        if (playerMovement != null)
            playerMovement.SetMovementEnabled(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasInteracted) return;

        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            playerMovement = other.GetComponent<movePlayer>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DoorObject : MonoBehaviour
{
    public string requiredKeyName = "basementKey";
    public string nextSceneName = "NextScene";

    public GameObject questionMarkPrefab;
    private GameObject questionMarkInstance;

    public GameObject dialogCanvas;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI characterNameText;

    private bool isPlayerNearby = false;
    private bool isDialogOpen = false;

    private movePlayer playerMovement;

    public Image characterPortrait;
    public Sprite portraitSprite;

    [TextArea]
    public string closedMessage = "Дверь закрыта. Нужно найти ключ.";

    private static bool isAnyDialogOpen = false;
    private static bool waitForNextPress = false;

    void Update()
    {
        if (isDialogOpen)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                CloseDialog();

                if (GameState.collectedKeys.Contains(requiredKeyName))
                {
                    SceneManager.LoadScene(nextSceneName);
                }

                waitForNextPress = true;
            }
            return;
        }

        if (isPlayerNearby)
        {
            if (questionMarkInstance == null)
            {
                Vector3 pos = transform.position + Vector3.up * 0.4f;
                questionMarkInstance = Instantiate(questionMarkPrefab, pos, Quaternion.identity);
            }

            if (!isAnyDialogOpen && !waitForNextPress && Keyboard.current.eKey.wasPressedThisFrame)
            {
                ShowDialog();
            }
        }

        if (waitForNextPress && !Keyboard.current.eKey.isPressed)
        {
            waitForNextPress = false;
        }
    }

    private void ShowDialog()
    {
        bool hasKey = GameState.collectedKeys.Contains(requiredKeyName);

        dialogCanvas.SetActive(true);
        dialogText.text = hasKey ? "Вы открыли дверь!" : closedMessage;
        isDialogOpen = true;
        isAnyDialogOpen = true;

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
        isAnyDialogOpen = false;

        if (characterPortrait != null)
            characterPortrait.gameObject.SetActive(false);

        if (characterNameText != null)
            characterNameText.gameObject.SetActive(false);

        if (playerMovement != null)
            playerMovement.SetMovementEnabled(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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

            if (questionMarkInstance != null && GameState.collectedKeys.Contains(requiredKeyName))
            {
                Destroy(questionMarkInstance);
                questionMarkInstance = null;
            }
        }
    }
}
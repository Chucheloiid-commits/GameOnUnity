using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeyPickupObject : MonoBehaviour
{
    public string keyName = "basementKey";

    public GameObject questionMarkPrefab;
    private GameObject questionMarkInstance;

    public GameObject dialogCanvas;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI characterNameText;

    private bool isPlayerNearby = false;
    private bool isDialogOpen = false;
    private bool isPickedUp = false;

    private movePlayer playerMovement;

    public Image characterPortrait;
    public Sprite portraitSprite;

    [TextArea]
    public string dialogMessage = "Вы нашли ключ!";

    private static bool isAnyDialogOpen = false;
    private static bool waitForNextPress = false;

    void Update()
    {
        if (isPickedUp) return;

        if (isDialogOpen)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                CloseDialog();
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
        else
        {
            if (questionMarkInstance != null)
            {
                Destroy(questionMarkInstance);
                questionMarkInstance = null;
            }
        }

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
        isPickedUp = true;
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

        if (!string.IsNullOrEmpty(keyName))
            GameState.collectedKeys.Add(keyName);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPickedUp) return;

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
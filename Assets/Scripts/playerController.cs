using UnityEngine;
using UnityEngine.InputSystem;

public class movePlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 25f;
    private Vector2 moveVector;
    private PlayerInput playerInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D не найден на игроке!");
        }
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput не найден на игроке!");
        }
        else
        {
            playerInput.currentActionMap.Enable();
            Debug.Log("PlayerInput и Action Map '" + playerInput.currentActionMap.name + "' включены.");
        }
    }

    // В Update только считываем ввод
    void Update()
    {
        moveVector = playerInput.actions["Move"].ReadValue<Vector2>();
        //Debug.Log("Move Vector: " + moveVector);
    }

    // В FixedUpdate применяем физическое движение
    void FixedUpdate()
    {
        // Движение, основанное на физике, должно быть в FixedUpdate
        // Для стабильности при использовании Rigidbody2D.MovePosition
        rb.MovePosition(rb.position + moveVector * speed * Time.fixedDeltaTime);
    }
    public void SetMovementEnabled(bool enabled)
    {
        if (playerInput != null)
        {
            if (enabled)
            {
                playerInput.actions.Enable();
            }
            else
            {
                playerInput.actions.Disable();
            }
        }
    }

}

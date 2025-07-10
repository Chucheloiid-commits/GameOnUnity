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
            Debug.LogError("Rigidbody2D �� ������ �� ������!");
        }
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput �� ������ �� ������!");
        }
        else
        {
            playerInput.currentActionMap.Enable();
            Debug.Log("PlayerInput � Action Map '" + playerInput.currentActionMap.name + "' ��������.");
        }
    }

    // � Update ������ ��������� ����
    void Update()
    {
        moveVector = playerInput.actions["Move"].ReadValue<Vector2>();
        //Debug.Log("Move Vector: " + moveVector);
    }

    // � FixedUpdate ��������� ���������� ��������
    void FixedUpdate()
    {
        // ��������, ���������� �� ������, ������ ���� � FixedUpdate
        // ��� ������������ ��� ������������� Rigidbody2D.MovePosition
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

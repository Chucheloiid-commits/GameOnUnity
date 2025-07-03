using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    CharacterController characterController;
    [SerializeField] float moveSpeed = 5f;
    Vector2 moveInput;
    Vector3 movement;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        movement.x = moveInput.x * moveSpeed;
        movement.y = moveInput.y * moveSpeed;
        characterController.Move(movement*Time.fixedDeltaTime);
    }
}

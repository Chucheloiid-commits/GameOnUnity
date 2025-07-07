using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerInput playerInput;
    private Vector2 moveVector;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogError("Animator Controller �� ��������! ��������� YunaController � ���������� Animator.");
        }
        else
        {
            Debug.Log("Animator Controller: " + animator.runtimeAnimatorController.name);
        }
    }

    void Update()
    {
        // �������� ������ �������� �� PlayerInput
        moveVector = playerInput.actions["Move"].ReadValue<Vector2>();

        // ������� �������� (����� �������)
        animator.SetFloat("Speed", moveVector.magnitude);

        // ������ ���� ���� �������� � ��������� �����������
        if (moveVector.magnitude > 0.01f)
        {
            if (Mathf.Abs(moveVector.x) > Mathf.Abs(moveVector.y))
            {
                // �������� ����� ��� ������
                animator.SetFloat("MoveX", moveVector.x > 0 ? 1 : -1);
                animator.SetFloat("MoveY", 0);
            }
            else
            {
                // �������� ����� ��� ����
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", moveVector.y > 0 ? 1 : -1);
            }
        }
        Debug.Log($"Speed: {moveVector.magnitude}, MoveX: {animator.GetFloat("MoveX")}, MoveY: {animator.GetFloat("MoveY")}");

    }
}

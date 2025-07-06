using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    [SerializeField] private GameObject boundaryGroup; // ������ ����� ������������� ������
    private BoxCollider2D playerCollider;
    private Vector2 boundaryMin, boundaryMax;

    void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();

        if (boundaryGroup == null)
        {
            Debug.LogWarning("Boundary Group �� ���������!");
            return;
        }

        // �������� ��� Collider2D ������ ������
        Collider2D[] colliders = boundaryGroup.GetComponentsInChildren<Collider2D>();

        if (colliders.Length == 0)
        {
            Debug.LogWarning("� ������ ��� �� ������ Collider2D!");
            return;
        }

        // �������� ����������� � ������� ����������
        Bounds combinedBounds = colliders[0].bounds;

        for (int i = 1; i < colliders.Length; i++)
        {
            combinedBounds.Encapsulate(colliders[i].bounds);
        }

        boundaryMin = combinedBounds.min + (Vector3)playerCollider.bounds.extents;
        boundaryMax = combinedBounds.max - (Vector3)playerCollider.bounds.extents;
    }

    void LateUpdate()
    {
        if (boundaryGroup == null) return;

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, boundaryMin.x, boundaryMax.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, boundaryMin.y, boundaryMax.y);
        transform.position = clampedPosition;
    }
    void OnDrawGizmos()
    {
        if (boundaryGroup != null && playerCollider != null)
        {
            Gizmos.color = Color.green; // ���� ��� ������
            Vector3 center = (Vector3)(boundaryMin + boundaryMax) / 2f;
            Vector3 size = (Vector3)(boundaryMax - boundaryMin);
            Gizmos.DrawWireCube(center, size);
        }
    }
}


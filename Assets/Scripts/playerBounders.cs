using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boundary;
    private BoxCollider2D playerCollider;
    private Vector2 boundaryMin, boundaryMax;

    void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();

        // Рассчитываем границы один раз при старте
        boundaryMin = boundary.bounds.min + (Vector3)playerCollider.bounds.extents;
        boundaryMax = boundary.bounds.max - (Vector3)playerCollider.bounds.extents;
    }

    void LateUpdate()
    {
        // Жёсткое ограничение позиции
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, boundaryMin.x, boundaryMax.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, boundaryMin.y, boundaryMax.y);
        transform.position = clampedPosition;
    }
}
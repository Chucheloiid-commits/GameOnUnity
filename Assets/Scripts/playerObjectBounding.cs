using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerObjectBounding : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private Collider2D forbiddenArea; // Коллайдер запретной зоны

    private Collider2D playerCollider;
    private Bounds forbiddenBounds;

    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        forbiddenBounds = forbiddenArea.bounds;
    }

    void Update()
    {
        Bounds playerBounds = playerCollider.bounds;

        // Если игрок пересекает запретную зону
        if (IsOverlapping(playerBounds, forbiddenBounds))
        {
            // Вычисляем корректную позицию СНАРУЖИ от границы
            Vector3 safePosition = CalculateSafePosition(playerBounds);
            transform.position = safePosition;
        }
    }

    bool IsOverlapping(Bounds player, Bounds forbidden)
    {
        return player.min.x < forbidden.max.x &&
               player.max.x > forbidden.min.x &&
               player.min.y < forbidden.max.y &&
               player.max.y > forbidden.min.y;
    }

    Vector3 CalculateSafePosition(Bounds playerBounds)
    {
        Vector2 playerCenter = playerBounds.center;
        Vector2 forbiddenCenter = forbiddenBounds.center;

        // Определяем, с какой стороны игрок ближе к границе
        float leftDist = playerBounds.max.x - forbiddenBounds.min.x;
        float rightDist = forbiddenBounds.max.x - playerBounds.min.x;
        float bottomDist = playerBounds.max.y - forbiddenBounds.min.y;
        float topDist = forbiddenBounds.max.y - playerBounds.min.y;

        // Выбираем ближайшую границу для выталкивания
        float minDist = Mathf.Min(leftDist, rightDist, bottomDist, topDist);

        Vector3 newPosition = transform.position;

        if (minDist == leftDist)
        {
            newPosition.x = forbiddenBounds.min.x - playerBounds.extents.x;
        }
        else if (minDist == rightDist)
        {
            newPosition.x = forbiddenBounds.max.x + playerBounds.extents.x;
        }
        else if (minDist == bottomDist)
        {
            newPosition.y = forbiddenBounds.min.y - playerBounds.extents.y;
        }
        else // topDist
        {
            newPosition.y = forbiddenBounds.max.y + playerBounds.extents.y;
        }
        newPosition.z =transform.position.z;
        return newPosition;
    }
}
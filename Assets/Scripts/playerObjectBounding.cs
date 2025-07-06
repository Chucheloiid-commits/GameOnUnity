using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerObjectBounding : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private GameObject forbiddenAreaGroup; // Ссылка на GameObject, который является группой

    private Collider2D playerCollider;
    private Bounds combinedForbiddenBounds; // Будет хранить объединенные границы всех запретных зон

    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        CalculateCombinedForbiddenBounds();
    }

    void Update()
    {
        Bounds playerBounds = playerCollider.bounds;

        // Если игрок пересекает объединенную запретную зону
        if (IsOverlapping(playerBounds, combinedForbiddenBounds))
        {
            // Вычисляем корректную позицию СНАРУЖИ от границы
            Vector3 safePosition = CalculateSafePosition(playerBounds);
            transform.position = safePosition;
        }
    }

    void CalculateCombinedForbiddenBounds()
    {
        if (forbiddenAreaGroup == null)
        {
            Debug.LogWarning("Forbidden Area Group не назначена!");
            return;
        }

        Collider2D[] collidersInGroup = forbiddenAreaGroup.GetComponentsInChildren<Collider2D>();

        if (collidersInGroup.Length == 0)
        {
            Debug.LogWarning("В группе Forbidden Area нет ни одного Collider2D!");
            return;
        }

        // Инициализируем объединенные границы первым коллайдером
        combinedForbiddenBounds = collidersInGroup[0].bounds;

        // Объединяем границы всех остальных коллайдеров
        for (int i = 1; i < collidersInGroup.Length; i++)
        {
            combinedForbiddenBounds.Encapsulate(collidersInGroup[i].bounds);
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
        Vector2 forbiddenCenter = combinedForbiddenBounds.center; // Используем объединенные границы

        // Определяем, с какой стороны игрок ближе к границе
        float leftDist = playerBounds.max.x - combinedForbiddenBounds.min.x;
        float rightDist = combinedForbiddenBounds.max.x - playerBounds.min.x;
        float bottomDist = playerBounds.max.y - combinedForbiddenBounds.min.y;
        float topDist = combinedForbiddenBounds.max.y - playerBounds.min.y;

        // Выбираем ближайшую границу для выталкивания
        float minDist = Mathf.Min(leftDist, rightDist, bottomDist, topDist);

        Vector3 newPosition = transform.position;

        if (minDist == leftDist)
        {
            newPosition.x = combinedForbiddenBounds.min.x - playerBounds.extents.x;
        }
        else if (minDist == rightDist)
        {
            newPosition.x = combinedForbiddenBounds.max.x + playerBounds.extents.x;
        }
        else if (minDist == bottomDist)
        {
            newPosition.y = combinedForbiddenBounds.min.y - playerBounds.extents.y;
        }
        else // topDist
        {
            newPosition.y = combinedForbiddenBounds.max.y + playerBounds.extents.y;
        }
        newPosition.z = transform.position.z;
        return newPosition;
    }
}
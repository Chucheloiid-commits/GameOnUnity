using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic; // Для использования HashSet

public class RoomDiscoverer : MonoBehaviour
{
    // Ссылка на Tilemap, который будет использоваться для тумана войны.
    // В вашем случае, это будет ваш Tilemap "Kitchen".
    public Tilemap fogTilemap;

    // Ссылка на сам Tile Asset (ваш черный квадратный тайл).
    // Перетащите сюда ваш 'FogTile' из окна Project.
    public TileBase fogTile;

    // Ссылка на ScriptableObject RoomData, который описывает эту комнату
    // и ее соседей. Создайте RoomData Asset и перетащите его сюда.
    public RoomData currentRoomData;

    // Статический HashSet для отслеживания уже открытых комнат.
    // static означает, что это общий список для всех экземпляров RoomDiscoverer.
    private static HashSet<RoomData> discoveredRooms = new HashSet<RoomData>();

    // Опционально: Если вы хотите, чтобы первая комната была открыта по умолчанию
    public bool isStartingRoom = false;

    public FogMask fogMask; // ? ссылка на новый компонент


    void Start()
    {
        // Проверяем и пытаемся найти Tilemap, если не назначен вручную.
        if (fogTilemap == null)
        {
            GameObject kitchenObject = GameObject.Find("Kitchen");
            if (kitchenObject != null)
            {
                fogTilemap = kitchenObject.GetComponent<Tilemap>();
            }

            if (fogTilemap == null)
            {
                Debug.LogError($"RoomDiscoverer на объекте '{gameObject.name}': Fog Tilemap (Kitchen) не найден или не имеет компонента Tilemap! Назначьте его вручную.");
            }
        }

        // Проверяем, назначен ли Tile Asset
        if (fogTile == null)
        {
            Debug.LogError($"RoomDiscoverer на объекте '{gameObject.name}': Fog Tile Asset не назначен! Перетащите ваш черный Tile из окна Project в поле 'Fog Tile'.");
        }

        // Проверяем, назначены ли данные комнаты
        if (currentRoomData == null)
        {
            Debug.LogError($"RoomDiscoverer на объекте '{gameObject.name}': Room Data не назначена! Создайте RoomData ScriptableObject и перетащите его в поле 'Current Room Data'.");
        }

        // Если это стартовая комната, открываем ее сразу
        if (isStartingRoom && currentRoomData != null && fogTilemap != null && fogTile != null)
        {
            RevealRoomAndNeighbors(currentRoomData);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Убедитесь, что триггер реагирует только на игрока.
        // У вашего игрока должен быть Tag "Player".
        if (other.CompareTag("Player"))
        {
            // Открываем комнату только если она еще не была обнаружена
            if (currentRoomData != null && !discoveredRooms.Contains(currentRoomData))
            {
                Debug.Log($"Игрок вошел в триггер комнаты: {currentRoomData.name}");
                RevealRoomAndNeighbors(currentRoomData);
            }
        }
    }

    /// <summary>
    /// Открывает указанную комнату и все ее смежные комнаты, если они еще не были открыты.
    /// </summary>
    /// <param name="roomToReveal">Данные комнаты, которую нужно открыть.</param>
    private void RevealRoomAndNeighbors(RoomData roomToReveal)
    {
        if (roomToReveal == null || fogTilemap == null || fogTile == null)
        {
            Debug.LogWarning("Невозможно открыть комнату: Отсутствуют ссылки на RoomData, Tilemap или Fog Tile.");
            return;
        }

        // Открываем текущую комнату
        if (!discoveredRooms.Contains(roomToReveal))
        {
            RevealSingleRoom(roomToReveal.roomOrigin, roomToReveal.roomSize);
            discoveredRooms.Add(roomToReveal);
            Debug.Log($"Открыта комната: {roomToReveal.name} (Origin: {roomToReveal.roomOrigin}, Size: {roomToReveal.roomSize})");
        }
    }

    /// <summary>
    /// Удаляет тайлы тумана для указанной области комнаты.
    /// </summary>
    /// <param name="origin">Нижний левый угол комнаты в координатах тайлов.</param>
    /// <param name="size">Размер комнаты в тайлах (ширина, высота).</param>
    private void RevealSingleRoom(Vector3Int origin, Vector2Int size)
    {
        if (fogMask == null)
        {
            Debug.LogError("FogMask не назначен!");
            return;
        }

        List<Vector3Int> tilesToReveal = new List<Vector3Int>();
        for (int x = origin.x; x < origin.x + size.x; x++)
        {
            for (int y = origin.y; y < origin.y + size.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (fogMask.GetFogMask().Contains(pos))
                {
                    tilesToReveal.Add(pos);
                }
            }
        }

        fogMask.RevealTiles(tilesToReveal);
        Debug.Log($"Открыто {tilesToReveal.Count} позиций тумана в комнате.");
    }
}
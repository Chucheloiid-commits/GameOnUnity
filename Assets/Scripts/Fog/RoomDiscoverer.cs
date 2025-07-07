using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic; // ��� ������������� HashSet

public class RoomDiscoverer : MonoBehaviour
{
    // ������ �� Tilemap, ������� ����� �������������� ��� ������ �����.
    // � ����� ������, ��� ����� ��� Tilemap "Kitchen".
    public Tilemap fogTilemap;

    // ������ �� ��� Tile Asset (��� ������ ���������� ����).
    // ���������� ���� ��� 'FogTile' �� ���� Project.
    public TileBase fogTile;

    // ������ �� ScriptableObject RoomData, ������� ��������� ��� �������
    // � �� �������. �������� RoomData Asset � ���������� ��� ����.
    public RoomData currentRoomData;

    // ����������� HashSet ��� ������������ ��� �������� ������.
    // static ��������, ��� ��� ����� ������ ��� ���� ����������� RoomDiscoverer.
    private static HashSet<RoomData> discoveredRooms = new HashSet<RoomData>();

    // �����������: ���� �� ������, ����� ������ ������� ���� ������� �� ���������
    public bool isStartingRoom = false;

    public FogMask fogMask; // ? ������ �� ����� ���������


    void Start()
    {
        // ��������� � �������� ����� Tilemap, ���� �� �������� �������.
        if (fogTilemap == null)
        {
            GameObject kitchenObject = GameObject.Find("Kitchen");
            if (kitchenObject != null)
            {
                fogTilemap = kitchenObject.GetComponent<Tilemap>();
            }

            if (fogTilemap == null)
            {
                Debug.LogError($"RoomDiscoverer �� ������� '{gameObject.name}': Fog Tilemap (Kitchen) �� ������ ��� �� ����� ���������� Tilemap! ��������� ��� �������.");
            }
        }

        // ���������, �������� �� Tile Asset
        if (fogTile == null)
        {
            Debug.LogError($"RoomDiscoverer �� ������� '{gameObject.name}': Fog Tile Asset �� ��������! ���������� ��� ������ Tile �� ���� Project � ���� 'Fog Tile'.");
        }

        // ���������, ��������� �� ������ �������
        if (currentRoomData == null)
        {
            Debug.LogError($"RoomDiscoverer �� ������� '{gameObject.name}': Room Data �� ���������! �������� RoomData ScriptableObject � ���������� ��� � ���� 'Current Room Data'.");
        }

        // ���� ��� ��������� �������, ��������� �� �����
        if (isStartingRoom && currentRoomData != null && fogTilemap != null && fogTile != null)
        {
            RevealRoomAndNeighbors(currentRoomData);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ���������, ��� ������� ��������� ������ �� ������.
        // � ������ ������ ������ ���� Tag "Player".
        if (other.CompareTag("Player"))
        {
            // ��������� ������� ������ ���� ��� ��� �� ���� ����������
            if (currentRoomData != null && !discoveredRooms.Contains(currentRoomData))
            {
                Debug.Log($"����� ����� � ������� �������: {currentRoomData.name}");
                RevealRoomAndNeighbors(currentRoomData);
            }
        }
    }

    /// <summary>
    /// ��������� ��������� ������� � ��� �� ������� �������, ���� ��� ��� �� ���� �������.
    /// </summary>
    /// <param name="roomToReveal">������ �������, ������� ����� �������.</param>
    private void RevealRoomAndNeighbors(RoomData roomToReveal)
    {
        if (roomToReveal == null || fogTilemap == null || fogTile == null)
        {
            Debug.LogWarning("���������� ������� �������: ����������� ������ �� RoomData, Tilemap ��� Fog Tile.");
            return;
        }

        // ��������� ������� �������
        if (!discoveredRooms.Contains(roomToReveal))
        {
            RevealSingleRoom(roomToReveal.roomOrigin, roomToReveal.roomSize);
            discoveredRooms.Add(roomToReveal);
            Debug.Log($"������� �������: {roomToReveal.name} (Origin: {roomToReveal.roomOrigin}, Size: {roomToReveal.roomSize})");
        }
    }

    /// <summary>
    /// ������� ����� ������ ��� ��������� ������� �������.
    /// </summary>
    /// <param name="origin">������ ����� ���� ������� � ����������� ������.</param>
    /// <param name="size">������ ������� � ������ (������, ������).</param>
    private void RevealSingleRoom(Vector3Int origin, Vector2Int size)
    {
        if (fogMask == null)
        {
            Debug.LogError("FogMask �� ��������!");
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
        Debug.Log($"������� {tilesToReveal.Count} ������� ������ � �������.");
    }
}
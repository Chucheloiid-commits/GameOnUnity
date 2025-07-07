using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRoomData", menuName = "Room/Room Data")]
public class RoomData : ScriptableObject
{
    public Vector3Int roomOrigin; // ��������� ����� (������ ����� ����) ������� � ����������� ����� Tilemap
    public Vector2Int roomSize;   // ������ ������� � ������ (������, ������)
    public List<RoomData> adjacentRooms; // ������ ������ �� ScriptableObject-� �������� ������
}
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRoomData", menuName = "Room/Room Data")]
public class RoomData : ScriptableObject
{
    public Vector3Int roomOrigin; // Начальная точка (нижний левый угол) комнаты в координатах сетки Tilemap
    public Vector2Int roomSize;   // Размер комнаты в тайлах (ширина, высота)
    public List<RoomData> adjacentRooms; // Список ссылок на ScriptableObject-ы соседних комнат
}
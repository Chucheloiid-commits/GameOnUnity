using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class FogMask : MonoBehaviour
{
    public Tilemap fogTilemap; // Сюда перетаскиваем Tilemap с туманом
    public TileBase fogTile;   // Указываем типовой туманный тайл (например, FogTile)

    // Позиции, где реально размещены туманные тайлы
    private HashSet<Vector3Int> fogPositions = new HashSet<Vector3Int>();

    void Awake()
    {
        if (fogTilemap == null || fogTile == null)
        {
            Debug.LogError("FogTilemap или FogTile не назначены.");
            return;
        }

        fogPositions.Clear();
        BoundsInt bounds = fogTilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = fogTilemap.GetTile(pos);
            if (tile != null && tile.name == fogTile.name)
            {
                fogPositions.Add(pos);
            }
        }

        Debug.Log($"Загружено {fogPositions.Count} позиций тумана.");
    }

    // Метод, чтобы открыть конкретную клетку
    public void RevealTile(Vector3Int cellPosition)
    {
        if (fogPositions.Contains(cellPosition))
        {
            fogTilemap.SetTile(cellPosition, null);
            fogPositions.Remove(cellPosition);
        }
    }

    // Метод, чтобы открыть целую комнату по маске
    public void RevealTiles(IEnumerable<Vector3Int> positions)
    {
        foreach (var pos in positions)
        {
            RevealTile(pos);
        }
    }

    // Можно получить копию маски
    public HashSet<Vector3Int> GetFogMask()
    {
        return new HashSet<Vector3Int>(fogPositions);
    }
}

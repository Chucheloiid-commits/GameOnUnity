using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FogMask : MonoBehaviour
{
    public Tilemap fogTilemap;
    public TileBase fogTile;
    public float waveDelay = 0.03f; // Задержка между тайлами в волне

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

    public void RevealTile(Vector3Int cellPosition)
    {
        if (fogPositions.Contains(cellPosition))
        {
            fogTilemap.SetTile(cellPosition, null);
            fogPositions.Remove(cellPosition);
        }
    }

    public void RevealTiles(IEnumerable<Vector3Int> positions, Vector3Int origin)
    {
        StartCoroutine(RevealWave(positions, origin));
    }

    private IEnumerator RevealWave(IEnumerable<Vector3Int> positions, Vector3Int origin)
    {
        // Сортируем по расстоянию от origin (например, позиции игрока)
        var sorted = positions.OrderBy(p => Vector3Int.Distance(p, origin)).ToList();

        foreach (var pos in sorted)
        {
            RevealTile(pos);
            yield return new WaitForSeconds(waveDelay);
        }
    }

    public HashSet<Vector3Int> GetFogMask()
    {
        return new HashSet<Vector3Int>(fogPositions);
    }
}

using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class FogMask : MonoBehaviour
{
    public Tilemap fogTilemap; // ���� ������������� Tilemap � �������
    public TileBase fogTile;   // ��������� ������� �������� ���� (��������, FogTile)

    // �������, ��� ������� ��������� �������� �����
    private HashSet<Vector3Int> fogPositions = new HashSet<Vector3Int>();

    void Awake()
    {
        if (fogTilemap == null || fogTile == null)
        {
            Debug.LogError("FogTilemap ��� FogTile �� ���������.");
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

        Debug.Log($"��������� {fogPositions.Count} ������� ������.");
    }

    // �����, ����� ������� ���������� ������
    public void RevealTile(Vector3Int cellPosition)
    {
        if (fogPositions.Contains(cellPosition))
        {
            fogTilemap.SetTile(cellPosition, null);
            fogPositions.Remove(cellPosition);
        }
    }

    // �����, ����� ������� ����� ������� �� �����
    public void RevealTiles(IEnumerable<Vector3Int> positions)
    {
        foreach (var pos in positions)
        {
            RevealTile(pos);
        }
    }

    // ����� �������� ����� �����
    public HashSet<Vector3Int> GetFogMask()
    {
        return new HashSet<Vector3Int>(fogPositions);
    }
}

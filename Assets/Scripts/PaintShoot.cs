using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PaintShoot : MonoBehaviour
{
    public Tilemap tilemap;
    Camera cam;
    void Start()
    {

    }
    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.right, Color.blue, 999f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 15f, LayerMask.GetMask("Block"));

        if (hit.collider != null)
        {
            tilemap.RefreshAllTiles();

            int x, y;
            Vector3 hitPos = hit.point;
            hitPos.x += hitPos.x >= 0f ? 0.1f : -0.1f;
            hitPos.y += hitPos.y >= 0f ? 0.1f : -0.1f;
            x = this.tilemap.WorldToCell(hitPos).x;
            y = this.tilemap.WorldToCell(hitPos).y;

            Vector3Int v3Int = new Vector3Int(x, y, 0);

            //타일 색 바꿀 때 이게 있어야 하더군요
            this.tilemap.SetTileFlags(v3Int, TileFlags.None);

            //타일 색 바꾸기
            this.tilemap.SetColor(v3Int, (Color.red));
        }
    }
}

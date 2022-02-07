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
        Debug.Log("1");
            Debug.DrawRay(transform.position, Vector2.right, Color.blue,999f); 
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 15f, LayerMask.GetMask("Block"));
            if(hit)
            {
                this.tilemap.RefreshAllTiles();

                int x, y;
                x = this.tilemap.WorldToCell(hit.transform.position).x;
                y = this.tilemap.WorldToCell(hit.transform.position).y;

                Vector3Int v3Int = new Vector3Int(x, y, 0);



                //타일 색 바꿀 때 이게 있어야 하더군요
                this.tilemap.SetTileFlags(v3Int, TileFlags.None);

                //타일 색 바꾸기
                this.tilemap.SetColor(v3Int, (Color.red));
            }
        }
}

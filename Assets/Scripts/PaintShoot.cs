using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PaintShoot : MonoBehaviour
{
    private LineRenderer line;

    private Vector2 mouseP;
    private Vector2 targetPos;
    private Vector2 targetDir;
    private Vector3Int v3Int;
    private RaycastHit2D hit;
    private Vector3 shootDir;
    [SerializeField]
    private Tile[] paintTile;
    [SerializeField]
    private Tilemap paintTileMap;

    public int Remaining; //남은 개수
    int tileX, tileY;
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }
    void Update()
    {
        mouseP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.y = mouseP.y - transform.position.y;
        targetPos.x = mouseP.x - transform.position.x;
        targetDir = targetPos.normalized;
        TargetMouse();
        hit = Physics2D.Raycast(transform.position, targetDir, 999f, LayerMask.GetMask("Block"));
        Debug.DrawRay(transform.position, targetDir * 999, Color.red);
        Vector3 hitPos = hit.point;
        hitPos += (Vector3)targetDir* 0.01f;
        tileX = GameManager.Inst.tileMap.WorldToCell(hitPos).x;
        tileY = GameManager.Inst.tileMap.WorldToCell(hitPos).y;

        v3Int = new Vector3Int(tileX, tileY, 0);
      
    }
    public void TargetMouse()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            line.enabled = true;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, targetDir * 999f);
            ShootPaint();
        }
        else
        {
            line.enabled = false;
        }
    }
    public void ShootPaint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider != null)
            {
                if (Remaining > 0)
                {
                    Remaining--;
                    shootDir = new Vector3(hit.point.x - (v3Int.x + 0.5f), hit.point.y - (v3Int.y + 0.5f)).normalized;
                    if (Mathf.Abs(shootDir.x) == Mathf.Abs(shootDir.y)) { Debug.Log("return"); return; }
                    ShootDir();
                    GameManager.Inst.tileMap.RefreshAllTiles();

                    //타일 색 바꿀 때 이게 있어야 하더군요
                    //GameManager.Inst.tileMap.SetTileFlags(v3Int, TileFlags.None);
                    //타일 색 바꾸기
                    //GameManager.Inst.tileMap.SetColor(v3Int, (Color.red));
                    GameManager.Inst.SetPaintBlock(v3Int.x, v3Int.y, true);
                }
            }
        }
    }
    public void ShootDir()
    {
       
        if (Mathf.Abs(shootDir.x) < Mathf.Abs(shootDir.y))
        {
            if (hit.point.y > v3Int.y + 0.5)
            {
                paintTileMap.SetTile(v3Int, paintTile[2]);
            }
            if (hit.point.y < v3Int.y + 0.5)
            {
                paintTileMap.SetTile(v3Int, paintTile[3]);
            }
        }
        else
        {
            if (hit.point.x > v3Int.x + 0.5)
            {
                paintTileMap.SetTile(v3Int, paintTile[1]);
            }
            if (hit.point.x < v3Int.x + 0.5)
            {
                paintTileMap.SetTile(v3Int, paintTile[0]);
            }
        }
    }
}

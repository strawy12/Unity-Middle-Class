using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PaintShoot : MonoBehaviour
{
    private LineRenderer line;
    public Tilemap tilemap;
    private Vector2 mouseP;
    private Vector2 targetPos;
    private Vector2 targetDir;
    private Vector3Int v3Int;
    private RaycastHit2D hit;
    int x, y;
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
    
        hit = Physics2D.Raycast(transform.position, targetDir, 999f, LayerMask.GetMask("Block"));
        Debug.DrawRay(transform.position, targetDir * 999, Color.red);
        Vector3 hitPos = hit.point;
        hitPos += (Vector3)targetDir*0.5f;
        x = this.tilemap.WorldToCell(hitPos).x;
        y = this.tilemap.WorldToCell(hitPos).y;

        v3Int = new Vector3Int(x, y, 0);
        TargetMouse();
    }
    public void TargetMouse()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            line.enabled = true;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, mouseP);
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
            Debug.Log("hit:"+hit.point);
            Debug.Log("v3int:"+v3Int);
            ShootDir();
            if (hit.collider != null)
            {
                tilemap.RefreshAllTiles();

                //타일 색 바꿀 때 이게 있어야 하더군요
                this.tilemap.SetTileFlags(v3Int, TileFlags.None);
                //타일 색 바꾸기
                this.tilemap.SetColor(v3Int, (Color.red));
            }
        }
    }
    public void ShootDir()
    {
        Vector3 dir;
        dir = new Vector3(hit.point.x - (v3Int.x + 0.5f),hit.point.y -(v3Int.y + 0.5f)).normalized;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (hit.point.x > v3Int.x + 0.5)
            {
                Debug.Log("right");
            }
            if (hit.point.x < v3Int.x + 0.5)
            {
                Debug.Log("left");
            }
        }
        else
        {
            if (hit.point.y > v3Int.y + 0.5)
            {
                Debug.Log("up");
            }
            if (hit.point.y < v3Int.y + 0.5)
            {
                Debug.Log("down");
            }
        }
    }
}

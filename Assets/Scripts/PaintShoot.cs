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
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform bulletPosition;

    private Animator animator;
    private bool isShoot = false;
    public int Remaining; //남은 개수
    int tileX, tileY;
    void Start()
    {
        line = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        mouseP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        SetTargetPos();
        SetHitPos();
        TargetMouse();
        hit = Physics2D.Raycast(transform.position, targetDir, 999f, LayerMask.GetMask("Block"));

 

        v3Int = new Vector3Int(tileX, tileY, 0);
    }
    public void TargetMouse()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            line.enabled = true;
            line.SetPosition(0, transform.position);
            if (hit.collider != null)
            {
                line.SetPosition(1, hit.point);
            }
            else
            {
                line.SetPosition(1, targetDir * 999f);
            }
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
           
            if (hit && hit.transform.CompareTag("Platform"))
            {
                if (Remaining > 0)
                {
                    //isShoot = true;
                    animator.Play("Slime Shoot");
                    FirePaint();
                    SoundManager.Inst.SetEffectSound(1);
                    Remaining--;

                    shootDir = new Vector3(hit.point.x - (v3Int.x + 0.5f), hit.point.y - (v3Int.y + 0.5f)).normalized;
                    if (Mathf.Abs(shootDir.x) == Mathf.Abs(shootDir.y)) { return; }
                    GameManager.Inst.tileMap.RefreshAllTiles();

                    //타일 색 바꿀 때 이게 있어야 하더군요
                    //GameManager.Inst.tileMap.SetTileFlags(v3Int, TileFlags.None);
                    //타일 색 바꾸기
                    //GameManager.Inst.tileMap.SetColor(v3Int, (Color.red));
                }
            }
        }
    }
    public void ShootDir(Vector3 hitP, Vector3Int v3I)
    {
        shootDir = new Vector3(hitP.x - (v3I.x + 0.5f), hitP.y - (v3I.y + 0.5f)).normalized;
        if (Mathf.Abs(shootDir.x) < Mathf.Abs(shootDir.y))
        {
            if (hitP.y > v3I.y + 0.5)
            {
                GameManager.Inst.SetPaintBlock(v3I.x, v3I.y, true, GravityState.Down);
                paintTileMap.SetTile(v3I, paintTile[2]);
            }
            if (hitP.y < v3I.y + 0.5)
            {
                GameManager.Inst.SetPaintBlock(v3I.x, v3I.y, true, GravityState.Up);
                paintTileMap.SetTile(v3I, paintTile[3]);
            }
        }
        else
        {
            if (hitP.x > v3I.x + 0.5)
            {
                GameManager.Inst.SetPaintBlock(v3I.x, v3I.y, true, GravityState.Left);
                paintTileMap.SetTile(v3I, paintTile[1]);
            }
            if (hitP.x < v3I.x + 0.5)
            {
                GameManager.Inst.SetPaintBlock(v3I.x, v3I.y, true, GravityState.Right);
                paintTileMap.SetTile(v3I, paintTile[0]);
            }
        }
    }
    public void FirePaint()
    {
        float angle = Mathf.Atan2(mouseP.y - transform.position.y, mouseP.x - transform.position.x) * Mathf.Rad2Deg;
        GameObject newBullet= Instantiate(bullet, bulletPosition);
        newBullet.GetComponent<BulletMove>().SetVariable(hit.point, v3Int);
        newBullet.transform.SetParent(null);
        newBullet.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
    public void SetTargetPos()
    {
        targetPos.y = mouseP.y - transform.position.y;
        targetPos.x = mouseP.x - transform.position.x;
        targetDir = targetPos.normalized;
    } 
    public void SetHitPos()
    {
        Vector3 hitPos = hit.point;
        hitPos += (Vector3)targetDir * 0.02f;
        tileX = GameManager.Inst.tileMap.WorldToCell(hitPos).x;
        tileY = GameManager.Inst.tileMap.WorldToCell(hitPos).y;
    }
}

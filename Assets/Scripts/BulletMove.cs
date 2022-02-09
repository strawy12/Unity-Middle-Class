using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private PaintShoot paintShoot;
    private float speed = 10f;
    private Vector3 hitPoint;
    private Vector3Int v3Int;
    void Start()
    {
        paintShoot = FindObjectOfType<PaintShoot>();
    }
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Square"))
        {
            Destroy(gameObject);
            paintShoot.ShootDir(hitPoint,v3Int);
        }
    }
    public void SetVariable(Vector3 hitP, Vector3Int v3I)
    {
        v3Int = v3I;
        hitPoint = hitP;
    }
}

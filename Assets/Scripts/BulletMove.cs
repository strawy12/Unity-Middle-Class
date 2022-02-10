using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private PaintShoot paintShoot;
    private float speed = 15f;
    private Vector3 hitPoint;
    private Vector3Int v3Int;
    void Start()
    {
        paintShoot = FindObjectOfType<PaintShoot>();
    }
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        speed += Time.deltaTime * 2f;
        HItCheck();
    }
    //private void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (col.gameObject.CompareTag("Square"))
    //    {
    //        Destroy(gameObject);
    //        Debug.Log("col");
    //        paintShoot.ShootDir(hitPoint,v3Int);
    //    }
    //}
    public void SetVariable(Vector3 hitP, Vector3Int v3I)
    {
        v3Int = v3I;
        hitPoint = hitP;
    }
    public void HItCheck()
    {
        if(Vector2.Distance(hitPoint,transform.position) < 0.5f)
        {
            Destroy(gameObject);
            Debug.Log("destroy");
            paintShoot.ShootDir(hitPoint, v3Int);
        }
    }
}

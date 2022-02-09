using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    private Vector2 currentGravityDir;

    public float speed;
    public float rotateSpeed = 10.0f;
    public float jumpForce = 1.0f; // 점프하는 힘

    Rigidbody2D body; // 컴포넌트에서 RigidBody를 받아올 변수

    [SerializeField] private bool isGround = true;
    [SerializeField] private GameObject player;

    [SerializeField] private Tilemap tilemap;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        //GetComponent를 활용하여 body에 해당 오브젝트의 Rigidbody를 넣어준다. 
        currentGravityDir = Vector2.down;
        EventManager.StartListening("CHANGEGRAVITYSTATE", Rotation);
    }

    void FixedUpdate()
    {
        Move();
        Jump();
        Gravity();
        DetectedRaycast();
    }


    public float maxDistance = 5f;

    void DetectedRaycast()
    {
        RaycastHit2D upHit = Physics2D.Raycast(transform.position, Vector2.up, maxDistance, LayerMask.GetMask("Gravity"));
        RaycastHit2D downHit = Physics2D.Raycast(transform.position, Vector2.down, maxDistance, LayerMask.GetMask("Gravity"));
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, maxDistance, LayerMask.GetMask("Gravity"));
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, maxDistance, LayerMask.GetMask("Gravity"));

        Debug.DrawRay(transform.position, Vector2.up * maxDistance, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * maxDistance, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * maxDistance, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * maxDistance, Color.red);

        GravityPosColor(upHit, GravityState.Up);
        GravityPosColor(downHit, GravityState.Down);
        GravityPosColor(leftHit, GravityState.Left);
        GravityPosColor(rightHit, GravityState.Right);
    }

    Vector3Int GravityPosColor(RaycastHit2D detectedRay, GravityState detectType)
    {
        int x = tilemap.WorldToCell(detectedRay.point).x;
        int y = tilemap.WorldToCell(detectedRay.point).y;

        Vector3Int tilepos = new Vector3Int(x, y, 0);

        Debug.Log(tilepos);

        if (tilemap.GetColor(tilepos) == Color.red)
        {
            GameManager.Inst.SetGravityState(detectType);
        }

        return tilepos;
    }

    void Move()
    {
        float moveX = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveX -= speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveX += speed;
        }

        transform.Translate(new Vector2(moveX, 0f) * 0.1f);
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            body.AddForce(currentGravityDir * jumpForce * -1f, ForceMode2D.Impulse);
            isGround = false;
        }
    }

    void Rotation()
    {
        float zRotate = 0f;

        zRotate = GameManager.Inst.GetZRotate();

        transform.rotation = Quaternion.Euler(0f, 0f, zRotate);

        currentGravityDir = GameManager.Inst.GetGravityDirection();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Square"))
        {
            isGround = true;
        }
    }

    void Gravity()
    {
        body.AddForce(currentGravityDir * 9.8f);
    }
}

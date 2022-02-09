using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{ 
    private Vector2 currentGravityDir;

    public float speed;
    public float rotateSpeed = 10.0f;
    public float jumpForce = 1.0f; // Á¡ÇÁÇÏ´Â Èû

    Rigidbody2D body; 
    [SerializeField] private bool isGround = true;


    [SerializeField] private Tilemap tilemap;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        currentGravityDir = Vector2.down;
    }

    void FixedUpdate()
    {
        Move();
        Jump();
        Gravity();
        DetectedRaycast();
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Square"))
        { 
            isGround = true;
        }
    }

    public float maxDistance = 3f;

    void DetectedRaycast()
    {
        RaycastHit2D upHit = Physics2D.Raycast(transform.position, Vector2.up, maxDistance);
        RaycastHit2D downHit = Physics2D.Raycast(transform.position, Vector2.down, maxDistance);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, maxDistance);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, maxDistance);

        Debug.DrawRay(transform.position, Vector2.up * maxDistance, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * maxDistance, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * maxDistance, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * maxDistance, Color.red);

        int x = tilemap.WorldToCell(upHit.point).x;
        int y = tilemap.WorldToCell(upHit.point).y;

        Vector3Int tilepos = new Vector3Int(x, y, 0);

        //Debug.Log("gi : " + tilepos);

        if (tilemap.GetColor(tilepos) == Color.black)
        {
            Debug.Log("gi : " + tilepos);
        }
    }

    void Rotation()
    {
        float zRotate = 0f;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentGravityDir = Vector2.left;
            GameManager.Inst.SetGravityState(GravityState.Left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentGravityDir = Vector2.right;
            GameManager.Inst.SetGravityState(GravityState.Right);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentGravityDir = Vector2.down;
            GameManager.Inst.SetGravityState(GravityState.Down);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentGravityDir = Vector2.up;
            GameManager.Inst.SetGravityState(GravityState.Up);
        }

        zRotate = GameManager.Inst.GetZRotate();

        transform.rotation = Quaternion.Euler(0f, 0f, zRotate);
    }

    void Gravity()
    {
        body.AddForce(currentGravityDir * 9.8f);
    }
}

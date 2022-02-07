using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    private Vector2 currentGravityDir;

    public float speed;
    public float rotateSpeed = 10.0f;
    public float jumpForce = 1.0f; // 점프하는 힘

    Rigidbody2D body; // 컴포넌트에서 RigidBody를 받아올 변수

    private bool isGround = true;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        //GetComponent를 활용하여 body에 해당 오브젝트의 Rigidbody를 넣어준다. 
        currentGravityDir = Vector2.down;
    }

    void FixedUpdate()
    {
        Move();
        Jump();
        Rotation();
        Gravity();
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

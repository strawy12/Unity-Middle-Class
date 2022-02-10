using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    private Vector2 currentGravityDir;
    private GravityState currentGravityState;

    public float speed;
    public float rotateSpeed = 10.0f;
    public float jumpForce = 1.0f; // 점프하는 힘
    public float maxDistance = 5f;

    Rigidbody2D body; // 컴포넌트에서 RigidBody를 받아올 변수
    private RaycastHit2D hit;
    [SerializeField] private bool isGround = true;
    [SerializeField] private GameObject player;

    private List<RaycastData> raycastDataList;

    public bool isSpaceCheck;
    private int doubleSpace = 2;

    private Vector3Int CurrentTilePos { get { return GameManager.Inst.tileMap.WorldToCell(transform.position); } }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        currentGravityState = GravityState.Down;
        InitiateRaycastDataList();
        //GetComponent를 활용하여 body에 해당 오브젝트의 Rigidbody를 넣어준다. 
        currentGravityDir = Vector2.down;
    }

    private void Update()
    {

        if (GameManager.Inst.gameState == GameState.Start)
        {
            Move();
            SpaceGravityCheck();
            Gravity();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DetectedRaycast();
            Jump();
        }

    }

    void RayCastOut()
    {
        currentGravityState = GravityState.Down;
        Rotation();

    }

    void SpaceGravityCheck()
    {
        bool detectCheck = false;

        for (int index = 0; index < (int)GravityState.Count; index++)
        {
            if(CheckDetectRaycast((GravityState)index))
            {
                detectCheck = true;
            }
        }

        isSpaceCheck = detectCheck && isSpaceCheck;

        if(!detectCheck)
        {
            Invoke("RayCastOut", .2f);
        }
    }

    private void InitiateRaycastDataList()
    {
        raycastDataList = new List<RaycastData>();

        for (int index = 0; index < (int)GravityState.Count; index++)
        {
            raycastDataList.Add(new RaycastData((GravityState)index));
        }
    }

    void DetectedRaycast()
    {
        GravityState priorityType = GravityState.None;
        float minDistance = 999f;
        float distance;

        for (int index = 0; index < raycastDataList.Count; index++)
        {
            if (raycastDataList[index].isdetected)
            {
                distance = raycastDataList[index].detectDistance;

                if (distance < minDistance)
                {
                    priorityType = raycastDataList[index].detectType;
                    minDistance = distance;
                }

                //else if(distance == minDistance)
                //{
                //    if(index < (int)priorityType)
                //    {
                //        priorityType = (GravityState)index;
                //        // for문이라 작은거부터 큰거로 비교해가지고
                //    }
                //}
            }
        }

        if (priorityType != GravityState.None)
        {
            if (currentGravityState != priorityType)
            {
                currentGravityState = priorityType;

                Rotation();
                //body.velocity = Vector2.zero;
            }
        }
        else
        {
            currentGravityState = GravityState.Down;
            Rotation();
        }
    }

    private bool CheckDetectRaycast(GravityState state)
    {
        Vector2 direction = GameManager.Inst.GetGravityDirection(state);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, LayerMask.GetMask("Block"));
        Debug.DrawRay(transform.position, direction * maxDistance, Color.red);
        Vector3Int tilepos = Vector3Int.zero;
        RaycastData raycastData = raycastDataList.Find((data) => data.detectType == state);
        //raycastData = raycastDataList[(int)state];


        if (!hit)
        {
            raycastData.isdetected = false;
            raycastData.detectDistance = 0f;

            return false;
        }

        Vector3 hitPos = hit.point;
        hitPos += (Vector3)direction * 0.01f;

        int x = GameManager.Inst.tileMap.WorldToCell(hitPos).x;
        int y = GameManager.Inst.tileMap.WorldToCell(hitPos).y;

        tilepos.x = x;
        tilepos.y = y;

        if (GameManager.Inst.PaintBlockCheck(tilepos.x, tilepos.y, state))
        {
            isSpaceCheck = currentGravityState != state;

            raycastData.isdetected = true;
            raycastData.detectDistance = Vector3Int.Distance(tilepos, CurrentTilePos);
           
            return true;
        }

        else
        {
            raycastData.isdetected = false;
            raycastData.detectDistance = 0f;

            return false;
        }
    }

    void Move()
    {
        float moveDirValue = 0f;
        switch (currentGravityState)
        {
            case GravityState.Down:
            case GravityState.Up:
                moveDirValue = HorizontalMove();
                break;

            case GravityState.Left:
            case GravityState.Right:
                moveDirValue = VerticalMove();
                break;
        }

        transform.Translate(new Vector2(moveDirValue , 0f)* speed * Time.deltaTime);
    }

    float HorizontalMove()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (currentGravityState == GravityState.Up)
        {
            moveX *= -1f;
        }

        return moveX;
    }

    float VerticalMove()
    {

        float moveY = Input.GetAxis("Vertical");

        if (moveY > 0f)
        {
            int posX = CurrentTilePos.x + (currentGravityState == GravityState.Left ? -1 : 1);
            //if (!GameManager.Inst.PaintBlockCheck(posX, CurrentTilePos.y + 1) && transform.position.y > CurrentTilePos.y + 0.5f)
            //{
            //    return 0f;
            //}
        }

        if (currentGravityState == GravityState.Left)
        {
            moveY *= -1f;
        }
        return moveY;
    }

    void Jump()
    {
        if (isGround && !isSpaceCheck)
        {
            if (currentGravityState != GravityState.Down)
            {
                if(doubleSpace > 0) { 
                    body.AddForce(currentGravityDir * jumpForce * -1f, ForceMode2D.Impulse);
                    doubleSpace--;
                }
            }
            else
            {
                isGround = false;
                body.AddForce(currentGravityDir * jumpForce * -1f, ForceMode2D.Impulse);
            } 
        }
    }

    void Rotation()
    {
        float zRotate = 0f;

        zRotate = GameManager.Inst.GetZRotate(currentGravityState);

        transform.rotation = Quaternion.Euler(0f, 0f, zRotate);


            SetGravityDirecction();

        
    }

    private void SetGravityDirecction()
    {
        currentGravityDir = GameManager.Inst.GetGravityDirection(currentGravityState);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Square"))
        {
            doubleSpace = 2;
            isGround = true;
        }
    }

    void Gravity()
    {
        body.AddForce(currentGravityDir * 9.8f);
    }
}

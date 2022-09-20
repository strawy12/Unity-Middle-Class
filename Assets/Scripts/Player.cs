using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    private Vector2 currentGravityDir;
    private GravityState currentGravityState;
    [SerializeField] private GameObject clearPanel;
    public float speed;
    public float rotateSpeed = 10.0f;
    public float jumpForce = 1.0f; // 점프하는 힘
    public float maxDistance = 5f;

    Rigidbody2D body; // 컴포넌트에서 RigidBody를 받아올 변수
    private RaycastHit2D hit;
    [SerializeField] private GameObject player;
    private Collider2D col;

    private Animator animator = null;
    private List<RaycastData> raycastDataList;

    public bool isSpaceCheck;
    public bool isChangeGravity;

    Coroutine coroutine = null;

    private Vector3Int CurrentTilePos { get { return GameManager.Inst.tileMap.WorldToCell(transform.position); } }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        currentGravityState = GravityState.Down;
        InitiateRaycastDataList();
        //GetComponent를 활용하여 body에 해당 오브젝트의 Rigidbody를 넣어준다. 
        currentGravityDir = Vector2.down;
    }

    private void FixedUpdate()
    {
        if (GameManager.Inst.gameState == GameState.Start)
        {
            Gravity();
        }
    }

    private void Update()
    {

        if (GameManager.Inst.gameState == GameState.Start)
        {
            if (isChangeGravity)
            {
                return;
            }

            Move();
            ChangeFace();
            SpaceGravityCheck();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DetectedRaycast();
            Jump();
        }

        if (body.velocity.y <= 0.1f)
        {
            animator.SetTrigger("dd");
        }

    }

    IEnumerator RayCastOut()
    {
        yield return new WaitForSeconds(.2f);
        if (currentGravityState != GravityState.Down && !isChangeGravity)
        {
            currentGravityState = GravityState.Down;
            Rotation();
        }
        coroutine = null;
    }

    void SpaceGravityCheck()
    {
        bool detectCheck = false;
        bool canMaintain = false;

        for (int index = 0; index < (int)GravityState.Count; index++)
        {
            if (CheckDetectRaycast((GravityState)index))
            {
                detectCheck = true;

                if (!canMaintain && (GravityState)index == currentGravityState)
                    canMaintain = true;
            }
        }

        isSpaceCheck = detectCheck && isSpaceCheck;

        if(canMaintain)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;  
            }
        }


        if (!detectCheck || !canMaintain)
        {
            if (coroutine != null) return;

            coroutine = StartCoroutine(RayCastOut());
        }
    }

    private void ChangeFace()
    {
        if (body.velocity.x < 0.1f && body.velocity.x > -0.1f)
        {
            float posX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

            if (posX > transform.position.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (posX < transform.position.x)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
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
        RaycastData data = null;
        float minDistance = 999f;
        float distance;

        for (int index = 0; index < raycastDataList.Count; index++)
        {
            if (raycastDataList[index].isdetected)
            {
                distance = raycastDataList[index].detectDistance;

                if (distance < minDistance)
                {
                    data = raycastDataList[index];
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

        if (data != null)
        {
            if (currentGravityState != data.detectType)
            {
                isChangeGravity = true;

                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                    coroutine = null;
                }

                currentGravityState = data.detectType;

                Rotation();
            }
        }
        else
        {
            if (currentGravityState != GravityState.Down)
            {
                currentGravityState = GravityState.Down;
                Rotation();
            }


        }
    }

    private bool CheckDetectRaycast(GravityState state)
    {
        Vector2 direction = GameManager.Inst.GetGravityDirection(state);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, LayerMask.GetMask("Block"));
        Vector3Int tilepos = Vector3Int.zero;
        RaycastData raycastData = raycastDataList.Find((data) => data.detectType == state);
        //raycastData = raycastDataList[(int)state];

        if (!hit || !hit.transform.gameObject.CompareTag("Platform"))
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
            raycastData.hitPos = hit.point;
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

        if (moveDirValue > 0.1f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (moveDirValue < -0.1f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        transform.Translate(new Vector2(moveDirValue, 0f) * speed * Time.deltaTime);
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
        if (IsGrounded() && !isSpaceCheck)
        {
            animator.Play("Slime Jump Up");
            SoundManager.Inst.SetEffectSound(2);
            body.AddForce(currentGravityDir * jumpForce * -1f, ForceMode2D.Impulse);
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
        body.velocity = Vector2.zero;
        currentGravityDir = GameManager.Inst.GetGravityDirection(currentGravityState);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            clearPanel.SetActive(true);
            GameManager.Inst.SetGameState(GameState.Clear);
            Time.timeScale = 0;
        }
    }

    void Gravity()
    {
        if (isChangeGravity && IsGrounded())
        {
            isChangeGravity = false;
        }
        if (isChangeGravity && !IsGrounded())
        {
            body.AddForce(currentGravityDir * 9.8f * 7f);
        }

        else
        {
            body.AddForce(currentGravityDir * 9.8f);
        }
    }
    private bool IsGrounded()
    {
        Vector2 pos = Vector2.zero;
        Vector2 size = new Vector2(col.bounds.size.x * 0.5f, col.bounds.size.y * 0.5f);

        switch (currentGravityState)
        {
            case GravityState.Down:
                pos = new Vector2(col.bounds.center.x, col.bounds.min.y);
                break;

            case GravityState.Up:
                pos = new Vector2(col.bounds.center.x, col.bounds.max.y);
                break;


            case GravityState.Left:
                pos = new Vector2(col.bounds.min.x, col.bounds.center.y);
                break;

            case GravityState.Right:
                pos = new Vector2(col.bounds.max.x, col.bounds.center.y);
                break;

        }

        return Physics2D.OverlapBoxAll(pos, size, 180f).Length > 1;
    }
}

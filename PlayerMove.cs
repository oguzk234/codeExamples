using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameObject MainCamera;
    public static PlayerMove Instance;
    private Rigidbody2D rg;
    private PlayerCollision playerCollision;

    [Header("Pixel Perfect")]
    [SerializeField] private float pixelSize = 0.0625f;

    [Header("Input")]
    public Vector2 MoveInput;
    public Vector2 MoveInputNormalized;
    public Vector3 MoveGo;
    public Vector3 MoveGoChecked;

    [Header("Move")]
    [SerializeField] private float moveFastCooldownMaxDef;
    [SerializeField] private float moveCooldownMaxDef;
    [SerializeField] private float moveCooldownMax;
    [SerializeField] private float moveCooldown;
    public bool isMovable;
    public bool isMoveInputGetting;


    [Header("Animations")]
    private Animator anim;  //ALT OBJENÝN ANIMI OLACAK
    public GameObject animObj;
    public float currentAnimationPercent;


    public enum MoveInputRot { none,right,left,up,down,rightUp,rightDown,leftUp,leftDown }
    public MoveInputRot moveInputRot;

    public Vector2 rightUp = new Vector2(1, 1);
    public Vector2 rightDown = new Vector2(1, -1);
    public Vector2 leftUp = new Vector2(-1, 1);
    public Vector2 leftDown = new Vector2(-1, -1);


    public Vector2 HeadingDirection;



    private void Awake()
    {
        Instance = this;
        rg = GetComponent<Rigidbody2D>();
        playerCollision = GetComponent<PlayerCollision>();
        anim = GetComponent<Animator>();
        pixelSize = Camera.main.orthographicSize * 2 / 216;
    }


    void Start()
    {
        
    }


    void Update()
    {
        MoveMain();
        Run();
    }




    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveCooldownMax = moveFastCooldownMaxDef;
        }
        else
        {
            moveCooldownMax = moveCooldownMaxDef;
        }
    }

    private void MoveMain()
    {
        if (!isMoveInputGetting) { return; }

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        MoveInput = new Vector2(xInput, yInput);
        MoveInputNormalized = MoveInput.normalized;


        MoveInputRot oldMoveInputRot = moveInputRot; 

        if(MoveInput == Vector2.zero)
        {
            moveInputRot = MoveInputRot.none;
        }
        else if(MoveInput == Vector2.right)
        {
            moveInputRot = MoveInputRot.right;
            if (oldMoveInputRot != moveInputRot) { PlayMoveAnimation(MoveInputRot.right); }
        }
        else if (MoveInput == Vector2.left)
        {
            moveInputRot = MoveInputRot.left;
            if (oldMoveInputRot != moveInputRot) { PlayMoveAnimation(MoveInputRot.left); }
        }
        else if (MoveInput == Vector2.up)
        {
            moveInputRot = MoveInputRot.up;
            if (oldMoveInputRot != moveInputRot) { PlayMoveAnimation(MoveInputRot.up); }
        }
        else if (MoveInput == Vector2.down)
        {
            moveInputRot = MoveInputRot.down;
            if (oldMoveInputRot != moveInputRot) { PlayMoveAnimation(MoveInputRot.down); }
        }
        else if (MoveInput == rightUp)
        {
            moveInputRot = MoveInputRot.rightUp;
        }
        else if (MoveInput == rightDown)
        {
            moveInputRot = MoveInputRot.rightDown;
        }
        else if (MoveInput == leftUp)
        {
            moveInputRot = MoveInputRot.leftUp;
        }
        else if (MoveInput == leftDown)
        {
            moveInputRot = MoveInputRot.leftDown;
        }

        MoveGo = Vector3.zero;

        if (!isMovable) { return; }


        moveCooldown -= Time.deltaTime;

        if(moveCooldown <= 0 && MoveInput != Vector2.zero)
        {
            HeadingDirection = MoveInput;
            MoveGo = (Vector3)MoveInput * pixelSize;
            MoveGoChecked = playerCollision.CheckFutureMoveCollision(MoveGo);

            this.gameObject.transform.position += MoveGoChecked;
            //rg.MovePosition(rg.position += (Vector2)MoveGo);
            
            if (isMoveInputSingular()) { moveCooldown = moveCooldownMax; }
            else { moveCooldown = moveCooldownMax * Mathf.Sqrt(2); }
            
        }

    }
    public bool isMoveInputSingular()
    {
        if(MoveInput.x != 0 && MoveInput.y != 0) { return false; } 
        return true;
    }   //NOT USING CURRENTLY
    private void PlayMoveAnimation(MoveInputRot Rot)
    {
        currentAnimationPercent = anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
        switch (Rot)
        {
            case MoveInputRot.down:
                anim.Play("MainCharWalkDown",0, currentAnimationPercent);
                anim.transform.localScale = new Vector3(1, 1, 1);
                break;
            case MoveInputRot.up:
                anim.Play("MainCharWalkUp",0,currentAnimationPercent);
                anim.transform.localScale = new Vector3(1, 1, 1);
                break;
            case MoveInputRot.right:
                anim.Play("MainCharWalkSide",0,currentAnimationPercent);
                anim.transform.localScale = new Vector3(1, 1, 1);
                break;
            case MoveInputRot.left:
                anim.Play("MainCharWalkSide",0,currentAnimationPercent);
                anim.transform.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    //public static PlayerCollision Instance;

    public float charSizeHorizontal = 13f;
    public float charSizeVertical = 10f;
    public float charSizeCross = 9f;
    public float RayRangeHorizontal = 0.945f;
    public float RayRangeVertical = 0.7425f;
    public float AltRaysHorizontalPlus;
    public float AltRaysVerticalPlus;
    public float RayRangeCross = 1f;
    public float RayRangeUpPlus;
    public bool autoRaySize;
    private PlayerMove playerMove;
    public LayerMask CollisionLayer;



    //public static int noCollisionLayer = 3;  //KAPININ ÝÇÝNE GÝRME VB.(IsBlocking == false OLAN OBJELERDE ICINDEN GECME BUGUNU ÖNLEMEK ICIN OBJEYE VERILIR)


    // + (RayRangeVerticalTopSidePlus * 0.0675f)

    private void Awake()
    {

        //Instance = this;
        //noCollisionLayer = LayerMask.NameToLayer("ColliderRay"); // == 3

        playerMove = GetComponent<PlayerMove>();

        if (autoRaySize) { RayRangeHorizontal = charSizeHorizontal * 0.0675f; RayRangeVertical = charSizeVertical * 0.0675f; RayRangeCross = charSizeCross * (0.0675f * Mathf.Sqrt(2)); }
    }

    public Vector2 CheckFutureMoveCollision(Vector2 MoveInput)
    {
        /*
        float rayRange = RayRange;

        if (PlayerMove.Instance.isMoveInputSingular())
        {
            rayRange = RayRange * Mathf.Sqrt(2);
        }
        */

        //float RayRange2X = RayRangeHorizontal * Mathf.Sqrt(2);

        List<RaycastHit2D>hitList = new List<RaycastHit2D>();
        RaycastHit2D hit1 = default;  //YATAY
        RaycastHit2D hit2 = default;  //YATAY2sag
        RaycastHit2D hit3 = default;  //YATAY3sol
        RaycastHit2D hit4 = default;  //DIKEY
        RaycastHit2D hit5 = default;  //DIKEY2sag
        RaycastHit2D hit6 = default;  //DIKEY3sol
        switch (playerMove.moveInputRot)
        {

            case PlayerMove.MoveInputRot.right:
                Debug.DrawRay(transform.position, Vector2.right * RayRangeHorizontal, Color.red);
                //Debug.DrawRay((Vector2)transform.position + Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.right * RayRangeHorizontal, Color.green);
                Debug.DrawRay((Vector2)transform.position + -Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.right * RayRangeHorizontal, Color.blue);

                hit1 = Physics2D.Raycast(transform.position, Vector2.right, RayRangeHorizontal,CollisionLayer);
                //hit2 = Physics2D.Raycast((Vector2)transform.position + Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.right, RayRangeHorizontal,collisionLayer);
                hit3 = Physics2D.Raycast((Vector2)transform.position + -Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.right, RayRangeHorizontal, CollisionLayer);
                break;

            case PlayerMove.MoveInputRot.left:
                Debug.DrawRay(transform.position, Vector2.left * RayRangeHorizontal, Color.red);
                //Debug.DrawRay((Vector2)transform.position + Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.left * RayRangeHorizontal, Color.green);
                Debug.DrawRay((Vector2)transform.position + -Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.left * RayRangeHorizontal, Color.blue);

                hit1 = Physics2D.Raycast(transform.position, Vector2.left, RayRangeHorizontal, CollisionLayer);
                //hit2 = Physics2D.Raycast((Vector2)transform.position + Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.left, RayRangeHorizontal,collisionLayer);
                hit3 = Physics2D.Raycast((Vector2)transform.position + -Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.left, RayRangeHorizontal, CollisionLayer);
                break;

            case PlayerMove.MoveInputRot.up:
                Debug.DrawRay(transform.position, Vector2.up * (RayRangeVertical + RayRangeUpPlus * 0.0675f), Color.red);
                Debug.DrawRay((Vector2)transform.position + Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.up * (RayRangeVertical + RayRangeUpPlus * 0.0675f), Color.green);
                Debug.DrawRay((Vector2)transform.position + -Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.up * (RayRangeVertical + RayRangeUpPlus * 0.0675f), Color.blue);

                hit4 = Physics2D.Raycast(transform.position, Vector2.up, RayRangeVertical + RayRangeUpPlus*0.0675f, CollisionLayer);
                hit5 = Physics2D.Raycast((Vector2)transform.position + Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.up, RayRangeVertical + RayRangeUpPlus * 0.0675f, CollisionLayer);
                hit6 = Physics2D.Raycast((Vector2)transform.position + -Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.up, RayRangeVertical + RayRangeUpPlus * 0.0675f, CollisionLayer);
                break;

            case PlayerMove.MoveInputRot.down:
                Debug.DrawRay(transform.position, Vector2.down * RayRangeVertical, Color.red);
                Debug.DrawRay((Vector2)transform.position + Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.down * RayRangeVertical, Color.green);
                Debug.DrawRay((Vector2)transform.position + -Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.down * RayRangeVertical, Color.blue);

                hit4 = Physics2D.Raycast(transform.position, Vector2.down, RayRangeVertical, CollisionLayer);
                hit5 = Physics2D.Raycast((Vector2)transform.position + Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.down, RayRangeVertical, CollisionLayer);
                hit6 = Physics2D.Raycast((Vector2)transform.position + -Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.down, RayRangeVertical, CollisionLayer);
                break;


            case PlayerMove.MoveInputRot.rightUp:

                Debug.DrawRay(transform.position, Vector2.right * RayRangeHorizontal, Color.red);
                //Debug.DrawRay((Vector2)transform.position + Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.right * RayRangeHorizontal, Color.green);
                Debug.DrawRay((Vector2)transform.position + -Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.right * RayRangeHorizontal, Color.blue);
                Debug.DrawRay(transform.position, Vector2.up * (RayRangeVertical + RayRangeUpPlus * 0.0675f), Color.red);
                Debug.DrawRay((Vector2)transform.position + Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.up * (RayRangeVertical + RayRangeUpPlus * 0.0675f), Color.green);
                Debug.DrawRay((Vector2)transform.position + -Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.up * (RayRangeVertical + RayRangeUpPlus * 0.0675f), Color.blue);

                hit1 = Physics2D.Raycast(transform.position, Vector2.right, RayRangeHorizontal, CollisionLayer);
                //hit2 = Physics2D.Raycast((Vector2)transform.position + Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.right, RayRangeHorizontal,collisionLayer);
                hit3 = Physics2D.Raycast((Vector2)transform.position + -Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.right, RayRangeHorizontal, CollisionLayer);
                hit4 = Physics2D.Raycast(transform.position, Vector2.up, RayRangeVertical + RayRangeUpPlus * 0.0675f, CollisionLayer);
                hit5 = Physics2D.Raycast((Vector2)transform.position + Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.up, RayRangeVertical + RayRangeUpPlus * 0.0675f, CollisionLayer);
                hit6 = Physics2D.Raycast((Vector2)transform.position + -Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.up, RayRangeVertical + RayRangeUpPlus * 0.0675f, CollisionLayer);
                break;

            case PlayerMove.MoveInputRot.rightDown:
                Debug.DrawRay(transform.position, Vector2.right * RayRangeHorizontal, Color.red);
                //Debug.DrawRay((Vector2)transform.position + Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.right * RayRangeHorizontal, Color.green);
                Debug.DrawRay((Vector2)transform.position + -Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.right * RayRangeHorizontal, Color.blue);
                Debug.DrawRay(transform.position, Vector2.down * RayRangeVertical, Color.red);
                Debug.DrawRay((Vector2)transform.position + Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.down * RayRangeVertical, Color.green);
                Debug.DrawRay((Vector2)transform.position + -Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.down * RayRangeVertical, Color.blue);

                hit1 = Physics2D.Raycast(transform.position, Vector2.right, RayRangeHorizontal,CollisionLayer);
                //hit2 = Physics2D.Raycast((Vector2)transform.position + Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.right, RayRangeHorizontal,collisionLayer);
                hit3 = Physics2D.Raycast((Vector2)transform.position + -Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.right, RayRangeHorizontal, CollisionLayer);
                hit4 = Physics2D.Raycast(transform.position, Vector2.down, RayRangeVertical, CollisionLayer);
                hit5 = Physics2D.Raycast((Vector2)transform.position + Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.down, RayRangeVertical, CollisionLayer);
                hit6 = Physics2D.Raycast((Vector2)transform.position + -Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.down, RayRangeVertical, CollisionLayer);
                break;

            case PlayerMove.MoveInputRot.leftUp:
                Debug.DrawRay(transform.position, Vector2.left * RayRangeHorizontal, Color.red);
                //Debug.DrawRay((Vector2)transform.position + Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.left * RayRangeHorizontal, Color.green);
                Debug.DrawRay((Vector2)transform.position + -Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.left * RayRangeHorizontal, Color.blue);
                Debug.DrawRay(transform.position, Vector2.up * (RayRangeVertical + RayRangeUpPlus * 0.0675f), Color.red);
                Debug.DrawRay((Vector2)transform.position + Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.up * (RayRangeVertical + RayRangeUpPlus * 0.0675f), Color.green);
                Debug.DrawRay((Vector2)transform.position + -Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.up * (RayRangeVertical + RayRangeUpPlus * 0.0675f), Color.blue);

                hit1 = Physics2D.Raycast(transform.position, Vector2.left, RayRangeHorizontal, CollisionLayer);
                //hit2 = Physics2D.Raycast((Vector2)transform.position + Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.left, RayRangeHorizontal,collisionLayer);
                hit3 = Physics2D.Raycast((Vector2)transform.position + -Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.left, RayRangeHorizontal, CollisionLayer);
                hit4 = Physics2D.Raycast(transform.position, Vector2.up, RayRangeVertical + RayRangeUpPlus * 0.0675f, CollisionLayer);
                hit5 = Physics2D.Raycast((Vector2)transform.position + Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.up, RayRangeVertical + RayRangeUpPlus * 0.0675f,CollisionLayer);
                hit6 = Physics2D.Raycast((Vector2)transform.position + -Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.up, RayRangeVertical + RayRangeUpPlus * 0.0675f,CollisionLayer);
                break;

            case PlayerMove.MoveInputRot.leftDown:
                Debug.DrawRay(transform.position, Vector2.left * RayRangeHorizontal, Color.red);
                //Debug.DrawRay((Vector2)transform.position + Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.left * RayRangeHorizontal, Color.green);
                Debug.DrawRay((Vector2)transform.position + -Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.left * RayRangeHorizontal, Color.blue);
                Debug.DrawRay(transform.position, Vector2.down * RayRangeVertical, Color.red);
                Debug.DrawRay((Vector2)transform.position + Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.down * RayRangeVertical, Color.green);
                Debug.DrawRay((Vector2)transform.position + -Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.down * RayRangeVertical, Color.blue);

                hit1 = Physics2D.Raycast(transform.position, Vector2.left, RayRangeHorizontal, CollisionLayer);
                //hit2 = Physics2D.Raycast((Vector2)transform.position + Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.left, RayRangeHorizontal,collisionLayer);
                hit3 = Physics2D.Raycast((Vector2)transform.position + -Vector2.up * (RayRangeHorizontal + (AltRaysHorizontalPlus * 0.0675f)), Vector2.left, RayRangeHorizontal, CollisionLayer);
                hit4 = Physics2D.Raycast(transform.position, Vector2.down, RayRangeVertical, CollisionLayer);
                hit5 = Physics2D.Raycast((Vector2)transform.position + Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.down, RayRangeVertical, CollisionLayer);
                hit6 = Physics2D.Raycast((Vector2)transform.position + -Vector2.right * (RayRangeVertical + (AltRaysVerticalPlus * 0.0675f)), Vector2.down, RayRangeVertical, CollisionLayer);
                break;
        }

        if (hit1 != default && hit1.collider != null) { hitList.Add(hit1); }
        if (hit2 != default && hit2.collider != null) { hitList.Add(hit2); }
        if (hit3 != default && hit3.collider != null) { hitList.Add(hit3); }
        if (hit4 != default && hit4.collider != null) { hitList.Add(hit4); }
        if (hit5 != default && hit5.collider != null) { hitList.Add(hit5); }
        if (hit6 != default && hit6.collider != null) { hitList.Add(hit6); }

        
        foreach(RaycastHit2D ray in hitList)
        {
            /*
            if (ray.collider.gameObject.TryGetComponent(out Teleporter teleporter))
            {
                if (teleporter.neededInteraction == false) { teleporter.TeleportNoInteraction(); }
                //print("TP BULUNDU");  //CALISIYOR
            }
            */
            if(ray.collider.gameObject.TryGetComponent(out IInteractableAuto interactableAuto))
            {
                interactableAuto.InteractAuto();
            }
        }










        bool Dir1Safe = true;
        bool Dir2Safe = true;


        if (hit1 != default && hit1.collider != null && hit1.collider.TryGetComponent(out BlockData BData1))
        {
            //Debug.Log("Ray hit: " + hit1.collider.name);
            if (BData1.isBlocking == true) { Dir1Safe = false; }
        }
        if (hit2 != default && hit2.collider != null && hit2.collider.TryGetComponent(out BlockData BData2))
        {
            //Debug.Log("Ray hit: " + hit2.collider.name);
            if (BData2.isBlocking == true) { Dir1Safe = false; }
        }
        if (hit3 != default && hit3.collider != null && hit3.collider.TryGetComponent(out BlockData BData3))
        {
            //Debug.Log("Ray hit: " + hit3.collider.name);
            if (BData3.isBlocking == true) { Dir1Safe = false; }
        }
        if (hit4 != default && hit4.collider != null && hit4.collider.TryGetComponent(out BlockData BData4))
        {
            //Debug.Log("Ray hit: " + hit4.collider.name);
            if (BData4.isBlocking == true) { Dir2Safe = false; }
        }
        if (hit5 != default && hit5.collider != null && hit5.collider.TryGetComponent(out BlockData BData5))
        {
            //Debug.Log("Ray hit: " + hit5.collider.name);
            if (BData5.isBlocking == true) { Dir2Safe = false; }   //BURADAKÝ DÝR1 OLARAK KALMIÞ ALÝHANINI S
        }
        if (hit6 != default && hit6.collider != null && hit6.collider.TryGetComponent(out BlockData BData6))
        {
            //Debug.Log("Ray hit: " + hit6.collider.name);
            if (BData6.isBlocking == true) { Dir2Safe = false; }
        }


        //Debug.LogError(Dir1Safe + "  " + Dir2Safe);  //ONEMLI
        

        if(Dir1Safe == false && Dir2Safe == false)
        {
            return Vector2.zero;
        }
        else if(Dir1Safe == true && Dir2Safe == false)
        {
            return new Vector2(MoveInput.x, 0);
        }
        else if(Dir1Safe == false && Dir2Safe == true)
        {
            return new Vector2(0, MoveInput.y);
        }
        else if(Dir1Safe == true && Dir2Safe == true)
        {
            return new Vector2(MoveInput.x, MoveInput.y);
        }




        /*
                    case PlayerMove.MoveInputRot.rightUp:
                Debug.DrawRay(transform.position, Vector2.right * RayRangeHorizontal, Color.red);
                Debug.DrawRay(transform.position, Vector2.up * RayRangeVertical, Color.green);
                Debug.DrawRay(transform.position, playerMove.rightUp.normalized * RayRangeCross, Color.blue);
                hit1 = Physics2D.Raycast(transform.position, Vector2.right, RayRangeHorizontal);
                hit2 = Physics2D.Raycast(transform.position, Vector2.up, RayRangeVertical);
                hit3 = Physics2D.Raycast(transform.position, playerMove.rightUp, RayRangeCross);
                break;

            case PlayerMove.MoveInputRot.rightDown:
                Debug.DrawRay(transform.position, Vector2.right * RayRangeHorizontal, Color.red);
                Debug.DrawRay(transform.position, Vector2.down * RayRangeVertical, Color.green);
                Debug.DrawRay(transform.position, playerMove.rightDown.normalized * RayRangeCross, Color.blue);
                hit1 = Physics2D.Raycast(transform.position, Vector2.right, RayRangeHorizontal);
                hit2 = Physics2D.Raycast(transform.position, Vector2.down, RayRangeVertical);
                hit3 = Physics2D.Raycast(transform.position, playerMove.rightDown, RayRangeCross);
                break;

            case PlayerMove.MoveInputRot.leftUp:
                Debug.DrawRay(transform.position, Vector2.left * RayRangeHorizontal, Color.red);
                Debug.DrawRay(transform.position, Vector2.up * RayRangeVertical, Color.green);
                Debug.DrawRay(transform.position, playerMove.leftUp.normalized * RayRangeCross, Color.blue);
                hit1 = Physics2D.Raycast(transform.position, Vector2.left, RayRangeHorizontal);
                hit2 = Physics2D.Raycast(transform.position, Vector2.up, RayRangeVertical);
                hit3 = Physics2D.Raycast(transform.position, playerMove.leftUp, RayRangeCross);
                break;

            case PlayerMove.MoveInputRot.leftDown:
                Debug.DrawRay(transform.position, Vector2.left * RayRangeHorizontal, Color.red);
                Debug.DrawRay(transform.position, Vector2.down * RayRangeVertical, Color.green);
                Debug.DrawRay(transform.position, playerMove.leftDown.normalized * RayRangeCross, Color.blue);
                hit1 = Physics2D.Raycast(transform.position, Vector2.left, RayRangeHorizontal);
                hit2 = Physics2D.Raycast(transform.position, Vector2.down, RayRangeVertical);
                hit3 = Physics2D.Raycast(transform.position, playerMove.leftDown, RayRangeCross);
                break;
        */


        /*
                if (hit1 != default && hit1.collider != null && hit1.collider.TryGetComponent(out BlockData BData1))
        {
            Debug.Log("Ray hit: " + hit1.collider.name);
            if (BData1.isBlocking == true) { print("engele çarptý"); return false; }
        }
        if (hit2 != default && hit2.collider != null && hit2.collider.TryGetComponent(out BlockData BData2))
        {
            Debug.Log("Ray hit: " + hit2.collider.name);
            if (BData2.isBlocking == true) { print("engele çarptý"); return false; }
        }
        if (hit3 != default && hit3.collider != null && hit3.collider.TryGetComponent(out BlockData BData3))
        {
            Debug.Log("Ray hit: " + hit3.collider.name);
            if (BData3.isBlocking == true) { print("engele çarptý"); return false; }
        }
        if (hit4 != default && hit4.collider != null && hit4.collider.TryGetComponent(out BlockData BData4))
        {
            Debug.Log("Ray hit: " + hit4.collider.name);
            if (BData4.isBlocking == true) { print("engele çarptý"); return false; }
        }
        if (hit5 != default && hit5.collider != null && hit5.collider.TryGetComponent(out BlockData BData5))
        {
            Debug.Log("Ray hit: " + hit5.collider.name);
            if (BData5.isBlocking == true) { print("engele çarptý"); return false; }
        }
        if (hit6 != default && hit6.collider != null && hit6.collider.TryGetComponent(out BlockData BData6))
        {
            Debug.Log("Ray hit: " + hit6.collider.name);
            if (BData6.isBlocking == true) { print("engele çarptý"); return false; }
        } 
        */











        //RaycastHit2D hit = Physics2D.Raycast(transform.position, playerMove.MoveGo, playerMove.MoveGo.magnitude*rayRange);
        //Debug.DrawRay(transform.position, playerMove.MoveGo * rayRange, Color.red);


        /*
        if(hit.collider != null && hit.collider.TryGetComponent(out BlockData BData))
        {
            Debug.Log("Ray hit: " + hit.collider.name);
            if (BData.isBlocking == true) { print("engele çarptý"); return false; }
        }
        */


        //return true;
        return Vector2.zero;

    }
}

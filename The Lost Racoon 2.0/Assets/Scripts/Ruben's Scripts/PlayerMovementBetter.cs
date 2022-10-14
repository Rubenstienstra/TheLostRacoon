using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementBetter : MonoBehaviour
{
    public CamFreezeScript camFreezeInfo;
    public Interact interactInfo;

    public float lookAtAngle;
    public Vector3 movementAngle;
    public float endAngle;
    public Camera playerCam;

    public float[] forwardWASD;
    public bool[] isMovingForwardWASD;
    private int checkingBools;

    public float multiplierSpeedBonus = 1;
    private float crspeedBonus = 1;
    public float timeToTurn;
    public float velocity;

    public float jump;
    public float beginJumpBonus = 1.5f;
    public float totalHeightJump = 1.5f;
    public float jumpMultiplier = 50;
    public float maxTimeHoldJump = 4;

    public Rigidbody rb;
    public Collision gameObjectCollision;

    public RaycastHit hit;
    public float maxDistanceRaycast;
    public Collider crCollider;
    public LayerMask filterMask;

    public bool moving;
    public bool isOnGround;
    public bool movementLock;
    public bool allowInteraction;
    public Animator animationMovement;

    public bool testing;

    
    public void OnForward(InputValue value)
    {
       forwardWASD[0] = value.Get<float>();
       CheckMoving(0);
    }
    public void OnLeft(InputValue value)
    {
        forwardWASD[1] = value.Get<float>();
        CheckMoving(1);
    }
    public void OnDown(InputValue value)
    {
        forwardWASD[2] = value.Get<float>();
        CheckMoving(2);
    }
    public void OnRight(InputValue value)
    {
        forwardWASD[3] = value.Get<float>();
        CheckMoving(3);
    }
    public void OnSprint(InputValue value)
    {
        if(value.Get<float>() == 1)
        {
            crspeedBonus = multiplierSpeedBonus;
        }
        else
        {
            crspeedBonus = 1;
        }
    }
    public void OnJump(InputValue value)
    {
        jump = value.Get<float>();
        if(isOnGround == true)
        {
            StartCoroutine(JumpTiming());
        }
    }
    public void CheckMoving(int movementNumber)
    {
        if(forwardWASD[movementNumber] == 1)
        {
            isMovingForwardWASD[movementNumber] = true;
        }
        else
        {
            isMovingForwardWASD[movementNumber] = false;
        }
        if(moving == false)
        {
            moving = true;
            StartCoroutine(Movement());
        }
    }
    public IEnumerator JumpTiming()
    {
        if (jump > 0)
        {
            if (totalHeightJump < maxTimeHoldJump +beginJumpBonus)
            {
                yield return new WaitForSeconds(0.1f);
                totalHeightJump += 0.1f;
                StartCoroutine(JumpTiming());
            }
        }
        else //als de speler geen spatie meer vast houdt komt er een force er bij
        {
            isOnGround = false;
            print(totalHeightJump);
            rb.AddForce(0,totalHeightJump * jumpMultiplier,0);
            totalHeightJump = beginJumpBonus;
        }
    }
    public void OnCollisionEnter(Collision col)
    {
        if(col.collider.gameObject.tag == "Ground")
        {
            isOnGround = true;
        }
    }
    public IEnumerator Movement()
    {
        
        if (!movementLock)
        {
            Vector3 addMovement = new Vector3(forwardWASD[3] + -forwardWASD[1], 0, -forwardWASD[2] + forwardWASD[0]) * Time.deltaTime;

            lookAtAngle = Mathf.Atan2(addMovement.x, addMovement.z) * Mathf.Rad2Deg + playerCam.transform.eulerAngles.y;
            endAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAtAngle, ref velocity, timeToTurn);
            

            if (!testing)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, endAngle, transform.rotation.y);
                movementAngle = Quaternion.Euler(0, endAngle, 0) * Vector3.forward;
                transform.position += movementAngle.normalized * crspeedBonus * Time.deltaTime;
            }
            else
            {
                rb.MoveRotation(Quaternion.Euler(transform.rotation.x, endAngle, transform.rotation.y));
                movementAngle = new Vector3 (transform.position.x + addMovement.x,transform.position.y, transform.position.z + addMovement.z);
                rb.MovePosition(movementAngle);
                
            }
                      
            

            yield return new WaitForSeconds(0.01f); // do not move
            for (int i = 0; i < isMovingForwardWASD.Length; i++)
            {
                if (isMovingForwardWASD[i] == true)
                {
                    checkingBools++;
                }
            }
            if (checkingBools > 0)
            {
                new WaitForSeconds(0.01f);
                animationMovement.SetBool("Walking", true);
                StartCoroutine(Movement());
            }
            else
            {
                animationMovement.SetBool("Walking", false);
                moving = false;
            }
            checkingBools = 0;

        }
    }
    public void OnInteract(InputValue value)
    {
        Physics.Raycast(transform.position, Vector3.forward, out hit, filterMask);
        crCollider = hit.collider;
        CollidedMinigame(hit.collider);
    }
    public void CollidedMinigame(Collider col)
    {
        if (col.gameObject.GetComponent<MouseTrackerMovement>())
        {
            col.gameObject.GetComponent<MouseTrackerMovement>().StartAreaMinigame();
        }
        else if (col.gameObject.GetComponent<MouseTrackerRectangleMovement>())
        {
            col.gameObject.GetComponent<MouseTrackerRectangleMovement>().StartAreaMinigame();
        }
        else if (col.gameObject.GetComponent<UIPuzzleColor>())
        {
            col.gameObject.GetComponent<UIPuzzleColor>().SpawnPuzzleUI();
        }
    }
    public void OnEnterMinigame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        movementLock = true;
        interactInfo.minigameBeingPlayed = true;
        camFreezeInfo.CamFreeze();
    }
    public void OnExitMinigame()
    {
        Cursor.visible = false;
        movementLock = false;
        Cursor.lockState = CursorLockMode.Locked;
        interactInfo.minigameBeingPlayed = false;
        camFreezeInfo.CamUnfreeze();
    }

}

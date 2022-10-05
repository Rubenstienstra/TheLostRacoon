using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementBetter : MonoBehaviour
{
    public CamFreezeScript camFreezeInfo;
    public Interact interactInfo;
    
    public float[] forwardWASD;
    public float jump;
    public float beginJumpBonus = 1.5f;
    public float totalHeightJump = 1.5f;
    public float jumpMultiplier = 75;
    public float maxTimeHoldJump = 4;

    public Rigidbody rb;
    public Animation jumpAnimation;
    public Collision gameObjectCollision;
    public Collider gameObjectCollider;

    public bool isOnGround;
    public bool movementLock;

    public void OnForward(InputValue value)
    {
       forwardWASD[0] = value.Get<float>();
    }
    public void OnLeft(InputValue value)
    {
        forwardWASD[1] = value.Get<float>();
    }
    public void OnDown(InputValue value)
    {
        forwardWASD[2] = value.Get<float>();
    }
    public void OnRight(InputValue value)
    {
        forwardWASD[3] = value.Get<float>();
    }
    public void OnJump(InputValue value)
    {
        jump = value.Get<float>();
        if(isOnGround == true)
        {
            StartCoroutine(JumpTiming());
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
        if(gameObjectCollider.gameObject.tag == "")
        {

        }
    }
    public void Update()
    {
        transform.localPosition += new Vector3(forwardWASD[3] + -forwardWASD[1], 0 ,-forwardWASD[2] + forwardWASD[0]) * Time.deltaTime;
    }








    public void OnEnterMinigame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

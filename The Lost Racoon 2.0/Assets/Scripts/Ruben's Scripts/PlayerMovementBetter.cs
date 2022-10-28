using System.Collections;
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

    public float multiplierSprintSpeed = 2;
    private float crspeedBonus = 1;
    public float timeToTurn;
    private float velocity;
    private Vector3 movingAngle;
    private Vector3 addMovement;
    public float increasedMoveSpeed = 1;

    public RaycastHit hitSlope;
    public Vector3 RaycastPos;
    public float crSlopeAngle;
    public float maxUpSlopeAngle;

    private float jumpInput;
    public float beginJumpBonus = 1.5f;
    public float totalHeightJump = 1.5f;
    public float jumpHeightMultiplier = 50;
    public float maxTimeHoldJump = 4;
    public float addJumpForceZ = 10; 
    public float sprintJumpBoost = 1.25f; //If the player jumps he goes further by pressing the sprint button. 

    public Rigidbody rb;
    public Animator animationMovement;

    public bool moving;
    public bool sprinting;
    public bool isOnGround;
    public bool movementLock;
    public bool allowInteraction;
    

    public void OnReset()
    {

    }
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
        if (value.Get<float>() == 1)
        {
            crspeedBonus = multiplierSprintSpeed;
            sprinting = true;
        }
        else
        {
            crspeedBonus = 1;
            sprinting = false;
        }
    }
    public void OnJump(InputValue value)
    {
        jumpInput = value.Get<float>();
        if (isOnGround == true)
        {
            StartCoroutine(JumpTiming());
        }
    }
    public void CheckMoving(int movementNumber)
    {
        if (forwardWASD[movementNumber] == 1)
        {
            isMovingForwardWASD[movementNumber] = true;
        }
        else
        {
            isMovingForwardWASD[movementNumber] = false;
        }
        if (moving == false)
        {
            moving = true;
            StartCoroutine(Movement());
        }
    }
    public IEnumerator JumpTiming()
    {
        if (jumpInput > 0)
        {
            if (totalHeightJump < maxTimeHoldJump + beginJumpBonus)
            {
                yield return new WaitForSeconds(0.1f);
                totalHeightJump += 0.1f;
                animationMovement.SetBool("Charging", true);
                StartCoroutine(JumpTiming());
            }
        }
        else //als de speler geen spatie meer vast houdt komt er een force er bij
        {
            animationMovement.SetBool("Charging", false);
            animationMovement.SetBool("Jumping", true);
            isOnGround = false;
            print(totalHeightJump);
            // = maxTimeHoldJump / totalHeightJump;
            if (sprinting)
            {
                rb.AddRelativeForce(0, totalHeightJump * jumpHeightMultiplier, addJumpForceZ * totalHeightJump * sprintJumpBoost);
            }
            else
            {
                rb.AddRelativeForce(0, totalHeightJump * jumpHeightMultiplier, addJumpForceZ * totalHeightJump);
            }
            totalHeightJump = beginJumpBonus;
        }
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.tag == "Ground")
        {
            isOnGround = true;
        }
        animationMovement.SetBool("Jumping", false);
    }
    public IEnumerator Movement()
    {

        if (!movementLock)
        {
            addMovement = new Vector3(forwardWASD[3] + -forwardWASD[1], 0, -forwardWASD[2] + forwardWASD[0]) * Time.deltaTime; // gets input values

            lookAtAngle = Mathf.Atan2(addMovement.x, addMovement.z) * Mathf.Rad2Deg + playerCam.transform.eulerAngles.y; // berekent de angle waar je naar kijkt
            endAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAtAngle, ref velocity, timeToTurn); // hiermee berekent je de angle van de speler naar links of rechts toe via de camera
            
            movementAngle = Quaternion.Euler(0, endAngle, 0) * Vector3.forward; // 
            
            movingAngle = Vector3.ProjectOnPlane(movementAngle, hitSlope.normal).normalized; //
            Physics.Raycast(RaycastPos + transform.position, Vector3.down, out hitSlope); // maakt een rayccast aan die naar beneden toe gaat
            crSlopeAngle = Vector3.Angle(Vector3.up, hitSlope.normal); //
            if (crSlopeAngle >= maxUpSlopeAngle)
            {
                crSlopeAngle = maxUpSlopeAngle;
            }
            if (movingAngle.y > 0)
            {
                crSlopeAngle = -crSlopeAngle;
            }

            transform.rotation = Quaternion.Euler(crSlopeAngle, endAngle, transform.rotation.y); // voegt telkens de rotatie(endAngle) toe aan de speler.
            transform.position += movingAngle.normalized * crspeedBonus * Time.deltaTime * increasedMoveSpeed; //

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
                if (!sprinting)
                {
                    animationMovement.SetBool("Running", false);
                    animationMovement.SetBool("Walking", true);
                }
                else
                {
                    animationMovement.SetBool("Walking", false);
                    animationMovement.SetBool("Running", true);
                }
                StartCoroutine(Movement());
            }
            else
            {
                animationMovement.SetBool("Walking", false);
                animationMovement.SetBool("Running", false);
                moving = false;
            }
            checkingBools = 0;
        }
    }
    public void ResettingAllAnimations()
    {
        animationMovement.SetBool("Walking",false);
        animationMovement.SetBool("Running", false);
        animationMovement.SetBool("Charging", false);
        animationMovement.SetBool("Jumping", false);
    }

    

}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerMovementBetter : MonoBehaviour
{
    public CamFreezeScript camFreezeInfo;
    public Interact interactInfo;
    public ScriptableSaving scriptableSavingInfo;
    public Tutorial tutorialInfo;
    public PlayerScript playerInfo;

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
    public Vector3 crMoveSpeed;

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

    public GameObject deathScreen;
    public GameObject freeLookCamera;
    public GameObject escMenu;
    public Vector3 instantCameraResetPos;

    public bool moving;
    public bool sprinting;
    public bool isOnGround;
    public bool movementLock;
    public bool allowInteraction;

    public Animator fadeOut;
    public GameObject FadeOutObject;
    public GameObject[] allInGameSoundSwaps;
    public void Start()
    {
        if(deathScreen == null)
        {
           deathScreen = GameObject.Find("DeathScreen");
        }
        if(scriptableSavingInfo.crCheckpointVector3 == new Vector3(0,0,0))
        {
           scriptableSavingInfo.crCheckpointVector3 = gameObject.transform.position;
           scriptableSavingInfo.crCheckpointRotation = gameObject.transform.rotation.eulerAngles;
        }
        if(scriptableSavingInfo.saveAndLoadSystem)
        {
            ActivateScriptbleObject();
        }
        if (!scriptableSavingInfo.tutorialStepsCompleted[0] && !scriptableSavingInfo.tutorialStepsCompleted[1])
        {
            tutorialInfo.tutorialSteps[0].SetActive(true);
            tutorialInfo.ActivateTutorial(0);
        }
    }
    public void OnEsc(InputValue value) //het enige probleem is nog wanneer een minigame wel een muis nodig heeft. heeft hij die niet.
    {
        if (!deathScreen.activeSelf && value.Get<float>() == 1 && !interactInfo.minigameBeingPlayed)
        {
            if (!escMenu.activeSelf )
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                escMenu.SetActive(true);
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1;
                escMenu.SetActive(false);
                Cursor.visible = false;
            }
        }
    }
    public void OnReset()
    {
        if (!interactInfo.minigameBeingPlayed)
        {
            rb.velocity = new Vector3(0, 0, 0);
            gameObject.transform.position = scriptableSavingInfo.crCheckpointVector3;
            gameObject.transform.rotation = Quaternion.Euler(scriptableSavingInfo.crCheckpointRotation);
            freeLookCamera.transform.position = instantCameraResetPos;

            if (scriptableSavingInfo.checkpointNames.Count >= 1 && scriptableSavingInfo.checkpointNames.Count <= 4)//MUSIC
            {
                GameObject.Find("Natuur Audio Source").GetComponent<AudioSource>().enabled = enabled;
                GameObject.Find("Kamers Audio Source").GetComponent<AudioSource>().enabled = !enabled;
            }
            else if (scriptableSavingInfo.checkpointNames.Count == 0)
            {
                GameObject.Find("Riool Audio Source").GetComponent<AudioSource>().enabled = enabled;
                GameObject.Find("Natuur Audio Source").GetComponent<AudioSource>().enabled = !enabled;
            }
            else if (scriptableSavingInfo.checkpointNames.Count == 5)
            {
                GameObject.Find("Kamers Audio Source").GetComponent<AudioSource>().enabled = enabled;
                GameObject.Find("Natuur Audio Source").GetComponent<AudioSource>().enabled = !enabled;
            }
            for (int i = allInGameSoundSwaps.Length - 1; i >= 0; i--)
            {
                allInGameSoundSwaps[i].GetComponent<SoundSwap>().toggleSwitch = false;
            }


            if (deathScreen == true)
            {
                Cursor.visible = false;
                deathScreen.SetActive(false);
                freeLookCamera.transform.position = instantCameraResetPos;
            }
        }
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
            //StartCoroutine(Movement());
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
            StopCoroutine(ReJumping());

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
            StartCoroutine(ReJumping());
            totalHeightJump = beginJumpBonus;
        }
    }
    public IEnumerator ReJumping()
    {
        yield return new WaitForSeconds(5);
        if(isOnGround == false)
        {
            isOnGround = true;
        }
    }
    public void FixedUpdate()
    {
        if (!movementLock && moving)
        {
            addMovement = new Vector3(forwardWASD[3] + -forwardWASD[1], 0, -forwardWASD[2] + forwardWASD[0]) * Time.deltaTime; // gets input values

            lookAtAngle = Mathf.Atan2(addMovement.x, addMovement.z) * Mathf.Rad2Deg + playerCam.transform.eulerAngles.y; // berekent de angle waar je naar kijkt
            endAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAtAngle, ref velocity, timeToTurn); // hiermee berekent je de angle van de speler naar links of rechts toe via de camera
            
            movementAngle = Quaternion.Euler(0, endAngle, 0) * Vector3.forward; // 
            
            movingAngle = Vector3.ProjectOnPlane(movementAngle, hitSlope.normal).normalized; //
            Physics.Raycast(RaycastPos + transform.position, Vector3.down, out hitSlope, 1); // maakt een rayccast aan die naar beneden toe gaat
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
            transform.position += movingAngle.normalized * crspeedBonus * increasedMoveSpeed * Time.deltaTime; //
            crMoveSpeed = movingAngle.normalized * crspeedBonus * increasedMoveSpeed * Time.deltaTime;

            if(Cursor.visible == true)
            {
                Cursor.visible = false;
            }
            //yield return new WaitForSeconds(0.01f); // do not move
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
                //StartCoroutine(Movement());
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
    public void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.tag == "Ground")
        {
            isOnGround = true;
        }
        animationMovement.SetBool("Jumping", false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Checkpoint")
        {
            scriptableSavingInfo.crCheckpointVector3 = other.gameObject.GetComponentInParent<Transform>().position;
            scriptableSavingInfo.crCheckpointRotation = other.gameObject.GetComponentInParent<Transform>().rotation.eulerAngles;
            scriptableSavingInfo.checkpointNames.Add(other.transform.parent.name.ToString());
            other.gameObject.SetActive(false); //scriptableSavingInfo.activatedCheckpoints[scriptableSavingInfo.activatedCheckpoints.Count - 1].SetActive(false);
        }
        else if(other.gameObject.tag == "Deathzone")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            deathScreen.SetActive(true);

            rb.velocity = new Vector3(0, 0, 0);
            gameObject.transform.position = scriptableSavingInfo.crCheckpointVector3;
            gameObject.transform.rotation = Quaternion.Euler(scriptableSavingInfo.crCheckpointRotation);
        }
        else if (other.gameObject.tag == "TutorialJumpActivate" && !scriptableSavingInfo.tutorialStepsCompleted[2])
        {
            tutorialInfo.tutorialSteps[2].SetActive(true);
            tutorialInfo.ActivateTutorial(1);
        }
        else if (other.gameObject.tag == "TutorialDifferentWayActivate" && !scriptableSavingInfo.tutorialStepsCompleted[3])
        {
            tutorialInfo.tutorialSteps[3].SetActive(true);
        }
        else if (other.gameObject.tag == "TheEnding")// fade out here
        {
            fadeOut.SetBool("Fade out",true);
            StartCoroutine(FadeOut());
        }
    }
    public IEnumerator FadeOut()
    {
        yield return new WaitForSecondsRealtime(1.075f);
        playerInfo.ResetProgressGuaranteedSucces();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);

        //yield return new WaitForSecondsRealtime(0.1f); //1.075f
        //if(fadeOut.GetBool("Fade out"))
        //{
        //    StartCoroutine(FadeOut());
        //}
        //else if(fadeOut.GetBool("Fade out"))
        //{
        //    OnReset();
        //    Cursor.visible = true;
        //    SceneManager.LoadScene(0);
        //}
        //yield return new WaitForSecondsRealtime(0.1f);
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "TutorialDifferentWayActivate")
        {
            tutorialInfo.tutorialSteps[3].SetActive(false);
            scriptableSavingInfo.tutorialStepsCompleted[3] = true;
        }
    }
    public void ResettingAllAnimations()
    {
        animationMovement.SetBool("Walking",false);
        animationMovement.SetBool("Running", false);
        animationMovement.SetBool("Charging", false);
        animationMovement.SetBool("Jumping", false);
    }
    public void ActivateScriptbleObject()
    {
        if (scriptableSavingInfo.checkpointNames.Count >= 1 && scriptableSavingInfo.checkpointNames.Count <= 4)//MUSIC
        {
            GameObject.Find("Natuur Audio Source").GetComponent<AudioSource>().enabled = enabled;
            GameObject.Find("Kamers Audio Source").GetComponent<AudioSource>().enabled = !enabled;
        }
        else if(scriptableSavingInfo.checkpointNames.Count == 0)
        {
            GameObject.Find("Riool Audio Source").GetComponent<AudioSource>().enabled = enabled;
            GameObject.Find("Natuur Audio Source").GetComponent<AudioSource>().enabled = !enabled;
        }
        else if(scriptableSavingInfo.checkpointNames.Count == 5)
        {
            GameObject.Find("Kamers Audio Source").GetComponent<AudioSource>().enabled = enabled;
            GameObject.Find("Natuur Audio Source").GetComponent<AudioSource>().enabled = !enabled; 
        }
        for (int i = allInGameSoundSwaps.Length - 1; i >= 0; i--)
        {
            allInGameSoundSwaps[i].GetComponent<SoundSwap>().toggleSwitch = false;
        }

        for (int i = scriptableSavingInfo.crActivatedCheckpoints.Count - 1; i >= 0; i--)//CHECKPOINTS. Deletes everycheckpoint so it can re assemble all the checkpoints.
        {
            scriptableSavingInfo.crActivatedCheckpoints.RemoveAt(i);
        }
        for (int i = 0; i < scriptableSavingInfo.checkpointNames.Count; i++) //uses the name to find the gameobject to put it in a list. also disables every gameobject in the array.
        {
            scriptableSavingInfo.crActivatedCheckpoints.Add(GameObject.Find(scriptableSavingInfo.checkpointNames[i]));
            scriptableSavingInfo.crActivatedCheckpoints[i].SetActive(false);
        }

        for (int i = 0; i < scriptableSavingInfo.tutorialStepsCompleted.Length; i++)
        {
            tutorialInfo.tutorialSteps[i].SetActive(false);
        }
        OnReset();
    }
}

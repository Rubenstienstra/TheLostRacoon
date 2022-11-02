using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public PlayerMovementBetter playerMovementInfo;
    public ScriptableSaving savingInfo;

    public GameObject[] tutorialSteps;
    
    public void ActivateTutorial(int tutorialPart)
    {
        StartCoroutine(TutorialStart(tutorialPart));
    }
    public IEnumerator TutorialStart(int tutorialPart)
    {
        switch(tutorialPart){
            case 0:
                if (playerMovementInfo.forwardWASD[0] == 1 && !savingInfo.tutorialStepsCompleted[0])
                {
                    tutorialSteps[0].SetActive(false);
                    savingInfo.tutorialStepsCompleted[0] = true;
                    tutorialSteps[1].SetActive(true);
                }
                if (playerMovementInfo.sprinting && !savingInfo.tutorialStepsCompleted[1])
                {
                    tutorialSteps[1].SetActive(false);
                    savingInfo.tutorialStepsCompleted[1] = true;
                }
                yield return new WaitForSeconds(0.25f);
                if (!savingInfo.tutorialStepsCompleted[0] || !savingInfo.tutorialStepsCompleted[1])
                {
                    StartCoroutine(TutorialStart(0));
                }
                break;
            case 1:
                if (playerMovementInfo.animationMovement.GetBool("Jumping") && !savingInfo.tutorialStepsCompleted[2])
                {
                    tutorialSteps[2].SetActive(false);
                    savingInfo.tutorialStepsCompleted[2] = true;
                }
                yield return new WaitForSeconds(0.25f);
                if (!savingInfo.tutorialStepsCompleted[2])
                {
                    StartCoroutine(TutorialStart(1));
                }
                break;
        }
        
    }
}

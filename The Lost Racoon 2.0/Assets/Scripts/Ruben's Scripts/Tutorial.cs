using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public PlayerMovementBetter playerMovementInfo;

    public GameObject[] TutorialSteps; 

    public bool completedTutorial;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ActivateTutorial()
    {
        StartCoroutine(TutorialStart());
    }
    public IEnumerator TutorialStart()
    {
        if (playerMovementInfo.forwardWASD[0] == 1)
        {

        }
        yield return new WaitForSeconds(0.5f);
        if (!completedTutorial)
        {
            StartCoroutine(TutorialStart());
        }

    }
}

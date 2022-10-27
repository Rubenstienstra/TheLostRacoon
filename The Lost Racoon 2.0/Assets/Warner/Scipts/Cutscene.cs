using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public Animator beau;
    public Animator warner;
    public Animator door;

    private void OnTriggerEnter(Collider other)
    {
        print(123);
        beau.SetTrigger("Cutscene");
        warner.SetTrigger("Cutscene");
        door.SetTrigger("Cutscene");
        
    }
}

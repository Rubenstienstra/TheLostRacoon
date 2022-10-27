using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoortClose : MonoBehaviour
{
    public Animator poort;

    private void OnTriggerEnter(Collider other)
    {
        print(123);
        poort.SetTrigger("PoortClose");

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{

    public int timesJumped;

    //de waardes in deze constructeur zijn de standaard waardes
    //waarmee het spel start als je een nieuwe game begint
    public GameData() {
        this.timesJumped = 0;   
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{

    private GameData gamedata;

    private List<IDataPresistence> dataPresistenceObjects;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake() {
        if (instance != null) {
            Debug.LogError("Meer dan 1 Data Persistence Manager in de scene gevonden.");
        }
        instance = this;
    }

    private void Start() {
        LoadGame();     
    }

    public void NewGame() {
        this.dataPresistenceObjects = FindAllDataPersistenceObjects();
        this.gamedata = new GameData(); 
    }

    public void LoadGame() {
        // TODO - laad een save data bestand van de schijf
        // als er geen data is om te laden, dab maakt dit een nieuwe game aan
        if(this.gamedata == null) {
            Debug.Log("Geen data gevonden. nieuwe game word gestart met de standaard data");
            NewGame();
        }

        // TODO - push de data naar alle scripts die het nodig hebben
        foreach(IDataPresistence dataPresistenceObj in dataPresistenceObjects) {
            dataPresistenceObj.LoadData(gamedata);
        }

        Debug.Log("geladen timesJumped = " + gamedata.timesJumped);
    }

    public void SaveGame() {
        // TODO - paas de data door aan andere scripts zodat ze de data kunnen updaten
        foreach(IDataPresistence dataPresistenceObj in dataPresistenceObjects) {
            dataPresistenceObj.SaveData(ref gamedata);
        }

        Debug.Log("opgeslagen timesJumped = " + gamedata.timesJumped);

        // TODO - gebruik de data handler om de data naar een bestand op te slaan
    }

    private void OnApplicationQuit() {
        SaveGame();
    }

    private List<IDataPresistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPresistence> dataPresistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPresistence>();
        return new List<IDataPresistence>(dataPresistenceObjects);      
    }
}


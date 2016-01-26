using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class HumanTestData : MonoBehaviour {
    public int a = 0;
    public List<List<HumanHolder>> allHumans = new List<List<HumanHolder>>();
    
    // Use this for initialization
    void Start () {
      //  Debug.Log(Application.persistentDataPath);
        GetAllHumans();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("j"))
        {
            GetAllHumans();

        }
    }

    void GetAllHumans() // just for testing
    {
        if (File.Exists(Application.persistentDataPath + Community.fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileOpen = File.Open(Application.persistentDataPath + Community.fileName, FileMode.Open);
            allHumans = (List<List<HumanHolder>>)bf.Deserialize(fileOpen);
            fileOpen.Close();
        }

    }
}

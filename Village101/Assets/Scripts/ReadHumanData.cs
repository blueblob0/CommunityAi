using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ReadHumanData : MonoBehaviour {
    public const string fileName = "/humandata.data";
    // Use this for initialization
    int targetnumber =33;
    void Start () {
        if (File.Exists(Application.persistentDataPath + Community.fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileOpen = File.Open(Application.persistentDataPath + fileName, FileMode.Open);
            List<List<HumanHolder>> allHumans = (List<List<HumanHolder>>)bf.Deserialize(fileOpen);
            fileOpen.Close();

            for (int i =0;i<10;i++)
            {
                if (allHumans[i].Count== targetnumber)
                {

                    Debug.Log(i);
                }


            }
        }

        

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

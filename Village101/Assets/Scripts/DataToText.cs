using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataToText : MonoBehaviour {

    public const string ScorefileName = "/humanscore";
    public const string fileEnd = ".data";
    public List<textData> holdData = new List<textData>();
    // Use this for initialization
    void Start () {

        bool hold = true;
        string filename;
        int c = 0;
        while (hold)
        {
            filename = ScorefileName + c + fileEnd;
            if (File.Exists(Application.persistentDataPath + filename))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fileOpen = File.Open(Application.persistentDataPath + filename, FileMode.Open);
               
                List < ScoreData > holds = (List<ScoreData>)bf.Deserialize(fileOpen);
                foreach(ScoreData s in holds)
                {
                    textData h = new textData();
                    h.Score = s.scoreData.ToString();
                    h.numPeopleStart = s.startData.Count.ToString();
                    foreach (HumanHolder hum in s.startData)
                    {
                        h.ages.Add(hum.age.ageYear.ToString());
                    }
                    holdData.Add(h);
                }
                fileOpen.Close();
                c++;
            }
            else
            {
                hold = false;
            }
        }

        StreamWriter filetext = File.CreateText(Application.persistentDataPath + "firstdata.txt");


        foreach(textData t in holdData)
        {
            filetext.WriteLine(t.Score +","+ t.numPeopleStart);
            Debug.Log(t.Score + "," + t.numPeopleStart);
        }

        Debug.Log("Done");
        filetext.Close();

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

public class textData
{
    public string Score;
    public string numPeopleStart;
    public List<string> ages = new List<string>();
    
    



}
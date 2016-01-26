using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// controls how the lad works looking at the current time and day, the weather and seasona
/// </summary>
public class TheLand : MonoBehaviour {

    public List<ScoreData> humanScores = new List<ScoreData>();
    public delegate void NewDayAction();  
    public static event NewDayAction NewDay;

    public delegate void EndDayAction();
    public static event EndDayAction EndDay;

    public delegate void SaveHuamansAction();
    public static event SaveHuamansAction SaveHumans;

    public Community theCommunity;
    public const string ScorefileName = "/humanscore";
    public const string fileEnd = ".data";
    int numChecksRun =0;

    private float startTime;
    public const float dayLengthSecs = 0.00000000000000000000000000000000000000000000000001f;
    public int dayCount;
    private bool first = false;
    public bool timeRun = true;
    bool endreached = false;
    bool canRun = false;
    public int currentIteration;

    public Text timeRecord;

    void OnEnable()
    {
        Community.AllDead += EndIteration;
        
    }


    void OnDisable()
    {
        Community.AllDead -= EndIteration;
       
    }


    // Use this for initialization
    void Start ()
    {
        theCommunity = gameObject.GetComponent<Community>();
        Application.runInBackground = true; //not part of game logic  if multiple of this change to only be called once 
        currentIteration = 0;      
        StartNewIteration();
        //canRun = true; // just for now
    }

    void StartNewIteration()
    {
        if (currentIteration >= 10)//theCommunity.allHumans.Count)
        {
            Debug.LogError("reachedEnd");
            endreached = true;
            return;
        }
        ScoreData holdData = new ScoreData();
        holdData.SetStartData(theCommunity.allHumans[currentIteration]);
        humanScores.Add(holdData);

        theCommunity.StartComunity(currentIteration);
        

        CanContinue();

    }

    public void EndIteration()
    {
        canRun = false;

        foreach(Human hum in theCommunity.GetHumansClass())
        {
            humanScores[currentIteration].addHuman(hum);
        }

        humanScores[currentIteration].WorkOutScore(theCommunity.food, theCommunity.fuel);

       currentIteration++;
        StartNewIteration();
    }

    void CanContinue()
    {
        
        startTime = Time.time;
        dayCount = 0;
        setTimeUI();
        first = true;
        canRun = true;

        
    }
    
	
	// Update is called once per frame
	void Update ()
    {

        if (canRun)
        {

            if (first)
            {
                StartNewDay();
                first = false;
            }
           
            // need to make this time based instead of input later 
            /*
            if (Input.GetKeyDown("g"))
            {            
                StartEndDay();
                dayCount++;
                setTimeUI();
                StartNewDay();
            }

            */
          
            if (startTime + dayLengthSecs <= Time.time && timeRun)
            {
                
                StartEndDay();
                
                startTime = Time.time;
                dayCount++;
                setTimeUI();
                StartNewDay();
            }

            if (dayCount >=365 * 50)
            {
                EndIteration();
                //StartNewIteration();
                Debug.Log("newLevel");
            }
        }
        else if (endreached)
        {
            BinaryFormatter bf = new BinaryFormatter();
            for (int i =0;i< humanScores.Count;i++)
            {
                Debug.Log(humanScores[i] + " " + humanScores[i].GetScoreData());
            }
            string holds = ScorefileName + numChecksRun.ToString() + fileEnd;
            FileStream file = File.Create(Application.persistentDataPath + holds);
            bf.Serialize(file, humanScores);
            file.Close();

            numChecksRun++;
            
            humanScores.Clear();
            currentIteration = 0;
            endreached = false;
            StartNewIteration();
        }

    }


   


    void setTimeUI()
    {
        float holdTime = ((float)dayCount / 365.0f);
        int holdYears = Mathf.FloorToInt(holdTime);
        int holdDays = Mathf.FloorToInt((holdTime - holdYears) * 365);
        timeRecord.text = "Year:" + Mathf.Floor(holdTime) + ", Day:" + holdDays;
    }

    private void StartNewDay()
    {
       // Debug.Log("Start Day " + dayCount);
        NewDay();        
    }


    private void StartEndDay()
    {
        EndDay();
    }

}

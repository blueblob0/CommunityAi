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

    public delegate void StartWorkAction(); // start work at 6 hours
    public static event StartWorkAction StartWork;

    public delegate void GoHomeAction();// go home at 14 hours
    public static event GoHomeAction GoHome;

    public delegate void SleepAction(); // sleep at 20 hours
    public static event SleepAction Sleep;

    public delegate void EndDayAction();
    public static event EndDayAction EndDay;

    public delegate void SaveHuamansAction();
    public static event SaveHuamansAction SaveHumans;

    public Community theCommunity;
    public const string scorefileName = "/humanscore";
    public const string fileEnd = ".data";
    int numChecksRun =0;

    private float startTime;
    public const float dayLengthSecs = 0.0000001f;
    private float hourTime;

    private int dayLength = 24;
    private int workTime = 6;
    private int homeTime = 14;
    private int sleepTime = 20;

    public int dayCount;
    private bool first = false;
    public bool timeRun = true;
    private bool endreached = false;
    private bool canRun = false;

    private bool hasWork = false;
    private bool hasHome = false;
    private bool hasSleep = false;

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
        hourTime = dayLengthSecs / 24f;
        theCommunity = gameObject.GetComponent<Community>();
        Application.runInBackground = true; //not part of game logic  if multiple of this change to only be called once 
        CanContinue();
        // currentIteration = 0;      
        //StartNewIteration();
        //canRun = true; // just for now

    }

    void StartNewIteration()
    {
        /*
        if (currentIteration >= 10)//theCommunity.allHumans.Count)
        {
            Debug.LogError("reachedEnd");
            endreached = true;
            return;
        }
        */
        ScoreData holdData = new ScoreData();
        //holdData.SetStartData(theCommunity.allHumans[currentIteration]);
        humanScores.Add(holdData);

        //theCommunity.StartComunity(currentIteration);
        

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
            setTimeUI();
            if (first)
            {
                StartNewDay();
                first = false;
            }
            if (timeRun)
            {
                if (!hasWork &&startTime + hourTime * workTime <= Time.time )
                {

                    StartWork();
                    hasWork = true;
                    Debug.Log("work");
                }
                else if (!hasHome && startTime + hourTime * homeTime <= Time.time)
                {

                    GoHome();
                    hasHome = true;
                    Debug.Log("GoHome");
                }
                else if (!hasSleep && startTime + hourTime * sleepTime <= Time.time)
                {

                    Sleep();
                    hasSleep = true;
                    Debug.Log("Sleep");
                }
                else if (startTime + dayLengthSecs <= Time.time)
                {

                    StartEndDay();

                    startTime = Time.time;
                    dayCount++;
                    setTimeUI();
                    StartNewDay();
                    hasWork = false;
                    hasHome = false;
                    hasSleep = false;

                }

            }
            //for running mutiple iterations to find best village
            if (dayCount >=365 * 50)
            {
                //EndIteration();
                //StartNewIteration();
               // Debug.Log("newLevel");
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
        }
        else if (endreached)
        {
            BinaryFormatter bf = new BinaryFormatter();
            for (int i =0;i< humanScores.Count;i++)
            {
                Debug.Log(humanScores[i] + " " + humanScores[i].GetScoreData());
            }
            string holds = scorefileName + numChecksRun.ToString() + fileEnd;
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
        int holdSeconds = Mathf.FloorToInt((Time.time- startTime) /hourTime);
        timeRecord.text = "Year:" + Mathf.Floor(holdTime) + ", Day:" + holdDays + ", Hours:" + holdSeconds;
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

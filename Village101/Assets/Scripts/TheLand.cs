using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// controls how the lad works looking at the current time and day, the weather and seasona
/// </summary>
public class TheLand : MonoBehaviour {

    public delegate void NewDayAction();  
    public static event NewDayAction NewDay;

    public delegate void EndDayAction();
    public static event EndDayAction EndDay;

    public delegate void SaveHuamansAction();
    public static event SaveHuamansAction SaveHumans;

    private float startTime;
    public const float dayLengthSecs = 0.00000000000000000000000000000000000000000000000001f;
    public int dayCount;
    private bool first = false;
    public bool timeRun = true;

    public Text timeRecord;

    
    // Use this for initialization
    void Start () {
        startTime = Time.time;
        dayCount = 0;
        setTimeUI();
        first = true;
       
    }

	
	// Update is called once per frame
	void Update ()
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
        
        if (dayCount >= 365*100)
        {
            SaveHumans();
           
            Debug.Log("newLevel");
        }
      

        // going to make a day 24 seconds


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

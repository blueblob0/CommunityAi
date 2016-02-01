using UnityEngine;
using System.Collections.Generic;

public class Shelter : MonoBehaviour {

    public Transform[] peoplePoint; //  for now we define the points outside the script, later on have points move depending on how mnay total can be fit in the house
    public Human[] peopleList;
    public int maxPeople;
    public int shelterID;
    /// <summary>
    /// used to mark that a baby is incoming
    /// </summary>
    public int pregant = 0;
    public List<Requirement> requirements = new List<Requirement>();
    Community communityObj;
   

    // Use this for initialization
    void Awake ()
    {
        maxPeople = peoplePoint.Length;
        peopleList = new Human[maxPeople];
        communityObj = FindObjectOfType<Community>();
    }
    /// <summary>
    /// for adding alert when enabled
    /// </summary>
    void OnEnable()
    {        
        Community.EndDay += EndShelterDay;
    }

    /// <summary>
    /// for removing alters when disabled
    /// </summary>
    void OnDisable()
    {
        Community.EndDay -= EndShelterDay;
    }

    /// <summary>
    /// placing a person in a house if false is returned then cant find a spot or the person does not exsist
    /// </summary>
    /// <param name="thePerson"> The person to place</param>
    /// <returns> if can be placed</returns>
    public  bool PlacePersonHouse(Human thePerson)
    {
       
        if (!thePerson) // if nothing is passed then cant find a point for them
        {
            Debug.Log("1");
            return false;

        }
        
        for (int i = 0; i < peopleList.Length; i++)
        {
            if (peopleList[i] == thePerson)
            {
                return false;
            }

        }
        int hold = NextEmptySlot();

        if (hold<0) // if there are no empty slots return false
        {
            Debug.LogError("no empty slots" + thePerson.name + " " + name);
            return false;
        }
        
        peopleList[hold] = thePerson;
        thePerson.transform.position = peoplePoint[hold].position;
        thePerson.shelterNum = shelterID;
        return true;
    }


    public bool RemovePersonHouse(Human thePerson)
    {
        if (!thePerson) // if nothing is passed then cant find a point for them
        {
            return false;
        }
       // Debug.Log("17");

        for(int i =0;i< peopleList.Length; i++)
        {
            if (peopleList[i] == thePerson)
            {
                //Debug.Log(peopleList[i]);
                peopleList[i] = null;               
                i = peopleList.Length;
            }
        } 
        return true;
    }

    public void ClearPeople()
    {
        for (int i = 0; i < peopleList.Length; i++)
        {
            peopleList[i] = null;
        }


    }

    
    private int NextEmptySlot()
    {

        int hold = pregant; 
        for (int i = 0; i < peopleList.Length; i++)
        {
            
            if(peopleList[i] == null)
            {
                if (hold>0)
                {
                    Debug.Log(hold);
                    hold--;
                }
                else
                {
                    return i;
                }               
            }
            
        }
        return -1;
    }

    public bool NewShelterDay()
    {
        // if the shelter is empty remove it
        if (CheckShelterEmpty())
        {
            //Debug.Log("122");
            return true;
        }

        //let the hunger and thirst handler know that it has been a new day        
        foreach (Requirement re in requirements)
        {

            re.NewDay();
            re.SetRequired(communityObj.ResourceAvailable(re));
        }
       
        if (CheckShelterEmpty())
        {
            Debug.Log("123");
            return true;
        }



        return false;

    }

    private void EndShelterDay()
    {
        

        foreach (Requirement re in requirements)
        {

            foreach(Human h in peopleList)
            {
                if (re.CheckDeath())
                {
                    h.dead = true;
                    return;
                }
            }

            
        }
        //then add the recourse generated from any job and clear the job
        //currentJob = new Sleeping();

        
    }




    public bool CheckShelterFull()
    {   
        if (NextEmptySlot() < 0) // if there are no empty slots return false
        {           
            return true;
        }
        return false;
    }
    public bool CheckPartnerSpots()
    {
        int count = 0;
        count -= -pregant;
        for (int i = 0; i < peopleList.Length; i++)
        {
            if (peopleList[i] == null)
            {
                count++;
            }

        }

        if (count >= 2) // if there are no empty slots return false
        {
            return true;
        }
        return false;
    }



    public bool CheckShelterEmpty()
    {
        bool hold = true;

        for (int i = 0; i < peopleList.Length; i++)
        {
            if (peopleList[i])
            {
                hold = false;
            }
        }
        return hold;
    }

}

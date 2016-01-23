using UnityEngine;
using System.Collections;

public class Shelter : MonoBehaviour {

    public Transform[] peoplePoint; //  for now we define the points outside the script, later on have points move depending on how mnay total can be fit in the house
    public Human[] peopleList;
    public int maxPeople;
    public int shelterID;

    Community communityObj;
    /*
    void OnEnable()
    {
        TheLand.NewDay += NewShelterDay;
    }


    void OnDisable()
    {
        TheLand.NewDay -= NewShelterDay;
    }
    */
    // Use this for initialization
    void Awake ()
    {
        maxPeople = peoplePoint.Length;
        peopleList = new Human[maxPeople];
        communityObj = GameObject.FindObjectOfType<Community>();
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
       
        int hold = NextEmptySlot();

        if (hold<0) // if there are no empty slots return false
        {
            //Debug.Log("2");
            return false;
        }
        
        peopleList[hold] = thePerson;
        thePerson.transform.position = peoplePoint[hold].position;
        
        return true;
    }


    public bool RemovePersonHouse(GameObject thePerson)
    {
        if (!thePerson) // if nothing is passed then cant find a point for them
        {
            return false;
        }
       
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
        for(int i = 0; i < peopleList.Length; i++)
        {
            if(peopleList[i] == null)
            {
                return i;
            }
            
        }
        return -1;
    }

    public bool NewShelterDay()
    {
        // if the shelter is empty remove it
        if (CheckShelterEmpty())
        {
            return true;
        }


        //try and get wood for the fire in the shelter
        if (communityObj.useWood())
        {
            for (int i = 0; i < peopleList.Length; i++)
            {
                if (peopleList[i])
                {
                    peopleList[i].UseFire();

                }
            }
            
        }
        else // if there is no wood kill people/ make them cold 
        {
            //Killing people now happens within the temperate class 
           // Debug.Log("Shelter kill people");
            /*
            for (int i = 0; i < peopleList.Length; i++)
            {
                if (peopleList[i])
                {
                    peopleList[i].dead = true;
                    
                }
            }
            */
        }
        if (CheckShelterEmpty())
        {
            return true;
        }



        return false;

    }

    public void RemoveThisShelter()
    {
        
        communityObj.shelters.Remove(this);
        Destroy(gameObject);


    }


    public bool CheckShelterFull()
    {   
        if (NextEmptySlot() < 0) // if there are no empty slots return false
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

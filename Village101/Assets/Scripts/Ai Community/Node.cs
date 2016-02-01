using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    public List<GameObject> peopleList = new List<GameObject>();
    public const float peopleWidth = 20f;
    public bool unlimtedPeople;
    public int maxPeopleNode;
    public int production;


    /// <summary>
    /// place a person in the node
    /// </summary>
    /// <param name="thePerson"></param>
    /// <returns></returns>
    public bool PlacePersonNode(GameObject thePerson)
    {
        if (!thePerson) // if nothing is passed then cant find a point for them
        {
            return false;
        }
        peopleList.Add(thePerson);
        PlacePeople();
        return true;
    }

    /// <summary>
    /// Space People around the node
    /// </summary>
    private void PlacePeople()
    {
        if (peopleList.Count == 0)
        {
            return;
        }
        Vector3 toPlace = transform.position;

        //work out the total distance aprt for the people
        float total = peopleWidth  * (peopleList.Count -1);
        // set the start point for people to be 
        //Debug.Log(total + " " + toPlace.x);
        toPlace.x -= (total / 2);
        toPlace.y -= (5);
        //Debug.Log(total + " " + toPlace.x);
        for (int i = 0; i < peopleList.Count; i++)
        {
            peopleList[i].transform.position = toPlace;
            toPlace.x += peopleWidth;
        }

    }


    /// <summary>
    /// remove a person from the node
    /// </summary>
    /// <param name="thePerson"></param>
    /// <returns></returns>
    public bool RemovePersonNode(GameObject thePerson)
    {
        if (!thePerson) // if nothing is passed then cant find a point for them
        {
            return false;
        }

        for (int i = 0; i < peopleList.Count; i++)
        {
            if (peopleList[i] == thePerson)
            {               
                peopleList.RemoveAt(i);
                i = peopleList.Count;
            }
        }
        PlacePeople(); 
        return true;


    }

    public void ClearPeople()
    {
        peopleList.Clear();


    }
}

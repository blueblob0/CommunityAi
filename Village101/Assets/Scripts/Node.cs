using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    public List<GameObject> peopleList;
    const float peopleWidth = 13f;

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
        //Debug.Log(total + " " + toPlace.x);
        for (int i = 0; i < peopleList.Count; i++)
        {
            peopleList[i].transform.position = toPlace;
            toPlace.x += peopleWidth;
        }
    }



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
                //Debug.Log(peopleList[i]);
                peopleList.RemoveAt(i);
                i = peopleList.Count;
            }
        }

        PlacePeople(); ;
        return true;


    }

    public void ClearPeople()
    {
        peopleList.Clear();


    }
}

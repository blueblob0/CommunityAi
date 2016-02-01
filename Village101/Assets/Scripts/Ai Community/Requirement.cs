using UnityEngine;
using System.Collections;

[System.Serializable]
public class Requirement
{
   
    public int resourcePos;

    /// <summary>
    /// the total nuimber required
    /// </summary>
    protected int numRequired; // the current hunger level
    /// <summary>
    /// The number required each day
    /// </summary>
    public int requiredDay;

    public Requirement(int arrayPos)
    {
        resourcePos = arrayPos;
        numRequired = 0;
    }

    /// <summary>
    /// Add the amount to current level if some are not needed pass them back
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int UseAmount(int amount)
    {

        if (numRequired >= amount)
        {

            numRequired += amount;
            //Debug.Log(requirement + " " + numRequired);
            return 0;
        }
        else
        {
            amount -= numRequired;
            numRequired = 0;
            Debug.Log("How did you get here");
            return amount;
        }
    }

    public void NewDay()
    {
        numRequired += requiredDay;
        //Debug.Log(numRequired);
    }

    /// <summary>
    /// for checking if needs more 
    /// </summary>
    public bool Stillrequires()
    {
        if (numRequired >= 1)
        {
            return true;
        }
        return false;
    }


    public virtual bool CheckDeath()
    {
        // if the human has been hungry for less than 20 days they wont die
        if (numRequired < 20 * requiredDay|| requiredDay ==0)
        {
            return false;
        }


        if (numRequired >= 60 * requiredDay) // after 60 days without food you die 
        {
            //Debug.Log("death from Lack " + numRequired +" "+ (requirement));
            return true;
        }

        if (numRequired >= 50 * requiredDay) // after 50 days without food you have 40% chance to die
        {
            // Debug.Log("40% chance to die");
            int x = Random.Range(0, 1000);
            if (x < 400)
            {
                Debug.Log("Lack");
                return true;
            }
            else
            {
                return false;
            }
        }
        if (numRequired >= 40 * requiredDay) // after 40 days without food you have 20% chance to die
        {
            //Debug.Log("20% chance to die");
            int x = Random.Range(0, 1000);
            if (x < 200)
            {
                Debug.Log("Lack");
                return true;
            }
            else
            {
                return false;
            }
        }

        if (numRequired >= 30 * requiredDay) // after 30 days without food you have 10% chance to die
        {
            // Debug.Log("10% chance to die");
            int x = Random.Range(0, 1000);
            if (x < 100)
            {
                Debug.Log("Lack");
                return true;
            }
            else
            {
                return false;
            }
        }

        if (numRequired >= 20 * requiredDay) // after 20 days without food you have 5% chance to die
        {
            // Debug.Log("5% chance to die");
            int x = Random.Range(0, 1000);
            if (x < 100)
            {
                Debug.Log("Lack");
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }


    public int GetRequired()
    {
        //Debug.Log(numRequired);
        return numRequired;
    }


    public void SetRequired(int amount)
    {
        numRequired = amount;


    }
}

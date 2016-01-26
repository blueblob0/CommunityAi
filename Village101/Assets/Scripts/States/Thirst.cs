using UnityEngine;
using System.Collections;
[System.Serializable]
public class Thirst  {
    int thirst; // the number of days without water
   

    public Thirst()
    {
        thirst = 0;
    }


    /// <summary>
    /// record that the person has drunk water instantly  removing any thirst from the person
    /// </summary>
    public void DrinkWater()
    {
        thirst = 0;
    }

    public void NewDay()
    {
        thirst++;        
    }
    
    /// <summary>
    /// Used to check if the person is going to die from lack of water
    /// </summary>
    /// <returns> returns true if they would die</returns>
    public bool CheckThirstDeath()
    {
       
        // if the person has not drunk water for 3 days they will die 
        if (thirst >= 3)
        {
            Debug.Log("death from lack of water");
            return true;
        }
        return false;

    }




}


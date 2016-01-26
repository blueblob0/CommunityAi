using UnityEngine;
using System.Collections;

[System.Serializable]
//later on this will be more complex taking temp from the season's temp if the human is working clothes and the shelter 
//for now just used to record days without fire in shelter and if would die
public class Temperature
{    
    int cold; // the number of days without fire
    
    public Temperature()
    {
        cold = 0;
    }

    /// <summary>
    /// Record that a fire has instantly warmed them up
    /// </summary>
    public void FireOn()
    {       
        cold = 0;
    }


    public void NewDay()
    {
        cold++;
    }

    /// <summary>
    /// Used to check if the person is going to die from lack of heat
    /// </summary>
    /// <returns> returns true if they would die</returns>
    public bool CheckColdDeath()
    {
        int DeathChance =0;

        if (cold >= 20) // after 10 days without heat you die 
        {
            DeathChance = 1001;          
        }
        else if(cold == 19) // after 10 days without heat you die 
        {
            DeathChance = 900;           
        }
        else if (cold == 18) // after 10 days without heat you die 
        {
            DeathChance = 790;                        
        }
        else if (cold == 16) // after 10 days without heat you die 
        {
            DeathChance = 650;            
        }
        else if (cold == 13) // after 10 days without heat you die 
        {
            DeathChance = 490;           
        }
        else if (cold == 10) // after 10 days without heat you die 
        {
            DeathChance = 320;          
        }
        else if (cold == 7) // after 10 days without heat you die 
        {
            DeathChance = 210;
        }
        else if (cold == 4) // after 10 days without heat you die 
        {
            DeathChance = 100;
        }
        else if (cold == 2) // after 10 days without heat you die 
        {
            DeathChance = 50;
        }
        

        int x = Random.Range(0, 1000);
        if (x < DeathChance)
        {            
            Debug.Log("Cold Death");
            return true;
        }
        else
        {
            return false;
        }
      

    }



}

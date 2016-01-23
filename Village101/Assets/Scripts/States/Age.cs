using UnityEngine;
using System.Collections;

//(later) means a featuer that will be implented when is important
// used for holding curernt age and how it will affect the person
// human start at 0 and between 0 and 10 they cant work
//(later) between 0 and 1 or 2 will need a female alwase
//(later) once not depenant on one female adult age or above can look after up to 5 (number might be changed) people 10 or under
// between 11 and 16 work at half efficiency (later efficiency will incraese eachyear e.g. 50% at 11 60 12 70 13 80 14 15 90 16 100 or something similar) 
// after 30 becomes old and works at half efficiency (later efficiency will decay eac hyear eachyear e.g. 99% at 30 and so on 98, 32 97, 33 96,34 95,35 90,36 85,37 80,38 70,39 55,40 40 or something similar ) 
//once get to 30 there is a 0.05% chance to die each day, increasing by 0.01 * years since 30  each year 0.05 at 30, 0.06 at 31 0.08 at 32 0.11 at 33 eact 
// 1, 0.05, 2 0.06 ,3 0.08, 4 0.11
// 1 0, 2 0.01, 3 0.03, 4 0.06, 5 0.10, 6 0.15, 

// only adults can be pregnant ( old people might have a small chance at this later)


public class Age
{
    public const int oldAge = 40;
    public int ageYear ;
    private int ageDay ;
    private ageType theAgeType ;
    const float baseDeathChance = 0.05f;
    float deathChance =0;
      

    public Age()
    {
       
        ageYear = 0;
        ageDay = 0;
        theAgeType = ageType.infant;
        deathChance = 0;
    }

     public Age(int day,int year )
    {
        ageYear = year;
        ageDay = day;
        CheckAgeType();
        WorkDeathChance();
        
    }

    private void CheckAgeType()
    {
        if (ageYear > oldAge)
        {
            theAgeType = ageType.oldAge;
           // GetComponentInChildren<Renderer>().material.color = Color.grey;
        }
        else if (ageYear > 16)
        {
            theAgeType = ageType.adult;
           // GetComponentInChildren<Renderer>().material.color = Color.green;
        }
        else if (ageYear > 11)
        {
            theAgeType = ageType.teen;
          //  GetComponentInChildren<Renderer>().material.color = Color.yellow;
        }
        else
        {
            //Debug.Log(ageYear);
            theAgeType = ageType.infant;
           // GetComponentInChildren<Renderer>().material.color = Color.red;
        }
    }

    public Color CheckAgeColour()
    {

        if (ageYear > oldAge)
        {
            theAgeType = ageType.oldAge;
             return  Color.grey;
        }
        else if (ageYear > 16)
        {
            theAgeType = ageType.adult;
            return Color.green;
        }
        else if (ageYear > 11)
        {
            theAgeType = ageType.teen;
            return Color.yellow;
        }
        else
        {
            theAgeType = ageType.infant;
            return Color.red;
        }
    }

    public bool isChild()
    {
        if(ageYear < 12)
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// used to check if the human is going to die from age will ONLY be called for humans above 30 
    /// </summary>
    public bool CheckAgeDeath()
    {
       
        float hold = Random.Range(0, 100.0f);
        if (hold <deathChance)
        {
            //Debug.Log("DeadAge" + hold +" <" + deathChance); // human is dead
            //Debug.Log("Too old ");
            return true;
           
        }
        return false;
       
    }

    /// <summary>
    /// called when year age goes up by one to increase any chance of death 
    /// </summary>
    private void WorkDeathChance()
    {

        if (ageYear < oldAge)
        {
            return;
        }


        int count = ageYear - oldAge;

         deathChance = 0;

        for (int i = 0; i < count; i++)
        {
            deathChance = deathChance + i +i;
        }

        deathChance /= 365;
    }


    public void NewDay()
    {
        //incease day counter
        ageDay++;

        // see if you need to increase year
        if (ageDay >= 365)
        {
            ageYear++;
            ageDay = 0;
            CheckAgeType(); // check to see if the age type has changed (infant adult ect)
            WorkDeathChance(); // work out chance to die from age
        }
        
    }

    public ageType GetAgeType()
    {

        return theAgeType;

    }

   


    public string CheckAge()
    {

        string hold = ageYear.ToString() + " years, " + ageDay.ToString() + " days ";
        return hold;
    }

}

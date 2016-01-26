using UnityEngine;
using System.Collections;
[System.Serializable]
public class Pregnancy
{
    bool pregnant;
    int lengthofPregnancy;
    public bool canBePregnant;
    int countnopreg;
    float chance =1;
    // for now just handles if the mother will give birth but later on might also be used to check if the mother can work in currrent state 
    // and if the mother does work if it has any affect on the baby 

    /// <summary>
    /// for initializing Pregnancy
    /// </summary>
    /// <param name="preg"> if the </param>
    public Pregnancy()
    {
        pregnant = false;
        lengthofPregnancy = 0;
        canBePregnant = false;
        countnopreg = 0;
    }

    /// <summary>
    ///set the person as pregnant
    /// </summary>
    private void setPregnant()
    {
        // if already pregnant something is wrong
        if (pregnant)
        {
            Debug.Log("Already pregnant");
            return;
        }

        pregnant = true;
        lengthofPregnancy = 0;
       // Debug.Log(countnopreg);
    }

    /// <summary>
    ///  the script for marking a new day, this may have to return something to say if a child is born
    /// </summary>
    /// <returns> If the Mother gives birth</returns>
    public bool NewDay()
    {
        
        if (!canBePregnant)
        {
            return false;
        }

        if (pregnant)
        {
            lengthofPregnancy++;
            // if they have been pregnant for more than 37 weeks then they can give birth 
            if (lengthofPregnancy>=259)
            {
              return(CheckGiveBirth());
            }
        }
        else
        {
            countnopreg++;
            // give the woman a random chance to get Pregnant for now
            RandomGetPreganant();
        }
        return false;
    }

    /// <summary>
    /// for now handle randomly getting segregant here later on may be more complex with family and stuff 
    /// </summary>
    void RandomGetPreganant()
    {
       // Debug.Log("2");
        float check = Random.Range(0.0f, 100.0f);
        if (check< chance)
        {
            setPregnant();
        }

    }

    public void SetChance(ageType age)
    {
        if(age== ageType.adult)
        {
            chance = 1;
            
        }
        else
        {
            
            chance =0.1f;
        }


    }


    /// <summary>
    /// used to check if the pregnant person will give birth  
    /// </summary>
    private bool CheckGiveBirth()
    {
        // mark the chance to give birth 
        float birthChance = 0;
        // the latest time normally give birth ( as this is a game we are working between 37 and 42 weeks for pregnancy)
        if (lengthofPregnancy >= 294)
        {
            birthChance = 101;
        }
        else if (lengthofPregnancy >= 290)
        {
            birthChance = 85;
        }
        else if(lengthofPregnancy >= 287)
        {
            birthChance = 70;
        }
        else if (lengthofPregnancy >= 284)
        {
            birthChance = 55;
        }
        else if (lengthofPregnancy >= 282)
        {
            birthChance = 45;
        }
        else if (lengthofPregnancy >= 280)
        {
            birthChance = 35;
        }
        else if (lengthofPregnancy >= 278)
        {
            birthChance = 25;
        }
        else if (lengthofPregnancy >= 275)
        {
            birthChance = 20;
        }
        else if (lengthofPregnancy >= 273)
        {
            birthChance = 15;
        }
        else if (lengthofPregnancy >= 270)
        {
            birthChance = 10;
        }
        else if (lengthofPregnancy >= 266)
        {
            birthChance = 5;
        }
        else if (lengthofPregnancy >= 259)
        {
            birthChance = 1;
        }





        float hold = Random.Range(0, 100);
        if(hold< birthChance)
        {
            //Debug.Log("gives birth on day " + lengthofPregnancy);
            // gives birth
            return true;

        }
        return false;
    }

    public void GiveBirth()
    {
        countnopreg = 0;
         pregnant = false;
         lengthofPregnancy =0;
    }

    /// <summary>
    /// Used to check if the player can work (current just returns false but will look at time of pregnancy and determine if she can still work or not, 
    /// later on may look at traits to see if the woman is concerned about the babies life over work )
    /// </summary>
    /// <returns></returns>
    public bool CanWork()
    {


        return true;
    }




    /// <summary>
    ///  for checking if the person is pregnant
    /// </summary>
    /// <returns></returns>
    public bool IsPregnant()
    {
        return pregnant;
    } 
      



}

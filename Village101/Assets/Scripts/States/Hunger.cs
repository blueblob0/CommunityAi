using UnityEngine;
using System.Collections;

public class Hunger
{    
    int hunger; // the current hunger level
    bool[] meals; // record if the human has eat  3 meals a day 

    public Hunger()
    {

        hunger = 0;
        meals = new bool[3];
        ResetMeals();
    }

    /// <summary>
    /// Eat some food
    /// </summary>
    /// <param name="c">The number of food to eat</param>
    public void EatMeal(int c)
    {
        if(c <=0){

            return;
        }


        for (int i = 0; i < c; i++)
        {
            EatMeal();
        }
    }

    

    /// <summary>
    /// eat one meal
    /// </summary>
    private void EatMeal()
    {
        bool checking = false;
        int i = 0;
        while (!checking)
        {
            if (!meals[i])
            {
                meals[i] = true;
                checking = true;
            }
            i++;
        }

        if (meals[2])
        {
            ResetMeals();
            hunger--;
        }
    }

    public void CheckHunger()
    {
        //Debug.Log(hunger);

    }

    public void NewDay()
    {
        hunger++;
        //Debug.Log(hunger);
    }

    /// <summary>
    /// for checking if needs more than 1 food to fill up 
    /// </summary>
    public bool IsHungry()
    {
        if(hunger >1)
        {
            return true;
        }
        return false;
    }



    public void ResetMeals()
    {
        meals[0] = false;
        meals[1] = false;
        meals[2] = false;
    }

    /// <summary>
    /// used to see if the human will die with current hunger level
    /// </summary>
    /// <returns> if dead</returns>
    public bool CheckHungerDeath()
    {

        // if the human has been hungry for less than 20 days they wont die
        if(hunger < 20)
        {
            return false;
        }
       
        if (hunger >= 60) // after 60 days without food you die 
        {
            Debug.Log("death from food");
            return true;
        }

        if (hunger >= 55) // after 55 days without food you have 60% chance to die
        {
           // Debug.Log("60% chance to die");
            int x = Random.Range(0, 1000);
            if (x < 600)
            {
                Debug.Log("Lack of food");
                return true;
            }
            else
            {
                return false;
            }   
        }

        if (hunger >= 50) // after 50 days without food you have 40% chance to die
        {
           // Debug.Log("40% chance to die");
            int x = Random.Range(0, 1000);
            if (x < 400)
            {
                Debug.Log("Lack of food");
                return true;
            }
            else
            {
                return false;
            }  
        }

        if (hunger >= 40) // after 40 days without food you have 20% chance to die
        {
            //Debug.Log("20% chance to die");
            int x = Random.Range(0, 1000);
            if (x < 200)
            {
                Debug.Log("Lack of food");
                return true;
            }
            else
            {
                return false;
            }   
        }

        if (hunger >= 30) // after 30 days without food you have 10% chance to die
        {
           // Debug.Log("10% chance to die");
            int x = Random.Range(0, 1000);
            if (x < 100)
            {
                Debug.Log("Lack of food");
                return true;
            }
            else
            {
                return false;
            }   
        }

        if (hunger >= 20) // after 20 days without food you have 5% chance to die
        {
           // Debug.Log("5% chance to die");
            int x = Random.Range(0, 1000);
            if (x<50)
            {
                Debug.Log("Lack of food");
                return true;
            }
            else
            {
                return false;
            }            
        }

        return false;
    }
}
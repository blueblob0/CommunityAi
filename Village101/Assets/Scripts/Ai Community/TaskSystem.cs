using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The Task System for asigning correct tasks to people to assure the survival of the community 
/// </summary>
public class TaskSystem
{
    
    

    Community thisCommunity;

    // need to make sure that there is enough food and water for everyone, that the temperate is good and that everyone has had enough sleep

   public TaskSystem(Community com)
    {
        thisCommunity = com;
    }
   
    /// <summary>
    /// When new tasks are to be assigned to people         
    /// </summary>
    /// <param name="people"> a list of the people in the community </param>
    void newAssigement(List<Human> people)
    {
        //bool check = false;

        //Run through each human to start the day so we dont get stuck with people still having old jobs 
        foreach (Human h in people)
        {
            h.StartHumanDay();
        }
        

        //list of the people unassgined to a task as people are assigned to tasks they are removed (copy without refrence) 
        // List<Human> unAssignedPeople = new List<Human>(people);

        // lists to hold all of the unassigned people infants cant work teens and old work at half and adults at full
        List<Human> unAssignedAdults = new List<Human>();
        List<Human> unAssignedTeens = new List<Human>();
        List<Human> unAssignedOldAge = new List<Human>();
        foreach (Human h in people)
        {
            if (h.age.GetAgeType() == AgeType.adult)
            {
                unAssignedAdults.Add(h);

            }
            else if (h.age.GetAgeType() == AgeType.teen)
            {

                unAssignedTeens.Add(h);
            }
            else if (h.age.GetAgeType() == AgeType.senior)
            {

                unAssignedOldAge.Add(h);
            }



        }
        //Get the resourses we need
        int needCount =0;
        foreach (Resource r in thisCommunity.allResources)
        {
            //Check if the village needs Resource
            needCount = thisCommunity.ResourceToSurivive(r);

           
            while (needCount > 0)
            {
               
                // assign some villagers to gather the amount NEEDED, includes children and old people
                List<Human> hold = ListToPass(unAssignedAdults, unAssignedTeens, unAssignedOldAge);
                
                if (hold != null)
                {                    
                    UnAssignedSetJob(hold, r);
                    needCount -= r.node.production;                    
                }
                else
                {
                    Debug.LogError("Not enough people for Task");
                    return;
                }
            }

        }

        // get all adults working to try and keep up with amount used
        foreach (Resource r in thisCommunity.allResources)
        {
            //Check if the village needs food
            //Try to keep food production at the level of the amount used
            needCount = thisCommunity.ResourceUsed(r);
            while (needCount > 0 && CheckStillUnAssigned(unAssignedAdults))
            {
                UnAssignedSetJob(unAssignedAdults, r);
                needCount -= r.node.production;
            }

        }

       






        /*
        //SLEEP

        //firstly cycle through the list of people for anyone who has not had enough sleep
        //This may be ordered by the amount of sleep needed
        //set them to a sleeping list and remove from unAssigned list so if you need more people for jobs then you can assign them to jobs  

        //WATER
        //Then see if water is needed (if there is a well no water needed if not well then water job needed)
        if (thisCommunity.GatherWater())
        {
            // if the water is needed asgined people to go to the water source depening on number of people in the village and the amount someone can carry
            int totalWater; // the number of people needed to get water

            totalWater = Mathf.CeilToInt((float)people.Count / (float)waterCarryAmount);

            for (int i = 0; i < totalWater; i++)
            {
                //mark people going to fetch water equal to the people needed to fetech water

            }

        }


        //FOOD AND FUEL

        //first look at what food and fuel is needed to survive e.g. if there is no backup and assign people to do food and then fuel producing jobs
        // the first look will also take people from the sleeping list if needed
        // then try to produce the food and fuel that is being consumed by the community that day and assign people to these jobs not taking from the sleep list

        //Check if the village needs food
        int needFood = thisCommunity.FoodToSurvive();
        //Debug.Log(needFood);
        while (needFood > 0)
        {
           // Debug.LogError(needFood);
            // assign some villagers to gather the amount of food NEEDED, includes children and old people as in future "sleeping" people   
            List<Human> hold = ListToPass(unAssignedAdults, unAssignedTeens, unAssignedOldAge);

            if (hold != null)
            {

                Farming checkFarm = new Farming();

                UnAssignedSetJob(hold, checkFarm);
                needFood -= checkFarm.jobProduction;
                // Debug.Log(needFood);
            }
            else
            {
                Debug.LogError("Not enough people for food");
                return;
            }
        }

        // check if the village need fuel
        int needFuel = thisCommunity.FuelToSurivive();


        while (needFuel > 0)
        {

            // assign some villagers to gather the amount of Fuel NEEDED, includes teens and old people as in future "sleeping" people               
            List<Human> hold = new List<Human>();
            hold = ListToPass(unAssignedAdults, unAssignedTeens, unAssignedOldAge);

            if (hold != null)
            {

                Logging checkLog = new Logging();
                UnAssignedSetJob(hold, checkLog);
                needFuel -= checkLog.jobProduction;
            }
            else
            {
                Debug.LogError("Not enough people for fuel ");
                Debug.LogError(needFuel);
                //needFuel = 0;
                return;
            }

        }


        // these 2 dont take people from the sleeping array, teen array  or old array


        //Try to keep food production at the level of the amount used
        needFood = thisCommunity.FoodUsed();
        while (needFood > 0 && CheckStillUnAssigned(unAssignedAdults))
        {
            // assign some villagers to gather the amount of food NEEDED
            Farming checkFarm = new Farming();
            UnAssignedSetJob(unAssignedAdults, checkFarm);
            needFood -= checkFarm.jobProduction;
        }

        //Try to keep fuel production at the level of the amount used
        needFuel = thisCommunity.FuelUsed();
        while (needFuel > 0 && CheckStillUnAssigned(unAssignedAdults))
        {
            // assign some villagers to gather the amount of fuel NEEDED
            Logging checkLog = new Logging();
            UnAssignedSetJob(unAssignedAdults, checkLog);
            needFuel -= checkLog.jobProduction;
        }

        //FINAL
        //Check to see if any people unsed and assign them to a job they want?/ just to the sleeping list 
        //Mark everyone on the sleeping list to get more sleep (either they get nap time or just count as resting for the day and get x sleep back from that 
        */

    }

    void UnAssignedSetJob(List<Human> unAssignedPeople, Resource theResource)
    {

        int hold = Random.Range(0, unAssignedPeople.Count);
        Human newPerson = unAssignedPeople[hold];
        unAssignedPeople.Remove(newPerson);
       
        if (newPerson.dead)
        {
            if (unAssignedPeople.Count <= 0)
            {
                return;
            }
            
            UnAssignedSetJob(unAssignedPeople, theResource);


        }
        else
        {

            newPerson.SetResource(theResource);
        }




        //Debug.Log(newPerson.firstName);
    }

    bool CheckStillUnAssigned(List<Human> unAssignedPeople)
    {
        if (unAssignedPeople.Count > 0)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// used to check who i needed to work
    /// </summary>
    /// <param name="unAssignedAdu"></param>
    /// <param name="unAssignedTee"></param>
    /// <param name="unAssignedOld"></param>
    /// <returns></returns>
    List<Human> ListToPass(List<Human> unAssignedAdu, List<Human> unAssignedTee, List<Human> unAssignedOld)
    {
        if (CheckStillUnAssigned(unAssignedAdu))
        {
            //Debug.Log("adu");
            return unAssignedAdu;

        }
        else if (CheckStillUnAssigned(unAssignedTee))
        {
            //Debug.Log("Tee");
            return unAssignedTee;

        }
        else if (CheckStillUnAssigned(unAssignedOld))
        {
            //Debug.Log("Old");
            return unAssignedOld;

        }
        return null;

    }


    // called by an event when a new day starts 
   public  void StartOfDay()
    {

        

        newAssigement(thisCommunity.GetHumansClass()); //pass a list of people currently in the community     

    }

  



}

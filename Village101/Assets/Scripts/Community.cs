using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class Community : MonoBehaviour {
       
    // The Names of the prefabs to be loaded for each type of item
    private const string humanPrefabName = "Human SomeAi";
    private const string shelterPrefabName = "Normal House"; // just one house for now if time later add more houses

    public const int foodPersonNeeds = 3;
    public const int waterPersonNeeds = 1;
    public const int fuelShelterNeeds = 1;

    public Node fuelNode;
    public Node foodNode;
    
    

    //Set to private later
    private List<Human> humans = new List<Human>();
    public List<Shelter> shelters = new List<Shelter>();
    public int food = 0; // each villager needs 3 food per a day, if food is not cooked it only counts as 1/2 a food, 3 chickens give 1 food per day, other animals indifferent amounts but also use food 
    public int fuel = 0; //Each shelter needs 1 wood per day to keep fire going, one tree gives 2-3 wood
    private int water = 0;  // the amount of water irrelevant if the village has a well, each person needs 1 water per day, gathering produces 5 water
    private int livestock; // live stock done my amount for a farmer to use so a couple of cattle or sheep ect!

    private WaterSources waterSource;
    public float maxScreenWidth; // represents the max width of the camera for placing houses


    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true; //not part of game logic  if multiple of this change to only be called once 



        //Start by creating villagers 

        //villagers not random now but might be later 
        int numVillagers =  10;


        for (int i = 0; i < numVillagers; i++)
        {
            CreateNewVillager();
        }

        //Then the starting Resourses (the idea is its a exsisting village not a new one with nothing)

        //Start with Shelter
        GenerateShelter(); // make shelter
        HousePeople(); // house villagers in shelter

        //Then Water
        GenerateWater();

        //Then Food
        GenerateFood();

        //Then wood
        GenerateWood();

        //Then Livestock although currently does nothing)
        GenerateLivestock();
       // food = 1000000000; 
    // fuel = 1000000000;

}

    void OnEnable()
    {        
        TheLand.EndDay += EndOfDay;
    }


    void OnDisable()
    {       
        TheLand.EndDay -= EndOfDay;
    }
   
    #region Generate starting resourses 

    /// <summary>
    /// Generate Starting shelter for the community 
    /// </summary>
    void GenerateShelter()
    {
        shelters.Clear(); // make sure the list is empty before starting
        maxScreenWidth = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize * 2; // this is run at the start so get the camera width for showing the shelters

        //Alwase need one shelter so start by making that 
        AddShelter();

        // work out max shelters needed for current people
        float max = (float)humans.Count / (float)shelters[0].maxPeople;
        double shelterNeeded = System.Math.Ceiling(max);

        // add the posibility to have 1 extra house (uses big numnbers to make it more random) remove this for the starting stuff 
        // shelterNeeded += System.Math.Floor( Random.Range(0,2000)/1000.0); 

        //generate houses
        while (shelters.Count < shelterNeeded)
        {
            AddShelter();
        }

    }


    /// <summary>
    /// spread the villagers out by house ( for now this is random by will assign people to houses perminatly later)
    /// </summary>
    void HousePeople()
    {
        int c = -1;
        foreach (Human v in humans)
        {

            bool check = false;

            while (!check)
            {
                increaseBySize(ref c, shelters.Count);
                check = shelters[c].PlacePersonHouse(v);
                          
            }
            v.shelterNum = shelters[c].shelterID;
            //Debug.Log(v.GetComponent<Human>().firstName + "shelter ID: " + shelters[c].shelterID + "human ID:  " + v.GetComponent<Human>().shelterNum);
          //  increaseBySize(ref c, shelters.Count);
        }
    }

    void increaseBySize(ref int toIncrease, int size)
    {
        toIncrease++;
        if (toIncrease >= size)
        {
            toIncrease = 0;

        }
    }

    public List<Human> GetHumansObject()
    {
        return humans;
    }

    public List<Human> GetHumansClass()
    {

        List<Human> hold = new List<Human>();

        foreach(Human g in humans)
        {
            hold.Add(g);
        }
        return hold;
    }

    void GenerateFood()
    {
        //food = Random.Range(villagers * 3*7, villagers*3*180);
        food = 300; // start with food for 10 days 
    }

    void GenerateWood()
    {
        fuel = 20; // wood for 10 days in inital    
        // wood = Random.Range(10, 50);
    }


    void GenerateWater()
    {
        waterSource = WaterSources.Well; // set the water to have a well for now later have possibly of river ect

        if (waterSource == WaterSources.Well) // for now this will alwase be true but if the water source is a river will be relevant
        {
            water = 0;
        }
    }

    void GenerateLivestock()
    {
        //starting livestock random amount between villagers (removed for now)    
        /*
       int toplivestockamount = (int)(System.Math.Ceiling(villagers.Count / 10.0f));
        //Debug.Log(toplivestockamount);
        livestock = toplivestockamount; //set to max for now will be random
        */

    }
    #endregion

    #region Add things to village


    /// <summary>
    /// creating a villager for now just makes them but later may do more
    /// </summary>
    void CreateNewVillager()
    {
        //create villager game objects and add them to the list
        GameObject a = Instantiate(Resources.Load(humanPrefabName)) as GameObject;

        int year = Random.Range(14, 35);
        if(year <= 16)
        {
            
            year = Random.Range(12, 17);

        }else if (year >30)
        {
            year = Random.Range(30,40);

        }

        a.GetComponent<Human>().StartHuman(Random.Range(0, 365), year,null);   

        humans.Add(a.GetComponent<Human>());
    }



    /// <summary>
    ///  creating a villager for now just makes them but later may do more
    /// </summary>
    /// <param name="mum">The baby's mum</param>
    /// <returns></returns>
    public Human CreateBabyVillager(Human mum)
    {
        if (AllSheltersFull())
        {
            AddShelter();
        }

        //create villager game objects and add them to the list
        GameObject a = Instantiate(Resources.Load(humanPrefabName)) as GameObject;
        Human ha = a.GetComponent<Human>();
        ha.StartHuman(0,0,mum);
        humans.Add(ha);
        HousePerson(ha);
        return ha;
        //Debug.Log("baby made");
    }

    /// <summary>
    /// used to check the majority sex  true then more males
    /// </summary>
    /// <returns>true then more males </returns>
    public bool? GetMajoritySex()
    {
        int maleC = 0;
        int femaleC = 0;
        foreach(Human h in humans)
        {
            if(h.sex == Human.maleS)
            {
                maleC++;
            }
            else
            {
                femaleC++;
            }
        }
        if (maleC > femaleC)
        {
            return true;

        }
        else if (maleC < femaleC)
        {
            return false;
        }
        return null;
    }


    /// <summary>
    /// spread the villagers out by house ( for now this is random by will assign people to houses perminatly later)
    /// </summary>
    private void HousePerson(Human person)
    {

        bool check = false;
        int hold = -1;
        for(int i =0; i< shelters.Count; i++)
        {
            if (!shelters[i].CheckShelterFull())
            {
                check = shelters[i].PlacePersonHouse(person);

                if (check)
                {
                    hold = i;
                    i = shelters.Count;
                }

            }
        }
        if (hold > -1)
        {
            person.GetComponent<Human>().shelterNum = shelters[hold].shelterID;

        }
        else
        {
            Debug.Log("help");
        }
       
       
        //Debug.Log(v.GetComponent<Human>().firstName + "shelter ID: " + shelters[c].shelterID + "human ID:  " + v.GetComponent<Human>().shelterNum);
        //  increaseBySize(ref c, shelters.Count);
    }


    public bool AllSheltersFull()
    {
        bool testing = true;
        for (int i = 0; i < shelters.Count; i++)
        {
            if (!shelters[i].CheckShelterFull())
            {
                testing = false;
            }
        }
        return testing;

    }


    /// <summary>
    /// For adding a new shelter
    /// </summary>
    public void AddShelter()
    {
        //start by geenrasting an instance of the shelter
        GameObject holdShelter = Instantiate(Resources.Load(shelterPrefabName)) as GameObject;

        Shelter temp = holdShelter.GetComponent<Shelter>();


        if (shelters.Count > 0)
        {
            temp.shelterID = GetShelterId(); // need to make sure when a shelter is destoryed you remove id ref from villgers 
            //temp.shelterID = shelters.Count; // this may create a bug later where 2 shelters have the same id so may need to look at this ( if one is destroyed and then another created)
        }
        else
        {
            temp.shelterID = 0;
        }


        
        // add the shelter to the list
        shelters.Add(temp);

        // calculate the seperation between shelters
        float seperation = (maxScreenWidth * 2) / (shelters.Count + 1);

        // display the shelters on the screen with correct speration 
        for (int i = 0; i < shelters.Count; i++)
        {
            // shelter x postion is -maxWidth + (serperation * i )
            Vector3 holdPos = shelters[i].transform.position;
            holdPos.x = -maxScreenWidth + (seperation * (i + 1));
            shelters[i].transform.position = holdPos;
        }
    }


    int GetShelterId()
    {
        int tempid = shelters.Count;
        if(IDFree(tempid))
        {
            return tempid;
        }

        tempid = -1;
        bool check = false;
        do
        {
            tempid++;
            check = IDFree(tempid);

        } while (!check);



        return tempid;
    }


    bool IDFree(int ID)
    {
        for (int i = 0; i < shelters.Count; i++)
        {
            if (ID == shelters[i].shelterID)
            {
                return false;

            }
        }
        return true;

    }

    /// <summary>
    /// Adds a person to there shelter by its id 
    /// </summary>
    /// <param name="Id">ID of the shelter </param>
    /// <param name="hum"> person to add</param>
    void AddToShelter(int Id, Human hum)
    {
        GetShelterID(Id).PlacePersonHouse(hum);                    
    }


    /// <summary>
    /// removes a person from shelter by its id 
    /// </summary>
    /// <param name="Id">ID of the shelter</param>
    /// <param name="hum">person to remove</param>
    void RemoveFromShelter(int Id, GameObject hum)
    {       
        GetShelterID(Id).RemovePersonHouse(hum);
    }

    void KillShelterID(int id)
    {
        foreach(Human h in humans)
        {
            if (h.shelterNum == id)
            {
                h.dead = true;
            }

        }


    }

    /// <summary>
    /// get a shelter by its id 
    /// </summary>
    /// <param name="Id">The id of teh shelter</param>
    /// <returns>The shelter</returns>
    Shelter GetShelterID(int Id)
    {
        for (int i = 0; i < shelters.Count; i++)
        {
            if (shelters[i].shelterID == Id)
            {
                return shelters[i];
            }
        }

        return null;
    }



    // this will be more complex in future or at least have the option too be ( difftent food types ect)
    public void AddFood(int newFood)
    {
        food += newFood;

    }


    //wood may have a life time before it can be used in future ect 
    public void AddWood(int newWood)
    {
        fuel += newWood;
    }

    /// <summary>
    /// for when wood is used by the shelter
    /// </summary>
    /// <returns> if wood can be taken or not</returns>
    public bool useWood()
    {
        
        if (fuel <= 0)
        {
            //Debug.LogError("1");
            return false;
        }
         
       // Debug.Log(fuel);
       fuel--;
       
        return true;
    }

    /// <summary>
    /// Add the relevent resoures when a task has been compelted 
    /// </summary>
    /// <param name="finishedTask"> the task thast has been finished </param>
    public void TaskDone(Task finishedTask)
    {

        if (finishedTask == null)
        {
            Debug.Log("no task");
            return;
        }
        else if (finishedTask.GetType() == typeof(Farming))
        {
            food += finishedTask.jobProduction;
        }
        else if (finishedTask.GetType() == typeof(Logging))
        {
            fuel += finishedTask.jobProduction;
        }
        else if (finishedTask.GetType() == typeof(WaterGather))
        {
            water += finishedTask.jobProduction;

        }
    }


    /// <summary>
    /// Function for marking a human dead
    /// </summary>
    /// <param name="deadHuman"> the human that is dead</param>
    public void ImDead(Human deadHuman)
    {

        humans.Remove(deadHuman);

    }


    #endregion

    #region checking village

    /// <summary>
    /// work out if there is food
    /// </summary>
    /// <param name="hungry">If a villager need more than 1 food</param>
    /// <returns>How much food they can eat</returns>
    public int FoodAvailable(bool hungry)
    {
        if (food < 1)
        {
            //Debug.Log(" no food ");
            return 0;
        }
        //Work out if there is extra food for a hungry villager
        if (hungry && food > humans.Count * 3)
        {
            food -= 2;
            return 2;
        }
        else
        {
            food--;
            return 1;
        }
    }

    /// <summary>
    /// used to see if there is water for the person to drink
    /// </summary>
    /// <returns></returns>
    public bool WaterAvailable()
    {
        // for now jsut checks if tehre is a well or the vilalger has water will later on a near river will alsso count.
        // it is possible that in  the futere this is just taken from the water resourse and someone draws from the well/nearby river for the rest oif the village
        if(waterSource == WaterSources.Well)
        {
            return true;
        }
        else if(water >0)
        {
            water--;
            return true;
        }
        return false;

    }


    /// <summary>
    /// used to see howe much food is need for the people to eat the required number of food
    /// </summary>
    /// <returns>The number of food needed</returns>
    public int FoodToSurvive()
    {

        int totalNeeded =foodPersonNeeds * humans.Count;
        //Debug.Log(humans.Count);
        return NeedXToSurvive(food, totalNeeded);
    }

    /// <summary>
    /// used to see how much fuel is needed to make it toi the next day 
    /// </summary>
    /// <returns> the number of fuel needed</returns>
    public int FuelToSurivive()
    {
        int totalNeeded = fuelShelterNeeds * shelters.Count;
        //return  NeedXToSurvive(5, totalNeeded);
        return NeedXToSurvive(fuel, totalNeeded);
    }



    /// <summary>
    /// works out the number of an item needed to survive
    /// </summary>
    /// <param name="amount">The amount of the thing</param>
    /// <param name="needed"> The numebr needed</param>
    /// <returns></returns>
    private int NeedXToSurvive(int amount, int needed)
    {        
        if (amount >= needed)
        {
            
            return 0;
        }
        
        return (needed - amount);
    }


   // for testing 
    private int NeedXTestToSurvive(int amount, int needed)
    {
        if (amount >= needed)
        {
            //Debug.Log(amount + " " + needed);
            return 0;
        }
        Debug.Log(food);
        Debug.Log(needed - amount + " " + needed + " " + amount);
        return (needed - amount);
    }

    /// <summary>
    /// used to check how much food will be used by people that day
    /// </summary>
    /// <returns></returns>
    public int FoodUsed()
    {
        return foodPersonNeeds * humans.Count;
    }

    /// <summary>
    /// used to check how much fuel will be used by people that day
    /// </summary>
    /// <returns></returns>
    public int FuelUsed()
    {
        return fuelShelterNeeds * shelters.Count;
    }


    /// <summary>
    /// Function to check if water needs to bhe gathered (depends on if there is a well or not)
    /// </summary>
    /// <returns>yes or no for wter needing to be gathered</returns>
    public bool GatherWater()
    {
        if(waterSource == WaterSources.Well)
        {
            return false;
        }

        return true;

    }
    
    public void StartShelterDay()
    {

        Shelter[] shelt = new Shelter[shelters.Count];
        int hold = 0;
        // for running through the new day things on the shelters 
        foreach(Shelter s in shelters)
        {
            // check if the shelter new day returns true and if it does it needs to be removed
            if(s.NewShelterDay())
            {
                shelt[hold] = s;
                hold++;
            }
        }

       for(int i =0;i< shelt.Length; i++)
        {
            if(shelt[i]!= null)
            {
                KillShelterID(shelt[i].shelterID);
                shelt[i].RemoveThisShelter();
            }
        }


    }


    public Human GetFreeOtherSex(string sex)
    {
        Human thePartner = null;
        foreach (Human h in humans)
        {
            if(h.sex != sex && h.CheckForPartner() == false)
            {
                return h;
            }
        }
        return thePartner;

    }



    
    #endregion


    public bool SetHumanLocation(PossLocations loc, Human theHuman)
    {
        bool check = false;
        if(loc == PossLocations.shelter)
        {
            AddToShelter(theHuman.shelterNum, theHuman);
        }
        else  if(loc == PossLocations.foodNode)
        {
            foodNode.PlacePersonNode(theHuman.gameObject);
            check = true;
        }
        else if (loc == PossLocations.fuelNode)
        {
            fuelNode.PlacePersonNode(theHuman.gameObject);
            check = true;
        }

        if (check)
        {
            RemoveFromShelter(theHuman.shelterNum, theHuman.gameObject); // remove it from thee sehlter as we are adding it to the new place 
            return true;
        }



        return false;
    }

    private void RemoveNullShelter()
    {
        int remove = -1;
        for (int i = 0; i < shelters.Count; i++)
        {
            if (shelters[i] == null)
            {
                Debug.Log(shelters[i]);
                remove = i;
            }          
        }

        if (remove != -1)
        {
            shelters.RemoveAt(remove);
            RemoveNullShelter();
        }


    }



    /// <summary>
    /// For handling the end of the day making sure people go bavk to home
    /// </summary>
    void EndOfDay()
    {

        //RemoveNullShelter();
        for ( int i =0;i< shelters.Count;i++)
        {
            shelters[i].ClearPeople();
            
            //Debug.Log(h.name +" " +h.transform.position);
        }

        // make sure to clear peple from jobs at end of the day people will move themselves from job when done in future
        foodNode.ClearPeople();
        fuelNode.ClearPeople();


        foreach (Human h in humans)
        {
            AddToShelter(h.shelterNum, h);
            //Debug.Log(h.name +" " +h.transform.position);
        }
    }

    public int NumHumans()
    {
        return humans.Count;

    }

    /// <summary>
    /// used while not real pregnancy to check that are not more than 1 child per couple
    /// </summary>
    /// <returns></returns>
    public bool ParentsCanHaveChildren()
    {
       
        int holdC = 0;
        int holdA = 0;
        for (int i =0;i < humans.Count; i++)
        {

            Human holdH = humans[i];

            if (holdH.age.isChild())
            {
                holdC++;
            }
            else 
            {
                holdA++;
            }
        }
        int childrenMax = Mathf.FloorToInt(holdA / 2.0f);
        if (holdC < childrenMax)
        {
           // Debug.Log(holdC + " " + childrenMax);
            return true;
        }


        return false;
    }
}

/// <summary>
/// locations where people can be 
/// </summary>
public enum PossLocations
{
    shelter,
    foodNode,
    fuelNode
    


}

public enum ageType
{
    infant,
    teen,
    adult,
    oldAge


}
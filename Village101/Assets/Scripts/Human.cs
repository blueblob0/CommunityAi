using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    #region Outside Varables    

    private const int maxHealth = 100;
    private const int maxMediumHealth = 80;
    private const int maxLowHealth = 40;
    public Text nameUI;

    Community communityObj;

    #endregion


    #region Internal Varables

    #region Human Vitals


    //public int ageYear =0;
    // public int ageDay=0;
    // public ageType theAgeType = ageType.adult;

    
    public bool dead = false;

    private Hunger food;
    private Thirst water;
    private Temperature tempera;
    private Pregnancy pregnant;
    public Age age;

  
    public Task currentTask = null;

    public int shelterNum = -1;

    public bool newDay = false;

    private Material myMat;

    #endregion

    #region Personality
    public string firstName;
    public string surname;
    public string sex;

    #endregion


    #region Relations
    private Human currentChild;
    private Human dad;
    private Human mum;
    private Human partner;
    private List<Human> children = new List<Human>();
    #endregion

    #region Programming Info
    public bool deadFinished = false;
    #endregion
    

    #endregion

    public const string femaleS = "female";
    public const string maleS = "male";


    #region Create Human

    // Creates the human with a age (has to be called to make starting age easier
    public void StartHuman(int day, int year,Human theMum)
    {
        dead = false;
      
        // get the community reference 
        communityObj = GameObject.FindObjectOfType<Community>();
        // assign a hunger class to keep track of the humans hunger level
        food = new Hunger();
        water = new Thirst();    
        age = new Age(day,year);
        tempera = new Temperature();
        GenerateSex();
        partner = null; // mark no partner at start;
        // if the child has a mum
        if (theMum)
        {
            surname = theMum.surname;
            mum = theMum;
            dad = theMum.partner;
        }
        else // otherwise create a new surname for them
        {
            mum = null;
            dad = null;
            int numNames = System.Enum.GetNames(typeof(Surnames)).Length;
            int rand = Random.Range(0, numNames);
            surname = System.Enum.GetName(typeof(Surnames), rand);            
        }
        pregnant = new Pregnancy();
        CanBePregnant();
        AssignPartner();


        // set the name of the game object to be the person name
        SetName();

        myMat = GetComponentInChildren<Renderer>().material;
    }

    private string GetWholeName()
    {
        return firstName + " " + surname;
    }

    private void SetName()
    {
        gameObject.name = GetWholeName();
        nameUI.text = firstName[0].ToString() + " \n" + surname[0].ToString();
    }


      
    
    private void CanBePregnant()
    {
        if( partner == null)
        {
            pregnant.canBePregnant = false;
            return;
        }

        if (currentChild !=null)
        {
            if (currentChild.age.GetAgeType() != ageType.infant)
            {
                currentChild = null;                
            }
            else
            {
                pregnant.canBePregnant = false;
            }
        }else if (sex == femaleS&& age.GetAgeType() >=ageType.adult) // if the person is female and is old enough then they can Be Pregnant
        {
           // Debug.Log("1");
           // Debug.Log(Child);
            //Debug.Log(Child!=null);
            pregnant.canBePregnant = true;
        }
    }
    
    /// <summary>
    /// for adding alert when enabled
    /// </summary>
    void OnEnable()
    {
        //TheLand.NewDay += StartHumanDay;
        TheLand.EndDay += EndHumanDay;
    }
    
    /// <summary>
    /// for removing alters when disabled
    /// </summary>
    void OnDisable()
    {
       // TheLand.NewDay -= StartHumanDay;
        TheLand.EndDay -= EndHumanDay;
    }
    
    private void GenerateSex()
    {
        int randCheck =500;
        bool? holdSex = communityObj.GetMajoritySex();
        if (holdSex == true)
        {
            //Debug.Log("34");
            randCheck = 800;
        }else if (holdSex == false)
        {
            /*Debug.Log("33");*/
            randCheck = 200;
        }
        // generate the person to be male or female
        float holdRand = Random.Range(0, 1000);
        
        if (holdRand < randCheck)
        {
            sex = femaleS;
            GenerateFemale();
        }
        else
        {
            sex = maleS;
            GenerateMale();
        }


    }

    /// <summary>
    /// generate females name
    /// </summary>
    private void GenerateFemale()
    {
        int numNames = System.Enum.GetNames(typeof(NamesFemale)).Length;
        int rand = Random.Range(0, numNames);
        firstName = System.Enum.GetName(typeof(NamesFemale), rand);
       
    }
  
    /// <summary>
    /// generate males name
    /// </summary>
    private void GenerateMale()
    {
        int numNames = System.Enum.GetNames(typeof(NamesMale)).Length;
        int rand = Random.Range(0, numNames);
        firstName = System.Enum.GetName(typeof(NamesMale), rand);
       

    }
   

    #endregion

    // Update is called once per frame
    private void Update()
    {   
        if (dead)
        {
            if (!deadFinished)
            {
                HandleDeath();
                return;
            }
        }       


        //need to make villager eat 3 food at end of day 
        
        // also make villager go to job

        // and the villager has to sleep

    }

    /// <summary>
    /// used to do everything for eating food for now just checks if the villager is hungry and gives more food
    /// </summary>
    private void EatFood()
    {
        //feeds the villager if there is food available in the community 

        //for now just eat 3 food at the start of day later on do this a set times 
        int foodNeeded = 3;

        if((int)age.GetAgeType() <  2)
        {

            int hold = communityObj.FoodAvailable(food.IsHungry());
            if (hold > 0)
            {
                food.EatMeal(hold);
                food.EatMeal(1); // child gets 2 food for 1 meal
            }
            else
            {
                food.EatMeal(hold);
                food.EatMeal(communityObj.FoodAvailable(food.IsHungry())); 

            }
            food.EatMeal(communityObj.FoodAvailable(food.IsHungry()));
        }
        else
        {
            for (int i = 0; i < foodNeeded; i++)
            {
                food.EatMeal(communityObj.FoodAvailable(food.IsHungry()));

            }

        }



        

       
    }

    public bool CheckForPartner()
    {
        if (partner)
        {
            return true;

        }
        return false;
    }


    public void UseFire()
    {
        tempera.FireOn();
    }

    /// <summary>
    /// used to do stuff for drinking water
    /// </summary>
    private void DrinkWater()
    {
        //if there is water avalible drink 
        if (communityObj.WaterAvailable())
        {
            water.DrinkWater();
        }
        
    }

    /// <summary>
    /// assign a partner to the human and mark surname accordingly 
    /// </summary>
    private void AssignPartner()
    {

        if (age.GetAgeType() < ageType.adult)
        {
            return;
        }

        Human hold  = communityObj.GetFreeOtherSex(sex);

        if (hold == null)
        {
            //no partner ready
            return;
        }
        else
        {
            partner = hold;
        }   
        // female gets male partner surname for now 
        if (sex == femaleS)
        {
            surname = partner.surname;

        }
        else
        {
            partner.surname = surname;
        }
        partner.AssignPartner(this);
        SetName();
    }

    public void AssignPartner(Human newPartner)
    {
        partner = this;
        SetName();
    }

    /// <summary>
    /// used for outside fucntions t ocheckif a vilalger has  job
    /// </summary>
    /// <returns>IF they have a job or not</returns>
    public bool VillagerHasJob()
    {
        if (currentTask != null)
        {
            return true;
        }
        return false;

    }


    /// <summary>
    /// set the job of the person on a new day
    /// </summary>
    /// <param name="a">The task to set the person</param>
    public void SetJob(Task a)
    {
        if (currentTask != null)
        {
            Debug.LogError(GetWholeName()); 
            Debug.Log("villager has a job already"); // there is no reason for this to happen but if it does might need to return true or false if the job can be assigned
            return;
        }
        else
        {
            // assign the person to do the job
            currentTask = a;
            //Debug.Log(firstName);
            // if the job is food or fuel move them to the corrent node 
            if (currentTask.payoff == JobPurpose.food)
            {
                communityObj.SetHumanLocation(PossLocations.foodNode,this);
            }
            else if (currentTask.payoff == JobPurpose.fuel)
            {
                communityObj.SetHumanLocation(PossLocations.fuelNode, this);
            }


        }




    }

    /// <summary>
    ///Handle death of the human
    /// </summary>
    private void HandleDeath()
    {
        gameObject.SetActive(false);
        deadFinished = true;
        communityObj.ImDead(this);
        Destroy(gameObject);
    }


    /// <summary>
    /// handles everything that happens when a new day starts
    /// </summary>
    public void StartHumanDay()
    {
        //start by doing the end of the day before things e.g. add result of jobs ect later on this may happen as a seperate event
        
        if (dead)
        {
            return;
        }

        //increase the age of the person by one day
        age.NewDay();
        myMat.color = age.CheckAgeColour();
        AssignPartner();



        //let the hunger handler know that it has been a new day
        food.NewDay();

        //let the thirst  handler know that it has been a new day
        water.NewDay();

        //let the Temperature handler know that it has been a new day
        tempera.NewDay();

        // check if the human Can Be Pregnant
        CanBePregnant();

        //let The pregnancy's holder know its a new day and check for new birth
        if (pregnant.NewDay())
        {
            currentChild = communityObj.CreateBabyVillager(this);
            children.Add(currentChild);
            pregnant.GiveBirth();
            Debug.Log(age.GetAgeType() + "Giving Birth ");
        }
        

        EatFood();

        DrinkWater();
        newDay = false;
    }

    /// <summary>
    /// Handles the end of a day for a human
    /// </summary>
    /// <returns>if false the human is dead</returns>
    private void EndHumanDay()
    {
        // first check to see if the human will die from lack of food
        if (dead || food.CheckHungerDeath() || water.CheckThirstDeath()|| age.CheckAgeDeath()||tempera.CheckColdDeath())
        {
            
            dead = true;
            return;
        }

        //then add the recourse generated from any job and clear the job
        //currentJob = new Sleeping();
        if (currentTask!=null)
        {
            //if the human has the sleeping job remove some tiredness from them 
            if (currentTask.GetType() == typeof(Sleeping))
            {
                // get the tiredness stat and remove the amount = to the task amount 


            }
            else
            {
                communityObj.TaskDone(currentTask);

            }
            currentTask = null;
        }
        newDay = true;
    }


   



}

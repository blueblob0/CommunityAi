using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Community : MonoBehaviour
{
    #region Events
    public delegate void StartWorkAction(); // start work at 6 hours
    public static event StartWorkAction StartWork;

    public delegate void GoHomeAction();// go home at 14 hours
    public static event GoHomeAction GoHome;

    public delegate void SleepAction(); // sleep at 20 hours
    public static event SleepAction Sleep;

    public delegate void EndDayAction();
    public static event EndDayAction EndDay;

    #endregion

    TaskSystem task;

    #region timekeeping
    private float startTime;
   
    private int dayCount;

    public float dayLengthSecs = 0.001f; 
    private float hourTime;   
    private int dayLength = 24;
    private int workTime = 6;
    private int homeTime = 14;
    private int sleepTime = 20; 
    #endregion

   
    private bool first = false;
        
    public bool timeRun = true; //If the program can run   
    public bool showTimeText = true; //If you should show the time text
    public Text timeRecord = null; // the timetoShow
    public int maxHumans = 33;   
    public string humanPrefabName = "Human SomeAi"; // The Names of the prefab to be loaded for the people
    public string shelterPrefabName = "Normal House"; // The Names of the prefab to be loaded for houses
    
    //public Node foodNode;
    public int numberShelters = 5;
    //public List<Resourse> resourseType =new List<Resourse>();
    public Resource[] allResources;
    public int resourseSize;
    //used for call to work, go home or sleep make sure its only done once
    private bool hasWork = false;
    private bool hasHome = false;
    private bool hasSleep = false;
    public bool designateShelters = true;
    public bool spawnNewShelters = false;
    public const string fileName = "/humandata.data";
   // private const int foodPersonNeeds = 3;
    //private const int waterPersonNeeds = 1;
   // private const int fuelShelterNeeds = 1;   
      
    private List<Human> humans = new List<Human>();
    public List<Shelter> shelters = new List<Shelter>();
    // private int food = 0; // each villager needs 3 food per a day, if food is not cooked it only counts as 1/2 a food, 3 chickens give 1 food per day, other animals indifferent amounts but also use food 
   // public int fuel = 0; //Each shelter needs 1 wood per day to keep fire going, one tree gives 2-3 wood
    //private int water = 0;  // the amount of water irrelevant if the village has a well, each person needs 1 water per day, gathering produces 5 water
       
    //private WaterSources waterSource;
    private float maxScreenWidth; // represents the max width of the camera for placing houses
    
        
    void Start()
    {
        task = new TaskSystem(this);
        hourTime = dayLengthSecs / dayLength;        
        Application.runInBackground = true; //not part of game logic  if multiple of this change to only be called once 
        startTime = Time.time;
        dayCount = 0;
        setTimeUI();
        first = true;

        if (!designateShelters)
        {
            //Start by making houses 
            GenerateShelters(numberShelters); // make shelter (haveing 5 to start with)  
        }
        else
        {
            SetShelters();
        }

        //Then make people
        GenerateVillagers();
        //Then the starting Resourses (the idea is its a exsisting village not a new one with nothing)
        

        foreach (Resource re in allResources)
        {
            re.numOf = Random.Range(re.startMin, re.startMax);
        }
        
        

    }

    // Update is called once per frame
    
    void Update()
    {
        setTimeUI();
        if (first)
        {
            StartNewDay();
            first = false;
        }
        if (timeRun)
        {
            if (!hasWork && startTime + hourTime * workTime <= Time.time)
            {

                StartWork();
                hasWork = true;
                //Debug.Log("work");
            }
            else if (!hasHome && startTime + hourTime * homeTime <= Time.time)
            {

                GoHome();
                hasHome = true;
                //Debug.Log("GoHome");
            }
            else if (!hasSleep && startTime + hourTime * sleepTime <= Time.time)
            {

                Sleep();
                hasSleep = true;
                //Debug.Log("Sleep");
            }
            else if (startTime + dayLengthSecs <= Time.time)
            {

                StartEndDay();

                startTime = Time.time;
                dayCount++;
                setTimeUI();
                StartNewDay();
                hasWork = false;
                hasHome = false;
                hasSleep = false;

                
            }
        }



    }  
   
       
   
    void GenerateVillagers()
    {        
        //This is used to record the year of the couple each one will be incrasingly older than the other one 
        int holdYear = Age.adultAge;
        int holdChildYear = 0;
        int holdTeenYear = Age.teenAge;
        //Start Creating a couple for each shelter
        for (int i = 0; i < shelters.Count; i++)
        {
            // use the for the gap between last couple and this one 
            int yearup = Random.Range(0, 100);
            if (yearup < 5)
            {
                holdYear += 1;
            }
            else if (yearup < 20)
            {
                holdYear += 2;
            }
            else if (yearup < 70)
            {
                holdYear += 3;
            }
            else if (yearup < 95)
            {
                holdYear += 4;
            }
            else if (yearup < 100)
            {
                holdYear += 5;
            }

            int holda = holdYear; // for recording                                 
            Human male = CreateNewVillager(null, Human.maleS, holdYear); //generate male    
              
            shelters[i].PlacePersonHouse(male);           
            if (humans.Count >= maxHumans)
            {
                Debug.Log("max villagers reached");
                return;
            }
            
            // use this to work out age diffrnece between partners
            yearup = Random.Range(0, 100);
            if (yearup < 15)
            {
                holdYear -= 2;
            }
            else if (yearup < 40)
            {
                holdYear -= 1;
            }
            else if (yearup < 60)
            {
                holdYear += 2;
            }
            else if (yearup < 85)
            {
                holdYear += 1;
            }
            else if (yearup < 100)
            {
                holdYear += 2;
            }
            //generate Female
            Human female = CreateNewVillager(male, Human.femaleS, holdYear);          
            shelters[i].PlacePersonHouse(female);
            // get the avg age of partners for the next partern top check from 
            holdYear = Mathf.FloorToInt((holdYear + holda) / 2);

            if (humans.Count >= maxHumans)
            {
                Debug.Log("max villagers reached");
                return;
            }


            //check that they are a teenager and that the adult is oldenough to have them as a child
            if (holdTeenYear < Age.adultAge && holdTeenYear < female.age.ageYear - Age.adultAge)
            {
                Human aTeen = CreateNewVillager(female, holdTeenYear);
                shelters[i].PlacePersonHouse(aTeen);

                if (humans.Count >= maxHumans)
                {
                    Debug.Log("max villagers reached");
                    return;
                }

                // generate the age for the next teenager
                yearup = Random.Range(0, 100);
                if (yearup < 10)
                {
                    holdTeenYear += 0;
                }
                else if (yearup < 70)
                {
                    holdTeenYear += 1;
                }
                else if (yearup < 100)
                {
                    holdTeenYear += 2;
                }

            }


            //check that they are a child and that the adult is oldenough to have them as a child
            if (holdChildYear < Age.teenAge && holdChildYear < female.age.ageYear - Age.adultAge)
            {      
                Human achild = CreateNewVillager(female, holdChildYear);               
                shelters[i].PlacePersonHouse(achild);
                female.SetStartChild(achild);
                if (humans.Count >= maxHumans)
                {
                    Debug.Log("max villagers reached");
                    return;
                }
                // generate the age for the next child
                yearup = Random.Range(0, 100);
                if (yearup < 10)
                {
                    holdChildYear += 0;
                }
                else if (yearup < 30)
                {
                    holdChildYear += 1;
                }
                else if (yearup < 60)
                {
                    holdChildYear += 2;
                }
                else if (yearup < 100)
                {
                    holdChildYear += 3;
                }
            }
            
        }
    }
    
       
    #region Generate  starting resourses 

  
    void GenerateShelters(int number)
    {
        shelters.Clear(); // make sure the list is empty before starting
        maxScreenWidth = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize * 2; // this is run at the start so get the camera width for showing the shelters

        //Alwase need one shelter so start by making that 
        AddShelter();        
       
        
               
        //generate houses
        while (shelters.Count < number)
        {
            AddShelter();
        }

    }

    /// <summary>
    /// spread the villagers out by house ( for now this is random by will assign people to houses perminatly later)
    /// </summary>
    
    void PlacePeopleHouses()
    {
        foreach (Human v in humans)
        {

           for(int i = 0; i < shelters.Count; i++)
            {
                if(shelters[i].shelterID == v.shelterNum)
                {
                    shelters[i].PlacePersonHouse(v);
                }
            }
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
    /*
    void GenerateFood()
    {
        //food = Random.Range(villagers * 3*7, villagers*3*180);
        food = 30 * humans.Count; // start with food for 10 days 
    }
  */
    
    
       
    
    /// <summary>
    /// creating a villager for now just makes them but later may do more
    /// </summary>
    Human CreateNewVillager(Human mum, int year)
    {
        //create villager game objects and add them to the list
        GameObject a = Instantiate(Resources.Load(humanPrefabName)) as GameObject;

        Human hum = a.GetComponent<Human>();
        hum.StartHuman(Random.Range(0, 365), year, mum);
        humans.Add(hum);

        SetHumanRequirements(hum);

        return hum;
    }

    /// <summary>
    /// creating a villager for now just makes them but later may do more
    /// </summary>
    Human CreateNewVillager(Human partner,string sex, int year)
    {
        //create villager game objects and add them to the list
        GameObject a = Instantiate(Resources.Load(humanPrefabName)) as GameObject;

        Human hum = a.GetComponent<Human>();
        hum.StartHuman(Random.Range(0, 365),year, sex, partner);
        SetHumanRequirements(hum);
        humans.Add(hum);
        return hum;
    }


    void SetHumanRequirements(Human hum)
    {
        foreach (Resource r in allResources)
        {
            if (r.user == ResourceUser.Human)
            {
                hum.requirements.Add(r.requirementType);
            }
        }

    }
    #endregion

    #region Add things to village

    /// <summary>
    ///  creating a villager for now just makes them but later may do more
    /// </summary>
    /// <param name="mum">The baby's mum</param>
    /// <returns></returns>
    public Human CreateBabyVillager(Human mum)
    {


      

        //create villager game objects and add them to the list
        GameObject a = Instantiate(Resources.Load(humanPrefabName)) as GameObject;
        Human ha = a.GetComponent<Human>();
        ha.StartHuman(0,0,mum);
        humans.Add(ha);
        SetHumanRequirements(ha);
        HousePerson(ha, mum);
        return ha;
        //Debug.Log("baby made");
    }

    /// <summary>
    /// used to check the majority sex  true then more males
    /// </summary>
    /// <returns>true then more males </returns>
    public int GetMajoritySex()
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
        return (maleC - femaleC);
    }

    /// <summary>
    /// try and put a person in the same house as their mum
    /// </summary>
    private void HousePerson(Human person,Human mum)
    {
        bool check = false;
        RemovePregShelter(mum.shelterNum);
        for (int i =0;i<shelters.Count;i++)
        {
            if(shelters[i].shelterID == mum.shelterNum)
            {
                
                if (!shelters[i].CheckShelterFull())
                {
                    shelters[i].PlacePersonHouse(person);
                    person.shelterNum = shelters[i].shelterID;
                    return;
                }
            }
        }
        Debug.Log("No house mum");
        int hold = -1;

        for (int i = 0; i < shelters.Count; i++)
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
            person.shelterNum = shelters[hold].shelterID;
        }
        else
           
        if (AllSheltersFull())
        {
            Debug.Log("How");
            AddShelter();
        }
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
        if (shelters.Count > 0)
        {
            AddShelter(GetShelterId()); // need to make sure when a shelter is destoryed you remove id ref from villgers 
            //temp.shelterID = shelters.Count; // this may create a bug later where 2 shelters have the same id so may need to look at this ( if one is destroyed and then another created)
        }
        else
        {
            AddShelter(0);
        }
    }

    public void MovePartnerTogether(Human humFemale, Human humMale)
    {

        foreach(Shelter s in shelters)
        {
            if (s.CheckShelterEmpty())
            {
                if (humFemale.pregnant.IsPregnant())
                {
                    RemovePregShelter(humFemale.shelterNum);
                    AddPregToShelter(s.shelterID);
                }


                RemoveFromShelter(humFemale.shelterNum, humFemale);
                RemoveFromShelter(humMale.shelterNum, humMale);
                AddToShelter(s.shelterID, humFemale);
                AddToShelter(s.shelterID, humMale);
                return;
            }

        }


        if (!MoveShelter(humFemale, humMale.shelterNum))
        {
            // if he cant go to her shelter then try to put her in his
            if (!MoveShelter(humMale, humFemale.shelterNum))               
            {
                //if they cant be put in either shelter
                AddToShelter(humFemale, humMale);
            }
        }
        
    }
    
    private void AddToShelter(Human humz,Human humx)
    {
        bool placed = false;
        foreach(Shelter s in shelters)
        {
            if (s.CheckPartnerSpots() && !placed)
            {
                placed = true;
                RemoveFromShelter(humz.shelterNum, humz);
                RemoveFromShelter(humx.shelterNum, humx);
                AddToShelter(s.shelterID, humz);
                AddToShelter(s.shelterID, humx);
            }
        }

        if (spawnNewShelters&&!placed)
        {
            int holdID = GetShelterId();
            AddShelter(holdID); // need to make sure when a shelter is destoryed you remove id ref from villgers 
                                //temp.shelterID = shelters.Count; // this may create a bug later where 2 shelters have the same id so may need to look at this ( if one is destroyed and then another created)
            RemoveFromShelter(humz.shelterNum, humz);
            RemoveFromShelter(humx.shelterNum, humx);
            AddToShelter(holdID, humz);
            AddToShelter(holdID, humx);
            //AddToShelter(humx.shelterNum, humx);
            // AddToShelter(humz.shelterNum, humz);
            //AddToShelter(humx.shelterNum, humx);
        }



    }

    public void AddShelter(int id)
    {
        //start by geenrasting an instance of the shelter
        GameObject holdShelter = Instantiate(Resources.Load(shelterPrefabName)) as GameObject;

        Shelter temp = holdShelter.GetComponent<Shelter>();

        temp.shelterID = id;

        SetShelterRequirements(temp);

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

    public void SetShelters()
    {

        foreach(Shelter s in shelters)
        {
            int id = GetShelterId();

            s.shelterID = id;

            SetShelterRequirements(s);
        }
    }

    void SetShelterRequirements(Shelter shelt)
    {
        foreach (Resource r in allResources)
        {
            if (r.user == ResourceUser.Shelter)
            {
                shelt.requirements.Add(r.requirementType);
            }
        }

    }

    /// <summary>
    /// Used to Get a free Id For the shelter
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Check that the given id is free
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
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
    /// Move human from orginal shelter to another a shelter by an id
    /// </summary>
    /// <param name="personTomove"></param>
    /// <param name="ShelterID"></param>
    /// <returns></returns>
    public bool MoveShelter(Human personTomove,int ShelterID)
    {
        // if they already there say they moved 
        if (personTomove.shelterNum == ShelterID)
        {
            return true;
        }
        // try removing them from the shelter and adding to the new one
        RemoveFromShelter(personTomove.shelterNum, personTomove);       
           
        if (AddToShelter(ShelterID, personTomove))
        {
            return true;
        }
        else
        {
            // adding back to old one if removed buy cant add to new
            AddToShelter(personTomove.shelterNum, personTomove);
            return false;
        }



        //return false;


    }
       

    /// <summary>
    /// Adds a person to there shelter by its id 
    /// </summary>
    /// <param name="Id">ID of the shelter </param>
    /// <param name="hum"> person to add</param>
    public bool AddToShelter(int Id, Human hum)
    {
       return GetShelterID(Id).PlacePersonHouse(hum);                    
    }

    public void AddPregToShelter(int Id)
    {
        GetShelterID(Id).pregant++;
    }

    public void RemovePregShelter(int Id)
    {
        GetShelterID(Id).pregant--;
    }

    public bool ShelterFull(int Id)
    {

        //Debug.Log(Id);
        return GetShelterID(Id).CheckShelterFull();


    }
    
    /// <summary>
    /// removes a person from shelter by its id 
    /// </summary>
    /// <param name="Id">ID of the shelter</param>
    /// <param name="hum">person to remove</param>
    bool RemoveFromShelter(int Id, Human hum)
    {
        return GetShelterID(Id).RemovePersonHouse(hum);
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



   
    

    /// <summary>
    /// Add the relevent resoures when a task has been compelted 
    /// </summary>
    /// <param name="finishedTask"> the task thast has been finished </param>
    public void TaskDone(Resource finishedTask,Human hum)
    {

        if (finishedTask == null)
        {
            Debug.Log("no task");
            return;
        }
        finishedTask.node.RemovePersonNode(hum.gameObject);

        foreach (Resource re in allResources)
        {
            if (re == finishedTask)
            {
                re.numOf += finishedTask.node.production;
            }
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
    /// Try and use the resourse and return how mnya more resoures are needed
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public int ResourceAvailable(Requirement req)
    {
        int needed = req.GetRequired();
        if (needed < 1)
        {
           // Debug.Log(" 13 ");
            return 0;
        }

        for(int i = 0; i < req.GetRequired(); i++)
        {
            if ( allResources[req.resourcePos].numOf<1)
            {
               // Debug.Log(" 12 ");
                return needed;
            }
            if (i <= req.requiredDay)
            {
                allResources[req.resourcePos].numOf--;
                //Debug.Log(req.resourcePos + " " + req.requiredDay);
                needed--;
            }
            else
            {
                if ((allResources[req.resourcePos].user == ResourceUser.Shelter && allResources[req.resourcePos].numOf > shelters.Count * req.requiredDay)|| 
                    (allResources[req.resourcePos].user == ResourceUser.Human && allResources[req.resourcePos].numOf > shelters.Count * req.requiredDay))
                {
                    allResources[req.resourcePos].numOf--;
                    
                    needed--;
                }
                else
                {
                   // Debug.Log(" 11 ");
                    return needed;
                }
            }
        }
       // Debug.Log(" 10 ");
        return needed;
       
    }
    

    public int ResourceToSurivive(Resource res)
    {

        int numUser = 0;
        if (res.user == ResourceUser.Human)
        {
            numUser = humans.Count;
        }
        else
        {
            numUser = shelters.Count;
        }

        int totalNeeded = res.requirementType.requiredDay * numUser;
        //Debug.Log(res.requirementType.requiredDay);
       // Debug.Log(numUser);
        //Debug.Log(totalNeeded);
        //Debug.Log(humans.Count);
        return NeedXToSurvive(res.numOf, totalNeeded);


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
    

    /// <summary>
    /// used to check how much Resource will be used by people that day
    /// </summary>
    /// <returns></returns>
    public int ResourceUsed(Resource res)
    {
        int numUser = 0;
        if (res.user == ResourceUser.Human)
        {
            numUser = humans.Count;
        }
        else
        {
            numUser = shelters.Count;
        }

        return res.requirementType.requiredDay * numUser;        
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

        if (!designateShelters)
        {
            for (int i = 0; i < shelt.Length; i++)
            {
                //any shelters that need to be removed have to have people in them killed and then remove the shetler 
                if (shelt[i] != null)
                {
                    KillShelterID(shelt[i].shelterID); // current just kill all humans but later by rehome dependind on cerumstance
                    Shelter holdShel = shelt[i];
                    shelters.Remove(holdShel); // remove and destroy the shelter
                    Destroy(holdShel.gameObject);
                }
            }
        }



      
    }


    public Human GetFreeOtherSex(string sex)
    {
        Human thePartner = null;
        foreach (Human h in humans)
        {
            if(h.sex != sex &&!h.foundPartner && h.CheckForPartner() == false&&h.age.GetAgeType()>= AgeType.adult)
            {
                return h;
            }
        }
        return thePartner;

    }

    public bool SurnameUsed(string theName)
    {
        bool used = false;
        foreach (Human h in humans)
        {
            if (h.surname == theName)
            {
                used = true;
                return used;
            }
        }
        return used;

    }
    
    #endregion


    public bool SetHumanLocation(Node theNode, Human theHuman)
    {
        bool check = false;
        //if(theNode.GetType() == typeof(Shelter))
        //{            
        //    AddToShelter(theHuman.shelterNum, theHuman);
        //    Debug.Log("1");
        //}
        //else  
        // {

       // Debug.Log(theNode);
       // Debug.Log(theHuman.gameObject);
        theNode.PlacePersonNode(theHuman.gameObject);
            check = true;
        //}        

        if (check)
        {
            RemoveFromShelter(theHuman.shelterNum, theHuman); // remove it from thee sehlter as we are adding it to the new place 
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
    /// For handling the end of the day making sure people go back to home
    /// </summary>
    void EndOfDay()
    {
        
        //RemoveNullShelter();
        for ( int i =0;i< shelters.Count;i++)
        {
            shelters[i].ClearPeople();           
        }

        // make sure to clear peple from jobs at end of the day people will move themselves from job when done in future
        allResources[0].node.ClearPeople();        

        foreach (Human h in humans)
        {           
            AddToShelter(h.shelterNum, h);  
        }      
        
     if(humans.Count == 0)
        {
            Debug.Log("All hUmans are dead");

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

   
    //TEMPEND
    //TEMP
    void setTimeUI()
    {
        if (!showTimeText)
        {
            return;
        }
        float holdTime = dayCount / 365.0f;
        int holdYears = Mathf.FloorToInt(holdTime);
        int holdDays = Mathf.FloorToInt((holdTime - holdYears) * 365);
        int holdHours = Mathf.FloorToInt((Time.time - startTime) / hourTime);
        string year0 = "";
        string day0 = "";
        string hour0 = "";

        if (holdYears < 10)
        {
            year0 = "00";
        }
        else if (holdYears < 100)
        {
            year0 = "0";
        }

        if (holdDays < 10)
        {
            day0 = "00";
        }
        else if (holdDays < 100)
        {
            day0 = "0";
        }

        if (holdHours < 10)
        {
            hour0 = "000";
        }
        else if (holdHours < 100)
        {
            hour0 = "00";
        }
        else if (holdHours < 1000)
        {
            hour0 = "0";
        }

        timeRecord.text = "Year:" + year0 + Mathf.Floor(holdTime) + ", Day:" + day0 + holdDays + ", Hours:" + hour0 + holdHours;
    }
    //TEMPEND

     
        //For The start of a day 00:00 hours
    private void StartNewDay()
    {
        StartShelterDay();
        task.StartOfDay();
    }
    
    //for the end of a day 23:59
    private void StartEndDay()
    {
        EndDay();
        EndOfDay();
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

public enum AgeType
{
    infant,
    teen,
    adult,
    senior
}


/*
    #region Generate Set starting resourses 

    /// <summary>
    /// Generate  shelters from prevous data
    /// </summary>
    void GenerateShelter(List<HumanHolder> currentHumans)
    {
        // work out how mnay shelters to create from the current amount
        List<int> shelterNums = new List<int>();
        foreach(HumanHolder h in currentHumans)
        {
            if (!shelterNums.Contains(h.shelterNum))
            {
                shelterNums.Add(h.shelterNum);
            }
        }
        shelters.Clear(); // make sure the list is empty before starting
        maxScreenWidth = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize * 2; // this is run at the start so get the camera width for showing the shelters
           

        //generate houses
        foreach (int s in shelterNums)
        {
            AddShelter(s);
        }
        
        

    }

    /// <summary>
    /// genrate the old villagers to use for testing
    /// </summary>
    void CreateNewVillager(HumanHolder holder)
    {
        //create villager game objects and add them to the list
        GameObject a = Instantiate(Resources.Load(humanPrefabName)) as GameObject;

        Human hum = a.GetComponent<Human>();

        hum.StartHuman(holder.food, holder.water, holder.age, holder.tempera, holder.pregnant,
            holder.mumFirsName, holder.mumsurName, holder.dadFirsName, holder.dadsurName, holder.myfirstName,
            holder.mysurname, holder.sex, holder.partnersName, holder.shelterNum);
        //Debug.Log(hum);
        humans.Add(hum);
        //Debug.Log(hum);
    }

    #endregion
    */

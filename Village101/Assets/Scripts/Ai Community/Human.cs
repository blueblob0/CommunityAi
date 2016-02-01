using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class Human : MonoBehaviour
{
    #region Outside Varables    

    public Text nameUI;
    public GameObject maleBody;
    public GameObject femaleBody;
    public GameObject pregIcon;
    // public GameObject woodcuttingIcon;
    // public GameObject farmingIcon;
    public GameObject sleepIcon;
    Community communityObj;

    #endregion


    #region Internal Varables

    #region Human Vitals


    //public int ageYear =0;
    // public int ageDay=0;
    // public ageType theAgeType = ageType.adult;


    public bool dead = false;

    // public Hunger food;
    //public Thirst water;
    // public Temperature tempera;
    public Pregnancy pregnant;
    public Age age;
    public List<Requirement> requirements = new List<Requirement>();


    //public Task currentTask = null;

    private Resource currentResource = null;
    public string iconName;

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
    public Human dad;
    public Human mum;
    public Human partner;
    public string firstPartnerString;
    public bool foundPartner = false;
    public bool parents;
    public List<Human> children = new List<Human>();
    #endregion

    #region Programming Info
    public bool deadFinished = false;
    #endregion


    #endregion

    public const string femaleS = "female";
    public const string maleS = "male";


    #region Create Human

    /// <summary>
    /// for adding alert when enabled
    /// </summary>
    void OnEnable()
    {
        //TheLand.NewDay += StartHumanDay;
        Community.StartWork += GoToJob;
        Community.GoHome += GoHome;
        Community.Sleep += Sleep;
        Community.EndDay += EndHumanDay;
    }

    /// <summary>
    /// for removing alters when disabled
    /// </summary>
    void OnDisable()
    {
        // TheLand.NewDay -= StartHumanDay;
        Community.StartWork -= GoToJob;
        Community.GoHome -= GoHome;
        Community.Sleep -= Sleep;
        Community.EndDay -= EndHumanDay;
    }

    
    // Creates the human with a age (has to be called to make starting age easier
    public void StartHuman(int day, int year, Human theMum)
    {
        InitalStart(day, year);     
        GenerateSex();
        partner = null; // mark no partner at start;
        foundPartner = false;
        // if the child has a mum
        if (theMum)
        {
            surname = theMum.surname;
            mum = theMum;
            dad = theMum.partner;
            parents = true;
        }
        else // otherwise create a new surname for them
        {
            mum = null;
            dad = null;

            SetSurname();
            parents = false;
        }
        AssignPartner();
        LastStart();
    }

    // Creates the human with a age (has to be called to make starting age easier
    public void StartHuman(int day, int year, string setSex, Human setPartner)
    {
        InitalStart(day, year);


        sex = setSex;
        if (sex == maleS)
        {
            GenerateMale();
        }
        else
        {
            GenerateFemale();
        }

        if (setPartner)
        {
            partner = setPartner;
            foundPartner = true;
            setPartner.partner = this;
            setPartner.foundPartner = true;
            surname = setPartner.surname;
            setPartner.firstPartnerString = GetWholeName();
            firstPartnerString = setPartner.GetWholeName();
        }
        else
        {
            foundPartner = false;
            partner = null; // mark no partner at start;
            SetSurname();
        }
        // if the child has a mum
        mum = null;
        dad = null;
        parents = false;

        LastStart();
    }

    private void InitalStart(int day, int year)
    {
        dead = false;
        currentResource = null;
        // get the community reference 
        communityObj = FindObjectOfType<Community>();
        // assign a hunger class to keep track of the humans hunger level
        //food = new Hunger();
        // water = new Thirst();
        age = new Age(day, year);

    }

    private void LastStart()
    {
        pregnant = new Pregnancy();
        //CanBePregnant();

        // set the name of the game object to be the person name
        SetName();

        myMat = GetComponentInChildren<Renderer>().material;

    }

    public void SetStartChild(Human child)
    {
        if (currentChild == null)
        {
            currentChild = child;

        }
        else
        {
            Debug.Log("called wrong");
        }

    }


    void SetSurname()
    {
        bool test = true;

        while (test)
        {
            int numNames = System.Enum.GetNames(typeof(Surnames)).Length;
            int rand = Random.Range(0, numNames);
            surname = System.Enum.GetName(typeof(Surnames), rand);
            test = communityObj.SurnameUsed(surname);

        }



    }

    public string GetWholeName()
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
        // needs a aprtner to be preganat
        if (partner == null)
        {
            pregnant.canBePregnant = false;
            return;
        }

        //needs a empty space in shelter to have child
        if (communityObj.ShelterFull(shelterNum))
        {
            pregnant.canBePregnant = false;
           // Debug.LogError("1");
            return;
        }



        if (currentChild != null)
        {
            if (currentChild.age.GetAgeType() != AgeType.infant)
            {
                currentChild = null;
            }
            else
            {
                pregnant.canBePregnant = false;
                return;
            }
        }

        if (sex == femaleS && age.GetAgeType() >= AgeType.adult && age.GetAgeType() < AgeType.senior) // if the person is female and is old enough then they can Be Pregnant
        {


            pregnant.SetChance(age.GetAgeType());


            pregnant.canBePregnant = true;
        }
    }

    private void GenerateSex()
    {
        int randCheck = 500; // start with the chance to be male or female equal
        int moreMale = communityObj.GetMajoritySex(); // then work out how mnay more male there are (negative for more fmeales)
        randCheck += moreMale * 50; // the wider the gap in numbers of a sex the higer chance for the other sex to be there

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
        femaleBody.SetActive(true);
        maleBody.SetActive(false);

    }

    /// <summary>
    /// generate males name
    /// </summary>
    private void GenerateMale()
    {
        int numNames = System.Enum.GetNames(typeof(NamesMale)).Length;
        int rand = Random.Range(0, numNames);
        firstName = System.Enum.GetName(typeof(NamesMale), rand);
        femaleBody.SetActive(false);
        maleBody.SetActive(true);

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





    public bool CheckForPartner()
    {
        if (partner)
        {
            return true;

        }
        return false;
    }




    /// <summary>
    /// assign a partner to the human and mark surname accordingly 
    /// </summary>
    private void AssignPartner()
    {

        if (age.GetAgeType() < AgeType.adult||foundPartner)
        {
            return;
        }

        Human hold = communityObj.GetFreeOtherSex(sex);

        if (hold == null)
        {
            //no partner ready
            return;
        }
        else
        {

            partner = hold;
        }
        // female gets male partner surname for now but male trys to move to her shelter
        if (sex == femaleS)
        {
            surname = partner.surname;

            communityObj.MovePartnerTogether(this, partner);
        }
        else
        {
            partner.surname = surname;
            communityObj.MovePartnerTogether(partner, this);

        }
        partner.AssignPartner(this);

        SetName();
    }

    public void AssignPartner(Human newPartner)
    {
        partner = newPartner;
        SetName();
    }

    /// <summary>
    /// used for outside fucntions t ocheckif a vilalger has  job
    /// </summary>
    /// <returns>IF they have a job or not</returns>
    public bool VillagerHasJob()
    {
        if (currentResource != null)
        {
            return true;
        }
        return false;

    }


    /// <summary>
    /// set the job of the person on a new day
    /// </summary>
    /// <param name="a">The task to set the person</param>
    public void SetResource(Resource r)
    {

        if (currentResource != null)
        {
            Debug.LogError(GetWholeName() + " villager has a job already " + currentResource.name);
            //Debug.Log(); // there is no reason for this to happen but if it does might need to return true or false if the job can be assigned
            return;
        }
        else
        {
            // assign the person to do the job
            currentResource = r;
            //Debug.Log(currentResource + " "+ currentResource.node);      
            //Debug.Log(firstName);
            // if the job is food or fuel move them to the corrent node 

        }
    }


    /// <summary>
    ///Handle death of the human
    /// </summary>
    private void HandleDeath()
    {
        if (pregnant.IsPregnant())
        {
            communityObj.RemovePregShelter(shelterNum);
        }



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


        if (partner == null)
        {
            AssignPartner();
        }

        //let the hunger and thirst handler know that it has been a new day        
        foreach (Requirement re in requirements)
        {

            re.NewDay();
            re.SetRequired(communityObj.ResourceAvailable(re));
        }

        // check if the human Can Be Pregnant
        CanBePregnant();
       
        bool testpreg = pregnant.IsPregnant();
        //Debug.Log(testpreg);

        //let The pregnancy's holder know its a new day and check for new birth
        if (pregnant.NewDay())
        {
            currentChild = communityObj.CreateBabyVillager(this);
            children.Add(currentChild);
            partner.children.Add(currentChild);
            pregnant.GiveBirth();
            pregIcon.SetActive(false);
             //Debug.Log(age.GetAgeType() + "Giving Birth ");
        }
        else if (pregnant.IsPregnant())
        {
            pregIcon.SetActive(true);

        }        
        //Debug.Log(pregnant.IsPregnant() +" " + testpreg);
        if (testpreg == false && pregnant.IsPregnant())
        {
            communityObj.AddPregToShelter(shelterNum);
            Debug.Log("done");
        }

        newDay = false;
    }

    /// <summary>
    /// Handles the end of a day for a human
    /// </summary>
    /// <returns>if false the human is dead</returns>
    private void EndHumanDay()
    {
        // first check to see if the human will die from lack of food
        if (dead || age.CheckAgeDeath())
        {
            dead = true;
            return;
        }

        foreach (Requirement re in requirements)
        {
            if (re.CheckDeath())
            {
                dead = true;
                return;
            }
        }
        //then add the recourse generated from any job and clear the job
        //currentJob = new Sleeping();

        newDay = true;
    }


    private void GoToJob()
    {
        sleepIcon.SetActive(false);
        if (currentResource != null)
        {
            //Debug.Log("1");
            communityObj.SetHumanLocation(currentResource.node, this);
            //if (nodeToGo.payoff == JobPurpose.food)
            //{
            //Debug.Log(firstName);
            //farmingIcon.SetActive(true);
            //    communityObj.SetHumanLocation(PossLocations.foodNode, this);
            //}


        }


    }

    private void GoHome()
    {
        //Debug.Log(currentResource);
        if (currentResource != null)
        {
            //if the human has the sleeping job remove some tiredness from them 
            //if (currentResource.GetType() == typeof(Sleeping))
           // {
                // get the tiredness stat and remove the amount = to the task amount 
           // }
           // else
            //{
                //mark that job has been done
                communityObj.TaskDone(currentResource, this);
                communityObj.AddToShelter(shelterNum, this);
            //}
            currentResource = null;


            ///THIS NOT WORKING
        }

        // farmingIcon.SetActive(false);
        // woodcuttingIcon.SetActive(false);
    }

    private void Sleep()
    {

        sleepIcon.SetActive(true);


    }



}

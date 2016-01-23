using UnityEngine;
using System.Collections;

public class Farming : Task
{

    public Farming()
    {
        payoff = JobPurpose.food;
        jobProduction = 5; //farming for the day gives 5 food which is nearly enough for 2 people
    }

}

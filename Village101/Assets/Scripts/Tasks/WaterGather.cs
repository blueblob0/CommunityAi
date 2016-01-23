using UnityEngine;
using System.Collections;

public class WaterGather : Task
{
    public WaterGather()
    {
        payoff = JobPurpose.water;
        //jobBaseCoverage = 5;
        jobProduction = 5; // prodices 5 water (which gives 5 people the water they need)
    }

}

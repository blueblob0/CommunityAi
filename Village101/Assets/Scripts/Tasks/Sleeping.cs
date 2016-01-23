using UnityEngine;
using System.Collections;

public class Sleeping : Task
{

    public Sleeping()
    {
        payoff = JobPurpose.sleep;
        //jobBaseCoverage = 5;
        jobProduction = 1; // sleeping for the day removed some sleepyness (maybe 2 nights worth)
    }
}

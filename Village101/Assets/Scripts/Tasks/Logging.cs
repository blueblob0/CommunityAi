using UnityEngine;
using System.Collections;

public class Logging : Task {

    public Logging()
    {
        payoff = JobPurpose.fuel;
        //jobBaseCoverage = 5;
        jobProduction = 4; //one tree gives 2-3 wood (for now just gives 3 wood but later make it random 
    }
}

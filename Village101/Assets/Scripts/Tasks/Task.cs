using UnityEngine;
using System.Collections;

public class Task {

    public JobPurpose payoff;
    //public int jobBaseCoverage; // lets you know how mnay people to job will provide for e.g 3 peoples worth of food or 5 people worth of clothes ect 
    public int jobProduction; // how much is really made 
}


public enum JobPurpose
{    
    food,
    fuel,
    water,
    sleep


}
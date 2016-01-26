using UnityEngine;
using System.Collections;

[System.Serializable]
public class HumanHolder  {

    public Hunger food;
    public Thirst water;
    public Temperature tempera;
    public Pregnancy pregnant;
    public Age age;
    public int shelterNum;
    public string sex;
    public string mumFirsName;
    public string dadFirsName;
    public int numChildren;
    public string myName;


    public HumanHolder( Hunger afood, Thirst awater,Temperature atempera, Pregnancy apregnant, Age aage, int ashelterNum, string asex, string amumFirsName, string adadFirsName, int anumChildren, string amyName)
    {
        food = afood;
        water = awater;
        tempera = atempera;
        pregnant = apregnant;
        age = aage;
        shelterNum = ashelterNum;
        sex = asex;
        mumFirsName = amumFirsName;
        dadFirsName = adadFirsName;
        numChildren = anumChildren;
        myName = amyName;

    }


}

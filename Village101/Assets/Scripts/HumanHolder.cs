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
    public string mumsurName;
    public string dadsurName;
    public int numChildren;
    public string myfirstName;
    public string mysurname;
    public string partnersName;
    /*
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
    */

    public HumanHolder(Hunger afood, Thirst awater, Temperature atempera, Pregnancy apregnant, Age aage, 
        int ashelterNum, string asex, string amumFirsName, string adadFirsName, string amumsurName, string adadsurName, int anumChildren, string amyNamefirst, string amyNamesur, string apartnerName)
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
        mumsurName = amumsurName;
        dadsurName = adadsurName;
        numChildren = anumChildren;
        myfirstName = amyNamefirst;
        mysurname = amyNamesur;
        partnersName = apartnerName;
    }

}

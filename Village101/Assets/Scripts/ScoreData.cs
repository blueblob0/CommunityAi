using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ScoreData  {

     List<HumanHolder> holder = new List<HumanHolder>();
     List<HumanHolder> startData = null;
     int scoreData;


    public void SetStartData(List<HumanHolder> data)
    {
        if (startData == null)
        {
            startData = data;
        }
    }

    public string GetScoreData()
    {

        string s = "End Score: " + scoreData + "Starting with: " + startData.Count.ToString() + "people";
        return s;

    }



    public void addHuman(Human h)
    {
        Human Humanref = h.mum;
        string mumName = null;
        string mumSurname = null;
        string dadname = null;
        string dadSurname = null;
        string parnterName = null;
        if (Humanref != null)
        {
            mumName = Humanref.firstName;
            mumSurname = Humanref.surname;
        }
        Humanref = h.dad;
        if (Humanref != null)
        {
            dadname = Humanref.firstName;
            dadSurname = Humanref.surname;
        }

        Humanref = h.partner;
        if (Humanref != null)
        {
            parnterName = Humanref.firstName;
        }
        HumanHolder aholder = new HumanHolder(h.food, h.water, h.tempera, h.pregnant, h.age, h.shelterNum, h.sex, mumName, dadname, mumSurname, dadSurname, h.children.Count, h.firstName, h.surname, parnterName);
        holder.Add(aholder);

    }

    public void WorkOutScore(int food,int fuel)
    {

        scoreData = food + fuel + holder.Count;

    }



}



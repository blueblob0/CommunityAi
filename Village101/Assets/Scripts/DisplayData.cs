﻿using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;




[CustomEditor(typeof(HumanTestData))]


public class DisplayData : Editor
{
    public override void OnInspectorGUI()
    {

        HumanTestData theData = (HumanTestData)target;
       
        for (int i=0;i< theData.allHumans.Count; i++)
        {
            //EditorGUILayout.TextArea(i.ToString());
            float hold =0;
            for (int c = 0; c < theData.allHumans[i].Count; c++)
            {
                if (theData.allHumans[i][c].age.GetAgeType() == ageType.adult)
                {
                    hold += theData.allHumans[i][c].numChildren;
                }                
                // EditorGUILayout.TextArea(theData.allHumans[i][c].myName + " " + theData.allHumans[i][c].parents);                
            }

            //Debug.Log(theData.allHumans[i].Count);
            if (theData.allHumans[i].Count > 0)
            {
                hold /= theData.allHumans[i].Count;
            }

           
            EditorGUILayout.TextArea(i.ToString() + "averge Chdilren  " + hold.ToString());
        }
    }


}
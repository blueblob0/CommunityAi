using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CustomEditor(typeof(Community))]


public class CommunityEditor : Editor
{

    public override void OnInspectorGUI()
    {
        Community theCommunity = (Community)target;

        theCommunity.timeRun = EditorGUILayout.Toggle("Run Simulation", theCommunity.timeRun);
        if (!theCommunity.timeRun)
        {
            return;
        }


        theCommunity.showTimeText = EditorGUILayout.Toggle("Show Time UI", theCommunity.showTimeText);

        if (theCommunity.showTimeText)
        {
            theCommunity.timeRecord = (Text)EditorGUILayout.ObjectField("TimeTextUI", theCommunity.timeRecord, typeof(Text), true);
        }

        theCommunity.dayLengthSecs = EditorGUILayout.FloatField("Seconds in a day", theCommunity.dayLengthSecs);
        EditorGUILayout.HelpBox("Enter the name of the prefab to be used, make sure the prefab in in a folder called 'Resources' ", MessageType.Info, true);
        theCommunity.humanPrefabName = EditorGUILayout.TextField("Human Preab Name", theCommunity.humanPrefabName);
        theCommunity.shelterPrefabName = EditorGUILayout.TextField("Shelter Preab Name", theCommunity.shelterPrefabName);

        theCommunity.maxHumans = EditorGUILayout.IntField("Max Humans to be made", theCommunity.maxHumans);
        theCommunity.numberShelters = EditorGUILayout.IntField("Number Shelters", theCommunity.numberShelters);

        theCommunity.designateShelters = EditorGUILayout.Toggle("Designate shelters?", theCommunity.designateShelters);




        if (theCommunity.designateShelters)
        {
            theCommunity.spawnNewShelters = EditorGUILayout.Toggle("Allow more shelters to be spawned", theCommunity.spawnNewShelters);

            List<Shelter> sheltHold = theCommunity.shelters;
            for (int i = 0; i < theCommunity.numberShelters; i++)
            {
                if (sheltHold.Count <= i)
                {
                    Shelter SheltTemp = (Shelter)EditorGUILayout.ObjectField("Shelt" + i, null, typeof(Shelter), true);
                    sheltHold.Add(SheltTemp);
                }
                else
                {
                    Shelter SheltTemp = (Shelter)EditorGUILayout.ObjectField("Shelt" + i, sheltHold[i], typeof(Shelter), true);
                    sheltHold[i] = SheltTemp;
                }
            }
            theCommunity.shelters = sheltHold;
        }


        ///theCommunity.fuelNode = (Node)EditorGUILayout.ObjectField("fuel Node", theCommunity.fuelNode, typeof(Node), true);
        //theCommunity.foodNode = (Node)EditorGUILayout.ObjectField("Fuel Node", theCommunity.foodNode, typeof(Node), true);





        theCommunity.resourseSize = EditorGUILayout.IntField("Num of Resources", theCommunity.resourseSize);

        if (theCommunity.allResources == null)
        {
            theCommunity.allResources = new Resource[theCommunity.resourseSize];
        }


        while (theCommunity.allResources.Length != theCommunity.resourseSize)
        {
            Resource[] newArray = new Resource[theCommunity.resourseSize];
            if (theCommunity.allResources.Length < theCommunity.resourseSize)
            {
                for (int x = 0; x < theCommunity.allResources.Length; x++)
                {
                    newArray[x] = theCommunity.allResources[x];
                }
            }
            else
            {
                for (int x = 0; x < theCommunity.resourseSize; x++)
                {
                    newArray[x] = theCommunity.allResources[x];
                }

            }
            for (int i = 0; i < newArray.Length; i++)
            {
                if (newArray[i] == null)
                {
                    newArray[i] = new Resource(i);

                }
            }
            theCommunity.allResources = newArray;
        }


        for (int i = 0; i < theCommunity.allResources.Length; i++)
        {
            EditorGUILayout.BeginVertical("Box");
            Resource holdRes = theCommunity.allResources[i];

            if (holdRes.requirementType == null)
            {
                holdRes.requirementType = new Requirement(i);
            }
            Requirement holdReq = holdRes.requirementType;

            holdRes.showPosition = EditorGUILayout.Foldout(holdRes.showPosition, "Resource " + i);
            if (holdRes.showPosition)
            {
                if (Selection.activeTransform)
                {
                    holdRes.name = EditorGUILayout.TextField("Resource " + i + " Name", holdRes.name);
                    holdReq.requiredDay = EditorGUILayout.IntField("Num used a day", holdReq.requiredDay);
                    holdRes.user = (ResourceUser)EditorGUILayout.EnumPopup("Resource User", holdRes.user);
                    holdRes.node = (Node)EditorGUILayout.ObjectField("Resource Node", holdRes.node, typeof(Node), true);
                    holdRes.startMin = EditorGUILayout.IntField("Min Starting", holdRes.startMin);
                    holdRes.startMax = EditorGUILayout.IntField("Max Starting", holdRes.startMax);
                    EditorGUILayout.LabelField("Num of the Resource", holdRes.numOf.ToString());

                }

                if (!Selection.activeTransform)
                {
                    holdRes.name = EditorGUILayout.TextField("Resource " + i + " Name", holdRes.name);
                    holdRes.showPosition = false;
                }

            }






            EditorGUILayout.EndVertical();
        }

        if (GUI.changed)
            EditorUtility.SetDirty(theCommunity);

    }
}

/*


public int numberShelters = 5;
*/

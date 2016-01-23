using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;



[CustomEditor(typeof(Human))]


public class HumanEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Human theHuman = (Human)target;
        EditorGUILayout.LabelField("Age", theHuman.age.CheckAge());
    }
   

}

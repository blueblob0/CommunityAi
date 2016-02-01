using UnityEngine;
using System.Collections;

[System.Serializable]
public class Resource
{
    public string name ="";
    public ResourceUser user;    
    public Node node;
    public int startMax;
    public int startMin;
    public int numOf;
    public Requirement requirementType;
    public bool showPosition = true;

    public Resource(int pos)
    {
        requirementType = new Requirement(pos);
    }
}

public enum ResourceUser

{
    Human,
    Shelter


}
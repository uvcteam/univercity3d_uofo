using UnityEngine;
using System.Collections;

public class SocialInterest : ScriptableObject
{
    private int id;
    private string interestName;
    private int parent;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Name
    {
        get { return interestName; }
        set { interestName = value; }
    }

    public int Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    public SocialInterest(int catId, string catName, int parent = -1)
    {
        Id = catId;
        Name = catName;
        Parent = parent;
    }
    public SocialInterest(SocialInterest interest)
    {
        id = interest.Id;
        interestName = interest.Name;
        parent = interest.Parent;
    }
}
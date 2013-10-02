using UnityEngine;
using System.Collections;

public class SocialInterest : ScriptableObject
{
    private int id;
    private string interestName;

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

    public SocialInterest(int catId, string catName)
    {
        Id = catId;
        Name = catName;
    }
    public SocialInterest(SocialInterest interest)
    {
        id = interest.Id;
        interestName = interest.Name;
    }
}
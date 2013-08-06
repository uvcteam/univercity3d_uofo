using UnityEngine;
using System.Collections;

public class SocialInterest : ScriptableObject
{
    public int Id;
    public string Name;

    public SocialInterest(int catId, string catName)
    {
        Id = catId;
        Name = catName;
    }
}
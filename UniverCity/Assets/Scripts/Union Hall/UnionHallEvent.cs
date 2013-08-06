using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Collections;

public class UnionHallEvent : MonoBehaviour
{
    public int Id = 0;
    public string Title = "";
    public string Who = "";
    public string Desc = "";
    public string Loc = "";
    public DateTime Start;

    public string Email = "";
    public string Phone = "";
    public int Min = 0;
    public int Max = 0;

    public List<int> interests = new List<int>();

    public UnionHallEvent(int id, string title, string desc, string who, string email, string phone, string loc, DateTime dateTime)
    {
        this.Id = id;
        this.Title = title;
        this.Desc = desc;
        this.Who = who;
        this.Loc = loc;
        this.Phone = phone;
        this.Email = email;
        this.Start = dateTime;
    }

    public string GetEventDateTime()
    {
        return Start.ToString("MMM dd '-' h':'mm tt", CultureInfo.InvariantCulture);
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
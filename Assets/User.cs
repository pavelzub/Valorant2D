using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class User
{
    private string username;
    private int score;

    public User(string username, int score)
    {
        this.username = username;
        this.score = score;
    }
    public override string ToString()
    {
        return UnityEngine.JsonUtility.ToJson(this, true);
    }

}

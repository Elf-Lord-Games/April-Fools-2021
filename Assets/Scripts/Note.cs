using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Note
{
    public bool played = false;
    public float startTime;
    public float length;
    public int type; //0 is keyboard, 1 is corcle
    public int key;
    public float x;
    public float y;

    public Note(float startTime, float length = 1, int type = 0, int key = 0, float x = 0, float y = 0, bool played = false)
    {
        this.played = played;
        this.startTime = startTime;
        this.length = length;
        this.type = type;
        this.key = key;
        this.x = x;
        this.y = y;
    }
}

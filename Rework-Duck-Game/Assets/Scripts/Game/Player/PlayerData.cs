using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public new string name;
    public int maxHP = 10;
    public int currentHP = 8;

    public int maxHunger = 100;
    public int currentHunger = 50;

    [Range(0,2)]
    public float speed = 0.5f;

}

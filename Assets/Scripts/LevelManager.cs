using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Transform player;
    public int score;
    public int levelItems;
    public Transform[] particles;  

    private void Awake()
    {
        instance = this;
    }
}

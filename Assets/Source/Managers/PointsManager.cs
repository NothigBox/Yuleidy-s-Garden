using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    private TMP_Text text;
    private int currentPoints;

    private void Awake()
    {
        currentPoints = 0;
        
        text = GetComponentInChildren<TMP_Text>();
    }

    void Start()
    {
        Ball.OnSumBallPoints += UpdatePoints;
    }

    void UpdatePoints(int points)
    {
        currentPoints += points;
        text.text = currentPoints.ToString("000000");
    }
}

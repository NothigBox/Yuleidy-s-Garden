using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallPoints : MonoBehaviour
{
    private const float OFFSET_Y = 0.75f;
    
    [SerializeField] private float duration; 
    
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }
    
    void Update()
    {
        if (transform.position.y > initialPosition.y - OFFSET_Y)
        {
            transform.position += Vector3.down * ((OFFSET_Y / duration) * Time.deltaTime);
        }
        else Destroy(gameObject);
    }

    public void SetPointsToShow(int points, BallType type)
    {
        if (Math.Abs(points) > 999) points = 999;
        var t = GetComponentInChildren<TMP_Text>();
        t.text = points.ToString();
        t.color = BallUtils.GetBallColorByType(type);
    }
}

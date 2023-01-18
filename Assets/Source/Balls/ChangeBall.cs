using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChangeBall : MonoBehaviour
{
    private Image image;

    public Action<bool> OnBallChanged;

    private void Awake()
    {
        image = GetComponent<Image>();  
    }

    public void DoChangeBall() 
    {
        OnBallChanged(false);
    }

    public void SetBallColor(BallType ballType) 
    {
        image.color = BallUtils.GetBallColorByType(ballType);
    }
}

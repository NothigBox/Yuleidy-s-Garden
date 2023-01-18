using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ball", menuName = "ScriptableObjects/Ball", order = 0)]
public class BallScriptableObject : ScriptableObject
{
    public Sprite foliage;
    public Sprite seed;
    public Sprite pink;

    public Ball ballFoliage;
    public Ball ballSeed;
    public Ball ballPink;

    public Color foliageColor;
    public Color seedColor;
    public Color pinkColor;

    public BallPoints points;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BallUtils
{
    static BallScriptableObject scriptableObject => Resources.Load("ScriptableObjects/Ball") as BallScriptableObject;

    public static BallPoints Points => scriptableObject.points;

    public static bool CanBeAttached(BallType type, BallType to) 
    {
        bool result = default;

        switch (type)
        {
            case BallType.Foliage:
                switch (to)
                {
                    case BallType.Foliage:
                        result = true;
                        break;

                    case BallType.Seed:
                    case BallType.Pink:
                        result = false;
                        break;
                }
                break;

            case BallType.Seed:
                switch (to)
                {
                    case BallType.Foliage:
                    case BallType.Pink:
                        result = true;
                        break;

                    case BallType.Seed:
                        result = false;
                        break;
                }
                break;

            case BallType.Pink:
                switch (to)
                {
                    case BallType.Seed:
                        result = true;
                        break;

                    case BallType.Foliage:
                    case BallType.Pink:
                        result = false;
                        break;
                }
                break;

            default:
                result = false;
                break;
        }

        return result;
    }

    public static Sprite GetBallSpriteByType(BallType type)
    {
        Sprite result = default;

        switch (type) 
        {
            case BallType.Foliage:
                result = scriptableObject.foliage;
                break;
            case BallType.Seed:
                result = scriptableObject.seed;
                break;
            case BallType.Pink:
                result = scriptableObject.pink;
                break;
        }

        return result;
    }

    public static Ball GetBallObjectByType(BallType type)
    {
        Ball result = default;

        switch (type)
        {
            case BallType.Foliage:
                result = scriptableObject.ballFoliage;
                break;
            case BallType.Seed:
                result = scriptableObject.ballSeed;
                break;
            case BallType.Pink:
                result = scriptableObject.ballPink;
                break;
        }

        return result;
    }

    public static Color GetBallColorByType(BallType type)
    {
        Color result = default;

        switch (type)
        {
            case BallType.Foliage:
                result = scriptableObject.foliageColor;
                break;
            case BallType.Seed:
                result = scriptableObject.seedColor;
                break;
            case BallType.Pink:
                result = scriptableObject.pinkColor;
                break;
        }

        return result;
    }

    public static Dictionary<MapDirection, Vector2> GetMapPositionsAdjacentTo(Vector2 position) 
    {
        Dictionary<MapDirection, Vector2> result = new Dictionary<MapDirection, Vector2>();

        int x = (int) position.x;
        int y = (int) position.y;

        if (y % 2 == 0)
        {
            result.Add(MapDirection.E, new Vector2(x + 1, y));
            result.Add(MapDirection.SE, new Vector2(x, y - 1));
            result.Add(MapDirection.SO, new Vector2(x - 1, y - 1));
            result.Add(MapDirection.O, new Vector2(x - 1, y));
            result.Add(MapDirection.NO, new Vector2(x - 1, y + 1));
            result.Add(MapDirection.NE, new Vector2(x, y + 1));
        }
        else
        {
            result.Add(MapDirection.E, new Vector2(x + 1, y));
            result.Add(MapDirection.SE, new Vector2(x + 1, y - 1));
            result.Add(MapDirection.SO, new Vector2(x, y - 1));
            result.Add(MapDirection.O, new Vector2(x - 1, y));
            result.Add(MapDirection.NO, new Vector2(x, y + 1));
            result.Add(MapDirection.NE, new Vector2(x + 1, y + 1));
        }
        
        return result;
    }
}

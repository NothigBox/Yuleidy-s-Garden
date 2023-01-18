using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSeed : Ball
{
    public override void Execute(Vector2 position, MapGrid map, Ball otherBall = null)
    {
        Dictionary<MapDirection, Vector2> positions = BallUtils.GetMapPositionsAdjacentTo(position);
        var directions = positions.Keys;

        foreach (MapDirection direction in directions) 
        {
            Ball b = map.GetGridBall(positions[direction]);

            if (b != null) 
            {
                if(b.Type == BallType.Pink) 
                {
                    b.Execute(b.gridPosition, map, this);
                    return;
                }
            }
        }
        
        base.Execute(position, map);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFoliage : Ball
{
    const int CLONES = 4;

    private static int clonesCount;
    private static Vector2 initialPosition;

    public override void Execute(Vector2 position, MapGrid map, Ball otherBall = null)
    {
        if (clonesCount == 0) initialPosition = position;
        if (clonesCount == CLONES - CLONES / 2) position = initialPosition;

        if(clonesCount < CLONES)
        {
            Dictionary<MapDirection, Vector2> positions = 
                BallUtils.GetMapPositionsAdjacentTo(position);
            var directions = positions.Keys;

            foreach (MapDirection direction in directions)
            {
                if (map.GetGridAvailableness(positions[direction]) != true) continue;

                clonesCount++;

                Ball newFoliage = map.SetBallOnGrid(positions[direction], this);
                newFoliage.Execute(positions[direction], map);
                return;
            }
            
            clonesCount = 0;
            base.Execute(position, map);
            
            return;
        }
        
        clonesCount = 0;
        base.Execute(position, map);
    }
}
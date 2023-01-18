using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPink : Ball
{
    public override void Execute(Vector2 position, MapGrid map, Ball otherBall = null)
    {
        if (otherBall != null) this.otherBall = otherBall;
        
        Dictionary<MapDirection, Vector2> pinkBallPositions = 
            BallUtils.GetMapPositionsAdjacentTo(position);
        var pinkBallDirections = pinkBallPositions.Keys;

        List<Vector2> seedPositions = new List<Vector2>();

        foreach (var direction in pinkBallDirections)
        {
            Ball b = map.GetGridBall(pinkBallPositions[direction]);
            if (b == null) continue;
            
            if (b.Type == BallType.Seed)
            {
                seedPositions.Add(pinkBallPositions[direction]);
            }
        }

        foreach (var seed in seedPositions)
        {
            Dictionary<MapDirection, Vector2> seedBallPositions = 
                BallUtils.GetMapPositionsAdjacentTo(seed);
            var seedBallDirections = seedBallPositions.Keys;

            foreach (var _direction in seedBallDirections)
            {
                var ultimatePosition = seedBallPositions[_direction];
                bool? isUltPositionAvailable = map.GetGridAvailableness(ultimatePosition);
                
                if (isUltPositionAvailable == null) continue;
                
                if (isUltPositionAvailable == false)
                {
                    map.RemoveBallFromGrid(ultimatePosition);
                }

                map.SetBallOnGrid(ultimatePosition, this);
            }
        }

        /*
        foreach (MapDirection direction in seedBallDirections)
        {
            if (map.GetGridAvailableness(seedBallPositions[direction]) == null) continue;
            if (map.GetGridAvailableness(seedBallPositions[direction]) == false)
            {
                map.RemoveBallFromGrid(seedBallPositions[direction]);   
            }

            map.SetBallOnGrid(seedBallPositions[direction], this);
        }
        */
        base.Execute(position, map);
    }
}

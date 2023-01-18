using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    private const int X = 9;
    private const float INITIAL_Y = 3.9f;
    private const float INITIAL_X = -2.1f;
    private const float INBETWEEN_Y = 0.51f;
    private const float INBETWEEN_X = 0.515f;
    private const float OFFSET_X_PER_ROW = 0.125f;

    public int length;

    private Dictionary<Vector2, Ball> ballsOnGrid;

    private void Awake()
    {
        ballsOnGrid = new Dictionary<Vector2, Ball>();
    }

    private void Start()
    {
        Ball.OnBallAttached += AttachBall;
    }

    public Ball SetBallOnGrid(Vector2 position, Ball ball)
    {
        if (GetGridAvailableness(position) != true)
        {
            print("Can't set Ball at " + position);
            return null;
        }

        Vector3 localPosition = GridToWorldPosition(position);

        Ball gridBall = Instantiate(BallUtils.GetBallObjectByType(ball.Type), localPosition, Quaternion.identity);
        gridBall.gridPosition = position;
        gridBall.transform.SetParent(transform);
        gridBall.ShowPoints();
        
        ballsOnGrid.Add(position, gridBall);

        return gridBall;
    }

    public void RemoveBallFromGrid(Vector2 position)
    {
        if (ballsOnGrid[position].Type == BallType.Seed) return;
            
        Destroy(ballsOnGrid[position].gameObject);
        ballsOnGrid.Remove(position);
    }

    private void AttachBall(Ball ball)
    {
        Vector2 position = WorldToGridPosition(ball.transform.position);

        if (GetGridAvailableness(position) != true)
        {
            Dictionary<MapDirection, Vector2> positions = 
                BallUtils.GetMapPositionsAdjacentTo(position);
            var directions = positions.Keys;
            
            foreach (MapDirection direction in directions)
            {
                if (GetGridAvailableness(positions[direction]) != true) continue;

                position = positions[direction];
                break;
            }
        }

        SetBallOnGrid(position, ball);

        ball.Execute(position, this);
    }

    public void MoveBalls(float distance) 
    {
        var positions = ballsOnGrid.Keys;

        foreach (var p in positions) 
        {
            Ball b = default;
            ballsOnGrid.TryGetValue(p, out b);

            if (b == null) return;
            


            var iP = b.transform.position;
            b.transform.SetPositionAndRotation(new Vector3(iP.x, iP.y + distance, iP.z), Quaternion.identity);
        }
    }

    public int[] GetRowCounts() 
    {
        List<int> result = new List<int>();

        for (int y = 0; y < length; y++)
        {
            try 
            {
                int v = result[y];
            }
            catch 
            {
                result.Add(0);
            }

            for (int x = 0; x < X; x++)
            { 
                if(GetGridAvailableness(new Vector2(x, y)) == false)
                {
                    ++result[y];
                }
            }

            if (result[y] == default) break;
        }

        return result.ToArray();
    }

    public bool IsRowFull(int rowCount) 
    {
        return rowCount == X;
    }

    public bool? GetGridAvailableness(Vector2 position)
    {
        if (position.x < 0 || position.x >= X) return null;
        if (position.y < 0 || position.y >= length) return null;
        
        return ballsOnGrid.ContainsKey(position) == false;
    }

    public Ball GetGridBall(Vector2 position)
    {
        if (GetGridAvailableness(position) == false)
        {
            return ballsOnGrid[position];
        }

        return null;
    }

    public Vector2 WorldToGridPosition(Vector3 position)
    {
        int y = Mathf.FloorToInt(Mathf.Abs(position.y - (transform.position.y + INITIAL_Y)) / INBETWEEN_Y);
        int x = Mathf.FloorToInt(Mathf.Abs(position.x - INITIAL_X) / INBETWEEN_X);
        
        return new Vector2(x, y);
    }
    
    public Vector3 GridToWorldPosition(Vector2 position)
    {
        Vector3 posY = transform.localPosition + Vector3.up * ((transform.position.y + INITIAL_Y) - INBETWEEN_Y * position.y);
        Vector3 posX = transform.localPosition + Vector3.right * ((INITIAL_X + INBETWEEN_X * position.x) + (OFFSET_X_PER_ROW * -Mathf.Pow(-1, position.y)));
        
        return posY + posX;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int y = 0; y < length; y++) 
        {
            for (int x = 0; x < X; x++)
            {
                Vector3 position = GridToWorldPosition(new Vector2(x, y));
                Gizmos.DrawWireSphere(position, 0.25f);
            }
        }
    }
}

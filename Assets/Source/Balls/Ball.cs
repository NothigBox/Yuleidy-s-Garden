using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ball : MonoBehaviour
{
    [SerializeField] private BallType type;
    [SerializeField] private int points;

    new private Rigidbody2D rigidbody;
    new private CircleCollider2D collider;
    private bool isOnGrid;
    
    protected Ball otherBall;

    public Vector2 gridPosition;
    
    public static event Action<Ball> OnBallAttached;
    public static event Action<bool> OnBallExecuted;
    public static event Action<int> OnSumBallPoints;

    public BallType Type => type;
    public Rigidbody2D Rigidbody => rigidbody;
    public bool IsOnGrid 
    {
        get { return isOnGrid; }

        set 
        {
            rigidbody.isKinematic = value;
            collider.radius = value ? (type == BallType.Seed? 0.55f : 0.5f) : 0.45f;

            isOnGrid = value;
        }
    }


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        
        IsOnGrid = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsOnGrid == true) return;

        if (collision.gameObject.CompareTag("Wall"))
        {
            if (type != BallType.Foliage) return;

            otherBall = null;
            CallAttached();
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            OnBallExecuted(true);
            
            Destroy(gameObject);
        }
        else
        {
            otherBall = collision.gameObject.GetComponent<Ball>();
            if (BallUtils.CanBeAttached(type, otherBall.Type))
            {
                CallAttached();
            }
        }
    }

    void CallAttached() 
    {
        OnBallAttached(this);

        Destroy(gameObject);
    }

    public virtual void Execute(Vector2 position, MapGrid map, Ball otherBall = null)
    {
        OnBallExecuted(true);
    }

    public void ShowPoints()
    {
        BallPoints p = Instantiate(BallUtils.Points, transform.position, Quaternion.identity);
        p.SetPointsToShow(points, type);

        OnSumBallPoints(points);
    }
}

public enum MapDirection { O, NO, NE, E, SE, SO }
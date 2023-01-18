using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayBall : MonoBehaviour
{
    private const float OFFSET_Z = 10f;

    [SerializeField] private float force;
    [SerializeField] private BallType currentBall;
    [SerializeField] Camera camera;
    
    private LineRenderer line;
    private Vector3 mousePos;
    private SpriteRenderer sprite;
    private SpriteMask mask;
    private bool canPlay;
    private bool canThrowBall;

    public bool CanPlay
    {
        get { return canPlay; }
        set
        {
            mask.enabled = value;
            sprite.enabled = value;
            canPlay = value;
        }
    }
    private void Awake()
    {
        canThrowBall = true;

        mask = GetComponent<SpriteMask>();
        sprite = GetComponent<SpriteRenderer>();
        line = GetComponent<LineRenderer>();

        SetBall(currentBall);
    }

    private void Start()
    {
        CanPlay = true;
    }

    private void Update()
    {
        if (CanPlay == false) return;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * OFFSET_Z;

        Vector3 direction = (transform.position - mousePos).normalized * -force;

        if (Input.GetMouseButton(0)) 
        {
            int h = Camera.main.pixelHeight;
            canThrowBall = Input.mousePosition.y > h / 8 &&
               Input.mousePosition.y < h - (h / 8) * 2;
            if (canThrowBall == true)
            {
                DrawProjection(direction);
            }
        }

        if (Input.GetMouseButtonUp(0)) 
        {
            if (canThrowBall == false) return;
            CanPlay = false;

            Ball playBall = Instantiate(BallUtils.GetBallObjectByType(currentBall), transform.position, Quaternion.identity);
            playBall.IsOnGrid = false;
            playBall.Rigidbody.AddForce(direction, ForceMode2D.Impulse);

            line.enabled = false;
        }
    }

    private void DrawProjection(Vector3 direction) 
    {
        float linePoints = 20f;
        float timeBetweenPoints = 0.01f;
        Color lineColor = BallUtils.GetBallColorByType(currentBall);

        line.enabled = true;
        line.startColor = lineColor;
        line.endColor = lineColor;
        line.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
        
        Vector3 startPosition = transform.position;
        Vector3 velocity = direction;

        int i = 0;
        line.SetPosition(i, startPosition);
        for (float time = 0; time < linePoints; time += timeBetweenPoints) 
        {
            i++;
            Vector3 point = startPosition + time * velocity;
            point.y = startPosition.y + time * velocity.y;

            line.SetPosition(i, point);

            Vector3 lastPosition = line.GetPosition(i - 1);

            var a = Physics2D.Raycast(lastPosition, (point - lastPosition).normalized, (point - lastPosition).magnitude);
            if (a != default) 
            {
                //BOUNCE PROGRAMMING HERE
                //Vector2 r = point - 2 * (Vector3.down * Vector3.Dot(point, Vector3.down));
                line.SetPosition(i, point);
                line.positionCount = i + 1;
                return;
            }
        }
    }

    public BallType SetBall(BallType newBallType)
    {
        currentBall = newBallType;
        
        sprite.sprite = BallUtils.GetBallSpriteByType(currentBall);

        CanPlay = true;

        return newBallType;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mousePos , 0.25f);
        Gizmos.DrawLine(transform.localPosition, mousePos);
    }

    private void OnValidate()
    {
        if(currentBall != previousBallTypeValidate) 
        {
            SetBall(currentBall);
            previousBallTypeValidate = currentBall;
        }
    }

    BallType previousBallTypeValidate;
}

public enum BallType { Foliage, Seed, Pink }
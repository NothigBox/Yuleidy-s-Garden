using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private const float PROBABILITY_FOLIAGE = 40f;
    private const float PROBABILITY_SEED = 35f;
    private const float PROBABILITY_PINK = 25f;

    [SerializeField] private LevelInfo[] levels;
    [SerializeField] private float moveSpeed;

    private LevelInfo initialnfo;
    private LevelInfo currentInfo;
    private int currentLevel;

    public int BallsCount => currentInfo.foliageAmount + currentInfo.seedAmount + currentInfo.pinkAmount;
    public int GridLength => levels[currentLevel].length;
    public LevelInfo CurrentLevel => levels[currentLevel];

    public void SetUpLevel()
    {
        initialnfo = CurrentLevel;
        currentInfo = initialnfo;
    }
    
    public BallType GetRandomBall()
    {
        BallType result = default;

        float r = UnityEngine.Random.Range(0f, 100f);

        if (r < PROBABILITY_FOLIAGE)
        {
            result = BallType.Foliage;
            currentInfo.foliageAmount--;
        }
        else if (r >= PROBABILITY_FOLIAGE &&
                 r < PROBABILITY_FOLIAGE + PROBABILITY_SEED)
        {
            result = BallType.Seed;
            currentInfo.seedAmount--;
        }
        else if (r >= PROBABILITY_FOLIAGE + PROBABILITY_SEED && 
                 r <= PROBABILITY_FOLIAGE + PROBABILITY_SEED + PROBABILITY_PINK)
        {
            result = BallType.Pink;
            currentInfo.pinkAmount--;
        }

        return result;
    }

    public void GoUp(float distance, params GameObject[] objects)
    {
        StartCoroutine(MoveCoroutine(distance, true, objects));
    }

    public void GoDown(float distance, params GameObject[] objects)
    {
        StartCoroutine(MoveCoroutine(distance, false, objects));
    }

    IEnumerator MoveCoroutine(float distance, bool goUp = true, params GameObject[] objects)
    {
        if (goUp == true)
        {
            float y = objects[0].transform.localPosition.y;
            float destination = y + distance;
            float delta = default;

            MapGrid grid = default;

            while (y < destination) 
            {
                for (int h = 0; h < objects.Length; h++)
                {
                    GameObject _object = objects[h];
                    Vector3 _position = _object.transform.localPosition;
                    Vector3 _destination = new Vector3(_position.x, _position.y + distance, _position.z);

                    var _grid = _object.GetComponent<MapGrid>();

                    bool isGrid = _grid != null;

                    if (isGrid == true) grid = _grid;

                    delta = moveSpeed * (isGrid? 0.5f : 2f);

                    _object.transform.position = Vector3.MoveTowards(_position, _destination, delta * Time.deltaTime);

                    
                    if (isGrid == true) 
                    {
                        grid.MoveBalls(moveSpeed * Time.deltaTime);
                    }
                    
                }

                y += delta * Time.deltaTime;

                yield return null;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveRotation : MonoBehaviour
{
    [SerializeField] float angularSpeed;

    new RectTransform transform;

    private void Awake()
    {
        transform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, angularSpeed);
    }
}

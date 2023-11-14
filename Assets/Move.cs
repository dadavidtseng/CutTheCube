using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 2.0f;
    public float distance = 5.0f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Mathf.Sin(Time.time * speed) * distance;
        transform.position = startPosition + Vector3.forward * delta;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 targetPosition;

    public float speed = 0.01f;
    public float minSpeed = 0;
    public float maxSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        if (targetTransform != null)
            targetPosition = targetTransform.position;

        Vector3 delta = targetPosition - transform.position;
        float speed = this.speed * delta.sqrMagnitude;
        
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        speed = Mathf.Min(speed, delta.magnitude);

        transform.position += delta.normalized * speed * Time.deltaTime;
    }
}

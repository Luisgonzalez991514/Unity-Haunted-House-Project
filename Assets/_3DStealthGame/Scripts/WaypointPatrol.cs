using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPatrol : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public Transform[] waypoints;

    private Rigidbody m_RigidBody;
    int m_CurrentWaypointIndex;
    
    private float pauseTimer = 0f;
    private float nextPauseTime;
    private bool isPaused = false;
    public float minPauseInterval = 3f;
    public float maxPauseInterval = 8f;
    public float minPauseDuration = 1f;
    public float maxPauseDuration = 3f;

    void Start ()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        nextPauseTime = Random.Range(minPauseInterval, maxPauseInterval);
    }

    void FixedUpdate ()
    {
        pauseTimer += Time.deltaTime;
        
        if (!isPaused && pauseTimer >= nextPauseTime)
        {
            StartPause();
        }
        
        if (isPaused)
        {
            m_RigidBody.linearVelocity = Vector3.zero;
            return;
        }
        
        Transform currentWaypoint = waypoints[m_CurrentWaypointIndex];
        Vector3 currentToTarget = currentWaypoint.position - m_RigidBody.position;

        if (currentToTarget.magnitude < 0.1f)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        }

        Quaternion forwardRotation = Quaternion.LookRotation(currentToTarget);
        m_RigidBody.MoveRotation(forwardRotation);
        m_RigidBody.MovePosition(m_RigidBody.position + currentToTarget.normalized * moveSpeed * Time.deltaTime);
    }
    
    void StartPause()
    {
        isPaused = true;
        m_RigidBody.linearVelocity = Vector3.zero;
        
        float pauseDuration = Random.Range(minPauseDuration, maxPauseDuration);
        Invoke("EndPause", pauseDuration);
    }

    void EndPause()
    {
        isPaused = false;
        pauseTimer = 0f;
        nextPauseTime = Random.Range(minPauseInterval, maxPauseInterval);
    }
}
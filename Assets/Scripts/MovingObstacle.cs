using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private Transform m_startWaypoint;
    [SerializeField] private Transform m_endWaypoint;
    [SerializeField] private float m_speed = 5;

    Vector3 StartPosition;
    Vector3 EndPosition;
    float timeElapsed;


    private Transform m_target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        m_target = m_startWaypoint;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 position = m_target.position;
        //position.y = transform.position.y + 0.1f*Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, m_target.position, m_speed * Time.deltaTime);

        //Vector3 SomewhereBetween = Vector3.Lerp(StartPosition, EndPosition, timeElapsed);
        //transform.position = SomewhereBetween;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MovingObstacleWaypoint"))
        {
            ChangeTarget();
        }
    }

    void ChangeTarget()
    {
        if (m_target == m_startWaypoint)
        {
            m_target = m_endWaypoint;
        }
        else
        {
            m_target = m_startWaypoint;
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    //config
    [SerializeField] List<Transform> waypoints = null;
    [SerializeField] float enemySpeed = 1f;

    //state
    int waypointIndex = 0;
    Transform currentWaypoint = null;

    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = waypoints[waypointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowards();
    }

    private void MoveTowards()
    {
        if (Vector2.Distance(transform.position, currentWaypoint.position) > 0f)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint.position, enemySpeed * Time.deltaTime);
        }
        else
        {
            //get next waypoint
            waypointIndex++;

            //if at last waypoint, destroy
            if (waypointIndex >= waypoints.Count)
            {
                Destroy(gameObject);
            }
            else
            {
                currentWaypoint = waypoints[waypointIndex];
            }

        }
    }
}

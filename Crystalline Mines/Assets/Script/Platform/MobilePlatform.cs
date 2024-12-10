using UnityEngine;

public class MobilePlatform : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _moveSpeed = 2f;
    private int _waypointIndex;
    void Start()
    {
        transform.position = _waypoints[_waypointIndex].transform.position;
        foreach (var waypoint in _waypoints)
        {
            if (waypoint.transform.position.z != 0)
            {
                waypoint.transform.position = new Vector3(waypoint.transform.position.x, waypoint.transform.position.y, 0);
            }
        }
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, _waypoints[_waypointIndex].transform.position, _moveSpeed * Time.deltaTime);
        if (transform.position == _waypoints[_waypointIndex].transform.position)
        {
            _waypointIndex += 1;
        }

        if (_waypointIndex >= _waypoints.Length)
        {
            _waypointIndex = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Vector3 playerPosition = other.transform.position;
        if (_waypointIndex == 0)
            playerPosition.x += _moveSpeed * Time.deltaTime;
        if (_waypointIndex == 1)
            playerPosition.x += -_moveSpeed * Time.deltaTime;
        other.transform.position = playerPosition;
    }
}

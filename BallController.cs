using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BallController : MonoBehaviour
{
    private NavMeshAgent _agent;

    void Awake() => _agent = GetComponent<NavMeshAgent>();

    private void Update() {

        if (Vector3.Distance(transform.position, _agent.destination) <0.1 ) {

            int[] coords = new int[2];

            coords[0] = Random.Range(-6, 6);
            coords[1] = Random.Range(-6, 6); 

            MoveBall(coords[0], coords[1]);
        }
         
    }

    void MoveBall(int x, int y) {

        Vector3 newLocation = new Vector3(x, 0.5f, y);
        _agent.Move(newLocation);
    }

    private void OnMouseDown() {

        CameraController.instance.followTransform = transform;
    }
}

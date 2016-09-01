using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float angleSpeed;
    public float speed;
    public Transform xEdge;
    public Transform yEdge;

    void Update() {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");

        Vector3 xAxis = xEdge.position - transform.position;
        Vector3 yAxis = yEdge.position - transform.position;

        transform.position += (xAxis * dx + yAxis * dy) * speed;

        Vector3 axis = yAxis * x - xAxis * y;
        transform.RotateAround(transform.position, axis, Vector3.Magnitude(axis) * angleSpeed);
    }
}

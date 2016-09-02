using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float angleSpeed;
    public float speed;
    public Transform xEdge;
    public Transform yEdge;
    public Transform cameraPoint;

    void Update() {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");
        float zoom = Input.GetAxis("Zoom");

        Vector3 xAxis = xEdge.position - transform.position;
        Vector3 yAxis = yEdge.position - transform.position;
        Vector3 camVec = cameraPoint.position - transform.position;

        transform.position += (xAxis * dx + yAxis * dy) * speed;

        Vector3 axis = yAxis * x + xAxis * y;
        transform.RotateAround(transform.position, axis, Vector3.Magnitude(axis) * angleSpeed);

        cameraPoint.position += camVec * zoom / Vector3.Magnitude(camVec);
    }
}

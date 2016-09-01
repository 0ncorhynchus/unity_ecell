using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float zoomSpeed;
    public float angleSpeed;
    public float speed;
    public float shortestDistance;
    public float longestDistance;
    // public Transform camera;
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

        transform.RotateAround(transform.position, yAxis, x * angleSpeed);
        transform.RotateAround(transform.position, xAxis, y * angleSpeed);

        UpdateCamera();
    }

    void UpdateCamera() {
        // float zoom = Input.GetAxis("Zoom");
        // float movement = zoom * zoomSpeed;
        // Vector3 cam = camera.position - transform.position;
        // if (cam.magnitude + movement < shortestDistance) {
        //     camera.position = cam.normalized * shortestDistance;
        // } else if (cam.magnitude + movement > longestDistance) {
        //     camera.position = cam.normalized * longestDistance;
        // } else {
        //     camera.position += cam.normalized * movement;
        // }
        // camera.position += cam.normalized * movement;
    }

}

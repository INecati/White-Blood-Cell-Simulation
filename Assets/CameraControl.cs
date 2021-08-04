using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float moveSpeed = 1;
    public float zoomSpeed = 1;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }
    private void LateUpdate()
    {
        MoveCam();
    }
    void MoveCam() {
        float horizontalMove = Input.GetAxis("Horizontal")*moveSpeed*Time.deltaTime;
        float verticalMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float zoom=0;
        if (Input.GetKey(KeyCode.Q)) {
            zoom = zoomSpeed*Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E)) {
            zoom = -zoomSpeed * Time.deltaTime;
        }
        transform.position+= new Vector3 (horizontalMove,verticalMove,0);
        cam.orthographicSize += zoom;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1.5f, 10f);
    }
}

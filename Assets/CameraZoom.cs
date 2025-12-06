using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera Cam;
    public float TargetZoom = 3;
    public float ScrollData;
    public float ZoomSpeed = 3;


    // Start is called before the first frame update
    void Start()
    {
        Cam = GetComponent<Camera>();
        TargetZoom = Cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        ScrollData = Input.GetAxis("Mouse ScrollWheel");

        TargetZoom = TargetZoom - ScrollData;

        TargetZoom = Mathf.Clamp(TargetZoom, 3, 5);

        Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, TargetZoom, Time.deltaTime * ZoomSpeed);
    }
}

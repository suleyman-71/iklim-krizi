using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
    public float zoomSpeed = 0.01f;
    public float panSpeed = 0.25f;

    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        if (Input.touchCount == 2) 
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;
            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * zoomSpeed);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Camera.main.transform.position += direction * panSpeed;
            //if (!EventSystem.current.IsPointerOverGameObject())
            //{
            //    Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    touchPos.z = Camera.main.transform.position.z;
            //    Camera.main.transform.position = touchPos;
            //}
        }

        void Zoom(float increment)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
        }

        // Kamera hareketini sınırla
        Vector3 pos = Camera.main.transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        Camera.main.transform.position = pos;
    }
}

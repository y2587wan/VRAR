using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour {

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.position += Camera.main.transform.forward * 1.5f * Time.deltaTime;
        }
    }
}

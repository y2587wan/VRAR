using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Vector3 spawnPoint;
    private bool isWalking = false;

    private void Start()
    {
        spawnPoint = transform.position;
    }

    void Update () {
        if (isWalking)
        {
            transform.position += Camera.main.transform.forward * 0.5f * Time.deltaTime;
        }

        if (transform.position.y < -10f)
        {
            transform.position = spawnPoint;
        }
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))  
        {
            if (hit.collider.tag == "Ground")
            {
                isWalking = false;
            }
            else
            {
                isWalking = true;
            }
        }
	}
}

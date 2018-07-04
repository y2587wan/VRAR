using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDetection : MonoBehaviour {

    [SerializeField]
    private Transform transform;

    private void Start()
    {
        transform = GetComponent<Transform>();

        var childrenCollider = transform.GetComponentsInChildren<Collider>();
        Debug.Log(childrenCollider.Length);

        foreach (var item in childrenCollider)
        {
            item.isTrigger = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.name);
    }
}

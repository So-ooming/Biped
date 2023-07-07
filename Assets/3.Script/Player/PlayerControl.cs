using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            rb.AddForce(rb.transform.forward * speed);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Rigidbody rb;
    [SerializeField] ConfigurableJoint leftLeg;
    [SerializeField] ConfigurableJoint rightLeg;
    [SerializeField] ConfigurableJoint leftFoot;
    [SerializeField] ConfigurableJoint rightFoot;

    [SerializeField] Transform head;
    Ray ray;
    RaycastHit hit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        /*if (Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1))
        {
            leftLeg.angularXMotion = ConfigurableJointMotion.Locked;
            leftLeg.angularYMotion = ConfigurableJointMotion.Locked;
            leftLeg.angularZMotion = ConfigurableJointMotion.Locked;

            rightLeg.angularXMotion = ConfigurableJointMotion.Locked;
            rightLeg.angularYMotion = ConfigurableJointMotion.Locked;
            rightLeg.angularZMotion = ConfigurableJointMotion.Locked;

            leftFoot.angularXMotion = ConfigurableJointMotion.Locked;
            leftFoot.angularYMotion = ConfigurableJointMotion.Locked;
            leftFoot.angularZMotion = ConfigurableJointMotion.Locked;

            rightFoot.angularXMotion = ConfigurableJointMotion.Locked;
            rightFoot.angularYMotion = ConfigurableJointMotion.Locked;
            rightFoot.angularZMotion = ConfigurableJointMotion.Locked;
        }*/

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            LookAtMouse();
        }

        /*if (Input.GetMouseButtonUp(0) && Input.GetMouseButtonUp(1))
        {
            leftLeg.angularXMotion = ConfigurableJointMotion.Free;
            leftLeg.angularYMotion = ConfigurableJointMotion.Free;
            leftLeg.angularZMotion = ConfigurableJointMotion.Free;

            rightLeg.angularXMotion = ConfigurableJointMotion.Free;
            rightLeg.angularYMotion = ConfigurableJointMotion.Free;
            rightLeg.angularZMotion = ConfigurableJointMotion.Free;
                                                              
            leftFoot.angularXMotion = ConfigurableJointMotion.Free;
            leftFoot.angularYMotion = ConfigurableJointMotion.Free;
            leftFoot.angularZMotion = ConfigurableJointMotion.Free;

            rightFoot.angularXMotion = ConfigurableJointMotion.Free;
            rightFoot.angularYMotion = ConfigurableJointMotion.Free;
            rightFoot.angularZMotion = ConfigurableJointMotion.Free;
        }*/
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            rb.AddForce(rb.transform.forward * speed);
        }
    }

    private void LookAtMouse()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float length;

        if(plane.Raycast(ray, out length))
        {
            Vector3 mousePoint = ray.GetPoint(length);
            //transform.parent.transform.LookAt(mousePoint);
            //transform.LookAt(mousePoint);
            Vector3 l_vector = mousePoint - transform.position;
            transform.rotation = Quaternion.LookRotation(l_vector).normalized;
        }
    }
}

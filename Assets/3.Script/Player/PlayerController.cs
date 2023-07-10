using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform leftLeg;
    [SerializeField] private Transform rightLeg;
    [SerializeField] private Transform leftPivot;
    [SerializeField] private Transform rightPivot;
    [SerializeField] private Transform body;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider col;
    [SerializeField] private PhysicMaterial highFric;       // 높은 마찰력 Material
    [SerializeField] private PhysicMaterial lowFric;        // 낮은 마찰력 Material

    Quaternion defaultLeftLeg;
    Quaternion defaultRightLeg;
    Quaternion clickLeftLeg;
    Quaternion clickRightLeg;
    Quaternion defaultBodyRotation;
    Quaternion clickLeftBody;
    Quaternion clickRightBody;

    [SerializeField] bool isLeft = false;
    [SerializeField] bool isRight = false;
    [SerializeField] bool isSliding = false;

    [SerializeField] float speed = 10f;
    [SerializeField] float rotSpeed = 6f;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        col = transform.GetComponent<CapsuleCollider>();
        defaultBodyRotation = body.localRotation;
        defaultLeftLeg = leftLeg.localRotation;
        defaultRightLeg = rightLeg.localRotation;
        clickLeftLeg = defaultLeftLeg * Quaternion.Euler(new Vector3(-90f, 0, 90f));
        clickRightLeg = defaultRightLeg * Quaternion.Euler(new Vector3(90f, 0, 90f));
        clickLeftBody = defaultBodyRotation * Quaternion.Euler(new Vector3(30f, 0, 0));
        clickRightBody = defaultBodyRotation * Quaternion.Euler(new Vector3(-30f, 0, 0));
    }
    
    void Update()
    {
        #region 좌클릭
        if (Input.GetMouseButtonDown(0) && !isRight && !isSliding)
        {
            isLeft = true;
            col.material = highFric;
            leftLeg.localRotation = defaultLeftLeg;
        }
        if (Input.GetMouseButton(0) && !isRight && !isSliding)
        {
            LookAtMousePointer(rightPivot.position, leftPivot.position);
            leftLeg.localRotation = Quaternion.Slerp(leftLeg.localRotation, defaultLeftLeg * clickLeftLeg, Time.deltaTime * rotSpeed);
            body.localRotation = Quaternion.Slerp(body.localRotation, defaultBodyRotation * clickLeftBody, rotSpeed * Time.deltaTime);
        }
        if (Input.GetMouseButtonUp(0))
        {
            col.material = lowFric;
            isLeft = false;
            leftLeg.localRotation = defaultLeftLeg;
            body.localRotation = defaultBodyRotation;
        }
        #endregion

        #region 우클릭
        if (Input.GetMouseButtonDown(1) && !isLeft && !isSliding)
        {
            isRight = true;
            col.material = highFric;
            rightLeg.localRotation = defaultRightLeg;
        }

        if (Input.GetMouseButton(1) && !isLeft && !isSliding)
        {
            LookAtMousePointer(leftPivot.position, rightPivot.position);
            rightLeg.localRotation = Quaternion.Slerp(rightLeg.localRotation, defaultRightLeg * clickRightLeg, Time.deltaTime * rotSpeed);
            body.localRotation = Quaternion.Slerp(body.localRotation, defaultBodyRotation * clickRightBody, rotSpeed * Time.deltaTime);            //rightLeg.position = new Vector3(-0.25f, 0.4f, 0);
        }

        if (Input.GetMouseButtonUp(1))
        {
            col.material = lowFric;
            isRight = false;
            rightLeg.localRotation = defaultRightLeg;
            body.localRotation = defaultBodyRotation;
        }
        #endregion
    }

    private void FixedUpdate()
    {
        #region 양쪽 모두 클릭
        if (Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1))
        {
            isSliding = true;
            col.material = lowFric;
            leftLeg.localRotation = defaultLeftLeg;
            rightLeg.localRotation = defaultRightLeg;
        }

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            //body.localRotation = defaultBodyRotation;
            LookAtMousePointer();
        }

        if((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) && isSliding)
        {
            isSliding = false;
            col.material = highFric;
        }
        #endregion
    }

    public void LookAtMousePointer(Vector3 pivot, Vector3 another)
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, transform.position);
        float rayLength;

        if (GroupPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            //Quaternion targetRot = Quaternion.Euler(pointToLook);
            //transform.LookAt(new Vector3(-pointToLook.x, transform.position.y, -pointToLook.z));
            //transform.Rotate(pivot, GetAngle(transform.position, pointToLook));
            //Debug.Log(GetAngle(pivot, pointToLook));
            transform.RotateAround(pivot, Vector3.up, rotSpeed * Time.deltaTime * GetAngle(pivot, another, pointToLook));
        }
    }

    public void LookAtMousePointer()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, transform.position);
        float rayLength;

        if (GroupPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Vector3 direction = pointToLook - transform.position;
            direction = direction.normalized;
            float distance = direction.magnitude;
            //transform.LookAt(new Vector3(-pointToLook.x, transform.position.y, -pointToLook.z));

            rb.AddForce(direction * speed * Time.deltaTime, ForceMode.Force);
        }
    }

    float GetAngle(Vector3 start, Vector3 another, Vector3 end)
    {
        Vector3 anotherV = new Vector3(end.x - start.x, 0, end.z - start.z).normalized; // pointToLook - pivot
        Vector3 baseV = new Vector3(another.x - start.x, 0, another.z - start.z).normalized;

        float dot = Vector3.Dot(baseV, anotherV);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        Vector3 cross = Vector3.Cross(baseV, anotherV);
        if(cross.y > 0)
        {
            return angle;
        }
        else
        {
            return -angle;
        }
    }
}

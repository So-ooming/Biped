using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Transform 및 물리 컴포넌트")]
    [SerializeField] private Transform leftLeg;
    [SerializeField] private Transform rightLeg;
    [SerializeField] private Transform leftPivot;
    [SerializeField] private Transform rightPivot;
    [SerializeField] private Transform body;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider col;
    [SerializeField] private PhysicMaterial highFric;       // 높은 마찰력 Material
    [SerializeField] private PhysicMaterial lowFric;        // 낮은 마찰력 Material

    [Header("스폰 관련")]
    [SerializeField] private Transform[] spawnPoint;
    private int currentSpawn = 0;
    float pressDuration = 1f;
    float pressTimer;

    [Header("회전 관련")]
    Quaternion defaultLeftLeg;
    Quaternion defaultRightLeg;
    Quaternion clickLeftLeg;
    Quaternion clickRightLeg;
    Quaternion defaultBodyRotation;
    Quaternion clickLeftBody;
    Quaternion clickRightBody;

    [Header("Bool 변수")]
    [SerializeField] bool isLeft = false;
    [SerializeField] bool isRight = false;
    [SerializeField] bool isSliding = false;

    [Header("힘과 속도")]
    [SerializeField] float speed = 10f;
    [SerializeField] float rotSpeed = 6f;
    [SerializeField] float jumpForce = 100f;
    [SerializeField] float maxVelX = 7f, maxVelZ = 7f;

    [Header("ETC")]
    [SerializeField] CameraController cameraController;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        col = transform.GetComponent<CapsuleCollider>();
        cameraController = FindObjectOfType<CameraController>();
        defaultBodyRotation = body.localRotation;
        defaultLeftLeg = leftLeg.localRotation;
        defaultRightLeg = rightLeg.localRotation;
        clickLeftLeg = defaultLeftLeg * Quaternion.Euler(new Vector3(-90f, 0, 90f));
        clickRightLeg = defaultRightLeg * Quaternion.Euler(new Vector3(90f, 0, 90f));
        clickLeftBody = defaultBodyRotation * Quaternion.Euler(new Vector3(30f, 0, 0));
        clickRightBody = defaultBodyRotation * Quaternion.Euler(new Vector3(-30f, 0, 0));
    }

    private void Update()
    {
        #region 좌클릭
        if (Input.GetMouseButtonDown(0) && !isRight)
        {
            isLeft = true;
            leftLeg.localRotation = defaultLeftLeg;
        }
        if (Input.GetMouseButton(0) && !isRight)
        {
            //col.material = highFric;
            //StopCoroutine("LeftMovement_co");
            //StartCoroutine("LeftMovement_co");
            LookAtMousePointer(rightPivot.position, leftPivot.position);
            leftLeg.localRotation = Quaternion.Slerp(leftLeg.localRotation, defaultLeftLeg * clickLeftLeg, Time.deltaTime * rotSpeed);
            body.localRotation = Quaternion.Slerp(body.localRotation, defaultBodyRotation * clickLeftBody, rotSpeed * Time.deltaTime);
        }
        /*if (Input.GetMouseButton(0) && !isRight && !isSliding)
        {
            
        }*/
        if (Input.GetMouseButtonUp(0) && !isRight)
        {
            isLeft = false;
            //StopCoroutine("LeftMovement_co");
            leftLeg.localRotation = defaultLeftLeg;
            body.localRotation = defaultBodyRotation;
        }
        #endregion

        #region 우클릭
        if (Input.GetMouseButtonDown(1) && !isLeft)
        {
            isRight = true;
            rightLeg.localRotation = defaultRightLeg;
        }
        if (Input.GetMouseButton(1) && !isLeft)
        {
            //col.material = highFric;
            LookAtMousePointer(leftPivot.position, rightPivot.position);
            rightLeg.localRotation = Quaternion.Slerp(rightLeg.localRotation, defaultRightLeg * clickRightLeg, Time.deltaTime * rotSpeed);
            body.localRotation = Quaternion.Slerp(body.localRotation, defaultBodyRotation * clickRightBody, rotSpeed * Time.deltaTime);
            //StopCoroutine("RightMovement_co");
            //StartCoroutine("RightMovement_co");
        }

        /*if (Input.GetMouseButton(1) && !isLeft && !isSliding)
        {
            //rightLeg.position = new Vector3(-0.25f, 0.4f, 0);
        }*/

        if (Input.GetMouseButtonUp(1) && !isLeft)
        {
            isRight = false;
            //StopCoroutine("RightMovement_co");
            rightLeg.localRotation = defaultRightLeg;
            body.localRotation = defaultBodyRotation;
        }
        #endregion
    }

    private void FixedUpdate()
    {
        

        #region 양쪽 모두 클릭
        if (Input.GetMouseButton(2))
        {
            rightLeg.localRotation = defaultRightLeg;
            leftLeg.localRotation = defaultLeftLeg;
            body.localRotation = defaultBodyRotation;
            LookAtMousePointer();
        }
        #endregion
    }

    private IEnumerator LeftMovement_co()
    {
        WaitForSeconds waitSec = new WaitForSeconds(0.01f);
        while (true)
        {
            LookAtMousePointer(rightPivot.position, leftPivot.position);
            leftLeg.localRotation = Quaternion.Slerp(leftLeg.localRotation, defaultLeftLeg * clickLeftLeg, Time.deltaTime * rotSpeed);
            body.localRotation = Quaternion.Slerp(body.localRotation, defaultBodyRotation * clickLeftBody, rotSpeed * Time.deltaTime);
            yield return waitSec;
        }
    }

    private IEnumerator RightMovement_co()
    {
        WaitForSeconds waitSec = new WaitForSeconds(0.01f);
        while (true)
        {
            LookAtMousePointer(leftPivot.position, rightPivot.position);
            rightLeg.localRotation = Quaternion.Slerp(rightLeg.localRotation, defaultRightLeg * clickRightLeg, Time.deltaTime * rotSpeed);
            body.localRotation = Quaternion.Slerp(body.localRotation, defaultBodyRotation * clickRightBody, rotSpeed * Time.deltaTime);
            yield return waitSec;
        }
    }

    private IEnumerator StopMovement_co()
    {
        col.material = highFric;
        Debug.Log("안 미끄러워");
        yield return new WaitForSeconds(0.5f);
        col.material = lowFric;
        Debug.Log("미끄러워");
    }

    private IEnumerator Skating_co()
    {
        WaitForSeconds waitSec = new WaitForSeconds(0.01f);
        while (true)
        {
            LookAtMousePointer();
            yield return waitSec;
        }
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
            Vector3 dir = -(direction * 100);
            //transform.LookAt(new Vector3(-pointToLook.x, transform.position.y, -pointToLook.z));
            transform.LookAt(new Vector3(dir.x, transform.position.y, dir.z));
            //Debug.DrawRay(Camera.main.transform.position, direction * 100, Color.red, Time.deltaTime);
            //Debug.Log(new Vector3(-pointToLook.x, transform.position.y, -pointToLook.z));

            rb.AddForce(direction * speed, ForceMode.Force);
            LimitSpeed();
        }
    }

    float GetAngle(Vector3 start, Vector3 another, Vector3 end)
    {
        Vector3 anotherV = new Vector3(end.x - start.x, 0, end.z - start.z).normalized; // pointToLook - pivot
        Vector3 baseV = new Vector3(another.x - start.x, 0, another.z - start.z).normalized;

        float dot = Vector3.Dot(baseV, anotherV);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        Vector3 cross = Vector3.Cross(baseV, anotherV);
        if (cross.y > 0)
        {
            return angle;
        }
        else
        {
            return -angle;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Slide"))
        {
            this.col.material = lowFric;
        }
        if (col.transform.CompareTag("ChangePoint"))
        {
            Debug.Log("콜리젼 부딪힘");
            cameraController.currentPoint++;
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            Die();
            Invoke("ReSpawn", 1f);
        }

        if (other.CompareTag("SpawnUpdate"))
        {
            other.gameObject.SetActive(false);
            currentSpawn++;
        }
        if (other.transform.CompareTag("Trampoline"))
        {
            Debug.Log("쩜프 !");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Die()
    {
        this.gameObject.SetActive(false);
    }

    void ReSpawn()
    {
        transform.position = spawnPoint[currentSpawn].position;
        this.gameObject.SetActive(true);
    }

    void LimitSpeed()
    {
        if (rb.velocity.x > maxVelX)
        {
            rb.velocity = new Vector3(maxVelX, rb.velocity.y, rb.velocity.z);
        }
        if (rb.velocity.x < -maxVelX)
        {
            rb.velocity = new Vector3(-maxVelX, rb.velocity.y, rb.velocity.z);
        }
        if(rb.velocity.z > maxVelZ)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxVelZ);
        }
        if(rb.velocity.z < -maxVelZ)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -maxVelZ);
        }
    }
}

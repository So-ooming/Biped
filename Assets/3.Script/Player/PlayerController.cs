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

    [SerializeField] private Transform[] spawnPoint;
    private int currentSpawn = 0;

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

    private void FixedUpdate()
    {
        #region 좌클릭
        if (Input.GetMouseButtonDown(0))
        {
            if (!isRight)                           // 오른쪽 마우스가 안 눌린 상태라면
            {
                isLeft = true;
                //col.material = highFric;
                leftLeg.localRotation = defaultLeftLeg;
                StopCoroutine("LeftMovement_co");
                StartCoroutine("LeftMovement_co");
            }

            else                                    // 오른쪽 마우스가 눌린 상태라면
            {
                isLeft = false;
                isSliding = true;
                StopCoroutine("RightMovement_co");
                StopCoroutine("Skating_co");
                StartCoroutine("Skating_co");
                Debug.Log("오른쪽 누른 상태에서 왼쪽 누름");
                leftLeg.localRotation = defaultLeftLeg;
                rightLeg.localRotation = defaultRightLeg;
                body.localRotation = defaultBodyRotation;
                Debug.Log("활주 실행 코드 이후");
            }
        }
        /*if (Input.GetMouseButton(0) && !isRight && !isSliding)
        {
            
        }*/
        if (Input.GetMouseButtonUp(0))
        {
            isLeft = false;
            Debug.Log("이즈레프트 펄스");
            StopCoroutine("LeftMovement_co");
            StopCoroutine("Skating_co");
            //StartCoroutine("StopMovement_co");
            leftLeg.localRotation = defaultLeftLeg;
            body.localRotation = defaultBodyRotation;
        }
        #endregion

        #region 우클릭
        if (Input.GetMouseButtonDown(1))
        {
            if (!isLeft)                            // 왼쪽 마우스가 안 눌린 상태라면
            {
                isRight = true;
                //col.material = highFric;
                rightLeg.localRotation = defaultRightLeg;
                StopCoroutine("RightMovement_co");
                StartCoroutine("RightMovement_co");
            }
            else                                    // 왼쪽 마우스가 눌린 상태라면
            {
                isRight = false;
                isSliding = true;
                StopCoroutine("LeftMovement_co");
                StopCoroutine("Skating_co");
                StartCoroutine("Skating_co");
                leftLeg.localRotation = defaultLeftLeg;
                rightLeg.localRotation = defaultRightLeg;
                body.localRotation = defaultBodyRotation;
            }
        }

        /*if (Input.GetMouseButton(1) && !isLeft && !isSliding)
        {
            //rightLeg.position = new Vector3(-0.25f, 0.4f, 0);
        }*/

        if (Input.GetMouseButtonUp(1))
        {
            //StartCoroutine("StopMovement_co");
            Debug.Log("이즈라이트 펄스");
            isRight = false;
            isSliding = false;
            StopCoroutine("Skating_co");
            StopCoroutine("RightMovement_co");
            rightLeg.localRotation = defaultRightLeg;
            body.localRotation = defaultBodyRotation;
        }
        #endregion
    }

    private IEnumerator LeftMovement_co()
    {
        WaitForSeconds waitSec = new WaitForSeconds(0.01f);
        while (isLeft)
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
        while (isRight)
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
        while (isSliding)
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
            transform.LookAt(new Vector3(-pointToLook.x, transform.position.y, -pointToLook.z));

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
}

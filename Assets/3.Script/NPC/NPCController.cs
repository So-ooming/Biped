using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] Transform leftLeg;
    [SerializeField] Transform rightLeg;
    [SerializeField] Transform body;
    [SerializeField] Transform point;
    [SerializeField] float rotSpeed = 10f;
    [SerializeField] Transform[] NPCTp;
    float timer;

    Quaternion defaultBodyRotation;
    Quaternion defaultLeftLeg;
    Quaternion defaultRightLeg;
    Quaternion clickLeftLeg;
    Quaternion clickRightLeg;
    Quaternion clickLeftBody;
    Quaternion clickRightBody;

    // 다리, 몸 포지션 기본값
    Vector3 defaultBodyPos;
    Vector3 defaultRightLegPos;
    Vector3 defaultLeftLegPos;

    // 좌 또는 우클릭했을 때 포지션 변화값, 다리 회전값
    Vector3 clickRightBodyPos = new Vector3(0.00064f, 0.00028f, 0);
    Vector3 clickLeftBodyPos = new Vector3(-0.00064f, 0.00028f, 0);
    Vector3 clickRightLegPos = new Vector3(0.00395f, -0.0003f, 0);
    Vector3 clickLeftLegPos = new Vector3(-0.00395f, -0.0003f, 0);
    Quaternion clickedRight_leftLeg;
    Quaternion clickedLeft_rightLeg;

    bool isLeft = false;
    bool isRight = false;

    // 대화박스 꺼지면서 말풍선 UI 활성화
    // NPC 오른쪽 다리 들기
    // 오른쪽 다리 들어서 특별한 이벤트 발생시키게 하기
    // 이벤트 발생 시 말풍선 내용 바꾸기
    // NPC 왼쪽 다리 들기
    // 왼쪽 다리 들어서 특별한 이벤트 발생시키게 하기
    // 이벤트 발생 시 말풍선 내용 바꾸기
    // NPC 움직이면서 왼쪽! 오른쪽! 하며 앞으로 이동
    // NPC 특정 위치 도달 시 이제 혼자서 해보도록 해! 말풍선 출력하고
    // 일정 시간 지나면 말풍선 비활성화

    private void Start()
    {
        defaultBodyPos = body.localPosition;
        defaultLeftLegPos = leftLeg.localPosition;
        defaultRightLegPos = rightLeg.localPosition;

        defaultBodyRotation = body.localRotation;
        defaultLeftLeg = leftLeg.localRotation;
        defaultRightLeg = rightLeg.localRotation;
        clickLeftLeg = defaultLeftLeg * Quaternion.Euler(new Vector3(-90f, 0, -90f));
        clickRightLeg = defaultRightLeg * Quaternion.Euler(new Vector3(90f, 0, 90f));
        clickLeftBody = defaultBodyRotation * Quaternion.Euler(new Vector3(8, 3, -24));
        clickRightBody = defaultBodyRotation * Quaternion.Euler(new Vector3(8, 3, 24));

        clickedRight_leftLeg = defaultLeftLeg * Quaternion.Euler(new Vector3(0, 0, -13.283f));
        clickedLeft_rightLeg = defaultRightLeg * Quaternion.Euler(new Vector3(0, 0, 13.283f));
    }

    public void NPCLeftLegMovement()
    {
        NPCInit();
        isLeft = true;
        leftLeg.localPosition = clickLeftLegPos;
        body.localPosition = clickLeftBodyPos;
        leftLeg.localRotation = defaultLeftLeg * clickLeftLeg;
        rightLeg.localRotation = defaultRightLeg * clickedLeft_rightLeg;
        body.localRotation = defaultBodyRotation * clickLeftBody;
        timer += Time.deltaTime;
    }

    public void NPCRightLegMovement()
    {
        NPCInit();
        isRight = true;
        rightLeg.localPosition = clickRightLegPos;
        body.localPosition = clickRightBodyPos;
        rightLeg.localRotation = defaultRightLeg * clickRightLeg;
        leftLeg.localRotation = defaultLeftLeg * clickedRight_leftLeg;
        body.localRotation = defaultBodyRotation * clickRightBody;
        timer += Time.deltaTime;
    }

    public void NPCInit()
    {
        // 로컬 포지션 초기화
        rightLeg.localPosition = defaultRightLegPos;
        leftLeg.localPosition = defaultLeftLegPos;
        body.localPosition = defaultBodyPos;

        // 로테이션 초기화
        rightLeg.localRotation = defaultRightLeg;
        leftLeg.localRotation = defaultLeftLeg;
        body.localRotation = defaultBodyRotation;
    }

    public IEnumerator NPC_Movement_co()
    {
        while (true)
        {
            NPCRightLegMovement();
            transform.RotateAround(leftLeg.position, Vector3.up, rotSpeed * Time.deltaTime * GetAngle(leftLeg.position, rightLeg.position, point.position));
            yield return new WaitForSeconds(0.5f);
            NPCLeftLegMovement();
            transform.RotateAround(rightLeg.position, Vector3.up, rotSpeed * Time.deltaTime * GetAngle(rightLeg.position, leftLeg.position, point.position));
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

    public IEnumerator SlidingNPC_co()
    {
        int index = 0;
        while (true)
        {
            Vector3 dir = (NPCTp[index].position - transform.position).normalized;
            transform.position += dir * 5f * Time.deltaTime;
            transform.LookAt(NPCTp[index]);
            if(Vector3.Distance(transform.position, NPCTp[index].position) < 0.02)
            {
                index++;
                if(index > 3)
                {
                    index = 0;
                }
            }
            yield return null;
        }
    }
}

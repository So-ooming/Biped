using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] Transform leftLeg;
    [SerializeField] Transform rightLeg;
    [SerializeField] Transform body;
    [SerializeField] float rotSpeed = 10f;
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

    // 우클릭했을 때 포지션 변화값, 왼쪽 다리 회전값
    Vector3 clickRightBodyPos = new Vector3(0.00064f, 0.00028f, 0);
    Vector3 clickRightLegPos = new Vector3(0.00395f, -0.0003f, 0);
    Quaternion clickedRight_leftLeg;

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
        defaultBodyRotation = body.localRotation;
        defaultLeftLeg = leftLeg.localRotation;
        defaultRightLeg = rightLeg.localRotation;
        clickLeftLeg = defaultLeftLeg * Quaternion.Euler(new Vector3(-90f, 0, 90f));
        clickRightLeg = defaultRightLeg * Quaternion.Euler(new Vector3(90f, 0, 90f));
        clickLeftBody = defaultBodyRotation * Quaternion.Euler(new Vector3(30f, 0, 0));
        clickRightBody = defaultBodyRotation * Quaternion.Euler(new Vector3(8, 3, 24));

        clickedRight_leftLeg = defaultLeftLeg * Quaternion.Euler(new Vector3(0, 0, -13.283f));
    }

    public void NPCLeftLegMovement()
    {
        isLeft = true;
        leftLeg.localRotation = defaultLeftLeg * clickLeftLeg;
        body.localRotation = defaultBodyRotation * clickLeftBody;
    }

    public void NPCRightLegMovement()
    {
        isRight = true;
        rightLeg.localPosition = clickRightLegPos;
        body.localPosition = clickRightBodyPos;
        rightLeg.localRotation = defaultRightLeg * clickRightLeg;
        leftLeg.localRotation = defaultLeftLeg * clickedRight_leftLeg;
        body.localRotation = defaultBodyRotation * clickRightBody;
        timer += Time.deltaTime;
    }
}

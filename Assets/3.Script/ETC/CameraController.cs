using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Camera cam;
    [SerializeField] Transform[] changePoint;
    public CinemachineVirtualCamera[] vcam;

    public int currentPoint = 0;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        PlayerPositionCheck(currentPoint);
    }

    void PlayerPositionCheck(int current)
    {
        /*if(currentPoint > 1)
        {
            currentPoint = 1;
        }*/
        float dis = Vector3.Distance(player.position, changePoint[current].position);
        if(current == 0)
        {
            if (dis <= 2f)
            {
                Debug.Log("0번 포인트 사정거리 안에 들어왔다잉");
                if (changePoint[current].position.z < player.position.z)
                {
                    vcam[1].transform.gameObject.SetActive(true);
                    vcam[0].transform.gameObject.SetActive(false);
                }

                else if (changePoint[current].position.z > player.position.z)
                {
                    vcam[1].transform.gameObject.SetActive(false);
                    vcam[0].transform.gameObject.SetActive(true);
                }
            }
        }
        /*else if(current == 1)
        {
            if (dis <= 4f)
            {
                Debug.Log("1번 포인트 사정거리 안에 들어왔다잉");
                if (changePoint[current].position.z < player.position.z)
                {
                    vcam[1].transform.gameObject.SetActive(false);
                    vcam[0].transform.gameObject.SetActive(true);
                }
            }
        }*/
    }

    public void SecondNPC_Cam()
    {
        vcam[1].gameObject.SetActive(false);
        vcam[4].gameObject.SetActive(true);
    }
}

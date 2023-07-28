using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
     public string[] script = {"안녕, 우주의 구원자들!  지구로 가기 전에 약간의 훈련이 필요해!",
    "다음은 조금 더 실용적인 걸 가르쳐 줄게! 나처럼 오른쪽 다리를 들어봐!", // ~ 패널
    "오른쪽 다리를 들어서 공중에 있는 노란색 표식을 밟아봐.",              // 말풍선 ~
    "오른쪽 다리를 들어서 궤적에 따라 회전시켜보자.",
    "다리를 바꿔서 해볼까?",
    "왼쪽 다리를 들어서 공중에 있는 노란색 표식을 밟아봐",
    "왼쪽 다리를 들어서 궤적에 따라 회전시켜보자.",
    "그럼 이제 걸어보자!",                                              // NPC 걷기
    "오른쪽! 왼쪽! 오른쪽! 왼쪽!",
    "계속 앞으로!",
    "이제 쭉 나아가도록 해!",                                         // 첫 번째 NPC 말풍선 끝
    "이렇게 매끈한 바닥에선 활주를 할 수 있어!",                         // 두 번째 NPC
    "이제 활주를 해볼까?",                                             // 두 번째 NPC 끝
    "어이! 신입은 오랜만이네. 방금 떨어질 때 다리를 다친 건 아니겠지?",   // 세 번째 NPC
    "잘 들어~ 우리 \'바이페드\'에게 가장 중요한 건 이 두 발이야! 꼭 이 두 발을 잘 사용하도록 해.",
    "이제 두 발을 사용해서 저 쪽에 있는 보물 상자를 열어보도록 해.",
    "바이페드의 발을 사용하면 다양한 시설을 사용할 수 있어. 꼭 열심히 연습하도록 해!", // 세 번째 NPC 끝
    "이 문은 어떻게 여는지 알아?",
    "성공이야! 문이 열렸어.",
    "계속 전진!",
    "여기까지 잘 해낼 줄 알았어!",
    "이 문을 열어줄게!",
    "훈련은 여기까지! 앞으로는 훈련이 아니라 실전이야!",
    "너의 임무는 지구로 가서 꺼져버린 등대를 다시 밝히는 거야. 모든 은하계가 네 손에 달렸어! " +
            "아... 물론 모든 은하계는 좀 오버긴 하지만 하하하~ 너라면 멋지게 해낼 수 있을 거야!"};

    [Header("패널 및 텍스트 이미지")]
    public GameObject dialogBox;
    public GameObject tutoPanel;
    public Text panelText;
    public Text speechText;
    public Sprite leftClickImage;
    public Sprite wheelClickImage;

    [Header("인덱스 번호")]
    public int currentDialog = 0;
    public int currentNPC = 0;

    [Header("현재 NPC")]
    public NPCController[] NPC;

    [Header("말풍선 위치 관련")]
    public RectTransform bubbleTransform;
    [SerializeField] Transform targetTransform;
    [SerializeField] Vector3 distance = Vector3.up * 40f;

    [Header("Other Component")]
    [SerializeField] PlayerController player;
    [SerializeField] CircularArrangement ca;
    [SerializeField] GameObject firstPyosik;
    [SerializeField] CameraController cameraController;

    GameObject secondPyosik;
    bool isCoroutinePlay = false;

    private void Start()
    {
        //NPC = FindObjectsOfType<NPCController>();
        StartCoroutine(Typing(panelText, script[currentDialog]));
        GameManager.instance.isPause = true;
        GameManager.instance.CoinUI.SetActive(false);
        player = FindObjectOfType<PlayerController>();
        ca = FindObjectOfType<CircularArrangement>();
        firstPyosik = GameObject.FindGameObjectWithTag("FPyosik");
        secondPyosik = GameObject.FindGameObjectWithTag("SPyosik");
        cameraController = FindObjectOfType<CameraController>();
        firstPyosik.SetActive(false);
        secondPyosik.SetActive(false);
        SoundManager.instance.PlaySFX("NPCAppear");
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)) && dialogBox.activeSelf)
        {
            currentDialog++;
            if (currentDialog > 1 && currentDialog < 10)
            {
                GameManager.instance.isPause = false;
                GameManager.instance.CoinUI.SetActive(true);
                dialogBox.SetActive(false);
                bubbleTransform.gameObject.SetActive(true);
            }
            else if (currentDialog > 10)
            {
                if(currentDialog < 24)
                {
                    StartCoroutine(Typing(panelText, script[currentDialog]));
                    if (currentDialog == 13)
                    {
                        GameManager.instance.isPause = false;
                        GameManager.instance.CoinUI.SetActive(true);
                        dialogBox.SetActive(false);
                        tutoPanel.SetActive(true);
                        tutoPanel.transform.GetChild(0).GetComponent<Image>().enabled = true;
                        tutoPanel.transform.GetChild(0).GetComponent<Image>().sprite = wheelClickImage;
                        tutoPanel.transform.GetComponentInChildren<Text>().text = "마우스 가운데의 휠 버튼을 클릭하면 활주할 수 있습니다.";
                        cameraController.vcam[3].gameObject.SetActive(false);
                        cameraController.vcam[1].gameObject.SetActive(true);
                        StartCoroutine(NPC[currentNPC].SlidingNPC_co());
                    }
                    if (currentDialog == 16)
                    {
                        GameManager.instance.isPause = false;
                        GameManager.instance.CoinUI.SetActive(true);
                        dialogBox.SetActive(false);
                        tutoPanel.SetActive(true);
                        tutoPanel.transform.GetChild(0).GetComponent<Image>().enabled = false;
                        tutoPanel.transform.GetComponentInChildren<Text>().text = "두 발을 상자의 노란 부분에 일정 시간 이상 접촉시키면 보물 상자가 열립니다!";
                    }
                    if (currentDialog == 17)
                    {
                        GameManager.instance.isPause = false;
                        GameManager.instance.CoinUI.SetActive(true);
                        dialogBox.SetActive(false);
                        cameraController.vcam[4].gameObject.SetActive(false);
                        cameraController.vcam[0].gameObject.SetActive(true);
                    }
                    if (currentDialog == 18)
                    {
                        GameManager.instance.isPause = false;
                        GameManager.instance.CoinUI.SetActive(true);
                        dialogBox.SetActive(false);
                        tutoPanel.SetActive(true);
                        tutoPanel.transform.GetComponentInChildren<Text>().text = "문 앞의 스위치를 일정 시간 이상 밟으면 문이 열립니다!";
                    }
                    if (currentDialog == 20)
                    {
                        GameManager.instance.isPause = false;
                        GameManager.instance.CoinUI.SetActive(true);
                        dialogBox.SetActive(false);
                    }
                }
                
                else
                {
                    dialogBox.SetActive(false);
                    GameManager.instance.Ending();
                }
            }
            else
            {
                StartCoroutine(Typing(panelText, script[currentDialog]));
            }
        }

        if((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)) && currentDialog == 2 && bubbleTransform.gameObject.activeSelf && !isCoroutinePlay)
        {
            StartCoroutine(Typing(speechText, script[currentDialog]));
            NPC[currentNPC].NPCRightLegMovement();
            //currentDialog++;
            //player.isRight = true;
            firstPyosik.SetActive(true);
            tutoPanel.SetActive(true);
        }

        if((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)) && currentDialog == 4 && !isCoroutinePlay)
        {
            currentDialog++;
            StartCoroutine(Typing(speechText, script[currentDialog]));
            NPC[currentNPC].NPCLeftLegMovement();
            //player.isLeft = true;
            secondPyosik.SetActive(true);
            tutoPanel.transform.GetChild(0).GetComponent<Image>().sprite = leftClickImage;
            tutoPanel.transform.GetChild(1).GetComponent<Text>().text =
                "마우스 좌클릭으로 왼쪽 다리를 컨트롤 할 수 있습니다.";
        }

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)) && currentDialog == 7 && !isCoroutinePlay)
        {
            currentDialog++;
            StartCoroutine(Typing(speechText, script[currentDialog]));
            tutoPanel.transform.GetChild(0).GetComponent<Image>().enabled = false;
            tutoPanel.transform.GetChild(1).GetComponent<Text>().text =
                "마우스 좌우클릭을 번갈아가며 앞으로 이동하세요.";
        }

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)) && currentDialog == 8 && !isCoroutinePlay)
        {
            currentDialog++;
            StartCoroutine(Typing(speechText, script[currentDialog]));
        }
        
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)) && currentDialog == 9 && !isCoroutinePlay)
        {
            currentDialog++;
            StartCoroutine(Typing(speechText, script[currentDialog]));
        }

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)) && currentDialog == 10 && !isCoroutinePlay)
        {
            cameraController.vcam[2].transform.gameObject.SetActive(false);
            cameraController.vcam[0].transform.gameObject.SetActive(true);
            bubbleTransform.gameObject.SetActive(false);
            tutoPanel.SetActive(false);
            currentDialog++;
            currentNPC++;
        }
    }

    public IEnumerator Typing(Text typingText, string message)
    {
        SoundManager.instance.PlaySFX("Dialog");
        isCoroutinePlay = true;
        for(int i = 0; i < message.Length; i++)
        {
            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(0.03f);
        }
        isCoroutinePlay = false;
    }

    void Setup(Transform target)
    {
        targetTransform = target;
    }

    private void LateUpdate()
    {
        if (targetTransform == null)
        {
            return;
        }

        Vector3 ScreenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        bubbleTransform.position = ScreenPosition + distance;
    }
}

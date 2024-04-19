using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
     public string[] script = {"�ȳ�, ������ �����ڵ�!  ������ ���� ���� �ణ�� �Ʒ��� �ʿ���!",
    "������ ���� �� �ǿ����� �� ������ �ٰ�! ��ó�� ������ �ٸ��� ����!", // ~ �г�
    "������ �ٸ��� �� ���߿� �ִ� ����� ǥ���� ��ƺ�.",              // ��ǳ�� ~
    "������ �ٸ��� �� ������ ���� ȸ�����Ѻ���.",
    "�ٸ��� �ٲ㼭 �غ���?",
    "���� �ٸ��� �� ���߿� �ִ� ����� ǥ���� ��ƺ�",
    "���� �ٸ��� �� ������ ���� ȸ�����Ѻ���.",
    "�׷� ���� �ɾ��!",                                              // NPC �ȱ�
    "������! ����! ������! ����!",
    "��� ������!",
    "���� �� ���ư����� ��!",                                         // ù ��° NPC ��ǳ�� ��
    "�̷��� �Ų��� �ٴڿ��� Ȱ�ָ� �� �� �־�!",                         // �� ��° NPC
    "���� Ȱ�ָ� �غ���?",                                             // �� ��° NPC ��
    "����! ������ �������̳�. ��� ������ �� �ٸ��� ��ģ �� �ƴϰ���?",   // �� ��° NPC
    "�� ���~ �츮 \'�������\'���� ���� �߿��� �� �� �� ���̾�! �� �� �� ���� �� ����ϵ��� ��.",
    "���� �� ���� ����ؼ� �� �ʿ� �ִ� ���� ���ڸ� ������� ��.",
    "��������� ���� ����ϸ� �پ��� �ü��� ����� �� �־�. �� ������ �����ϵ��� ��!", // �� ��° NPC ��
    "�� ���� ��� ������ �˾�?",
    "�����̾�! ���� ���Ⱦ�.",
    "��� ����!",
    "������� �� �س� �� �˾Ҿ�!",
    "�� ���� �����ٰ�!",
    "�Ʒ��� �������! �����δ� �Ʒ��� �ƴ϶� �����̾�!",
    "���� �ӹ��� ������ ���� �������� ��븦 �ٽ� ������ �ž�. ��� ���ϰ谡 �� �տ� �޷Ⱦ�! " +
            "��... ���� ��� ���ϰ�� �� ������ ������ ������~ �ʶ�� ������ �س� �� ���� �ž�!"};

    [Header("�г� �� �ؽ�Ʈ �̹���")]
    public GameObject dialogBox;
    public GameObject tutoPanel;
    public Text panelText;
    public Text speechText;
    public Sprite leftClickImage;
    public Sprite wheelClickImage;

    [Header("�ε��� ��ȣ")]
    public int currentDialog = 0;
    public int currentNPC = 0;

    [Header("���� NPC")]
    public NPCController[] NPC;

    [Header("��ǳ�� ��ġ ����")]
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
                        tutoPanel.transform.GetComponentInChildren<Text>().text = "���콺 ����� �� ��ư�� Ŭ���ϸ� Ȱ���� �� �ֽ��ϴ�.";
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
                        tutoPanel.transform.GetComponentInChildren<Text>().text = "�� ���� ������ ��� �κп� ���� �ð� �̻� ���˽�Ű�� ���� ���ڰ� �����ϴ�!";
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
                        tutoPanel.transform.GetComponentInChildren<Text>().text = "�� ���� ����ġ�� ���� �ð� �̻� ������ ���� �����ϴ�!";
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
                "���콺 ��Ŭ������ ���� �ٸ��� ��Ʈ�� �� �� �ֽ��ϴ�.";
        }

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)) && currentDialog == 7 && !isCoroutinePlay)
        {
            currentDialog++;
            StartCoroutine(Typing(speechText, script[currentDialog]));
            tutoPanel.transform.GetChild(0).GetComponent<Image>().enabled = false;
            tutoPanel.transform.GetChild(1).GetComponent<Text>().text =
                "���콺 �¿�Ŭ���� �����ư��� ������ �̵��ϼ���.";
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

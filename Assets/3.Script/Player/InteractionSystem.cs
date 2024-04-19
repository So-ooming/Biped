using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [Header("½Ã°£ °ü·Ã º¯¼ö")]
    [SerializeField] float pressTimer = 0f;
    [SerializeField] float pressDuration = 2f;
    [SerializeField] float coinDuration = 1.5f;
    [SerializeField] float coinForce = 50f;

    [Header("¿ÀºêÁ§Æ®")]
    [SerializeField] GameObject doorSwitch;
    [SerializeField] GameObject Coin;

    [Header("¸ÓÅ×¸®¾ó")]
    [SerializeField] Material pressedMaterial;
    [SerializeField] Material defaultMaterial;

    [Header("Rigidbody")]
    [SerializeField] Rigidbody foot;

    [Header("Æ©Åä¸®¾ó °ü·Ã")]
    public int pyosikCnt = 0;
    [SerializeField] CircularArrangement ca;
    [SerializeField] NPCManager npcManager;
    [SerializeField] PlayerController player;
    //[SerializeField] FixedJoint playerfj;
    //[SerializeField] FixedJoint objfj;

    //Vector3 defSwitchPos;
    //Vector3 switchValue;

    Quaternion doorRot = Quaternion.Euler(new Vector3(0, 120, 0));
    [SerializeField] float rotSpeed = 5f;
    public bool isOpen = false;

    private void Start()
    {
        defaultMaterial = doorSwitch.transform.parent.transform.GetComponent<MeshRenderer>().material;
        foot = transform.GetComponent<Rigidbody>();
        ca = FindObjectOfType<CircularArrangement>();
        npcManager = FindObjectOfType<NPCManager>();
        player = FindObjectOfType<PlayerController>();
        //playerfj = transform.GetComponent<FixedJoint>();
        //defSwitchPos = doorSwitch.transform.parent.transform.localPosition;
        //switchValue = doorSwitch.transform.parent.transform.localPosition + new Vector3(0, -0.0002f, 0);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pyosik"))
        {
            pyosikCnt++;
            Destroy(other.gameObject);
            if (pyosikCnt == 12)
            {
                pyosikCnt = 0;
                npcManager.currentDialog++;
                StartCoroutine(npcManager.Typing(npcManager.speechText, npcManager.script[npcManager.currentDialog]));
                //Debug.Log(npcManager.currentDialog);
                npcManager.NPC[npcManager.currentNPC].NPCInit();
                //StartCoroutine(npcController.NPC_Movement_co());
            }
        }

        if (other.CompareTag("DoorOpen"))
        {
            other.transform.parent.transform.GetComponent<MeshRenderer>().material = pressedMaterial;
            //doorSwitch.transform.parent.transform.localPosition = switchValue;
            Debug.Log("¹öÆ°¿¡ ´êÀ½");
        }

        if (other.CompareTag("tutoDoorOpen"))
        {
            other.transform.parent.transform.GetComponent<MeshRenderer>().material = pressedMaterial;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DoorOpen"))
        {
            pressTimer += Time.deltaTime;
            if (pressTimer >= pressDuration)
            {
                DoorOpen(other);
                StartCoroutine(DoorOpenDelay_co(other));
                Debug.Log("¹® ¿­·È´ç");
            }
        }

        if (other.CompareTag("CoinContainer"))
        {
            Debug.Log("¶Ñ²±¿¡ ´êÀ½");
            pressTimer += Time.deltaTime;
            if (pressTimer >= coinDuration)
            {
                pressTimer = 0;
                other.transform.GetComponent<Rigidbody>().isKinematic = false;
                //coinContainer.AddForce(Vector3.up * coinForce, ForceMode.Impulse);
                other.transform.localRotation = Quaternion.Slerp(other.transform.rotation, Quaternion.Euler(new Vector3(14f, 0, -180f)), 100f * Time.deltaTime);
                StartCoroutine(SpawnCoin(other));
                Debug.Log("ÄÚÀÎ ¶Ñ²± µûÁü");
            }
        }

        if (other.CompareTag("tutoCoinContainer"))
        {
            Debug.Log("¶Ñ²±¿¡ ´êÀ½");
            pressTimer += Time.deltaTime;
            if (pressTimer >= coinDuration)
            {
                GameManager.instance.isPause = true;
                pressTimer = 0;
                isOpen = true;
                other.transform.GetComponent<Rigidbody>().isKinematic = false;
                //coinContainer.AddForce(Vector3.up * coinForce, ForceMode.Impulse);
                other.transform.localRotation = Quaternion.Slerp(other.transform.rotation, Quaternion.Euler(new Vector3(14f, 0, -180f)), 100f * Time.deltaTime);
                StartCoroutine(SpawnCoin(other));
                player.PlayerDefaultState();
                npcManager.tutoPanel.SetActive(false);
                npcManager.dialogBox.SetActive(true);
                StartCoroutine(npcManager.Typing(npcManager.speechText, npcManager.script[npcManager.currentDialog]));
                Debug.Log("ÄÚÀÎ ¶Ñ²± µûÁü");
            }
        }

        if (other.CompareTag("tutoDoorOpen"))
        {
            //doorSwitch.transform.parent.transform.localPosition = switchValue;
            Debug.Log("¹öÆ°¿¡ ´êÀ½");
            pressTimer += Time.deltaTime;
            if (pressTimer >= pressDuration)
            {
                DoorOpen(other);
                Debug.Log("¹® ¿­·È´ç");
                StartCoroutine(DoorOpenDelay_co(other));
                
                npcManager.tutoPanel.SetActive(false);
                npcManager.dialogBox.SetActive(true);
                StartCoroutine(npcManager.Typing(npcManager.speechText, npcManager.script[npcManager.currentDialog]));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DoorOpen"))
        {
            other.transform.parent.transform.GetComponent<MeshRenderer>().material = defaultMaterial;
            //doorSwitch.transform.parent.transform.localPosition = defSwitchPos;
            pressTimer = 0f;
        }

        if (other.CompareTag("tutoDoorOpen"))
        {
            other.transform.parent.transform.GetComponent<MeshRenderer>().material = defaultMaterial;
            //doorSwitch.transform.parent.transform.localPosition = defSwitchPos;
            pressTimer = 0f;
        }

        if (other.CompareTag("CoinContainer"))
        {
            //coinContainer.isKinematic = true;
            pressTimer = 0f;
        }
    }

    void DoorOpen(Collider other)
    {
        other.transform.parent.parent.parent.parent.parent.parent.GetChild(5).GetChild(1).GetChild(0).localRotation = 
            Quaternion.Slerp(other.transform.parent.parent.parent.parent.parent.parent.GetChild(5).GetChild(1).GetChild(0).rotation, doorRot, rotSpeed * Time.deltaTime);
        other.transform.parent.parent.parent.parent.parent.parent.GetChild(4).GetChild(1).GetChild(0).localRotation = 
            Quaternion.Slerp(other.transform.parent.parent.parent.parent.parent.parent.GetChild(4).GetChild(1).GetChild(0).rotation, Quaternion.Inverse(doorRot), rotSpeed * Time.deltaTime);
    }

    IEnumerator DoorOpenDelay_co(Collider other)
    {
        yield return new WaitForSeconds(1.5f);
        other.transform.parent.transform.GetComponent<MeshRenderer>().material = defaultMaterial;
        other.enabled = false;
        pressTimer = 0;
    }

    IEnumerator SpawnCoin(Collider other)
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject coin = Instantiate(Coin, other.transform.position, Quaternion.identity);
            //Debug.Log(coin.transform.position);
            yield return new WaitForSeconds(0.5f);
        }

        StopCoroutine("SpawnCoin");
    }
}

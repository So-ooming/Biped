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
    //[SerializeField] FixedJoint playerfj;
    //[SerializeField] FixedJoint objfj;

    //Vector3 defSwitchPos;
    //Vector3 switchValue;

    Quaternion doorRot = Quaternion.Euler(new Vector3(0, 120, 0));
    [SerializeField] float rotSpeed = 5f;

    private void Start()
    {
        defaultMaterial = doorSwitch.transform.parent.transform.GetComponent<MeshRenderer>().material;
        foot = transform.GetComponent<Rigidbody>();
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
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DoorOpen"))
        {
            other.transform.parent.transform.GetComponent<MeshRenderer>().material = pressedMaterial;
            //doorSwitch.transform.parent.transform.localPosition = switchValue;
            Debug.Log("¹öÆ°¿¡ ´êÀ½");
            pressTimer += Time.deltaTime;
            if (pressTimer >= pressDuration)
            {
                DoorOpen(other);
                Debug.Log("¹® ¿­·È´ç");
            }
        }

        if (other.CompareTag("CoinContainer"))
        {
            Debug.Log("¶Ñ²±¿¡ ´êÀ½");
            pressTimer += Time.deltaTime;
            if (pressTimer >= coinDuration)
            {
                other.transform.GetComponent<Rigidbody>().isKinematic = false;
                //coinContainer.AddForce(Vector3.up * coinForce, ForceMode.Impulse);
                other.transform.localRotation = Quaternion.Slerp(other.transform.rotation, Quaternion.Euler(new Vector3(14f, 0, -180f)), 100f * Time.deltaTime);
                StartCoroutine(SpawnCoin(other));
                Debug.Log("ÄÚÀÎ ¶Ñ²± µûÁü");
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

    IEnumerator FixedFoot(Collider other)
    {
        yield return new WaitForSeconds(0.2f);
        //playerfj.connectedBody = coinContainer;
        //objfj.connectedBody = foot;
    }

    IEnumerator SpawnCoin(Collider other)
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject coin = Instantiate(Coin, other.transform.position, Quaternion.identity);
            Debug.Log(coin.transform.position);
            yield return new WaitForSeconds(0.5f);
        }

        StopCoroutine("SpawnCoin");
    }
}

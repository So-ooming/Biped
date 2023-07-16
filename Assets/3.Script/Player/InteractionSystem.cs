using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] float pressTimer = 0f;
    [SerializeField] float pressDuration = 2f;
    [SerializeField] float coinDuration = 1.5f;
    [SerializeField] float coinForce = 50f;
    [SerializeField] GameObject doorSwitch;
    [SerializeField] Material pressedMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] Rigidbody coinContainer;
    [SerializeField] Rigidbody foot;

    //Vector3 defSwitchPos;
    //Vector3 switchValue;

    Quaternion doorRot = Quaternion.Euler(new Vector3(0, 120, 0));
    [SerializeField] float rotSpeed = 5f;

    private void Start()
    {
        defaultMaterial = doorSwitch.transform.parent.transform.GetComponent<MeshRenderer>().material;
        foot = transform.GetComponent<Rigidbody>();
        //defSwitchPos = doorSwitch.transform.parent.transform.localPosition;
        //switchValue = doorSwitch.transform.parent.transform.localPosition + new Vector3(0, -0.0002f, 0);
    }

    private void Update()
    {
        Debug.Log(coinContainer.transform.rotation);
        if (coinContainer.transform.rotation.x >= 14f)
        {
            coinContainer.isKinematic = true;
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
            if(pressTimer >= coinDuration)
            {
                coinContainer.isKinematic = false;
                coinContainer.AddForce(Vector3.up * coinForce, ForceMode.Impulse);

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
}

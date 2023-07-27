using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        StartCoroutine("Slowly");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetComponent<Image>().color = new Color32(255, 255, 255, 50);
        transform.localScale = new Vector3(1, 1, 1);
    }

    IEnumerator Slowly()
    {
        Vector3 a = new Vector3(0.008f, 0.008f, 0);
        for(int i = 0; i < 25; i++)
        {
            transform.localScale += a;
            yield return null;
        }
    }
}

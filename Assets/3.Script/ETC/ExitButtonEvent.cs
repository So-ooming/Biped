using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExitButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Text text;
    Image image;

    private void Start()
    {
        image = transform.GetChild(1).GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //text.fontSize = 40;
        StartCoroutine(Slowly());
        text.color = new Color32(255, 180, 0, 255);
        image.gameObject.SetActive(false);
        SoundManager.instance.PlaySFX("MousePointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //text.fontSize = 35;
        text.color = new Color32(89, 89, 89, 255);
        image.gameObject.SetActive(true);
        transform.localScale = new Vector3(1, 1, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //text.fontSize = 35;
        text.color = new Color32(89, 89, 89, 255);
        image.gameObject.SetActive(true);
        transform.localScale = new Vector3(1, 1, 0);
        SoundManager.instance.PlaySFX("MousePointerEnter");
    }

    IEnumerator Slowly()
    {
        for (int i = 0; i < 20; i++)
        {
            transform.localScale += new Vector3(0.01f, 0.01f, 0);
            yield return null;
        }
        StopCoroutine(Slowly());
    }
}

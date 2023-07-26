using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThirdSceneButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Image image;
    [SerializeField] Image keyImage;
    [SerializeField] Text text;

    private void Start()
    {
        keyImage = transform.GetChild(1).GetComponent<Image>();
        image = transform.GetComponent<Image>();
        text = transform.GetComponentInChildren<Text>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color32(255, 255, 255, 255);
        text.color = new Color32(255, 185, 0, 255);
        keyImage.gameObject.SetActive(false);
        StartCoroutine(Slowly());
        SoundManager.instance.PlaySFX("MousePointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color32(255, 255, 255, 100);
        text.color = new Color32(50, 50, 50, 255);
        transform.localScale = new Vector3(1, 1, 0);
        keyImage.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        image.color = new Color32(255, 255, 255, 100);
        text.color = new Color32(50, 50, 50, 255);
        transform.localScale = new Vector3(1, 1, 0);
        keyImage.gameObject.SetActive(true);
        SoundManager.instance.PlaySFX("MousePointerEnter");
    }

    IEnumerator Slowly()
    {
        for(int i = 0; i < 20; i++)
        {
            transform.localScale += new Vector3(0.01f, 0.01f, 0);
            yield return null;
        }
        StopCoroutine(Slowly());
    }
}

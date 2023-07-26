using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Button btn;
    [SerializeField] Image image;
    [SerializeField] Image btnBack;
    [SerializeField] Text text;
    [SerializeField] Sprite newSprite;       // 버튼 배경 이미지

    private void Start()
    {
        btn = transform.GetComponentInChildren<Button>();
        btnBack = btn.transform.GetComponent<Image>();
        image = transform.GetChild(1).GetComponent<Image>();
        text = transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.gameObject.SetActive(true);
        btnBack.sprite = newSprite;
        btnBack.color = new Color(255, 255, 255, 255);
        text.color = new Color32(255, 185, 0, 255);
        transform.GetComponent<Animator>().enabled = true;
        SoundManager.instance.PlaySFX("MousePointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.gameObject.SetActive(false);
        btnBack.sprite = null;
        btnBack.color = new Color(255, 255, 255, 0);
        text.color = new Color(0, 0, 0, 255);
        transform.GetComponent<Animator>().enabled = false;
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        image.gameObject.SetActive(false);
        btnBack.sprite = null;
        btnBack.color = new Color(255, 255, 255, 0);
        text.color = new Color(0, 0, 0, 255);
        transform.GetComponent<Animator>().enabled = false;
        transform.localScale = new Vector3(1, 1, 1);
        SoundManager.instance.PlaySFX("MousePointerEnter");
    }
}

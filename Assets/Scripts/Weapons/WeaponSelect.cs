using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject box;

    private void Awake()
    {
        box.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        box.SetActive(true);
        this.transform.localScale = Vector3.one * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        box.SetActive(false);
        this.transform.localScale = Vector3.one;
    }
}

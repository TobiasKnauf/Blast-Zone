using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStatsDescriptionUpdater : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string desc;
    [SerializeField] private TMP_Text box;

    public void OnPointerEnter(PointerEventData eventData)
    {
        box.text = desc;
        this.transform.localScale = Vector3.one * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.localScale = Vector3.one;

    }
}

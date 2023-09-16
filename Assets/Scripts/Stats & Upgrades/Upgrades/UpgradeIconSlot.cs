using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeIconSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text level;

    private ScriptableUpgrade upgrade;
    private TMP_Text descriptionWindow;

    public void Init(ScriptableUpgrade _upgrade, TMP_Text _textWindow)
    {
        upgrade = _upgrade;

        icon.sprite = _upgrade.Icon;
        descriptionWindow = _textWindow;
    }

    private void Update()
    {
        level.text = $"{upgrade.CurrentLevel} / {upgrade.MaxLevels}";
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        descriptionWindow.text = upgrade.DescriptionText;
        this.transform.localScale = Vector3.one * 1.2f;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.localScale = Vector3.one;
    }
}

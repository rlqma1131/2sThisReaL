using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildItemUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Button selectButton;

    public void Setup(BuildItemData data, int count, bool isAvailable, System.Action onClick)
    {
        iconImage.sprite = data.icon;
        countText.text = $"x{count}";
        
        Color color = isAvailable ? Color.white : new Color(1, 1, 1, 0.4f);
        iconImage.color = color;
        countText.color = color;
        selectButton.interactable = isAvailable;
        
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => onClick?.Invoke());
    }
}

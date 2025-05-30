using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI makeCountText; // 제작 가능한 개수 표시용 텍스트

    [SerializeField] private GameObject resourceIcon; // 재료 아이콘 UI 오브젝트

    public void SetItem(ItemData data, int count)
    {
        iconImage.sprite = data.itemIcon;
        makeCountText.text = count >= 1 ? count.ToString() : string.Empty;
        resourceIcon.SetActive(data != null); // 아이템이 null이 아닐 때만 아이콘 표시

    }

}

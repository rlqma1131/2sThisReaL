using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building/Build Item")]
public class BuildItemData : ScriptableObject
{
    public string name;
    public Sprite icon;
    public GameObject previewPrefab;
    public int itemID;
}

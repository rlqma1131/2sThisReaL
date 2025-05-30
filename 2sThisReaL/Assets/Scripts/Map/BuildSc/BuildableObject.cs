using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BuildableObject : MonoBehaviour, IBuildable
{
    private float currentRotation = 0;
    [SerializeField] private int cost = 10;
    [SerializeField] private int itemID;
    [SerializeField] private GameObject finalPrefab;
    
    public void InitializePreview()
    {
        SetTransparent(true);
    }

    public void SetPreviewPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void RotatePreview()
    {
        currentRotation += 90f;
        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    }

    public void CancelPreview()
    {
        Destroy(gameObject);
    }

    public GameObject GetFinalPrefab()
    {
        return finalPrefab;
    }

    public int GetCost()
    {
        return cost;
    }

    public int GetItemID()
    {
        return itemID;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }
    
    public Vector3 GetCurrentPosition()
    {
        return transform.position;
    }

    private void SetTransparent(bool transparent)
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in renderer.materials)
            {
                if (transparent)
                {
                    mat.shader = Shader.Find("Transparent/Diffuse");
                    Color c = mat.color;
                    c.a = 0.5f;
                    mat.color = c;
                }
                else
                {
                    mat.shader = Shader.Find("Standard");
                    Color c = mat.color;
                    c.a = 1f;
                    mat.color = c;
                }
            }
        }
    }
}

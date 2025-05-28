using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BuildableObject : MonoBehaviour, IBuildable
{
    private float currentRotation = 0;
    [SerializeField] private int cost = 10;
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
        // // 프리뷰 오브젝트 대신 최종 프리팹을 인스턴스화하여 반환
        // GameObject finalObject = Instantiate(finalPrefab, transform.position, transform.rotation);
        //
        // foreach (var renderer in finalObject.GetComponentsInChildren<Renderer>())
        // {
        //     foreach (var mat in renderer.materials)
        //     {
        //         mat.shader = Shader.Find("Standard");
        //         Color c = mat.color;
        //         c.a = 1;
        //         mat.color = c;
        //     }
        // }
        //
        // Destroy(gameObject); // 프리뷰 오브젝트 제거
        return finalPrefab;
    }

    public int GetCost()
    {
        return cost;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
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

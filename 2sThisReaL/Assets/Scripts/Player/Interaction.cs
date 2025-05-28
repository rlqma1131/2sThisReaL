using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngineInternal;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    public IInteractable curInteractable;

    public TextMeshProUGUI prompText;

    public Transform rayOrigin;

    void Start()
    {
        if (rayOrigin == null)
        {
            CreateRayOrigin();
        }
    }
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {

                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    if (curInteractable == null)
                    {
                        Debug.LogWarning("Hit object  " + hit.collider.gameObject.name);
                    }

                    //SetPromptText();
                }
            }
            if (rayOrigin == null)
            {
                Debug.LogWarning("rayOrigin is null!");
                return;
            }
            //else
            //{
            //    curInteractGameObject = null;
            //    curInteractable = null;
            //    prompText.gameObject.SetActive(false);
            //}
        }
    }
    void CreateRayOrigin()
    {
        GameObject rayOriginObject = new GameObject("RayOrigin");
        rayOriginObject.transform.SetParent(transform);
        rayOriginObject.transform.localPosition = Vector3.zero;
        rayOriginObject.transform.localRotation = Quaternion.identity;
        rayOriginObject.transform.localPosition = new Vector3(0, 1.6f, 0);

        rayOrigin = rayOriginObject.transform;
    }
    private void OnDrawGizmos()
    {
        if (rayOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayOrigin.position, rayOrigin.forward * maxCheckDistance);
        }
    }
    //private void SetPromptText()
    //{
    //    if (prompText == null || curInteractable == null)
    //    {
    //        return;
    //    }
    //    prompText.gameObject.SetActive(true);
    //    prompText.text = curInteractable.GetInteractPrompt();
    //}
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (curInteractable == null)
            {
                Debug.LogWarning("No interactable object in range.");
            }
            else
            {
                curInteractable.OnInteract();
                curInteractGameObject = null;
                curInteractable = null;
                prompText?.gameObject.SetActive(false);
            }
        }
    }
}

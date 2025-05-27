using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngineInternal;

public class Interaction : MonoBehaviour
{
/*    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    public Interactable curInteractable;

    public TextMeshProUGUI prompText;
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<Interactable>();
                    if (curInteractable == null)
                    {
                        Debug.LogWarning("Hit object에 Interactable이 없습니다: " + hit.collider.gameObject.name);
                    }

                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                prompText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        if (prompText == null || curInteractable == null)
        {
            Debug.LogWarning("PromptText 또는 curInteractable이 null입니다!");
            return;
        }
        prompText.gameObject.SetActive(true);
        prompText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            prompText.gameObject.SetActive(false);
        }
    }*/
}

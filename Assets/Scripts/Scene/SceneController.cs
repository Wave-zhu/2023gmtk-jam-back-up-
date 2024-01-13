using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] 
    List<GameObject> GhostObjects = new List<GameObject>();

    [SerializeField] private GameObject gery;
    private Vector3 offset;
    private Transform objectToDrag;
    private void Update()
    {
        if (GameInputManager.MainInstance.GhostVision)
        {
            gery.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero,1f,1<<8);

                if (hit.collider != null)
                {
                    objectToDrag = hit.transform;
                    offset = objectToDrag.position - mousePosition;
                }
            }
            else if (Input.GetMouseButton(0) && objectToDrag != null)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                objectToDrag.position = new Vector3(mousePosition.x + offset.x, mousePosition.y + offset.y, objectToDrag.position.z);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                objectToDrag = null;
            }
        }
        else
        {
            gery.SetActive(false);
        }
    }
}

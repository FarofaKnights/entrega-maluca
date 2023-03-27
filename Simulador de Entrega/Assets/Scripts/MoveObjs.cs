using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjs : MonoBehaviour
{
    public float forceAmount = 2000;

    Rigidbody selectedRigidbody;
    public Transform pR, pL;
    Camera targetCamera;
    Vector3 originalScreenTargetPosition;
    Vector3 originalRigidbodyPos;
    float selectionDistance;

    // Start is called before the first frame update
    void Start()
    {
        targetCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (!targetCamera)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            //Verifica se vc está clicando em um RigidBody, se sim, seleciona
            selectedRigidbody = GetRigidbodyFromMouseClick();
        }
        if (Input.GetMouseButtonUp(0) && selectedRigidbody)
        {
            //Solta o OBJ
            selectedRigidbody = null;
        }
    }

    void FixedUpdate()
    {
        if (selectedRigidbody)
        {
            //Calcula para onde o Obj selecionado vai
            Vector3 mousePositionOffset = targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance)) - originalScreenTargetPosition;
            selectedRigidbody.velocity = (originalRigidbodyPos + mousePositionOffset - selectedRigidbody.transform.position) * forceAmount * Time.deltaTime;
        }
    }
    
    Rigidbody GetRigidbodyFromMouseClick()
    {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(ray, out hitInfo);
        if (hit)
        {
            if (hitInfo.collider.gameObject.GetComponent<Rigidbody>())
            {
                selectionDistance = Vector3.Distance(ray.origin, hitInfo.point);
                originalScreenTargetPosition = targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance));
                originalRigidbodyPos = hitInfo.collider.transform.position;
                Debug.Log("!");
                return hitInfo.collider.gameObject.GetComponent<Rigidbody>();
            }
        }

        return null;
    }
}

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MovementController : MonoBehaviour {
    public LayerMask clickableArea;

    private NavMeshAgent myAgent;

    private void Start() {
        myAgent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        if(Input.GetButtonDown("Fire1")) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if(Physics.Raycast(ray, out hitInfo, 100, clickableArea)) {
                myAgent.SetDestination(hitInfo.point);
            }
        }
    }
}

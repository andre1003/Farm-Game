using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MovementController : MonoBehaviour {
    #region Singleton
    public static MovementController instance;
    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of MovementController found!");
            return;
        }

        instance = this;
    }

    #endregion

    public LayerMask clickableArea;
    public Animator animator;

    private NavMeshAgent myAgent;
    private bool isPlanting = false;

    private Transform lookAt;

    private void Start() {
        myAgent = GetComponent<NavMeshAgent>();
        myAgent.updateRotation = false;
    }

    private void Update() {
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        InputHandler();
        ArriveDestinationHandler();
    }

    private void LateUpdate() {
        if(myAgent.velocity.sqrMagnitude > Mathf.Epsilon) {
            transform.rotation = Quaternion.LookRotation(myAgent.velocity.normalized);
        }
    }

    // Method for handling input
    private void InputHandler() {
        // Dance button
        if(Input.GetKeyDown(KeyCode.D)) {
            animator.SetBool("isDancing", !animator.GetBool("isDancing"));

        }

        // Movement button
        if(Input.GetButtonDown("Fire1") && !InGameSaves.GetIsBusy()) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if(Physics.Raycast(ray, out hitInfo, 100, clickableArea)) {
                myAgent.SetDestination(hitInfo.point);
                animator.SetBool("isWalking", true);
            }
        }
    }

    // Method for handling player arrive to destination
    private void ArriveDestinationHandler() {
        // Check if player reached the destination
        if(!myAgent.pathPending) {
            if(myAgent.remainingDistance <= myAgent.stoppingDistance) {
                if(!myAgent.hasPath || myAgent.velocity.sqrMagnitude == 0f) {
                    // Done
                    animator.SetBool("isWalking", false);
                    if(isPlanting) {
                        InGameSaves.ChangeIsBusy();
                        StartCoroutine(Wait(8f));
                        myAgent.transform.LookAt(lookAt);
                        isPlanting = false;
                        animator.Play("Plant");
                    }
                }
            }
        }
    }

    // Method for send to plantation zone
    public void SendTo(Vector3 target, Transform gameObject) {
        lookAt = gameObject;
        myAgent.SetDestination(target);
        animator.SetBool("isWalking", true);
        isPlanting = true;
    }

    // Method for wait animation over
    private IEnumerator Wait(float seconds) {
        yield return new WaitForSeconds(seconds);
        InGameSaves.ChangeIsBusy();
    }

    public void StopPlayer() {
        myAgent.isStopped = true;
        animator.SetBool("isWalking", false);
    }

    public void ResumePlayer() {
        myAgent.isStopped = false;
        if(myAgent.hasPath)
            animator.SetBool("isWalking", true);
    }
}

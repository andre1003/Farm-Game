using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class MovementController : MonoBehaviour
{
    #region Singleton
    public static MovementController instance;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of MovementController found!");
            return;
        }

        instance = this;
    }

    #endregion

    // Mask
    public LayerMask clickableArea;
    
    // Animator
    public Animator animator;

    
    // Navigation mesh agent
    private NavMeshAgent myAgent;

    // Is planting?
    private bool isPlanting = false;

    // Look at
    private Transform lookAt;


    // Start method
    void Start()
    {
        // Set navigation mesh agent
        myAgent = GetComponent<NavMeshAgent>();
        myAgent.updateRotation = false;
    }

    // Update method
    void Update()
    {
        // If the agent is not enable, exit
        if(!myAgent.enabled)
        {
            return;
        }

        // If mouse is over a game object
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // Handle input and arrive destination
        InputHandler();
        ArriveDestinationHandler();
    }

    // Late update method
    void LateUpdate()
    {
        // If agent is not enable, exit
        if(!myAgent.enabled)
        {
            return;
        }
            
        // Correct the rotation, if needed
        if(myAgent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(myAgent.velocity.normalized);
        }
    }

    /// <summary>
    /// Method for handling input.
    /// </summary>
    private void InputHandler()
    {
        // Dance button
        if(Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("isDancing", !animator.GetBool("isDancing"));

        }

        // Movement button
        if(Input.GetButtonDown("Fire1") && !InGameSaves.GetIsBusy())
        {
            // Get a ray from mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            // If hit any point on clickable area, send player to the hit point
            if(Physics.Raycast(ray, out hitInfo, 100, clickableArea))
            {
                myAgent.SetDestination(hitInfo.point);
                animator.SetBool("isWalking", true);
            }
        }
    }

    /// <summary>
    /// Method for handling player arrive to destination.
    /// </summary>
    private void ArriveDestinationHandler()
    {
        // Check if player reached the destination
        if(!myAgent.pathPending)
        {
            // If there is no remaining distance from destination
            if(myAgent.remainingDistance <= myAgent.stoppingDistance)
            {
                // If there is no path for agent OR agent is not moving
                if(!myAgent.hasPath || myAgent.velocity.sqrMagnitude == 0f)
                {
                    // Stop walking animation
                    animator.SetBool("isWalking", false);

                    // If player is planting
                    if(isPlanting)
                    {
                        // Change busy status
                        InGameSaves.ChangeIsBusy();

                        // Wait 8 seconds and change busy status again
                        StartCoroutine(Wait(8f));

                        // Change player rotation to look at plantation zone
                        myAgent.transform.LookAt(lookAt);

                        // Set is planting to false
                        isPlanting = false;

                        // Play Plant animation
                        animator.Play("Plant");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Stop player movement.
    /// </summary>
    private void StopMovement()
    {
        animator.SetBool("isWalking", false);
    }

    /// <summary>
    /// Method for send to plantation zone.
    /// </summary>
    /// <param name="target">Destination.</param>
    /// <param name="gameObject">Object to look at.</param>
    public void SendTo(Vector3 target, Transform gameObject)
    {
        lookAt = gameObject;
        myAgent.SetDestination(target);
        animator.SetBool("isWalking", true);
        isPlanting = true;
    }

    /// <summary>
    /// Method for wait animation over.
    /// </summary>
    /// <param name="seconds">Seconds to wait.</param>
    private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        InGameSaves.ChangeIsBusy();
    }

    /// <summary>
    /// Stop player movement immediately.
    /// </summary>
    public void StopPlayer()
    {
        // If agent is enable, stop it
        if(myAgent.enabled)
        {
            myAgent.isStopped = true;
            animator.SetBool("isWalking", false);
        }
    }

    /// <summary>
    /// Makes player move again.
    /// </summary>
    public void ResumePlayer()
    {
        // If agent is enable, move it again
        if(myAgent.enabled)
        {
            myAgent.isStopped = false;
            if(myAgent.hasPath)
            {
                animator.SetBool("isWalking", true);
            }
        }
    }

    /// <summary>
    /// Set agent active status.
    /// </summary>
    /// <param name="value">New agent active status.</param>
    public void SetMyAgentActive(bool value)
    {
        StopMovement();
        myAgent.enabled = value;
    }

    /// <summary>
    /// Set animation.
    /// </summary>
    /// <param name="animation">Animation name.</param>
    /// <param name="value">Animation value.</param>
    public void SetAnimation(string animation, bool value)
    {
        animator.SetBool(animation, value);
    }
}

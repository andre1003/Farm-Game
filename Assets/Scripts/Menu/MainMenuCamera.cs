using System.Collections;
using UnityEngine;


public class MainMenuCamera : MonoBehaviour
{
    // Animator
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait(30f));
    }

    /// <summary>
    /// Wait for seconds and execute command.
    /// </summary>
    /// <param name="seconds">Seconds to wait.</param>
    IEnumerator Wait(float seconds)
    {
        // Wait for seconds
        yield return new WaitForSeconds(seconds);

        // Call StartDancing method
        StartDancing();
    }

    /// <summary>
    /// Start NPC dance.
    /// </summary>
    public void StartDancing()
    {
        // Set animator param isDancing to true
        animator.SetBool("isDancing", true);
    }
}

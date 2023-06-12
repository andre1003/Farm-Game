using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait(25f));
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        StartDancing();
    }

    public void StartDancing()
    {
        animator.SetBool("isDancing", true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    public Slider progressBar;
    
    public AudioSource audioSource;
    public AudioClip click;
    public AudioClip hover;


    // Cursors
    public Texture2D normalCursor;
    public Texture2D hoverCursor;

    // Hot spot
    public Vector2 hotSpot = Vector2.zero;


    public void Play() {
        ClickSound();
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(LoadAsyncScene(index));
    }

    private IEnumerator LoadAsyncScene(int index) {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        // Wait until the asynchronous scene fully loads
        while(!asyncLoad.isDone) {
            float progress = Mathf.Clamp01(asyncLoad.progress / .9f);
            progressBar.value = progress;

            yield return null;
        }
    }

    /// <summary>
    /// Quit the game.
    /// </summary>
    public void QuitGame()
    {
        ClickSound();
        Application.Quit();
    }

    public void HoverCursor()
    {
        Cursor.SetCursor(hoverCursor, hotSpot, CursorMode.Auto);
        audioSource.clip = hover;
        audioSource.Play();
    }

    public void NormalCursor()
    {
        Cursor.SetCursor(normalCursor, hotSpot, CursorMode.Auto);
    }

    private void ClickSound()
    {
        audioSource.clip = click;
        audioSource.Play();
    }
}

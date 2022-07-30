using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    public Slider progressBar;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Play() {
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
}

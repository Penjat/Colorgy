using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour {
	private static string TAG = "LOADING MANAGER: ";
	private bool loadScene = false;

  [SerializeField]
  private int scene;
  [SerializeField]
  public Text loadingText;
	public Slider loadingBar;


	void Start(){
		StartLoadingScene(1);

	}
	public void StartLoadingScene(int sceneNum){
		loadScene = true;

		// ...change the instruction text to read "Loading..."
		loadingText.gameObject.SetActive(true);


		// ...and start a coroutine that will load the desired scene.
		//Canvas canvas = GameObject.Find("Canvas").GetComponent<Canavas>();
		scene = sceneNum;
		StartCoroutine(LoadNewScene());


	}


    // Updates once per frame
    void Update() {

        // If the player has pressed the space bar and a new scene is not loading yet...
        if (loadScene == false) {

            // ...set the loadScene boolean to true to prevent loading a new scene more than once...


        }

        // If the new scene has started loading...
        if (loadScene == true) {

            // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));

        }

    }


    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene() {

        // This line waits for 3 seconds before executing the next line in the coroutine.
        // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        //yield return new WaitForSeconds(3);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = Application.LoadLevelAsync(scene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone) {
						Debug.Log("progress = " + async.progress);
						loadingBar.value = async.progress;
            yield return null;
        }
				Debug.Log(TAG + "Done Loading");


    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

	private MainManager mainManager;
	private static string TAG = "TUTORIAL MANAGER: ";

	private float timer;
	private bool paused;

	public Text tutorialMsg;
	public Animator animator;

	public void SetUp(MainManager m){
		Debug.Log(TAG + "Setting up.");
		mainManager = m;
	}

	void Update(){
		//pause before entering level
		if(paused){
			timer-= Time.deltaTime;
			if(timer <=0.0f){
				paused = false;
				animator.speed = 0.4f;
				mainManager.StartPointers(true);
			}
		}
	}
	public void Hide(){
		tutorialMsg.text = "";
		tutorialMsg.gameObject.SetActive(false);
	}
	public void CheckTutorial(Level level,int numOfTools){

		Debug.Log(TAG + "number of tools is: " + numOfTools);

		string msg = level.GetTutorialMsg();

		//if no msg, hide the text box
		if (msg == "" ){
			tutorialMsg.gameObject.SetActive(false);
			return;
		}

		//show the text box and set the msg
		tutorialMsg.gameObject.SetActive(true);
		tutorialMsg.text = msg;

		//wait until all the tools are done before fading in the tutorial msg
		paused = true;
		animator.speed = 0.0f;
		timer = 3.6f + numOfTools*0.3f;
		//check if is first level
		mainManager.StartPointers(true);

	}
	public void FadeOut(){
		animator.Play("fadeOutTutorial");
	}
}

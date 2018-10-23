using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnimate : MonoBehaviour {
	private static string TAG = "INTRO ANIMATE: ";
	public SoundFXManager soundManager;

	public GameObject uiMenuPrefab;

	private bool introHasHappened = false;

	public Renderer t1;
	public Renderer t2;
	public Material solidMat;
	public Material fadeMat;

	public Animator symbolAnimator;
	public Animator buttonAnimator1;
	public Animator buttonAnimator2;
	public Animator buttonAnimator3;
	public Animator buttonAnimator4;
	public Animator buttonAnimator5;
	public Animator buttonAnimator6;

	private float timer;
	private bool isFading;
	private bool isPaused;

	public Color c1;
	public Color c2;

	public void ReStartIntro(){
		introHasHappened = false;
	}
	public void StartIntro(){
		Debug.Log(TAG + "starting intro.");


		//check if the intro has already happened
		if(introHasHappened){
			symbolAnimator.Play("normal");

		

			t1.material = solidMat;
			t2.material = solidMat;
			t1.material.color = c2;
			t2.material.color = c2;

			buttonAnimator1.speed = 1.0f;
			buttonAnimator2.speed = 0.8f;
			buttonAnimator3.speed = 0.6f;
			buttonAnimator4.speed = 0.5f;
			buttonAnimator5.speed = 0.3f;
			buttonAnimator6.speed = 0.2f;
			return;
		}

		soundManager.PlayTitleBG();
		t1.material = fadeMat;
		t2.material = fadeMat;
		t1.material.color = c1;
		t2.material.color = c1;

		symbolAnimator.SetBool("introDone",true);
		isPaused = true;
		isFading = false;
		timer = 3.0f;

		buttonAnimator1.speed = 0.0f;
		buttonAnimator2.speed = 0.0f;
		buttonAnimator3.speed = 0.0f;
		buttonAnimator4.speed = 0.0f;
		buttonAnimator5.speed = 0.0f;
		buttonAnimator6.speed = 0.0f;

	}
	void Update(){
		if(isPaused){
			timer -= Time.deltaTime;

			if(timer <= 0){
				isPaused = false;
				FadeInText();

			}

			return;
		}

		if(isFading){
			float rate = 0.5f;
			timer -= Time.deltaTime*rate;
			Color c = Color.Lerp(c2, c1, timer);

			t1.material.color = c;
			t2.material.color = c;

			if(timer <= -0.0){
				isFading = false;
				t1.material = solidMat;
				t2.material = solidMat;
				t1.material.color = c2;
				t2.material.color = c2;

				buttonAnimator1.speed = 1.0f;
				buttonAnimator2.speed = 0.8f;
				buttonAnimator3.speed = 0.6f;
				buttonAnimator4.speed = 0.4f;
				buttonAnimator5.speed = 0.3f;
				buttonAnimator6.speed = 0.2f;
				introHasHappened = true;
				return;
			}

		}
	}
	public void FadeInText(){
		Debug.Log(TAG + "fading text");
		t1.gameObject.SetActive(true);
		t2.gameObject.SetActive(true);
		t1.material = fadeMat;
		t2.material = fadeMat;
		isFading = true;
		timer = 1.0f;
	}
	public GameObject GetMenu(){
		return Instantiate(uiMenuPrefab);
	}
	public bool GetIntroHasHappened(){
		return introHasHappened;
	}
}

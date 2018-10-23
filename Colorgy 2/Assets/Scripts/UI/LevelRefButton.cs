using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelRefButton : MonoBehaviour {
	//will replace level button

	private static string TAG = "LEVEL REF BUTTON: ";
	public Button button;
	//public Animator animator;
	private MenuManager menuManager;

	private int chapterNum;//specifies which folder
	private int levelNum;
	private bool isCustom;
	private bool isLocked;

	private bool isPaused;
	private float timer =0.0f;
	private static float animationSpeed = 0.5f;

	void Update(){
		if(isPaused){
			timer -= Time.deltaTime;
			if(timer <= 0){
				isPaused = false;
				//animator.speed = animationSpeed;
			}
		}
	}

	public void SetUp(int cNum, int lNum, MenuManager mm,bool b){
		Debug.Log(TAG + "setting up: " + cNum + " : "+ lNum );

		chapterNum = cNum;
		levelNum = lNum;
		menuManager = mm;
		isCustom = b;
		button.GetComponentInChildren<Text>().text = (levelNum + 1).ToString();

		//animator.speed = 0;
		//isPaused = true;
		timer = lNum*0.1f;

	}
	public void SetLocked(bool l){
		isLocked = l;
		if(isLocked){
			button.image.color = Color.grey;
			button.interactable = false;
			button.GetComponentInChildren<Text>().text = "";
		}
	}
	public bool GetIsCustom(){
		return isCustom;
	}

	public void Press(){
		Debug.Log(TAG + "pressed: "+ chapterNum + " : " + levelNum);
		menuManager.SelectLevel(this);
	}

	public int GetChapter(){
		return chapterNum;
	}
	public int GetLevelNum(){
		return levelNum;
	}
	public void Highlight(){
		button.image.color = Color.red;
	}
	public void IsCurrent(){
		button.image.color = Color.green;
	}
}

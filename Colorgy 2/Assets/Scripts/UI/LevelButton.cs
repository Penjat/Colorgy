using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

	private MenuManager menuManager;
	public Text buttonText;

	private int levelNum;
	private bool editMode;

	public void Press(){
		Debug.Log("Pressed the level Button.");
		//TODO change from zero for propor folder
		menuManager.SelectLevelEditMode(levelNum);
	}


	public void SetLevelNum(int i,MenuManager m,bool b){
		levelNum = i;
		menuManager = m;
		editMode = b;
	}
	public void SetName(string s){
		buttonText.text = s;
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolSelect : MonoBehaviour {

	private string toolType = "none";
	private bool menuIsOpen = false;


	public GameObject toolMenu;
	public Text toolDisplay;//displays the current selected color
	public ColorSelect colorSelect;
	public Text toolNumber;

	void Start(){
		//hide the drop down menu
		toolMenu.SetActive(false);

	}
	public void SetUp(int i){
		//toolNumber.text = (i+1).ToString();
	}

	public void SetVal(string s){
		Debug.Log("setting val: " + s);
		toolType = s;
		toolDisplay.text = toolType;
		ToggleToolMenu();
	}
	public string GetToolType(){
		return toolType;
	}
	public string GetColorVal(){
		int val = colorSelect.GetVal();

		return val.ToString();
	}
	public void SetToolMenu(bool b){
		Debug.Log("ToolMenu = " + b);
		menuIsOpen = b;
		toolMenu.SetActive(b);
	}
	public void ToggleToolMenu(){

		SetToolMenu(!menuIsOpen);
	}
}

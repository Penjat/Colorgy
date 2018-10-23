using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelect : MonoBehaviour {

	private int val = 0;
	private bool menuIsOpen = false;


	public GameObject colorMenu;
	public Button colorDisplay;//displays the current selected color

	void Start(){
		//hide the drop down menu
		colorMenu.SetActive(false);

	}

	public void SetVal(int i){
		Debug.Log("setting val: " + i);
		val = i;
		colorDisplay.image.color = Calc.GetColor(val);
		ToggleColorMenu();
	}
	public int GetVal(){
		return val;
	}
	public void SetColorMenu(bool b){
		Debug.Log("colorMenu = " + b);
		menuIsOpen = b;
		colorMenu.SetActive(b);
	}
	public void ToggleColorMenu(){
		
		SetColorMenu(!menuIsOpen);
	}
}

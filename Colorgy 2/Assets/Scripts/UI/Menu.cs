using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
	private static string TAG = "MENU: ";
	private ButtonManager buttonManager;


	public void Press(int i){
		//every button calls this method when pressed
		buttonManager.Press(i);
	}
	public void SetButtonManager(ButtonManager b){
		buttonManager = b;
	}
	public void ClosePopUp(){
		//TODO fade out animation
		Destroy(gameObject);
	}
}

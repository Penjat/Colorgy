using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {
	//manages all the button presses for all the menus
	private static int TO_TITLE = 0;
	private static int TO_LEVEL_SEL = 1;
	private static int TO_SETTINGS = 2;

	public MenuManager menuManager;

	public void Press(int i){

		switch(i){

		case 0:
			menuManager.ToTitle();
			break;

		case 1:
			menuManager.ToLevelSelect();
			break;
		case 2:
			menuManager.OpenSettings();
			break;
		case 3:
			menuManager.OpenCustomLevels();
			break;
		case 4:
			menuManager.ToLevelEditor();
			break;
		case 10:
			menuManager.CloseContactMenu();
			break;
		case 11:
			menuManager.OpenLink(5);
			break;
		case 12:
			menuManager.OpenLink(6);
			break;
		case 13:
			menuManager.OpenLink(7);
			break;
		}
	}
}

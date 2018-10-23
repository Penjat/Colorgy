using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {
	
	private static string TAG = "INPUT MANAGER: ";

	public Camera mainCamera;
	public GridManager gridManager;
	public ToolManager toolManager;
	public LevelEditor levelEditor;
	public MenuManager menuManager;
	public SoundFXManager soundManager;

	private Hex curHex;
	private ButtonTool curButton;//refers to the current tool button
	private MenuButton curMenuButton;//refers to all navigationl buttons



	public void CheckOverGrid(bool editMode){
		if(EventSystem.current.IsPointerOverGameObject()){
			return;
		}

		RaycastHit hit;
		Vector3 dir = transform.TransformDirection(-Vector3.up);
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);


		if (Physics.Raycast(ray, out hit,30.0f) ){
			//print("There is something in front of the object!");

			//if it is the grid
			if(hit.collider.tag == "grid"){
				
				if(curHex && curHex.gameObject == hit.collider.transform.parent.gameObject){
					//if mouse already over the object
					return;
				}

				//if over a new hex
				Hex newHex = hit.collider.GetComponentInParent<Hex>();

				if(!toolManager.HighlightCurTool(curHex,newHex)){
					//if there is not a curTool, then Input manager takes care of the highlighting
					if(curHex){
						curHex.LightUp(false);
					}

					newHex.LightUp(true);
				}
				curHex = newHex;
				//toolManager.SetCurToolSnap(curHex);//snap the curTool to the grid
				return;
			}

			//if it is a Button
			if(hit.collider.tag == "button"){
				if(curButton && curButton.gameObject == hit.collider.transform.parent.gameObject){
					//if mouse already over the object
					return;
				}
				ButtonTool newButton = hit.collider.GetComponent<ButtonTool>();

				//TODO check if tool selected
				if(curButton){
					curButton.Highlight(false);
				}
				newButton.Highlight(true);
				curButton = newButton;
				return;
			}
			if(hit.collider.tag == "menuButton"){
				if(curMenuButton && curMenuButton.gameObject == hit.collider.transform.parent.gameObject){
					//if mouse already over the object
					return;
				}
				MenuButton newButton = hit.collider.GetComponentInParent<MenuButton>();
				if(curMenuButton){
					curMenuButton.LightUp(false);
				}
				newButton.LightUp(true);
				curMenuButton = newButton;
				return;

			}





		}

		//if raycast hits nothing, unhighlight what was 
		if(curHex){
			//check if there is a tool
			if(!toolManager.HighlightCurTool(curHex,null)){
				//if no current tool, unhighlight
				curHex.LightUp(false);
				curHex = null;

			}

		}
		if(curButton){
			curButton.Highlight(false);
			curButton = null;
		}
		if(curMenuButton){
			curMenuButton.LightUp(false);
			curMenuButton = null;
		}
		curHex = null;
		//toolManager.SetCurToolSnap(null);//snap the curTool to the grid

	}
	public void CheckClick(bool editMode){
		if(EventSystem.current.IsPointerOverGameObject()){
			return;
		}
		if (Input.GetMouseButtonDown(0)){
			Debug.Log(TAG + "Mouse clicked");
			RaycastHit hit;
			Vector3 dir = transform.TransformDirection(-Vector3.up);
			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			//check if hits a tool button
			if (Physics.Raycast(ray, out hit,30.0f) ){
				if(hit.collider.tag == "button"){
					Debug.Log(TAG + "hit a button");
					ButtonTool t = hit.collider.GetComponentInParent<ButtonTool>();
					toolManager.SetCurTool(t);

					return;
				}
				if(hit.collider.tag == "tool"){
					Debug.Log(TAG + "hit a tool.");
					return; //taking out this functionallity for now
					Tool tool = hit.collider.GetComponentInParent<Tool>();

					tool.EndUse();
					toolManager.ClearCurTool();
					return;
				}
				if(hit.collider.tag == "menuButton"){
					Debug.Log(TAG + "menu button pressed");
					MenuButton menuButton = hit.collider.GetComponentInParent<MenuButton>();
					menuManager.MenuButtonPress(menuButton.Press());
				}
			}
			//if it is over a hex, press it
			if(curHex!=null){
				
				MouseClick(editMode);
				return;
			}

			//if it does not hit a hex 



		}
	}
	public void MouseClick(bool editMode){
		//check if edit mode
		if(editMode){
			int val = levelEditor.GetHexVal();
			curHex.SetVal(val);
			curHex.UpdateColor();
			return;
		}

		//for playMode
		Tool curTool = toolManager.GetCurTool();
		if(curTool == null){
			return;
		}

		curTool.Use(curHex);
		//soundManager.UseTool(curTool);
		curHex = null;

	}



}

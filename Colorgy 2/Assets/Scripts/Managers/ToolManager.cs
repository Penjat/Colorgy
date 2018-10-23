using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour {
	private static string TAG = "TOOL MANAGER: ";

	public SoundFXManager soundManager;
	public TutorialManager tutorialManager;
	private Tool curTool;
	private ButtonTool curToolButton;

	private TutorialPointers tutorialPointers;

	public GameObject rightScreenPos;

	public GameObject toolContainer;
	public Camera c;

	//prefabs for all tools
	public GameObject novaPrefab;
	public GameObject cubeNovaPrefab;
	public GameObject diamondNovaPrefab;
	public GameObject spherePrefab;
	public GameObject squarePrefab;
	public GameObject diamondPrefab;
	public GameObject beamPrefab;
	public GameObject cubeBeamPrefab;
	public GameObject diamondBeamPrefab;
	public GameObject pyramidPrefab;
	public GameObject vortexNovaPrefab;
	public GameObject vortexBeamPrefab;
	public GameObject vortexPrefab;
	public GameObject finalNova;

	private ButtonTool[] tools;




	public int SetUp(Level level){
		Debug.Log(TAG + "Setting up.");

		//position the tool container at the edge of the screen
		Vector3 v = toolContainer.transform.position;

		toolContainer.transform.position = c.ScreenToWorldPoint(new Vector3 (Screen.width,Screen.height,v.z) )-new Vector3(1.2f,0,1.2f);

		tools = new ButtonTool[6];
		Level.LevelTool[] t = level.GetTools();
		t = ScrambleTools(t);

		int place = 0;
		for(int i=0;i<6;i++){
			Level.LevelTool levelTool =  t[i];

			place = CreateTool(place,levelTool,level);

		}
		//returns the number of active tools for this level
		//used for calculating when the tutorial msg should be displayed
		return place;

	}
	public int CreateTool(int place,Level.LevelTool levelTool,Level level){
		// converts the LevelTool into the prefab
		string toolType = levelTool.GetToolType();
		int val = levelTool.GetVal();
		//modulate the tools
		int modNum = level.GetModNum();
		val = Calc.ModThree(val,modNum);

		GameObject prefab = FindPrefab(toolType);

		if(prefab == null){
			return place;
		}

		GameObject g = Instantiate(prefab);

		g.transform.position = toolContainer.transform.position + new Vector3(0,0,-place*1.41f);
		g.transform.rotation = toolContainer.transform.rotation;
		g.transform.SetParent(toolContainer.transform,true);

		ButtonTool buttonTool = g.GetComponentInChildren<ButtonTool>();
		tools[place] = buttonTool;
		buttonTool.SetUp(val,place,soundManager);
		place++;
		return place;
	}
	public GameObject FindPrefab(string toolType){
		if(toolType == "none"){
			return null;
		}
		if(toolType == "sphere"){
			return spherePrefab;
		}
		if(toolType == "nova"){
			return novaPrefab;
		}
		if(toolType == "cube"){
			return squarePrefab;
		}
		if(toolType == "beam"){
			return beamPrefab;
		}
		if(toolType == "pyramid"){
			return pyramidPrefab;
		}
		if(toolType == "diamond"){
			return diamondPrefab;
		}
		if(toolType == "cubeBeam"){
			return cubeBeamPrefab;
		}
		if(toolType == "diamondBeam"){
			return diamondBeamPrefab;
		}
		if(toolType == "cubeNova"){
			return cubeNovaPrefab;
		}
		if(toolType == "diamondNova"){
			return diamondNovaPrefab;
		}
		if(toolType == "vortexNova"){
			return vortexNovaPrefab;
		}
		if(toolType == "vortexBeam"){
			return vortexBeamPrefab;
		}
		if(toolType == "vortex"){
			return vortexPrefab;
		}

		return null;
	}
	public Tool GetCurTool(){
		return curTool;
	}

	public void SetCurTool(ButtonTool t){



		if(curToolButton){
			//if there already is a tool

			//if is the same as the new tool
			if(curToolButton == t){
				if( curTool.IsPlaced()){
					//if it is used, end use
					Debug.Log(TAG + "ending tool use.");
					//curToolButton.SetSelected(false);
					curTool.EndUse();
					curTool = null;
					curToolButton = null;
					return;
				}
				Debug.Log(TAG + "deselecting tool.");
				curToolButton.SetSelected(false);
				curTool.gameObject.SetActive(false);
				curTool = null;
				curToolButton = null;
				if(tutorialPointers != null){
					tutorialPointers.SetState(0);
					tutorialPointers.FindPos();
				}
				return;
			}


			if( curTool.IsPlaced()){
				//if it is used, end use
				Debug.Log(TAG + "ending tool use.");
				curToolButton.SetSelected(false);
				curTool.EndUse();
				curTool = null;
				curToolButton = null;
			}else{
				// if not used, just deselect the button
				Debug.Log(TAG + "deselecting tool.");
				curToolButton.SetSelected(false);
				curTool = null;
				curToolButton = null;

			}


		}

		//set the current tool and buttonTool
		Debug.Log(TAG + "selecting tool.");
		t.SetSelected(true);
		curTool = t.GetTool();
		curToolButton = t;
		if(tutorialPointers != null){
			tutorialPointers.SetState(1);
			tutorialPointers.FindPos();
		}

	}
	public void Clear(){
		Debug.Log(TAG + "clearing tools.");
		if(curTool){
			curTool.StartFade(1.0f,1.0f,10.0f);
			//Destroy(curTool.gameObject);
		}
		curTool = null;
		if(tools != null){
			foreach(ButtonTool t in tools){
				if(t){
					t.EndUse();
					//Destroy(t.gameObject);
				}
			}
		}

	}
	public bool HighlightCurTool(Hex oldHex,Hex newHex){
		if(curTool == null){
			//Debug.Log(TAG + "no current tool");
			//returns false and Highlights normally
			return false;
		}
		//Debug.Log(TAG + "is a  tool");
		curTool.Highlight(oldHex,newHex);
		//highlights based on the current tool
		return true;

	}
	public void ClearCurTool(){
		Debug.Log(TAG + "clearing current tool.");
		curTool = null;
		curToolButton = null;
	}
	public void SetCurToolSnap(Hex hex){
		if(curTool){
			curTool.SetGridPos(hex);
		}
	}
	public Level.LevelTool[] ScrambleTools(Level.LevelTool[] inputToolArray){
		//scrambels the order of the tool array to make puzzels more challanging

		Level.LevelTool t1;
		Level.LevelTool t2;

		int numOfTools = 0;
		for(int i=0;i<inputToolArray.Length;i++){
			int r = Random.Range(0,6);

			//get two tools
			t1 = inputToolArray[i];
			t2 = inputToolArray[r];

			//swap them
			inputToolArray[i] = t2;
			inputToolArray[r] = t1;


		}

		return inputToolArray;
	}
	public void SetPointers(TutorialPointers t){
		//is use to set the Tutorial if it is the first level
		tutorialPointers = t;
	}
	public ButtonTool[] GetTools(){
		return tools;
	}
}

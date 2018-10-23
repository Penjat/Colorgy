using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {
	private static string TAG = "MAIN MANAGER: ";



	private static bool DEMO =false;
	private static int buildVersion = 3;
	private static string versionNumber = "1.0";
	//0 is Android
	//1 is IOS
	//2 is standalone
	//3 is WebGL
	//4 is Android TV



	public GridManager gridManager;
	public InputManager inputManager;
	public LevelManager levelManager;
	public ToolManager toolManager;
	public MenuManager menuManager;
	public LevelEditor levelEditor;
	public ChapterManager chapterManager;
	public TutorialManager tutorialManager;
	public SoundFXManager soundManager;
	public GameObject exitButton;
	private DebugManager debugManager;

	public GameObject pointersPrefab;//shows some arrows to make the first level easier
	private TutorialPointers tutorialPointers;

	private bool editMode =false;



	void Start () {


		//------------------Set up orentation--------------------------
		DeviceOrientation orientation = Input.deviceOrientation;
		if (orientation == DeviceOrientation.LandscapeLeft){
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}
		else if (orientation == DeviceOrientation.LandscapeRight){
			Screen.orientation = ScreenOrientation.LandscapeRight;
		}
		//------------------------------------------------------------
		if(buildVersion != 2){
			//only show exit button for standalone
			exitButton.SetActive(false);
		}
		chapterManager.SetUp();
		levelManager.SetUp(chapterManager);
		levelEditor.SetUp();
		tutorialManager.SetUp(this);

		//levelManager.LoadCustomLevels(0);
		menuManager.ToTitle();

		//unit test
		Debug.Log(TAG + "0 + 2 should be 2 is: " + Calc.ModThree(0,2));
		Debug.Log(TAG + "5 - 2 should be 3 is: " + Calc.ModThree(5,2));

		//Find the edge of the screen
		if(ProgressManager.GetDebug()){
			debugManager = menuManager.CreateDebug();

		}




		//Level l = levelManager.GetLevel();
		//gridManager.CreateGrid(l);
		//toolManager.SetUp(l);
	}

	void Update() {
		inputManager.CheckOverGrid(editMode);
		inputManager.CheckClick(editMode);
	}

	public string GetVersionNuber(){
		return versionNumber;
	}
	public void StartGame(LevelRefButton levelButton){
		int chapterNum = levelButton.GetChapter();
		int levelNum = levelButton.GetLevelNum();
		bool isCustom = levelButton.GetIsCustom();
		StartGame(chapterNum,levelNum,isCustom);
	}
	public void StartPointers(bool b){
		if(tutorialPointers != null){
			tutorialPointers.Show();
		}
	}
	public void StartGame(int chapterNum,int levelNum,bool isCustom){
		Debug.Log(TAG + "starting game.");
		ClearGame();

		//levelManager.GetLevels()[i];
		Level level = levelManager.GetLevel(chapterNum,levelNum,isCustom);//sets cur level and folder as well

		gridManager.CreateGrid(level,editMode);
		menuManager.ToGamePlay();
		int numOfTools = toolManager.SetUp(level);
		tutorialManager.CheckTutorial(level,numOfTools);

		//soundManager.SetCurChord(0);
		soundManager.SetStartChord(numOfTools);
		soundManager.CheckLastLevel(levelManager,chapterNum,level,isCustom);//checks if should play credits music

		if(ProgressManager.GetDebug()){
			debugManager.SetUp(level.GetFileName(),chapterNum,levelNum);
		}
		if(chapterNum == 0 && levelNum == 0){
			//is the first level of first chapter
			//show some arrows
			GameObject g = Instantiate(pointersPrefab);
			tutorialPointers = g.GetComponent<TutorialPointers>();
			return;
		}
		//if it is not the first level
		if(tutorialPointers != null){
			tutorialPointers.End();
		}

	}

	public void ClearGame(){
		Debug.Log(TAG + "clearing level.");
		gridManager.Clear();
		toolManager.Clear();

	}
	public void ResetLevel(){
		toolManager.Clear();
		ClearGame();
		Debug.Log(TAG + "reseting level.");
		Level level = levelManager.GetCurLevel();
		gridManager.CreateGrid(level,editMode);

		int numOfTools = toolManager.SetUp(level);
		//menuManager.ToGamePlay();
		menuManager.Reset();
		soundManager.SetStartChord(numOfTools);
		soundManager.PlayUISound(5);

	}
	public void LevelComplete(){
		Debug.Log(TAG + "level complete.");
		menuManager.LevelBeaten();
		toolManager.Clear();
		if(tutorialPointers != null){
			tutorialPointers.End();
		}
	}
	public void NextLevel(){
		levelManager.NextLevel();
		int chapterNum = levelManager.GetCurFolder();
		int levelNum = levelManager.GetCurLevelNum();
		bool isCustom = levelManager.GetIsCustom();

		StartGame(chapterNum,levelNum,isCustom);

	}
	public void NextChapter(){
		levelManager.NextChapter();
		int chapterNum = levelManager.GetCurFolder();
		int levelNum = levelManager.GetCurLevelNum();
		bool isCustom = levelManager.GetIsCustom();

		menuManager.StartChapterIntro(chapterNum,levelNum);
		//StartGame(chapterNum,levelNum,isCustom);

	}
	public int GetBuildVersion(){
		return buildVersion;
	}
	public void OpenLevelEditor(){
		Debug.Log(TAG + "opening Level Editor.");


		if(buildVersion == 3){
			Debug.Log(TAG + "cannot open levelEditor in WebGL");
			menuManager.WebGLSorry();
			return;
		}

		editMode = true;
		Level level = new Level();
		level.CreateBlankGrid();
		gridManager.CreateGrid(level,editMode);

		menuManager.ToLevelEditor();
	}
	public void ExitLevelEditor(){
		editMode = false;
		levelManager.SetUp(chapterManager);
		gridManager.Clear();
		menuManager.ToTitle();
	}

	public void ResetProgress(){
		Debug.Log(TAG + "reseting game progress.");
		//TODO have a pop-up are you sure
		//TODO don't delete all, keep sound and music vol etc
		PlayerPrefs.DeleteAll();
	}
	public bool GetISDemo(){
		return DEMO;
	}

	public void Quit(){
		Application.Quit();
	}
}

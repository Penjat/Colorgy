using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
//using UnityEditor;

public class LevelEditor : MonoBehaviour {

	private static string TAG = "LEVEL EDITOR: ";

	public MainManager mainManager;
	public MenuManager menuManager;
	public LevelManager levelManager;

	public GameObject menuSave;
	public GameObject tools;
	public GameObject loadMenu;
	public GameObject optionMenu;
	public GameObject loadButtonContainer;

	public GameObject ToolSelectPrefab;

	public InputField fileNameInput;
	public InputField levelNameInput;

	private ToolSelect[] toolSelects;

	public ColorSelect hexColor;


	public RectTransform chapterScrollView;
	public RectTransform levelScrollView;

	public GameObject loadChapterCon;
	public GameObject saveChapterCon;
	public GameObject levelCon;
	public GameObject menuNewChapter;
	public GameObject popUpDelete;

	private LevelButton[] levelButtons;
	private ChapterButton[] chapterButtons;

	private string curChapter = "";
	private int chapterRefNum = 0;
	private ChapterButton curChapterButton;

	public InputField newChapterName;

	private bool deleteMode = false;
	public Button deleteButton;
	private string toDelete;
	private bool toDeleteFile;//informs whether it is a file or folder that should be deleted

	public GameObject chapterButtonPrefab;


	public void SetUp(){
		CreateToolSelects();
		PositionMenus();
		CloseAll();


	}
	public void ToggleDeleteMode(){
		SetDeleteMode(!deleteMode);
	}
	public void SetDeleteMode(bool b){
		Debug.Log(TAG + "Delete Mode = " + b);
		deleteMode = b;
		if(b){
			deleteButton.image.color = Color.red;
			return;
		}
		deleteButton.image.color = Color.grey;
	}
	public void PositionMenus(){
		Debug.Log(TAG + "positioning the menus");
		RectTransform loadCon = loadButtonContainer.GetComponent<RectTransform>();

		float width,height,padT,padL,padB;

		width = loadCon.rect.width;
		height = loadCon.rect.height;

		//set the padding
		padT = 40.0f;
		padL = 10.0f;
		padB = 10.0f;

		//set the size of the chapters container
		chapterScrollView.offsetMin = new Vector2(padL,-height + padB  );
		chapterScrollView.offsetMax = new Vector2(width*0.3f,-padT);

		//set the size of the levels container
		levelScrollView.offsetMin = new Vector2(width*0.3f + padL,-height + padB);
		levelScrollView.offsetMax = new Vector2(width-padL,-padT);

	}
	public void CreateToolSelects(){
		Debug.Log(TAG + "creating tool selects");
		toolSelects = new ToolSelect[6];
		RectTransform toolCon = tools.GetComponent<RectTransform>();

		float totalWidth = toolCon.rect.width;
		float tsWidth = totalWidth/6.0f;
		float tsHeight = 30.0f;
		float padR = 10.0f;
		for(int i=0;i<6;i++){
			GameObject g = Instantiate(ToolSelectPrefab);
			g.transform.SetParent(tools.transform,false);
			RectTransform r = g.GetComponent<RectTransform>();


			float x1 = tsWidth*i;
			float y1 = 0;
			float x2 = x1+tsWidth-padR;
			float y2 = y1 + tsHeight;
			r.offsetMin = new Vector2(x1,y1);
			r.offsetMax = new Vector2(x2,y2);
			Debug.Log(TAG + x1 + " " + y1 +" " + x2 + " " + y2);

			ToolSelect ts = g.GetComponentInChildren<ToolSelect>();
			toolSelects[i] = ts;
			ts.SetUp(i);

			ts.SetVal("none");
			ts.colorSelect.SetVal(7);
			//close all the menus
			ts.colorSelect.SetColorMenu(false);
			ts.SetToolMenu(false);

		}

	}

	public void CloseAll(){
		//set up the menus
		//allows for menus to be open or closed in the editor for convieniance
		menuSave.SetActive(false);
		tools.SetActive(true);
		loadMenu.SetActive(false);
		menuNewChapter.SetActive(false);
		popUpDelete.SetActive(false);
		optionMenu.SetActive(false);
	}
	public void OptionMenu(bool b){
		CloseAll();
		optionMenu.SetActive(b);
	}
	public void OpenCreateChapter(bool b){
		menuNewChapter.SetActive(b);
	}
	public int GetHexVal(){
		//gives inputmanager the color val to change the curHex
		//+1 as zero is empty for hexes

		return hexColor.GetVal() + 1;
	}

	public void SaveMenu(bool b){
		//opens and closes the save menu
		Debug.Log(TAG + "save menu = " + b);
		optionMenu.SetActive(false);
		menuSave.SetActive(b);
		if(b){
			CreateChapterButtons(saveChapterCon,false);
			return;
		}


	}

	public void Save(){
		Debug.Log(TAG + "saving...");


		//Get the path
		string fileName = fileNameInput.text;
		//string path = "Assets/Resources/levels/Custom/"+fileName + ".txt";
		//string path = Application.persistentDataPath + "/CustomChapters/" + fileName + ".txt";
		string path = curChapter + "/" + fileName + ".txt";
		//open streamwritter
		StreamWriter writer = new StreamWriter(path, false);

		//write level name
		string levelName = levelNameInput.text;
		writer.WriteLine(levelName);

		//write the grid
		SaveGrid(writer);

		//write the tools
		SaveTools(writer);

		writer.Close();
		menuSave.SetActive(false);
		Debug.Log(TAG + "level saved succsessfully.");

	}

	public void SaveGrid(StreamWriter writer){
		
		//get the grid
		GridManager gridManager = GameObject.Find("Managers").GetComponent<GridManager>();

		for(int y=0;y<10;y++){
			string line = "";
			for(int x=0;x<10;x++){
				Hex hex = gridManager.GetHex(x,y);
				int val = hex.GetVal();
				if(val == 8){
					val = 0;
				}
				string v = val.ToString();

				//put a ',' between each number except the last one 
				string divider = ",";
				if(x==9){
					divider = "";
				}
				line = line + v + divider;

			}
			writer.WriteLine(line);

		}
		Debug.Log(TAG + "grid saved.");
	}

	public void SaveTools(StreamWriter writer){
		for(int i=0;i<6;i++){
			ToolSelect ts = toolSelects[i];
			string toolName = ts.GetToolType();
			string toolVal = ts.GetColorVal();

			string toolText = toolName + ":" + toolVal;
			writer.WriteLine(toolText);
		}
		Debug.Log(TAG + "tools saved.");

	}

	public void Exit(){
		Debug.Log(TAG + "Exitting Level Editor.");
		//AssetDatabase.Refresh();
		CloseAll();
		mainManager.ExitLevelEditor();

	}
	public void ClearAll(){
		Debug.Log(TAG + "clearing");
		GridManager gridManager = GameObject.Find("Managers").GetComponent<GridManager>();

		//Clear the grid
		for(int y=0;y<10;y++){
			for(int x=0;x<10;x++){
				Hex h = gridManager.GetHex(x,y);
				h.SetVal(0);
				h.SetColor(true);
			}
		}

		//Clear the tools
		foreach(ToolSelect t in toolSelects){
			t.SetVal("none");
			t.colorSelect.SetVal(7);
			//close all the menus
			t.colorSelect.SetColorMenu(false);
			t.SetToolMenu(false);
		}
	}

	public void OpenLoadMenu(bool b){
		Debug.Log(TAG + "opening load menu." );
		ClearLevelButs();
		SetDeleteMode(false);
		loadMenu.SetActive(b);
		optionMenu.SetActive(false);
		levelManager.LoadAllCustomChapters();
		if(b){
			Level[] levels = levelManager.GetCustomLevels(chapterRefNum);
			Debug.Log(TAG + "levels length = " + levels.Length );
			levelButtons = menuManager.CreateLevelButs(levelCon,levels,true);
			CreateChapterButtons(loadChapterCon,true);
			return;
		}

	}
	public void ClearLevelButs(){
		Debug.Log(TAG + "clearing level buttons.");
		if(levelButtons != null){
			foreach(LevelButton lb in levelButtons){
				Destroy(lb.gameObject);
			}
		}
		levelButtons = null;
	}
	public void OpenPopUpDelete(bool b){
		popUpDelete.SetActive(b);

	}
	public void SelectLevel(int levelNum){
		Debug.Log(TAG + "selecting level, delete mode is " + deleteMode );
		if(deleteMode){
			toDeleteFile = true;
			OpenPopUpDelete(true);
			string folder = curChapter;
			string[] fileArray = Directory.GetFiles(folder,"*.txt");
			Debug.Log(TAG + fileArray[levelNum]);

			Text msgText = popUpDelete.GetComponentInChildren<Text>();
			toDelete = fileArray[levelNum];
			string[] fileName = toDelete.Split('/');
			msgText.text = "Are you sure you want to delete " + fileName[fileName.Length -1] + "?";
			return;
		}
		LoadLevel(levelNum);

	}
	public void UpdateSaveInputs(Level level){
		Debug.Log(TAG + "updating the save menu input fields.");
		levelNameInput.text = level.GetName();
		fileNameInput.text = level.GetFileName();

	}
	public void DeleteFile(){
		if(toDeleteFile){
			File.Delete(toDelete);
			Debug.Log(TAG + toDelete + " was deleted.");
			OpenPopUpDelete(false);
			OpenLoadMenu(true);
			return;
		}
		//if it is a folder
		Directory.Delete(toDelete, true);
		Debug.Log(TAG + toDelete + " was deleted.");
		chapterRefNum = 0;
		OpenPopUpDelete(false);
		OpenLoadMenu(true);
	}
	public void LoadLevel(int levelNum){
		Debug.Log(TAG + "loading the level" );


		Level level = levelManager.GetCustomLevels(chapterRefNum)[levelNum];

		 
		LoadGrid(level);
		LoadTools(level);

		//makes the inputs on the save menu match the level that was just loaded
		//makes editing existing levels more efficient
		UpdateSaveInputs(level);
	
	}

	public void LoadTools(Level level){
		Debug.Log(TAG + "loading the tools" );
		Level.LevelTool[] levelTools = level.GetTools();

		for(int i=0;i<6;i++){
			ToolSelect t = toolSelects[i];
			Level.LevelTool levelTool = levelTools[i];

			string toolType = levelTool.GetToolType();
			int val = levelTool.GetVal();

			if(toolType == "none"){
				val = 7;
			}

			t.SetVal(toolType);
			t.colorSelect.SetVal(val);
			t.colorSelect.SetColorMenu(false);
			t.SetToolMenu(false);
		}
	}

	public void LoadGrid(Level level){
		Debug.Log(TAG + "loading the Grid" );
		GridManager gridManager = GameObject.Find("Managers").GetComponent<GridManager>();
		for(int y=0;y<10;y++){

			for(int x=0;x<10;x++){
				Hex hex = gridManager.GetHex(x,y);
				int val = level.GetVal(x,y);
				hex.SetVal(val);
				hex.SetColor(true);

			}
		}
	}
	public void SelectChapter(string s,bool updateButtons,int i,ChapterButton c){
		//used for selecting the chapter the user wishes to save to
		//TODO could just refernce the curChapterButton and get rid of all the other variables
		Debug.Log(TAG + "Selecting Chapter: " + s);
		curChapter = s;
		chapterRefNum = i;
		if(curChapterButton != null){
			curChapterButton.Select(false);
		}
		if(deleteMode){
			toDeleteFile = false;
			OpenPopUpDelete(true);



			Text msgText = popUpDelete.GetComponentInChildren<Text>();
			toDelete = curChapter;
			string[] fileName = toDelete.Split('/');
			msgText.text = "Are you sure you want to delete " + fileName[fileName.Length -1] + "?";
			return;
		}



		curChapterButton = c;
		if(updateButtons){
			ClearLevelButs();
			Level[] levels = levelManager.GetCustomLevels(chapterRefNum);
			if(levels == null){
				Debug.Log(TAG + "no level button to create.");
				return;
			}
			levelButtons = menuManager.CreateLevelButs(levelCon,levels,true);
		}
	}
	public void CreateChapterButtons(GameObject container,bool updateButtons){
		Debug.Log(TAG + "Clearing the old chapter buttons");
		if(chapterButtons != null){
			foreach(ChapterButton c in chapterButtons ){
				Destroy(c.gameObject);
			}
			chapterButtons = null;
		}
		Debug.Log(TAG + "Creating Chapterr buttons.");


		string[] folders = Directory.GetDirectories(Application.persistentDataPath + "/CustomChapters/");
		Debug.Log(TAG + "___________________there are " + folders.Length + " custom chapters________________");

		//TODO clear out old chapter buttons
		chapterButtons = new ChapterButton[folders.Length];
		float x1,y1,x2,y2,buttonWidth,buttonHeight,padT;
		buttonWidth =  150.0f;
		buttonHeight = 35.0f;
		padT = 5.0f;

		x1 = 0;
		x2 = x1 + buttonWidth;
		y1 = -buttonHeight-padT;
		y2 = y1 + buttonHeight;

		for(int i=0;i< folders.Length;i++){
			string s = folders[i];
			Debug.Log("folder = " + s);
			GameObject g = Instantiate(chapterButtonPrefab);
			g.transform.SetParent(container.transform);
			ChapterButton chapterButton = g.GetComponent<ChapterButton>();
			chapterButton.SetUp(s,updateButtons,i,this);

			RectTransform r = g.GetComponent<RectTransform>();

			r.offsetMin = new Vector2(x1,y1);
			r.offsetMax = new Vector2(x2,y2);

			y1 -= buttonHeight+padT;
			y2 = y1 + buttonHeight;
			chapterButtons[i] = chapterButton;

		}
		chapterButtons[0].Press();//Select a chapter button by default
		curChapterButton = chapterButtons[0];


	}
	public void CreateNewChapter(){
		Debug.Log(TAG + "creating new chapter.");
		//TODO check if chapter alreaady exists
		//TODO check if is a valid folder name
		Directory.CreateDirectory(Application.persistentDataPath + "/CustomChapters/" + newChapterName.text);

		CreateChapterButtons(saveChapterCon,false);
		OpenCreateChapter(false);

	}

}

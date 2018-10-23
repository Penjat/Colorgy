using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	private static string TAG = "MENU MANAGER: ";

	public Camera c;
	public RectTransform canvas;
	public Transform worldMenuPos;

	public MainManager mainManager;
	public LevelManager levelManager;
	public LevelEditor levelEditor;
	public ChapterManager chapterManager;
	public TutorialManager tutorialManager;
	public ButtonManager buttonManager;
	public SoundFXManager soundManager;


	public IntroAnimate intro;

	public GameObject menuTitle;
	public GameObject menuTitleSolid;
	public GameObject menuLevelSel;
	public GameObject menuGamePlay;//realworld gameObjects
	public GameObject menuGamePlayUI;//Canvas elements of the ui
	public GameObject menuInGame;
	public GameObject menuLevelBeaten;
	public GameObject menuAllLevelsBeaten;
	public GameObject menuLevelEditor;
	public GameObject levelButtonCon;
	public GameObject menuPlayCustomLevels;
	public GameObject menuSettings;
	public GameObject textChapterIntro;
	public GameObject menuGameOver;
	public GameObject backToCustomLevels;
	public GameObject sorryWebGl;
	public GameObject menuDemo;

	public ScrollRect scrollRect;
	private GameObject curMenu;
	private GameObject curWorldMenu;
	public GameObject menuContactPrefab;


	public GameObject levelButPrefab;
	private LevelButton[] levelButtons;

	public Animator levelBeatenAni;
	public Animator folderBeatenAni;
	public Animator customFolderBeatenAni;

	public GameObject menuButton;
	public GameObject resetButton;

	public Text versionNumber;

	public Text folderBeatenText;

	private bool chapterIntro;
	public Text textChapterNum;
	public Text textChapterName;
	private float timer;

	private int chapterNum;//is stored for pausing while inntroducing chapters
	private int levelNum;

	public GameObject debugPrefab;


	private bool highlightCurrent;//controls whether the current level should be highlited

	public GameObject endCreditsPrefab;

	private CreditsManager creditManager;

	void Update(){

		if(chapterIntro){
			timer -= Time.deltaTime;

			if(timer<=0){
				//soundManager.PlayUISound(0);
				chapterIntro = false;
				mainManager.StartGame(chapterNum,levelNum,false);
			}
		}
	}
	public void CloseContactMenu(){
		Destroy(curMenu);
		ToTitle();
	}
	public void OpenContactMenu(){
		CloseAll();
		GameObject g = Instantiate(menuContactPrefab);
		g.transform.SetParent(canvas,false);
		curMenu = g;
		Menu menu = g.GetComponent<Menu>();
		menu.SetButtonManager(buttonManager);
	}
	public void OpenLink(int i){
		switch(i){
		case 0:
			Application.OpenURL("https://penjat.itch.io/colorgy");
			break;
		case 1:
			Application.OpenURL("http://play.google.com/store/apps/details?id=<com.Anxiomancer.Colorgy>");
			break;
		case 5:
			Application.OpenURL("https://Colorgy.wordpress.com");
			break;
		case 6:
			Application.OpenURL("https://thespencersymington.wordpress.com");
			break;
		case 7:
			Application.OpenURL("https://fb.me/Anxiomancer");
			break;

			Application.OpenURL("https://penjat.itch.io/colorgy");
		}
	}
	public DebugManager CreateDebug(){
		GameObject g = Instantiate(debugPrefab);
		g.transform.SetParent(canvas,false);
		return g.GetComponent<DebugManager>();
	}

	void Start(){
		//Sets the positioning of the buttons depending on screen size
		//In Game menu
		Vector3 v = menuButton.transform.position;

		Vector3 edgeOfScreen = c.ScreenToWorldPoint(new Vector3 (0,0,v.z) );


		menuButton.transform.position = edgeOfScreen + new Vector3(1.2f,0,1);
		//Reset Button
		v = resetButton.transform.position;
		resetButton.transform.position = edgeOfScreen + new Vector3(2.8f,0,1);
	}

	public void CloseAll(){
		menuTitle.SetActive(false);
		menuLevelSel.SetActive(false);
		menuGamePlay.SetActive(false);
		menuGamePlayUI.SetActive(false);
		menuInGame.SetActive(false);
		menuLevelBeaten.SetActive(false);
		menuGameOver.SetActive(false);
		menuLevelEditor.SetActive(false);
		menuAllLevelsBeaten.SetActive(false);
		menuTitleSolid.SetActive(false);
		chapterManager.ClearChapters();
		menuPlayCustomLevels.SetActive(false);
		menuSettings.SetActive(false);
		textChapterIntro.SetActive(false);
		backToCustomLevels.SetActive(false);
		menuDemo.SetActive(false);
		tutorialManager.Hide();
	
	}
	public void WebGLSorry(){
		//open when trying to accsess level editor or custom levels from webGL
		GameObject g = Instantiate(sorryWebGl);
		g.transform.SetParent(canvas,false);

	}
	public void OpenSettings(){
		CloseAll();
		menuSettings.SetActive(true);
		versionNumber.text = "version: "+ mainManager.GetVersionNuber();
	}


	public void Reset(){
		menuLevelBeaten.SetActive(false);
	}

	public void OpenCustomLevels(){
		if(mainManager.GetBuildVersion() == 3){
			WebGLSorry();
			return;
		}
		CloseAll();
		levelManager.LoadAllCustomChapters();
		chapterManager.LoadCustomChapters();
		menuPlayCustomLevels.SetActive(true);

	}

	public void ToLevelSelect(){
		Debug.Log(TAG + "to level select.");
		if(!intro.GetIntroHasHappened()){
			//wait for intro to finish
			return;

		}

		CloseAll();
		//Check if this is the first time playing the game

		if(ProgressManager.IsFirstTimePlaying()){
			//skip the level select and go straight to the game
			StartChapterIntro(0,0);

			//mainManager.StartGame(0,0,false);
			return;
		}
		chapterManager.LoadNormalChapters();
		menuLevelSel.SetActive(true);
		//scrollRect.verticalNormalizedPosition = 0f;
		/*
		levelManager.SetCurFolder(i);
		CloseAll();
		levelButtons =  GetLevelButtons(levelButtonCon);
		menuLevelSel.SetActive(true);
		*/
	}
	public void BackToTitle(){
		//call this after playing credits

		intro.ReStartIntro();
		ToTitle();

	}
	public void ToTitle(){
		Debug.Log(TAG + "to title menu.");
		CloseAll();

		menuTitle.SetActive(true);
		menuTitleSolid.SetActive(true);



		highlightCurrent = false;
		intro.StartIntro();
		if(creditManager){
			creditManager.StopCredits();
		}
	}

	public void ToGamePlay(){
		Debug.Log(TAG + "to game.");

		CloseAll();
		menuGamePlay.SetActive(true);
		menuGamePlayUI.SetActive(true);

	}
	public void ClearButtons(){
		if(levelButtons!= null){
			foreach(LevelButton l in levelButtons){
				Destroy(l.gameObject);
			}
		}
	}


	public LevelButton[] GetLevelButtons(GameObject container){
		ClearLevelButs();
		Level[] levels = levelManager.GetLevels();

		return CreateLevelButs(container,levels,false);
	}
	public LevelButton[] CreateLevelButs(GameObject container,Level[] levels,bool editMode){
		
		Debug.Log(TAG + "creating the level select button: " + levels.Length);

		LevelButton[] levelBs = new LevelButton[levels.Length];
		float containerWidth = container.GetComponent<RectTransform>().rect.width;

		Debug.Log(TAG + "container width = " + containerWidth);


		float buttonWidth = 100;
		float buttonHeight = 50;
		float topPadding = 5.0f;
		float sidePadding = 5.0f;


		float x1 = sidePadding;
		float y1 = -buttonHeight-topPadding;
		float x2 = x1+buttonWidth;
		float y2 = y1+buttonHeight;

		for(int i=0;i<levels.Length;i++){


			GameObject g = Instantiate(levelButPrefab);
			g.transform.SetParent(container.transform);
			RectTransform rect = g.GetComponent<RectTransform>();


			LevelButton levelButton = g.GetComponent<LevelButton>();
			Level level = levels[i];
			levelButton.SetLevelNum(i,this,editMode);
			levelButton.SetName(level.GetName());
			levelBs[i] = levelButton;

			rect.offsetMin = new Vector2(x1,y1);
			rect.offsetMax = new Vector2(x2,y2);

			x1 += buttonWidth+sidePadding;

			x2 = x1+buttonWidth;


			//if the next button will be out of the container, make a new row

			if(x2  > containerWidth){
				x1 = sidePadding;
				x2 = x1+buttonWidth;

				y1 += -buttonHeight-topPadding;
				y2 = y1+buttonHeight;

			}


		}
		return levelBs;
	}
	public void SelectLevelEditMode(int levelNum){
		levelEditor.SelectLevel(levelNum);
	}
	public void SelectLevel(LevelRefButton levelButton){
		Debug.Log(TAG + "selecting level.");
		//if(b){
			//TODO fix for level editor
		//levelEditor.LoadLevel(chapterNum,levelNum);
			//return;
		//}
		if(levelButton.GetLevelNum() == 0 && !levelButton.GetIsCustom()){
			StartChapterIntro(levelButton.GetChapter(),0);
			return;
		}
		mainManager.StartGame(levelButton);
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

	public void InGameMenu(bool b){
		Debug.Log(TAG + "inGame menu = " + b);
		menuInGame.SetActive(b);
		soundManager.PlayUISound(4);
	}
	public void ToggleInGameMenu(){
		//Debug.Log(TAG + "inGame menu = ");

		menuInGame.SetActive(!menuInGame.activeSelf);
		soundManager.PlayUISound(4);
	}
	public void OpenDemoMenu(){
		CloseAll();
		menuDemo.SetActive(true);
	}
	public void LevelBeaten(){
		
		Debug.Log(TAG + "level beaten menu.");
		int curFolder = levelManager.GetCurFolder();
		int curLevel = levelManager.GetCurLevelNum();
		bool isCustom = levelManager.GetIsCustom();

		tutorialManager.FadeOut();

		Level[] levels;
		if(isCustom){
			levels = levelManager.GetCustomLevels(curFolder);
		}else{
			levels = levelManager.GetLevels();
		}
		//If still levels left
		if(levels.Length > curLevel+1){
			menuAllLevelsBeaten.SetActive(false);
			menuLevelBeaten.SetActive(true);
			levelBeatenAni.Play("fadeInComplete");
			ProgressManager.CheckLevelBeaten(curFolder,curLevel);

			return;
		}
		//if this was the last level
		//check if there are more chapters
		//don't do it for custom chapters
		if(levelManager.GetIsCustom()){
			menuAllLevelsBeaten.SetActive(false);
			menuLevelBeaten.SetActive(false);
			backToCustomLevels.SetActive(true);
			customFolderBeatenAni.Play("fadeInComplete");
			return;
		}
		if(chapterManager.GetChapters().Length > levelManager.GetCurFolder() + 1){
			Debug.Log(TAG + "there are more chapters");
			if(mainManager.GetISDemo() && levelManager.GetCurFolder() + 1 > 2){
				OpenDemoMenu();
				ProgressManager.CheckChapterBeaten(curFolder);
				return;
			}
			folderBeatenText.text = "Chapter "+(curFolder+1).ToString() + " Complete";
			menuLevelBeaten.SetActive(false);
			menuAllLevelsBeaten.SetActive(true);
			folderBeatenAni.Play("fadeInComplete");
			ProgressManager.CheckChapterBeaten(curFolder);
			return;
		}

		Debug.Log(TAG + "all levels complete");

		menuLevelBeaten.SetActive(false);
		//menuGameOver.SetActive(true);
		folderBeatenAni.Play("fadeInComplete");

		//Show the end credits
		GameObject g = Instantiate(endCreditsPrefab);
		menuButton.SetActive(false);
		resetButton.SetActive(false);
		g.transform.position = c.transform.position - new Vector3(0,20,0);
		CreditsManager creditManager = g.GetComponent<CreditsManager>();
		creditManager.LoadCredits(this,soundManager);

		creditManager.CreateCreditOb();

	}

	public void ToLevelEditor(){
		Debug.Log(TAG + "opening level editor.");
		CloseAll();
		menuLevelEditor.SetActive(true);
	}

	public void MenuButtonPress(int i){
		switch(i){

		case 3:
			ToggleInGameMenu();
			break;
		case 4:
			mainManager.ResetLevel();
			break;
		}

	}
	public void BackToLevelSelect(){
		//is used from the inGame menu to navigate back to the level select menu
		//checks whether it should be the normal or custom levels
		highlightCurrent = true;
		soundManager.PlayUISound(4);
		if(levelManager.GetIsCustom()){
			OpenCustomLevels();
			return;
		}
		ToLevelSelect();
		scrollRect.verticalNormalizedPosition = 0f;
	}
	public void StartChapterIntro(int cNum, int lNum){
		Debug.Log(TAG + "showing the chapter intro.");
		chapterNum = cNum;
		levelNum = lNum;
		textChapterNum.text = "Chapter " + (chapterNum + 1).ToString();

		ChapterManager.ChapterRef chapter = chapterManager.GetChapters()[chapterNum];
		textChapterName.text = chapter.GetName();
		ToGamePlay();
		textChapterIntro.SetActive(true);
		chapterIntro = true;
		timer = 3.5f;
		int chapterSound = chapter.GetStartSound();
		soundManager.PlayUISound(chapterSound,0.8f);
	}
	public void CheckLevelCur(LevelRefButton l){
		if(!highlightCurrent){
			return;
		}
		int lNum = levelManager.GetCurLevelNum();
		int cNum = levelManager.GetCurFolder();

		if(cNum == l.GetChapter() && lNum == l.GetLevelNum() ){
			l.IsCurrent();

		}

	}
	public bool GetIsDemo(){
		return mainManager.GetISDemo();
	}
}

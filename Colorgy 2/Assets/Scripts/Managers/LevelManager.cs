using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class LevelManager : MonoBehaviour {
	private static string TAG = "LEVEL MANAGER: ";


	private LevelCon[] levelFolder;
	private LevelCon[] customLevelFolder;//TODO make with custom chapters
	private int curLevel;
	private int curFolder;
	private bool isCustom;



	private bool willmodulate = true;

	public void SetUp(ChapterManager chapterManager){
		Debug.Log(TAG + "setting up");
		ChapterManager.ChapterRef[] chapters = chapterManager.GetChapters();
		levelFolder = new LevelCon[chapters.Length];

		for(int i=0;i<chapters.Length;i++){
			ChapterManager.ChapterRef chapter = chapters[i];
			string folder = chapter.GetFolder();
			levelFolder[i] = new LevelCon(LoadCurLevels(folder));
		}


		if(!Directory.Exists(Application.persistentDataPath + "/CustomChapters")){
			Debug.Log(TAG + "no folder found, creating now...");
			Directory.CreateDirectory(Application.persistentDataPath + "/CustomChapters");
			//there will be no levels so just return
			//return;
		}
		//posibly don't have to load custom chapters until needed
		//LoadAllCustomChapters();


	}
	public void LoadAllCustomChapters(){
		string[] folders = Directory.GetDirectories(Application.persistentDataPath + "/CustomChapters/");

		Debug.Log(TAG + "___________________there are " + folders.Length + " custom chapters________________");
		customLevelFolder = new LevelCon[folders.Length];
		for(int i=0;i<folders.Length;i++){
			
			string folder = folders[i];
			string[] s = folder.Split('/'); 
			string n = s[s.Length-1];
			string[] nn = n.Split('.');
			string name = nn[0];

			Debug.Log(TAG + "folder is "+ folder);
			LevelCon l = new LevelCon(LoadCustomLevels(folder));
			l.SetFolderName(name);
			customLevelFolder[i] = l;


		}
	}
	public Level[] GetCustomLevels(int i){
		Debug.Log(TAG + "getting the custom levels ");
		Debug.Log(TAG + "custom level folder length =  " +customLevelFolder.Length);

		if(customLevelFolder.Length > i){
			return customLevelFolder[i].GetLevels();
		}
		return null;
	}

	public Level[] LoadCustomLevels(string folder){
		Debug.Log(TAG + "loading custom levels " + folder);


		string[] fileArray = Directory.GetFiles(folder,"*.txt");
		Debug.Log(TAG + "File array length = " + fileArray.Length);

		Level[] levels = new Level[fileArray.Length];
		for(int i=0;i<levels.Length;i++){
			levels[i] = LoadLevel(fileArray[i]);

		}
		if(levels == null){
			Debug.Log(TAG + "levels is null");
		}else{
			Debug.Log(TAG + "levels is not null");
		}
		return levels;

	}
	public void SetCurFolder(int i){
		curFolder = i;
	}
	public int GetCurFolder(){
		return curFolder;
	}
	public void SetCurLevel(int i){
		curLevel = i;
	}
	public int GetCurLevelNum(){
		return curLevel;
	}

	public Level[] GetLevels(){
		
		return levelFolder[curFolder].GetLevels();
	}
	public Level GetLevel(int chapterNum,int levelNum,bool c){
		curFolder = chapterNum; 
		curLevel = levelNum; 
		isCustom = c;
		if(isCustom){
			return customLevelFolder[curFolder].GetLevels()[curLevel];
		}

		return levelFolder[curFolder].GetLevels()[curLevel];
	}
	public Level GetCurLevel(){
		if(isCustom){
			return customLevelFolder[curFolder].GetLevels()[curLevel];
		}
		return levelFolder[curFolder].GetLevels()[curLevel];
	}
	public bool GetIsCustom(){
		//is used to tell if we should get the custom folder or the normal folder
		return isCustom;
	}
	public Level[] GetLevels(int i){

		return levelFolder[i].GetLevels();
	}
	public LevelCon[] GetLevelFolder(){
		return levelFolder;
	}
	public LevelCon[] GetCustomLevelFolder(){
		return customLevelFolder;
	}

	public Level LoadLevel(string filePath){
		//this method is used for custom levels
		Debug.Log(TAG + "loading the level " + filePath);
		StreamReader reader = new StreamReader(filePath); 


		Level level = new Level();
		//string[] levelData = textFile.ToString().Split('\n');//Split the 

		//Get the level name
		string name = reader.ReadLine();

		level.SetName(name);

		//Get the grid
		int[,] grid = new int[10,10];
		for(int y=0; y<10;y++){
			string row = reader.ReadLine();

			string[] items = row.Split(',');
			for(int x=0; x<10;x++){
				grid[x,y] = int.Parse( items[x].ToString());

			}
		}
		level.SetGrid(grid);
		level.SetFileName(filePath);


		//Get the tools
		for(int i=0;i<6;i++){
			string tool = reader.ReadLine();

			string[] s = tool.Split(':');
			int val = int.Parse( s[1]);
			level.AddTool(i,s[0],val);
		}

		//Set the random modulation
		if(level.GetWillMod()){
			int r = Random.Range(0,3);
			level.SetModNum(r);

			if(Random.value > 0.5f){
				level.SetRotation(180.0f);
			}else{
				level.SetRotation(0.0f);
			}
			return level;
		}

		//If it doesn't modualte
		level.SetRotation(0.0f);
		level.SetModNum(0);

		return level;
	}
	public Level LoadLevel(TextAsset textFile){
		//Turns the textAsset into a level
		//This method is used for levels included in the game
		Debug.Log(TAG + "loading the level " + textFile.name);
		//StreamReader reader = new StreamReader(fileName); 

		int curLine = 0;
		Level level = new Level();
		string[] levelData = textFile.ToString().Split('\n');//Split the 

		//Get the level name
		string name = levelData[curLine];
		curLine++;
		level.SetName(name);
		level.SetFileName(textFile.name);
		//Get the grid
		int[,] grid = new int[10,10];
		for(int y=0; y<10;y++){
			string row = levelData[curLine];
			curLine++;
			string[] items = row.Split(',');
			for(int x=0; x<10;x++){
				grid[x,y] = int.Parse( items[x].ToString());

			}
		}
		level.SetGrid(grid);
		//Not setting the level's name here as it probably won't be needed for the built in levels

		//Get the tools
		for(int i=0;i<6;i++){
			string tool = levelData[curLine];
			curLine++;
			string[] s = tool.Split(':');
			int val = int.Parse( s[1]);
			level.AddTool(i,s[0],val);
		}
		level = GetExtraValues(levelData,curLine,level);


		//Set the random modulation
		if(level.GetWillMod()){
			int r = Random.Range(0,3);
			level.SetModNum(r);

			if(Random.value > 0.5f){
				level.SetRotation(180.0f);
			}else{
				level.SetRotation(0.0f);
			}
			return level;
		}

		//If it doesn't modualte
		level.SetRotation(0.0f);
		level.SetModNum(0);

		return level;
	}

	public Level GetExtraValues(string[] levelData, int curLine, Level level){
		int dataLength = levelData.Length;
		Debug.Log(TAG + "adding extra. CurLine = " + curLine + " dataLength =" + dataLength  );
		//check if line is balnk as old levels will not have this data
		if(dataLength > curLine && levelData[curLine] != ""){
			string willMod = levelData[curLine].Split(':')[1];
			level.SetWillMod(willMod);
			curLine++;
		}
		if(dataLength > curLine && levelData[curLine] != ""){
			string willShuffle = levelData[curLine].Split(':')[1];
			level.SetShuffleTools(willShuffle);
			curLine++;
		}
		if(dataLength > curLine && levelData[curLine] != ""){
			string tutorialMsg = levelData[curLine].Split(':')[1];
			level.SetTutorialMsg(tutorialMsg);
			curLine++;
		}
		Debug.Log(TAG + "done adding extra.");
		return level;
	}

	public Level[] LoadCurLevels(string folder){
		//Loads all the levels in the specified folder


		TextAsset[] textAssets = Resources.LoadAll<TextAsset>("Levels/" + folder);
		Debug.Log(TAG + "there are files "+textAssets.Length);
		Level[] levels = new Level[textAssets.Length];

		int i=0;
		foreach(TextAsset textFile in textAssets){
			Debug.Log("name is: "+textFile.name);
			//levels[i] = LoadLevel("Assets/Resources/Levels/"+folder +"/"+o.name+".txt");
			levels[i] = LoadLevel(textFile);
			i++;
		}
		return levels;
	}
	public class LevelCon{
		//a container for each level array
		private string folderName;
		Level[] levels;
		public LevelCon(Level[] l){
			levels = l;
		}
		public Level[] GetLevels(){
			return levels;
		}
		public void SetFolderName(string s){
			folderName = s;
		}
		public string GetFolderName(){
			return folderName;
		}
	}
	public void NextLevel(){
		curLevel++;
		Debug.Log(TAG + "next level: " + curLevel);


	}
	public void NextChapter(){
		curFolder++;
		curLevel = 0;
		Debug.Log(TAG + "next chapter: " + curFolder);
	}


}

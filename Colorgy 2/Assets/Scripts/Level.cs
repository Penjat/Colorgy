using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level  {
	private int[,] grid;
	private LevelTool[] tools;
	private bool willMod = true;//will the level modualte
	private bool willShuffleTools = true;
	private int modulationNumber = 0;
	private float rot;
	private string fileName;
	private string tutorialMsg = "";

	private string name;

	public Level(){
		Debug.Log("Creating a level");
		tools = new LevelTool[6];
	}
	public void SetRotation(float f){
		rot = f;
	}
	public float GetRotation(){
		return rot;
	}

	public void SetModNum(int i){
		//sets the modulation number
		modulationNumber = i;
	}
	public int GetModNum(){
		return modulationNumber;
	}

	public int GetVal(int x,int y){
		return grid[x,y];
	}
	public void CreateBlankGrid(){
		Debug.Log("Creating a blank grid.");
		grid = new int[10,10];
	}

	public void SetGrid(int[,] g){
		grid = g;
	}

	public void AddTool(int number,string s,int v){
		tools[number] = new LevelTool(s,v);

	}
	public LevelTool[] GetTools(){
		return tools;
	}
	public string GetName(){
		return name;
	}
	public void SetName(string s){
		name = s;
	}
	public string GetFileName(){
		return fileName;
	}
	public void SetFileName(string s){
		//fileName is used as an easy refernce of where to resave the file
		string[] s1 = s.Split('/');
		string s2 = s1[s1.Length-1];
		string[] s3 = s2.Split('.');
		fileName = s3[0];
	}
	public void SetTutorialMsg(string s){
		tutorialMsg = s;
	}
	public void SetWillMod(string s){
		willMod = (s == "true");
	}
	public bool GetWillMod(){
		return willMod;
	}
	public void SetShuffleTools(string s){
		willShuffleTools = (s == "true");
	}
	public string GetTutorialMsg(){
		return tutorialMsg;
	}


	public class LevelTool{
		string toolType;
		int val;
		public LevelTool(string s, int v){
			toolType = s;
			val = v;
		}
		public string GetToolType(){
			return toolType;
		}
		public int GetVal(){
			return val;
		}
	}


}

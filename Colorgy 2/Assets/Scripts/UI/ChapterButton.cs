using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterButton : MonoBehaviour {
	private LevelEditor levelEditor;
	string chapterName;
	public Text t;
	public Image im;
	private bool updateButtons;
	private int refNum; 


	public void SetUp(string s,bool b,int i,LevelEditor l){
		Debug.Log("setting up Chapter Button");
		chapterName = s;
		string[] ss = s.Split('/');
		t.text = ss[ss.Length-1];
		updateButtons = b;
		refNum = i;
		levelEditor = l;
		Select(false);
	}
	public void Press(){
		Debug.Log("pressing Chapter Button: " + chapterName);

		Select(true);
		levelEditor.SelectChapter(chapterName,updateButtons,refNum,this);


	}
	public void Select(bool b){
		if(b){
			im.color = Color.red;
			return;
		}
		im.color = Color.grey;
	}
}

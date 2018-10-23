using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour {

	public Text levelName;
	public Text levelNumber;
	public Text chapterNumber;


	public void SetUp(string s,int cNum, int lNum){
		levelName.text = s;
		levelNumber.text = lNum.ToString();
		chapterNumber.text = cNum.ToString();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chapter : MonoBehaviour {
	private static string TAG = "CHAPTER: ";

	public GameObject levelButPrefab;


	public Text chapterNum;
	public Text chapterName;
	public Text amtComplete;
	public GameObject levelCon;




	public void SetName(int i,string name,float amt){
		chapterNum.text = "CHAPTER "+ i.ToString();
		chapterName.text = name;
		//amtComplete = amt.ToString();//TODO calculate and also only show one deccimal

	}
	public float SetUp(float width,Level[] levels,int folderNum,MenuManager menuManager,bool isCustom){
		Debug.Log(TAG + "setting up.");

		Debug.Log(TAG + "create the level buts");
		float topPadding = 100.0f;
		float x1,x2,y1,y2;
		float levelWidth,levelHeight;
		float padR,padL,padT,padB;

		padL = 10.0f;
		padT = 10.0f;

		levelWidth = 50.0f;
		levelHeight = 50.0f;

		x1 = 0.0f + padL;
		y1 = - levelHeight-padT;
		x2 = x1 + levelWidth;
		y2 = y1 +levelHeight;
		RectTransform levelConRec = levelCon.GetComponent<RectTransform>();

		levelConRec.offsetMin = new Vector2(0,-500);
		levelConRec.offsetMax = new Vector2(width,-topPadding);

		if(!isCustom && ProgressManager.CheckChapterLocked(folderNum)){
			gameObject.SetActive(false);
			return 0;
		}


		bool didIncrease = false;//is used to tell if we did increase y1 just before the loop finishes


		for(int i=0;i<levels.Length;i++){
			didIncrease = false;
			GameObject g = Instantiate(levelButPrefab);
			g.transform.SetParent(levelCon.transform);
			RectTransform r = g.GetComponent<RectTransform>();

			r.offsetMin = new Vector2(x1,y1);
			r.offsetMax = new Vector2(x2,y2);



			x1 += levelWidth+padL;
			//y1 = - levelHeight;

			if(x2 > levelConRec.rect.width){
				x1=0.0f + padL;
				y1 -=levelHeight+padT;
				didIncrease = true;
			}
			x2 = x1 + levelWidth;
			y2 = y1 +levelHeight;

			LevelRefButton levelRef = g.GetComponent<LevelRefButton>();

			levelRef.SetUp(folderNum,i,menuManager,isCustom);

			//check whether this is the current
			menuManager.CheckLevelCur(levelRef);

			//check if this is the furthest unlocked level
			if(!isCustom){
				ProgressManager.CheckLocked(levelRef);
			}


		}


		if(didIncrease){
			return y1 - topPadding;
		}
		return y1 -levelHeight+padT- topPadding;
	}

}

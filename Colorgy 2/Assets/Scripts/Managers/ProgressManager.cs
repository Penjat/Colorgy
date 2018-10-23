using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProgressManager{
	private static string TAG = "PROGRESS MANAGER: ";
	private static string LEVEL = "LEVEL";
	private static string CHAPTER = "CHAPTER";
	private static bool DEBUG = false;

	public static void ResetProgress(){
		PlayerPrefs.DeleteAll();
	}

	public static void CheckLocked(LevelRefButton button){
		//Checks whether the button should be locked based on the current progress
		if(DEBUG){
			return;
		}

		int levelProgress = PlayerPrefs.GetInt(LEVEL);
		int chapterProgress= PlayerPrefs.GetInt(CHAPTER);

		int levelNum = button.GetLevelNum();
		int chapterNum = button.GetChapter();

		Debug.Log(TAG + "levelP = " + levelProgress + " Chapter p = " + chapterProgress + " levelNum = " + levelNum + " chapterNum = " + chapterNum   );
		//if this is the chapter with incomplete levels
		if(chapterProgress == chapterNum){
			

			//if this is the next level to beat
			if(levelProgress == levelNum){
				button.Highlight();
				return;
			}

			if(levelProgress < levelNum){
				button.SetLocked(true);
				return;
			}
			button.SetLocked(false);
			return;
		}
		//have already beaten this chapter
		if(chapterProgress > chapterNum){
			button.SetLocked(false);
			return;
		}
		//haven't reached this chapter yet
		button.SetLocked(true);
	}
	public static bool CheckChapterLocked(int chapterNum){
		if(DEBUG){
			return false;
		}
		int chapterProgress= PlayerPrefs.GetInt(CHAPTER);

		if(chapterProgress< chapterNum){
			return true;
		}
		return false;
	}

	public static void CheckLevelBeaten(int chapterNum, int levelNum ){
		
		//check if we need to increase the game progress
		//is called when a level is beaten

		if(DEBUG){
			return;
		}

		int levelProgress = PlayerPrefs.GetInt(LEVEL);
		int chapterProgress= PlayerPrefs.GetInt(CHAPTER);

		//if this is the current chapter and level
		if(chapterNum == chapterProgress && levelNum == levelProgress){
			PlayerPrefs.SetInt(LEVEL,levelProgress+ 1);
		}

	}
	public static void CheckChapterBeaten(int chapterNum){
		//check if we need to increase the game progress
		//is called when a chapter is beaten
		if(DEBUG){
			return;
		}
		int chapterProgress= PlayerPrefs.GetInt(CHAPTER);

		//if this is the current chapter and level
		if(chapterNum == chapterProgress){
			PlayerPrefs.SetInt(CHAPTER,chapterProgress+ 1);
			PlayerPrefs.SetInt(LEVEL,0);
		}

	}
	public static bool GetDebug(){
		return DEBUG;
		}
	public static bool IsFirstTimePlaying(){
		//Checks if no levels have been beatten
		if(DEBUG){
			return false;
		}
		if(PlayerPrefs.GetInt(LEVEL)==0 && PlayerPrefs.GetInt(CHAPTER) == 0){
			Debug.Log(TAG + "starting at the begining");
			return true;
		}
		return false;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ChapterManager : MonoBehaviour {
	private static string TAG = "CHAPTER MANAGER: ";

	public LevelManager levelManager;
	public MenuManager menuManager;
	public GameObject chapterPrefab;

	public RectTransform chapterCon;
	public RectTransform scrollBox;
	public RectTransform levelSelect;
	public RectTransform customLevelSelect;

	public RectTransform customChapterCon;
	public RectTransform customScrollBox;

	private ChapterRef[] customChapters;
	private ChapterRef[] normalChapters;
	private Chapter[] chapterButtons;
	public GameObject demoInfo;

	public void SetUp(){
		Debug.Log(TAG + "setting up.");
		Debug.Log(TAG + "creating the chapters.");
		normalChapters = new ChapterRef[10];

		normalChapters[0] = new ChapterRef("Novas","Chapter 1",0);
		normalChapters[1] = new ChapterRef("Cubes and Diamonds","Chapter 2",1);
		normalChapters[2] = new ChapterRef("Tricky","Chapter 3",2);
		normalChapters[3] = new ChapterRef("Beams","Chapter 4",1);
		normalChapters[4] = new ChapterRef("Shapes","Chapter 5",0);
		normalChapters[5] = new ChapterRef("Tangeled","Chapter 6",2);
		normalChapters[6] = new ChapterRef("The Vortex","Chapter 7",1);
		normalChapters[7] = new ChapterRef("Twisted","Chapter 8",0);
		normalChapters[8] = new ChapterRef("The Great Pyramid","Chapter 9",1);
		normalChapters[9] = new ChapterRef("The Last One","Chapter 10",2);

	}
	public ChapterRef[] GetChapters(){
		return normalChapters;
	}
	public void LoadNormalChapters(){
		Debug.Log(TAG + "loading the built in chapters");
		LoadChapters(chapterCon,scrollBox,normalChapters,levelManager.GetLevelFolder(),false,levelSelect);
	}
	public void LoadCustomChapters(){
		Debug.Log(TAG + "loading the user made chapters");
		LevelManager.LevelCon[] folder = levelManager.GetCustomLevelFolder();
		customChapters = new ChapterRef[folder.Length];
		for(int i=0;i<folder.Length;i++){
			string chapterName = folder[i].GetFolderName();
			int r = Random.Range(0,3);
			customChapters[i] = new ChapterRef(chapterName,i.ToString(),r);
		}
		LoadChapters(customChapterCon,customScrollBox,customChapters,levelManager.GetCustomLevelFolder(),true,customLevelSelect);

	}
	public void LoadChapters(RectTransform container, RectTransform box,ChapterRef[] chapters,LevelManager.LevelCon[] levelConFolder,bool isCustom,RectTransform menu){
		Debug.Log(TAG + "setting container size.");
		int numToLoad = chapters.Length;
		if(menuManager.GetIsDemo()&& !isCustom){
			numToLoad = 3;
		}
		chapterButtons = new Chapter[numToLoad];
		float screenWidth = menu.rect.width;

		float padL,chapterConWidth;

		padL = screenWidth*0.15f;//15%of screen size
		chapterConWidth = screenWidth*0.6f;//70% screen size


		
		Debug.Log(TAG + "loading chapters.");

		float x1,x2,y1,y2;
		float chapterWidth,chapterHeight;

		chapterWidth = chapterConWidth;
		chapterHeight = 100;

		x1 = 0;
		y1 = -chapterHeight;
		x2 = x1 + chapterWidth;
		y2 = y1 + chapterHeight;


		for(int i=0;i<numToLoad;i++){
			GameObject g = Instantiate(chapterPrefab);
			g.transform.SetParent(container.transform);

			RectTransform r = g.GetComponent<RectTransform>();



			r.offsetMin = new Vector2(x1,y1);
			r.offsetMax = new Vector2(x2,y2);

			Chapter chapter = g.GetComponent<Chapter>();

			//------------
			//y1 += chapter.SetUp(chapterWidth,levelManager.GetLevels(i),i,menuManager);
			y1 += chapter.SetUp(chapterWidth,levelConFolder[i].GetLevels(),i,menuManager,isCustom);
			y2 = y1 + chapterHeight;

			ChapterRef c = chapters[i];
			chapter.SetName(i+1,c.GetName(),20.0f);
			chapterButtons[i] = chapter;
		}
		y1 = CheckDemoInfo(y1,isCustom,chapterWidth,container );

		container.offsetMin = new Vector2( padL, y1);
		container.offsetMax = new Vector2( padL+chapterConWidth, 0);

		box.offsetMin = new Vector2(0,y1);
		box.offsetMax = new Vector2(600,0);


	}
	public float CheckDemoInfo(float y1,bool isCustom,float chapterWidth,RectTransform container ){
		if(!isCustom && menuManager.GetIsDemo() && !ProgressManager.CheckChapterLocked(4)){
			Debug.Log(TAG + "showing demo info");
			GameObject g = Instantiate(demoInfo);
			g.transform.SetParent(container.transform);
			RectTransform r = g.GetComponent<RectTransform>();

			float x1,x2,y2;
			float chapterHeight = r.rect.height;

			x1 = 0;
			y1 -= chapterHeight;
			x2 = x1 + chapterWidth;
			y2 = y1 + chapterHeight;


			r.offsetMin = new Vector2(x1,y1);
			r.offsetMax = new Vector2(x2,y2);


			return y1;
		}
		Debug.Log(TAG + "don't need to show the info for demo");
		return y1;
	}
	public void ClearChapters(){
		Debug.Log(TAG + "clearing chapter buttons.");
		if(chapterButtons == null){
			return;
		}
		foreach(Chapter c in chapterButtons){
			Destroy(c.gameObject);
		}
		chapterButtons = null;
	}

	public class ChapterRef{
		//is a refernce used by chapter manager to orginize the name and location of each chapter
		private string name;
		private string folderName;
		private int startSound;

		public ChapterRef(string n,string folder,int sound){
			name = n;
			folderName = folder; 
			startSound = sound;
		}
		public string GetName(){
			return name;
		}
		public string GetFolder(){
			return folderName;
		}
		public int GetStartSound(){
			return startSound;
		}

	}

}

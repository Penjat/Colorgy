using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

	private static string TAG = "GRID MANAGER: ";

	public MainManager mainManager;

	private int width = 10;
	private int height = 10;


	public GameObject hexPrefab;
	public GameObject gridContainer;
	public GameObject gridConCon;//container for the grid container

	private float novaRate = 0.2f;
	private Hex[,] hexGrid;

	public void CreateGrid(Level level,bool editMode){
		Debug.Log(TAG + "creating the grid");
		hexGrid = new Hex[width,height];
		int modVal = 1;
		//reset the container's angles
		gridConCon.transform.eulerAngles = new Vector3(0,0,0);
		gridContainer.transform.eulerAngles = new Vector3(0,0,0);
		gridContainer.transform.localPosition = new Vector3(0,0,0);


		for(int x=0;x<width;x++){
			for(int y=0;y<height;y++){
				GameObject g = Instantiate(hexPrefab);
				g.transform.SetParent(gridContainer.transform,false);
				g.transform.rotation = gridContainer.transform.rotation;
				float xOffset = 0;
				float yOffset = 0;
				if(Calc.isOdd(y)){
					xOffset = .8f;
				}
				g.transform.localPosition = new Vector3((x*1.8f)-xOffset,0,y*1.6f);
				Hex h = g.GetComponent<Hex>();
				int val = level.GetVal(x,y);

				//modulate the color
				if(val != 0){
					//leave 0 as empty
					//add 1, minus one for the offset of the hex values
					int modNum = level.GetModNum();
					val = Calc.ModThree(val-1,modNum)+1;
				}
				h.SetUp(x,y,val,this);
				h.SetColor(editMode);
				h.SetFadeIn();
				hexGrid[x,y] = h;

			}

		}
		//adjust the container angles
		float angle = level.GetRotation();
		gridContainer.transform.RotateAround(hexGrid[4,4].transform.position,new Vector3(0,1,0),angle);
		gridConCon.transform.eulerAngles = new Vector3(19.0f,0,0);

	}

	public Hex GetHex(int x,int y){
		return hexGrid[x,y];
	}
	public Hex GetHex(Hex hex, int xdir,int ydir){
		return GetHex(hex.GetX(),hex.GetY(),xdir,ydir);
	}
	public Hex GetHex(int x, int y, int xdir,int ydir){
		//gets the upper or lower hexes adjusting for even or odd rows
		Hex hex;
		int x2 = x;
		int y2 = y + ydir;
		if(ydir == 0){

			hex = CheckForHex( x2+xdir,  y2);
			//Debug.Log(TAG + "returning hex " + x2+xdir + y2);
			return hex;
		}

		if(Calc.isOdd(y)){//if it is odd
			if(xdir== -1){
				x2 = x + xdir;
			}
			if(xdir==1){
				x2 = x;
			}
		}
		if(!Calc.isOdd(y)){//if its is even
			if(xdir==-1){
				x2= x;
			}
			if(xdir==1){
				x2 = x + xdir;
			}
		}

		hex = CheckForHex( x2,  y2);

		//Debug.Log(TAG + "returning hex " + x2 + y2);
		return hex;
	}
	public Hex CheckForHex(int x,int y){
		if(x < 0 || y <0 || x >= hexGrid.GetLength(0) || y >= hexGrid.GetLength(1)){
			return null;
		}
		Hex hex = hexGrid[x,y];

		return hex.HexCheck();
	}
	public Hex[] FindAdjacent(Hex hex){
		int x = hex.GetX();
		int y = hex.GetY();
		return FindAdjacent(x,y);

	}
	public Hex[] FindAdjacent(int x,int y){
		Hex[] hexes = new Hex[6];
		hexes[0] = GetHex(x,y,1,1);
		hexes[1] = GetHex(x,y,-1,1);
		hexes[2] = GetHex(x,y,1,0);
		hexes[3] = GetHex(x,y,-1,0);
		hexes[4] = GetHex(x,y,1,-1);
		hexes[5] = GetHex(x,y,-1,-1);
		return hexes;

	}
	public void Nova(Hex hex){
		if(!hex){
			return;
		}

		Hex[] hexes = FindAdjacent(hex);
		ToolNova activeNova = (ToolNova)hex.GetActiveTool();

		foreach(Hex h in hexes){
			activeNova.CheckNova(h,hex);

		}
	}
	public void Clear(){
		Debug.Log(TAG + "clearing grid.");
		if(hexGrid != null){
			foreach(Hex h in hexGrid){
				h.StartFade();
				Destroy(h.gameObject,2.0f);
			}
		}
		hexGrid = null;
	}

	public void CheckIfComplete(){
		for(int x=0;x<width;x++){
			for(int y=0;y<height;y++){
				if(hexGrid[x,y].GetVal() != 0){
					return;
				}
			}
		}
		Debug.Log(TAG + "the grid is clear.");
		mainManager.LevelComplete();

	}


}

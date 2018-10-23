using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calc : MonoBehaviour {

	private static string TAG = "CALC: ";

	//TODO setup colors more exact
	private static Color c0 = Color.red;
	private static Color c1 = Color.blue;
	private static Color c2 = Color.yellow;
	private static Color c3 = Color.magenta;
	private static Color c4 = new Color(1,0.4f,0);
	private static Color c5 = Color.green;
	private static Color c6 = Color.white;
	private static Color c7 = Color.grey;
	private static Color c8 = new Color(2.0f,0.0f,1.5f);//TODO make a rainbow shader


	public static Color GetColor(int i){
		//a global method for getting the colors
		switch(i){

		case 0:
			return c0;
		case 1:
			return c1;
		case 2:
			return c2;
		case 3:
			return c3;
		case 4:
			return c4;
		case 5:
			return c5;
		case 6:
			return c6;
		case 7:
			return c7;
		case 8:
			return c8;

		}
		return c7;
	}
	public static bool isOdd(int i){
		int v = i%2;
		if(v==1){
			return true;
		}
		return false;
	}
	public static int FindDistance(Hex g1, Hex g2){
		int x1 = g1.GetX();
		int y1 = g1.GetY();
		int x2 = g2.GetX();
		int y2 = g2.GetY();
		return FindDistance(x1,y1,x2,y2);
	}
	public static int FindDistance(int x1,int y1,int x2,int y2){
		//find the distance between two points on a he grid
		int distance;
		int xDist = x2-x1;
		int yDist = Mathf.Abs(y2-y1);
		int xDir;
		if(xDist == 0){
			return yDist;
		}else if (xDist >0){
			xDir = 1;
		}else{
			xDir = -1;
		}
		xDist = Mathf.Abs(xDist);
		int v = 0;
		if(isOdd(y2)==isOdd(y1)){//if they are both in odd rows or both in even rows
			v = yDist/2;//no remainder
			distance = yDist + (xDist - v);
			if(v > xDist){
				distance = yDist;
				return distance;
			}
			return distance;
		}

		//if one is odd and one even it is more tricky
		if((xDir == 1 && isOdd(y1)) || (xDir == -1 && !isOdd(y1))  ){
			v = yDist/2;
		}
		if((xDir == 1 && !isOdd(y1)) || (xDir == -1 && isOdd(y1))  ){
			v = (yDist/2) + 1;
		}

		if(v > xDist){
			distance = yDist;
			return distance;
		}
		distance = yDist + (xDist - v);
		return distance;
	}

	public static int GetMix(int v1, int v2){
		return v1 + v2 + 2;
	}
	public static int GetAbsorb(int v1, int v2){
		return v1 - v2 - 2;
	}
	public static HexDirection GetHexDir(Hex startHex, Hex endHex){
		//get the direction between two hexes
		//does not return exact direction
		int xdir,ydir,xdist,ydist;

		int x1 = startHex.GetX();
		int y1 = startHex.GetY();
		int x2 = endHex.GetX();
		int y2 = endHex.GetY();

		xdist = x2 - x1;
		ydist = y2 - y1;

		xdir = 1;
		ydir = 0;

		if(ydist > 0){
			ydir = 1;
		}else if(ydist < 0){
			ydir = -1;
		}

		if(xdist > 0){
			xdir = 1;
		}
		if(xdist < 0){
			xdir = -1;
		}
		if(xdist == 0){
			if(isOdd(y1)){
				xdir = 1;
			}else{
				xdir = -1;
			}
		}


		
		return new HexDirection(xdir,ydir);



	}

	public class HexDirection{
		private int xDir,yDir;
		public HexDirection(int xd,int yd){
			xDir = xd;
			yDir = yd;
		}
		public int GetXDir(){
			return xDir;
		}
		public int GetYDir(){
			return yDir;
		}

	}
	public static int ModThree(int oldVal, int delta){
		//takes the old value and modulates by the delta

		if(delta == 0 || oldVal > 5){
			//if there is no change
			//or it is a white val
			//just return the old val
			return oldVal;
		}
		if(oldVal < 3){
			//if it is a Primary color
			int newVal = (oldVal+delta) % 3;
			return newVal;
		}

		if(oldVal >= 3){
			//if it is a Secondary color
			int newVal = (oldVal+(3-delta)) % 3;
			newVal += 3;
			return newVal;
		}
		Debug.Log(TAG + "did not change val...");
		return oldVal;

	}
	public static int GetOpposite(int val){
		//returns the opposite color val
		//TODO manager for white val (6)
		return 5-val;

	}

}

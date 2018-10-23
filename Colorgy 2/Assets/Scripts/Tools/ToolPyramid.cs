using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolPyramid : ToolSphere {
	private static string TAG = "PYRAMID: ";
	private static float rotRate = 0.4f;
	public override void Update ()
	{
		base.Update ();
		transform.Rotate(new Vector3(0,1,0)*rotRate);
	}
	public override void Use(Hex hex){
		

		int hexVal = hex.GetVal() -1;
		if(isDone){
			return;
		}

		if(!isPlaced && CanClear(hexVal,val)){
			isPlaced = true;
			gameObject.SetActive(true);
			previousHex = hex;
			x = hex.GetX();
			y = hex.GetY();
			transform.position = hex.GetPlacePos().position;
			transform.rotation = hex.GetPlacePos().rotation;
			//hex.SetWillClear(0.0f);
			SetVal(hexVal);
			UpdateColor();
			soundManager.PlayPyramid();
			return;
		}

		if(isPlaced && IsAdjacent(hexVal,val)){
			SetVal(hexVal);
			Clear(hex);
			UpdateColor();
			soundManager.PlayPyramid();
			//UpdateColor();
		}
		Debug.Log(TAG + "hex val: " + hexVal + " val: " + val );

	}

	public bool IsAdjacent(int valHex, int valTool){
		//returns true if the colors are adjacent on the color wheel

		//should it be able to clear the current color
		//Red
		if(valTool == 0){
			if(valHex == 0 || valHex == 3 || valHex == 4){
				//SetVal(valHex);
				return true;
			}
			return false;
		}
		//Blue
		if(valTool == 1){
			if(valHex == 1 || valHex == 3 || valHex == 5){
				//SetVal(valHex);
				return true;
			}
			return false;
		}
		//Yellow
		if(valTool == 2){
			if(valHex == 2 || valHex == 4 || valHex == 5){
				//SetVal(valHex);
				return true;
			}
			return false;
		}
		//Purple
		if(valTool == 3){
			if(valHex == 3 || valHex == 0 || valHex == 1){
				//SetVal(valHex);
				return true;
			}
			return false;
		}
		//Orange
		if(valTool == 4){
			if(valHex == 4 || valHex == 0 || valHex == 2){
				
				//SetVal(valHex);
				return true;
			}
			return false;
		}
		//Green
		if(valTool == 5){
			if(valHex == 5 || valHex == 1 || valHex == 2){
				//SetVal(valHex);
				return true;
			}
			return false;
		}
		return false;
	}

	public override int CheckPosMoves(Hex hex, int numOfMoves){
		if(hex && !hex.GetWillClear() && IsAdjacent(hex.GetVal()-1,val)){
			Debug.Log(TAG + previousHex.GetX() + " " + previousHex.GetY()+ "Can move here: " + hex.GetX() + " " + hex.GetY() );
			numOfMoves++;
		}

		return numOfMoves;
	}
	public override int GetID(){
		return 12;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolDiamondBeam : ToolBeam {

	private static string TAG = "TOOL DIAMOND BEAM: ";

	public override void Update(){
		base.Update();
	}
	public override int SetUp(int v,ButtonTool bt,SoundFXManager sm){
		//make sure it is a primary color for all diamond and cube type tools

		if(v == 3 || v == 4 || v == 5){
			Debug.Log("wrong value");
			val = 6;


			return base.SetUp(val,bt,sm);
		}
		return base.SetUp(v,bt,sm);

	}
	public override void Use(Hex hex){
		Debug.Log(TAG + "using.");
		if(isDone){
			return;
		}
		int hexVal = hex.GetVal() -1;

		//if is not placed
		if(!isPlaced){
			if(val == 6){
				//if it is white
				val = hexVal;
				Place(hex);
				UpdateColor();
				return;
			}
			if(CanMix(hexVal,val)|| hexVal == GetVal()){
				Place(hex);
				UpdateColor();
				return;
			}
			return;

		}

		//if is placed
		if(isPlaced){
			buttonTool.EndUse();
			int numCleared = ClearLine(hex);
			soundManager.FireDiamondBeam(numCleared,xdir);
			FallBack();

		}
	}
	public override void HighlightTarget (){

		//check if need to create new list
		if(highlightHexList == null){
			highlightHexList = new List<Hex>();
		}

		//clear the old list
		foreach(Hex hex in highlightHexList){
			hex.LightUp(false);
		}
		highlightHexList.Clear();

		//add new hexes to the list
		Hex h = startHex;
		for(int i=0;i<20;i++){
			highlightHexList.Add(h);
			h.LightUp(true);

			h = h.GetNeighbor(xdir,ydir);

			//if no hex return
			if(h ==null){

				return;
			}
			target = h.transform;
			int hexVal = h.GetVal()-1;

			//if not the same val


			if(hexVal != GetVal()){

				//check if they can mix
				if(!CanMix(hexVal,val)){

					return;
				}
			}



		}
	}
	public virtual int ClearLine(Hex hex){

		//count up to 20 just to be sure
		//could use a while loop but I don't like to
		Hex h = startHex;



		h.LightUp(false);

		//don't add color if they are the same
		if(h.GetVal()-1 != GetVal()){
			h.SetVal(Calc.GetMix(h.GetVal(),GetVal()));


		}
		h.SetWillMix(0.0f);
		h.SetAddColorVal(this);
		for(int i=0;i<20;i++){

			h = h.GetNeighbor(xdir,ydir);
			//if no hex, return
			if(h==null){

				EndUse();
				return i;
			}
			int hexVal = h.GetVal()-1;

			//check if they are the same
			if(hexVal == GetVal()){
				//still add the beam effect if they are the same
				h.SetWillMix(0.2f*(i+1));
				h.SetAddColorVal(this);

			}
			if(hexVal != GetVal()){


				//check if it can absorb
				if(CanMix(hexVal,GetVal())){
					//mix
					h.SetVal(Calc.GetMix(hexVal+1,GetVal()));
					h.SetWillMix(0.2f*(i+1));
					h.SetAddColorVal(this);

					//h.UpdateColor();
					//h.LightUp(false);
				}else{
					//return if can't mix
					EndUse();
					return i;
				}

			}else{
				//if they are the same
				//h.SetWillClear(0.1f*(i+1));

			}

		}
		return 10;

	}
	public bool DiamondCanMix(Hex hex){
		if(!hex){
			return false;
		}
		int hexVal = hex.GetVal()-1;

		if(CanMix(hexVal,val)|| hexVal == GetVal() || (GetVal() == 6 && hexVal<3 )){
			return true;
		}
		return false;
	}
	public override void Highlight(Hex oldHex, Hex newHex){
		if(!isPlaced){
			if(oldHex){
				oldHex.LightUp(false);
			}
			if(DiamondCanMix(newHex)){
				newHex.LightUp(true);

				SetGridPos(newHex);
				return;
			}	
			SetGridPos(null);
		}
		if(newHex && isPlaced){
			//Debug.Log(TAG + "nex hex x = " + newHex.GetX() + " y = " + newHex.GetY() );
			Calc.HexDirection dir = Calc.GetHexDir(startHex,newHex);
			Debug.Log(TAG + "nex hex xdir = " + dir.GetXDir() + " y = " + dir.GetYDir() );

			if(xdir != dir.GetXDir() || ydir != dir.GetYDir()){
				xdir = dir.GetXDir();
				ydir = dir.GetYDir();
				HighlightTarget();
			}
		}
	}
	public override int GetID(){
		return 6;
	}
}

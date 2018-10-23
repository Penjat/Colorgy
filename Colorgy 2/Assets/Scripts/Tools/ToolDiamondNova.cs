using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolDiamondNova : ToolNova {
	private static string TAG = "TOOL DIAMOND NOVA: ";

	private List<Hex> hexList;
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

		//Check if can place
		//only place on Primary colors that are not the same as its own
		if(Check(hex)){
			gameObject.SetActive(true);
			transform.position = hex.transform.position + new Vector3(0,1,0);
			hexList = new List<Hex>();
			hex.SetNova(0.0f,this);
			isPlaced = true;
			isRising = true;
			isDone = true;
			transform.SetParent(null);
			buttonTool.EndUse();
			StartFade(1, 1.0f, 1.5f);
			soundManager.PlayDiamondNova(val);
		}
	}
	public override void Activate(Hex h){
		//diamond nova adds its color
		int hexVal = h.GetVal()-1;


		if(hexVal == GetVal()){
			hexList.Add(h);
			if(h.GetVal()-1 == GetVal()){
				h.LightUp(false);

			}
			return;
			//don't mix, add to the list so it doesn't get checked again
		}
		h.SetVal(Calc.GetMix(h.GetVal(),GetVal()));
		h.SetWillMix(0.0f);
		//h.Clear();
	}
	public bool Check(Hex h){

		if(h == null){
			return false;
		}
		int hexVal = h.GetVal()-1;

		//if is a white nova, hasent been placed and the hex is a primary color
		if(GetVal()== 6 && isPlaced == false && hexVal < 3){
			val = hexVal;
		}

		if(h.IsActive() //if the hex is active
			&& !h.GetNova() //and it is not already waiting to explode
			&& (hexVal<3)   //and it is a Primary color
			//&& hexVal != GetVal() //and it is not the same color as the nova

		){
			return true;
		}
		return false;
	}
	public override void CheckNova(Hex h, Hex hex){
		//checks surrounding hexes if they will also nova
		//hex is the point of origin and h is one of the surrounding

		if(Check(h)){
			if(hexList.Contains(h)){
				//don't nova if it already has
				return;
			}
			//the make it nova

			h.SetNova(novaRate,this);
			h.Wave();
		}
	}
	public override void Highlight(Hex oldHex,Hex newHex){
		//do the regular highlight by default
		if(oldHex){
			oldHex.LightUp(false);
		}
		if(newHex && GetVal() == 6 && newHex.GetVal()-1 < 3){
			newHex.LightUp(true);

			SetGridPos(newHex);
			return;
		}
		if(Check(newHex)){
			newHex.LightUp(true);

			SetGridPos(newHex);
			return;
		}	
		SetGridPos(null);
	}
	public override int GetID(){
		return 2;
	}

}

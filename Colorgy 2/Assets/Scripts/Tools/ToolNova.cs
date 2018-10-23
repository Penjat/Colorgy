using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolNova : Tool {

	private static string TAG = "TOOL NOVA: ";

	protected float novaRate = 0.2f;




	public override void Use(Hex hex){
		Debug.Log(TAG + "using.");
		if(isDone){
			return;
		}
		int hexVal = hex.GetVal() -1;

		if(hex.GetVal()-1 == GetVal()|| GetVal() == 6 || GetVal() == 8){
			if(GetVal()== 6){
				SetVal(hex.GetVal()-1);
			}

			hex.SetNova(0.0f,this);

			isPlaced = true;
			isRising = true;

			transform.SetParent(null);
			buttonTool.EndUse();

			StartFade(1, 1, 1.5f);
			isDone = true;
			soundManager.PlayNova(val);
		}
	}
	public override void Activate(Hex h){
		//basic nova clears the hex
		h.Clear(0,this);
	}
	public virtual void CheckNova(Hex h, Hex hex){
		//checks surrounding hexes if they will also nova
		//hex is the point of origin and h is one of the surrounding

		if(h && h.IsActive() && !h.GetNova() && (h.GetVal() == hex.GetVal() || GetVal() == 8)){
			h.SetNova(novaRate,this);
			h.Wave();
		}

	}
	public override void Highlight(Hex oldHex,Hex newHex){
		//do the regular highlight by default
		if(oldHex){
			oldHex.LightUp(false);
		}
		if(newHex && CanClear(newHex.GetVal()-1,val)){
			newHex.LightUp(true);

			SetGridPos(newHex);
			return;
		}	
		SetGridPos(null);
	}
	public virtual void EndUse(){
		Debug.Log(TAG + "End use.");
		Destroy(gameObject);
	}
	public override int GetID(){
		return 0;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolVortexNova : ToolNova {

	private static string TAG = "TOOL NOVA: ";


	private List<Hex> hexList;


	public override void Use(Hex hex){
		Debug.Log(TAG + "using.");
		if(isDone){
			return;
		}
		hexList = new List<Hex>();
		int hexVal = hex.GetVal() -1;

		if(hex.GetVal()-1 == GetVal()|| GetVal() == 6|| GetVal() == 8){
			if(GetVal()== 6){
				SetVal(hex.GetVal()-1);
			}
			hex.SetNova(0.0f,this);


			hexList.Add(hex);
			isPlaced = true;
			isRising = true;
			isDone = true;
			hex.Flip();
			transform.SetParent(null);
			buttonTool.EndUse();
			StartFade(1, 1, 1.5f);
			soundManager.PlayVortexNova(val);
		}

	}
	public override void Activate(Hex h){
		//Vortext sets to the opposite

		int v = Calc.GetOpposite(h.GetVal()-1);
		h.SetVal(v+1);
		h.SetWillMix(0.0f);

		//h.Clear(0,this);

	}
	public override void CheckNova(Hex h, Hex hex){
		//checks surrounding hexes if they will also nova
		//hex is the point of origin and h is one of the surrounding


		if(h && h.IsActive() && !h.GetNova() && (h.GetVal() == hex.GetVal() || GetVal() == 8)){
			if(hexList.Contains(h)){
				//don't nova if it already has
				return;
			}

			h.SetNova(novaRate,this);
			h.Flip();
			hexList.Add(h);
		}

	}
	public override int GetID(){
		return 3;
	}


}

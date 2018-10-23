using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolVortex : ToolSphere {
	private static string TAG = "TOOL VORTEX: ";



	public override void Update (){
		base.Update ();
		transform.Rotate(new Vector3(0,1,0));
	}

	public override void Use(Hex hex){
		Debug.Log(TAG + "using.");
		if(isDone){
			return;
		}
		int hexVal = hex.GetVal() -1;
		rend.material.color = Calc.GetColor(val);
		if(val == 8){
			rend.material = rainbowMat;
		}

		if(hexVal == GetVal() || GetVal() == 6 || GetVal() == 8){
			

			Debug.Log(TAG + "vortexing");
			if(!isPlaced){
				isPlaced = true;
				x = hex.GetX();
				y = hex.GetY();
				if(val == 6){
					val=hex.GetVal()-1;
				}
				gameObject.SetActive(true);
				transform.position = hex.GetPlacePos().transform.position;
				transform.rotation = hex.transform.rotation;
				int v = Calc.GetOpposite(hex.GetVal()-1);
				hex.SetVal(v+1);
				hex.SetWillMix(0.0f);
				hex.Flip();
				soundManager.PlayVortex();
				return;
			}
			if(Calc.FindDistance(x,y,hex.GetX(),hex.GetY()) == 1){

				x = hex.GetX();
				y = hex.GetY();
				transform.position = hex.GetPlacePos().transform.position;
				transform.rotation = hex.transform.rotation;
				int v = Calc.GetOpposite(hex.GetVal()-1);
				hex.SetVal(v+1);
				hex.SetWillMix(0.0f);
				hex.Flip();
				soundManager.PlayVortex();
			}
		}
	}
	public override int GetID(){
		return 11;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolVortexBeam : ToolBeam {

	public override int ClearLine(Hex hex){

		//count up to 20 just to be sure
		//could use a while loop but I don't like to
		Hex h = startHex;

		int v = Calc.GetOpposite(h.GetVal()-1);
		h.SetVal(v+1);
		h.SetWillMix(0.0f);
		h.Flip();
		for(int i=0;i<20;i++){

			h = h.GetNeighbor(xdir,ydir);
			//if no hex, return
			if(h==null){

				//EndUse();
				return i;
			}
			int hexVal = h.GetVal()-1;

			//check if they are the same

			if(GetVal()!=8 && hexVal != GetVal()){
				//if not same, return
				return i;
			}
			v = Calc.GetOpposite(h.GetVal()-1);
			h.SetVal(v+1);
			h.SetWillMix(0.2f*(i+1));
			h.Flip(0.2f*(i+1));

		}
		//should never reach here, 10 is max value
		return 10;

	}
	public override int GetID(){
		return 7;
	}
	public override void BeamSound(int numCleared){
		soundManager.FireVortexBeam(numCleared,xdir);
	}
}

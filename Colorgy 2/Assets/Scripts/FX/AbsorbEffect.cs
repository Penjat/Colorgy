using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbEffect : HexEffect {
	
	private Tool activeTool;
	public AbsorbEffectParticle[] cubes; 

	public override void SetUp(Tool tool,Hex hex){
		Debug.Log("Absorb effect created");
		activeTool = tool;
		foreach(AbsorbEffectParticle c in cubes){
			c.SetUp(activeTool.GetTarget(),activeTool.GetVal());
		}
	}

	void Update(){
		
		bool cubesLeft = false;
		foreach(AbsorbEffectParticle c in cubes){
			if(c){
				cubesLeft = true;
				//if it reaches target
				if(c.MoveToTarget()){
				
					Destroy(c.gameObject);
				}
			}

			
		}
		if(!cubesLeft){
			Debug.Log("Effect finished");
			activeTool.EffectFinish(this);
			Destroy(gameObject);
		}
	}

}

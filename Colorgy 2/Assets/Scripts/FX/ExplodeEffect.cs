using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEffect : HexEffect {

	public ParticleSystem p;
	void Start(){
		Destroy(gameObject,23.0f);
	}
	public override void SetUp(Tool activeTool,Hex hex){
		
		int val= hex.GetVal()-1;
		SetColor(val);

	}
	public void SetColor(int val){
		//Debug.Log("Changing the color " + val);
		var main = p.main;
		main.startColor = Calc.GetColor(val);
	}
}

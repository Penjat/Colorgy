using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddColorEffect : HexEffect {

	public ParticleSystem p;

	public override void SetUp(Tool activeTool,Hex hex){

		int val= hex.GetVal()-1;
		SetColor(val);

	}
	public void SetColor(int val){
		Debug.Log("Changing the color " + val);
		var main = p.main;
		main.startColor = Calc.GetColor(val);
	}
}

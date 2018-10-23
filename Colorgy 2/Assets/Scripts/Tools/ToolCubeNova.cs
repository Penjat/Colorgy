using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCubeNova : ToolNova {
	private static string TAG = "TOOL CUBE NOVA: ";

	public GameObject novaPoint;
	private int numAbsorbed, numToAbsorb;
	private bool willExplode;

	public GameObject explodePrefab;//in case cube nova needs to explode

	void Update(){
		if(willExplode){
			transform.Rotate(new Vector3(1,1,0)*Time.deltaTime*100);
			timer -= Time.deltaTime;
			if(timer <= 0){
				//Code for making cube nova explode
				willExplode = false;
				GameObject g = Instantiate(explodePrefab);
				Destroy(g,23.0f);
				g.transform.position = transform.position;
				ParticleSystem p = g.GetComponent<ParticleSystem>();
				//p.main.startColor = Calc.GetColor(val);
				var main = p.main;
				main.startColor = Calc.GetColor(val);
				//main.maxParticles = numAbsorbed*6;
				soundManager.RandomDrum();
				Destroy(gameObject);
			}
		}
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

		//Check if can place
		//only place on Primary colors that are not the same as its own
		if(Check(hex)){
			
			novaPoint.SetActive(true);
			novaPoint.transform.position = hex.transform.position + new Vector3(0,1,0);
			hex.SetNova(0.0f,this);
			isPlaced = true;
			isDone = true;
			buttonTool.EndUse();
			novaPoint.transform.SetParent(null);
			GetComponent<Rigidbody>().isKinematic = false;
			UpdateColor();
			soundManager.PlayCubeNova(val);
		}
	}
	public override void Activate(Hex h){
		//diamond nova adds its color
		int hexVal = h.GetVal()-1;


		if(hexVal == GetVal()){
			h.Clear(1,this);
			numToAbsorb++;
			return;
			//don't mix, add to the list so it doesn't get checked again
		}
		//h.SetVal(Calc.GetAbsorb(h.GetVal(),GetVal()));
		h.Absorb(Calc.GetAbsorb(h.GetVal(),GetVal()),this);
		numToAbsorb++;
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
			&& (hexVal>2 || hexVal==GetVal()) //and it is a Secondary color or it equals the nova's color
			&& CanAbsorb(hexVal,GetVal())
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
			
			//the make it nova

			h.SetNova(novaRate,this);
			h.Wave();
		}
	}
	public override GameObject GetTarget(){
		//gets the target for any hex effect
		return novaPoint;
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
		if(newHex && CanAbsorb(newHex.GetVal()-1,val)){
			newHex.LightUp(true);

			SetGridPos(newHex);
			return;
		}	
		SetGridPos(null);
	}

	public override void EffectFinish(HexEffect hexEffect){
		Debug.Log(TAG + "effect finished.");
		numAbsorbed++;
		if(numAbsorbed >= numToAbsorb){
			
			willExplode = true;
			timer = 0.25f;
			Debug.Log(TAG + "done absorbing");

		}

	}
	public override int GetID(){
		return 1;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolCubeBeam : ToolBeam{

	private static string TAG = "TOOL CUBE BEAM: ";
	private int numToAbsorb;
	private int numAbsorbed;
	private bool willExplode;

	public GameObject explodePrefab;
	public override int SetUp(int v,ButtonTool bt,SoundFXManager sm){
		//make sure it is a primary color for all diamond and cube type tools

		if(v == 3 || v == 4 || v == 5){
			Debug.Log("wrong value");
			val = 6;


			return base.SetUp(val,bt,sm);
		}
		return base.SetUp(v,bt,sm);

	}

	void Update(){
		if(willExplode){
			timer -= Time.deltaTime;
			if(timer <= 0){
				GameObject g = Instantiate(explodePrefab);
				g.transform.position = transform.position;
				ParticleSystem p = g.GetComponent<ParticleSystem>();
				var main = p.main;
				main.startColor = Calc.GetColor(val);
				main.maxParticles = numAbsorbed*3;
				soundManager.RandomDrum();
				Destroy(gameObject);

			}
		}
		if(isPlaced){
			if( target){
				Vector3 t = new Vector3(target.position.x,target.position.y+2,target.position.z);
				transform.LookAt(t);
				return;
			}
			//if no target
			transform.LookAt(Camera.main.transform);
		}
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
				UpdateColor();
				Place(hex);

				return;
			}
			if(CanAbsorb(hexVal,val)){
				Place(hex);
				UpdateColor();
				return;
			}
			return;

		}

		//if is placed
		if(isPlaced){
			
			transform.SetParent(null);
			int numCleared = ClearLine(hex);
			soundManager.FireCubeBeam(numCleared,xdir);
			Debug.Log(TAG + numToAbsorb + " to absorb");
			Rigidbody r = GetComponent<Rigidbody>();
			r.isKinematic = false;
			r.AddForce( new Vector3(0,1,0.2f)*210.0f);

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
				if(!CanAbsorb(hexVal,val)){
					
					return;
				}
			}



		}
	}
	public virtual int ClearLine(Hex hex){

		//count up to 20 just to be sure
		//could use a while loop but I don't like to
		Hex h = startHex;
		//h.SetWillClear(0);
		for(int i=0;i<20;i++){


			//if no hex, return
			if(h==null){
				isDone = true;
				buttonTool.EndUse();
				//EndUse();
				return i;
			}
			int hexVal = h.GetVal()-1;

			//check if they are the same

			if(hexVal != GetVal()){
				

				//check if it can absorb
				if(CanAbsorb(hexVal,GetVal())){
					//mix
					//h.SetVal(Calc.GetAbsorb(hexVal+1,GetVal()));//TODO delay the mixing
					//h.SetWillMix(0.1f*(i+1));
					//h.UpdateColor();
					h.Absorb(Calc.GetAbsorb(h.GetVal(),GetVal()),this,i*0.1f);
					numToAbsorb++;
					//h.LightUp(false);
				}else{
					//return if can't mix
					isDone = true;
					buttonTool.EndUse();
					//EndUse();
					return i;
				}
			}else{
				//if they are the same
				//h.SetWillClear(0.1f*(i+1));
				//h.Clear(1,this);
				h.Absorb(0,this,i*0.1f);
				numToAbsorb++;

			}
			h = h.GetNeighbor(xdir,ydir);
			
		}
		return 10;



	}
	public override GameObject GetTarget(){
		//gets the target for any hex effect
		return gameObject;
	}
	public override void Highlight(Hex oldHex, Hex newHex){
		if(!isPlaced){
			if(oldHex){
				oldHex.LightUp(false);
			}
			if(newHex && (CanClear(newHex.GetVal()-1,val) || CanAbsorb(newHex.GetVal()-1,val))){
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
	public override void EffectFinish(HexEffect hexEffect){
		numAbsorbed++;
		Debug.Log(TAG + "effect finished. left to Absorb = " + numToAbsorb);
		if(numToAbsorb == numAbsorbed){
			
			willExplode = true;
			timer = 0.5f;
		}
	}
	public override int GetID(){
		return 5;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBeam : Tool {
	

	private static string TAG = "TOOL BEAM: ";

	protected int xdir,ydir;
	protected Hex startHex;
	protected List<Hex> highlightHexList;
	protected Transform target;

	public override void Use(Hex hex){
		Debug.Log(TAG + "using. ");

		if(isDone){
			return;
		}
		int hexVal = hex.GetVal() -1;


		//if not placed
		if(!isPlaced && CanClear(hexVal,val)){
			gameObject.SetActive(true);

			Place(hex);
			UpdateColor();
			return;
		}


		//if is placed
		if(isPlaced){
			int numCleared = ClearLine(hex);
			BeamSound(numCleared);
			isDone = true;
			transform.SetParent(null);
			buttonTool.EndUse();
			FallBack();
			//TODO put force in corisponding direction
		}

	}
	public virtual void BeamSound(int numCleared){
		soundManager.FireBeam(numCleared,xdir);
	}
	public void FallBack(){
		StartFade(1.0f,1.0f,2.0f);
		Rigidbody r = GetComponent<Rigidbody>();
		r.isKinematic = false;
		Vector3 forceDir = -transform.forward*0.5f + transform.up;
		r.AddForce( forceDir*210.0f);
	}

	public override void Update(){
		base.Update();
		//look at mouse
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

	public override void Place(Hex hex){
		//base.Place(hex);

		isPlaced = true;
		gameObject.SetActive(true);
		x = hex.GetX();
		y = hex.GetY();
		if(val == 6){
			val=hex.GetVal()-1;
		}

		xdir = 1;
		ydir = 0;
		transform.position = hex.transform.position + new Vector3(0,1,0);
		startHex = hex;
		HighlightTarget();

		//Highlight
	}
	public override void Highlight(Hex oldHex, Hex newHex){
		
		if(!isPlaced || isDone){
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


	public virtual void HighlightTarget (){
		
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

			//check if they are the same,or if is a rainbow beam
			if(GetVal()!=8 && hexVal != GetVal()){
				return;
			}
			//if not the same val

			/* taking out mixing
			if(hexVal != GetVal()){
				
				//check if they can mix
				if(!CanMix(val,hexVal)){
					
					return;
				}
			}
			*/


		}
	}


	public virtual int ClearLine(Hex hex){

		//count up to 20 just to be sure
		//could use a while loop but I don't like to
		Hex h = startHex;
		h.SetWillClear(0);
		for(int i=0;i<20;i++){
			
			h = h.GetNeighbor(xdir,ydir);
			//if no hex, return
			if(h==null){
				
				//EndUse();
				return i;
			}
			int hexVal = h.GetVal()-1;

			//check if they are the same,or if is a rainbow beam

			if(GetVal()!=8 && hexVal != GetVal()){
				//if not same, return
				return i;
			}
			h.SetWillClear(0.1f*(i+1));
				/* taking out the mixing
				if(CanMix(GetVal(),hexVal)){
					//mix
					h.SetVal(Calc.GetMix(hexVal+1,GetVal()));//TODO delay the mixing
					h.SetWillMix(0.1f*(i+1));
					//h.UpdateColor();
					h.LightUp(false);
				}else{
					//return if can't mix
					return;
				}
			}else{
				//if they are the same
				h.SetWillClear(0.1f*(i+1));

			}
			*/
		}
		//should never reach here
		return 10;

	}
	public override void EndUse(){
		Debug.Log(TAG + "End use.");
		//GetComponent<Rigidbody>().isKinematic = false;
		//Destroy(transform.parent.gameObject,2.0f);
	}

	public override int GetID(){
		return 4;
	}



}

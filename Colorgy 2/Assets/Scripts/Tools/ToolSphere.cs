using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSphere : Tool {
	private static string TAG = "TOOL SPHERE: ";
	public Animator animator;
	protected Hex previousHex;

	private Vector3 sitPos = new Vector3(0.0f,0.5f,0.0f);
	private bool willEndUse;
	private float endTimer;
	public Material rainbowMat;


	public override void Update(){
		if(willEndUse){
			//no moves left
			endTimer -= Time.deltaTime;
			if(endTimer <=0){
				willEndUse = false;
				EndUse();
			}
		}
		base.Update();
	}


	public override void Use(Hex hex){
		Debug.Log(TAG + "using.");
		if(isDone){
			return;
		}
		int hexVal = hex.GetVal() -1;

		if(CanClear(hexVal,val)){
			
			soundManager.PlaySphere(!isPlaced);
			Clear(hex);

		}
	}

	public void Clear(Hex hex){
		Debug.Log(TAG + "Clearing.");
		if(!isPlaced){
			Debug.Log(TAG + "placing");
			gameObject.SetActive(true);


			isPlaced = true;
			transform.SetParent(null);
			x = hex.GetX();
			y = hex.GetY();
			if(val == 6){
				val=hex.GetVal()-1;
			}
			if(val == 8){
				rend.material = rainbowMat;
			}
			rend.material.color = Calc.GetColor(GetVal());
			animator.Play("SphereEnter");
			transform.position = hex.GetPlacePos().position;//transform.position + new Vector3(0,2,0);
			transform.rotation = hex.GetPlacePos().rotation;
			previousHex = hex;
			previousHex.Press(true);
			//hex.SetWillClear(0.0f);
			CheckNeighbors();
			return;
		}
		if(Calc.FindDistance(x,y,hex.GetX(),hex.GetY()) == 1){

			x = hex.GetX();
			y = hex.GetY();
			transform.position = hex.transform.position+ sitPos;
			previousHex.SetWillClear(0.0f);
			previousHex = hex;
			previousHex.Press(true);
			CheckNeighbors();
			//hex.SetWillClear(0.0f);
		}
	}
	public override void EndUse(){
		Debug.Log(TAG + "End use.");
		Rigidbody r = GetComponent<Rigidbody>();
		r.isKinematic = false;

		Vector3 forceDir = 0.5f*transform.up;
		r.AddForce( forceDir*100.0f);
		print("is kinematic = " + r.isKinematic);
		if(previousHex){
			previousHex.SetWillClear(0.0f);
		}

		StartFade(1.0f,1.0f,2.0f);
		buttonTool.EndUse();
	}
	public virtual int CheckPosMoves(Hex hex, int numOfMoves){
		
		if(hex && !hex.GetWillClear() && (hex.GetVal()-1 == GetVal() || val == 8)){
			Debug.Log(TAG + previousHex.GetX() + " " + previousHex.GetY()+ "Can move here: " + hex.GetX() + " " + hex.GetY() );
			numOfMoves++;
		}

		return numOfMoves;
	}
	public bool CheckNeighbors(){
		Debug.Log(TAG + "checking if moves left");
		Hex[] hexes = previousHex.GetNeighbors();
		int numOfMoves = 0;
		foreach(Hex hex in hexes){
			numOfMoves = CheckPosMoves(hex,numOfMoves);
		}
		Debug.Log(TAG + previousHex.GetX() + " " + previousHex.GetY() + " number of moves is: " + numOfMoves);
		if(numOfMoves > 0){
			return true;
		}
		willEndUse= true;
		endTimer = 0.6f;

		return false;

	}
	public override int GetID(){
		return 8;
	}
}

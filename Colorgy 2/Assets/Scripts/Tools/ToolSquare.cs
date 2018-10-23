using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSquare : Tool {
	private static string TAG = "TOOL SQUARE: ";
	private Hex previousHex;
	public GameObject cube;
	public Animator animator;
	public GameObject absorbEffect;

	private float rotRate = 0.6f;
	private bool willEndUse;
	private float endTimer;

	private bool changingColor;
	private Color newColor;
	private Color color;
	private float colorTimer;

	public override void Update(){
		if(willEndUse){
			//no moves left
			endTimer -= Time.deltaTime;
			if(endTimer <=0){
				willEndUse = false;
				EndUse();
			}
		}
		if(changingColor){
			colorTimer += Time.deltaTime;
			Color c = Color.Lerp(color,newColor,colorTimer);
			rend.material.color = c;
			if(colorTimer >= 1){
				changingColor = false;
			}
		}
		base.Update();
		cube.transform.Rotate(new Vector3(1,1,0)*rotRate);
	}


	public override void Use(Hex hex){
		Debug.Log(TAG + "using.");

		int hexVal = hex.GetVal() -1;
		if(isDone){
			return;
		}
		if(hex == previousHex){
			return;
		}
		if(CheckCanClear(hexVal)){
			
			if(previousHex != null){
				previousHex.Press(false);
				CheckPreviousHex();
			}

			if(!isPlaced){
				gameObject.SetActive(true);
				animator.Play("SphereEnter");

				isPlaced = true;
				x = hex.GetX();
				y = hex.GetY();

				transform.position = hex.transform.position + new Vector3(0,1f,0);
				transform.rotation = hex.transform.rotation;
				//hex.SetWillClear(0.0f,false);
				previousHex = hex;
				hex.Press(true);
				//hex.Clear(1,this);
				//CreateAbsorbEffect(hex.transform);
				soundManager.PlayCube();

			}
			if(Calc.FindDistance(x,y,hex.GetX(),hex.GetY()) == 1){

				x = hex.GetX();
				y = hex.GetY();
				transform.position = hex.transform.position + new Vector3(0,1f,0);
				//hex.SetWillClear(0.0f,false);
				previousHex = hex;
				hex.Press(true);
				//hex.Clear(1,this);
				//hex.UpdateColor();
				//CreateAbsorbEffect(hex.transform);
				soundManager.PlayCube();
			}
			CheckNeighbors();
			return;
		}
		int stealVal = CanSteal(hexVal);
		if(stealVal == -1){
			Debug.Log(TAG + "Can't steal.");
			return;
		}
		if(previousHex != null){
			previousHex.Press(false);
			CheckPreviousHex();
		}
		if(!isPlaced){
			isPlaced = true;
			x = hex.GetX();
			y = hex.GetY();
			gameObject.SetActive(true);
			rend.material.color = Calc.GetColor(val)*2.0f;
			transform.position = hex.transform.position+ new Vector3(0,1f,0);
			//hex.SetVal(stealVal+1);
			//hex.UpdateColor();
			//hex.Absorb(stealVal+1,this);
			hex.Press(true);
			previousHex= hex;
			//CreateAbsorbEffect(hex.transform);
			soundManager.PlayCube();
		}
		if(Calc.FindDistance(x,y,hex.GetX(),hex.GetY()) == 1){

			x = hex.GetX();
			y = hex.GetY();
			transform.position = hex.transform.position+ new Vector3(0,1f,0);
			//hex.Absorb(stealVal+1,this);
			hex.Press(true);

			previousHex= hex;
			//hex.SetVal(stealVal+1);
			//hex.UpdateColor();
			//CreateAbsorbEffect(hex.transform);
			soundManager.PlayCube();
		}
		CheckNeighbors();

	}
	public void CheckPreviousHex(){
		int hexVal = previousHex.GetVal()-1;
		if(CheckCanClear(hexVal)){
			previousHex.Clear(1,this);
			return;
		}
		int stealVal = CanSteal(hexVal);
		previousHex.Absorb(stealVal+1,this);
	}
	public void CreateAbsorbEffect(Transform t){
		GameObject g = Instantiate( absorbEffect);
		g.transform.position = t.position;

		ParticleSystem p =g.GetComponentInChildren<ParticleSystem>();
		var main = p.main;
	
		main.startColor = Calc.GetColor(GetVal());
		Destroy(g,1.0f);
	}
	public bool CheckCanClear(int hexVal){
		if(val == hexVal){
			if(!isPlaced){
				rend.material.color = Calc.GetColor(val)*2.0f;
			}
			return true;
		}
		//if it is white
		if(val == 6){
			//and is placed on a primary
			if(hexVal == 0 || hexVal == 1 || hexVal == 2  ){
				changingColor = true;
				val = hexVal;
				colorTimer = 0;
				newColor = Calc.GetColor(val)*2.0f;
				color = Color.white*2.0f;
				return true;
			}
		}
		return false;
	}
	public int CanSteal(int hexVal){
		//if cube is white and it becomes that color
		if(val == 6){
			//and if hex is a primary
			if(hexVal == 0 || hexVal == 1 || hexVal == 2){

			}

		}
		if(val == 0){
			if(hexVal == 3 || hexVal == 4){
				return hexVal -2 -val;
			}
		}
		if(val == 1){
			if(hexVal == 3 || hexVal == 5){
				return hexVal -2 -val;
			}
		}
		if(val == 2){
			if(hexVal == 4 || hexVal == 5){
				return hexVal -2 -val;
			}
		}
		return -1;
	}
	public override void EndUse(){
		Debug.Log(TAG + "End use.");
		if(isRising || isDone){
			return;
		}
		if(previousHex != null){
			previousHex.Press(false);
			CheckPreviousHex();
		}
		isRising = true;
		isDone = true;
		StartFade(1.0f,1.0f,3f);
		//GetComponentInChildren<Rigidbody>().isKinematic = false;
		buttonTool.EndUse();
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
	public override GameObject GetTarget(){
		//gets the target for any hex effect
		return cube;
	}
	public bool CheckNeighbors(){
		Debug.Log(TAG + "checking if moves left");
		Hex[] hexes = previousHex.GetNeighbors();
		int numOfMoves = 0;
		foreach(Hex hex in hexes){
			if(hex && !hex.GetWillClear() && (hex.GetVal()-1 == GetVal() || CanSteal(hex.GetVal()-1) != -1 )){
				Debug.Log(TAG + previousHex.GetX() + " " + previousHex.GetY()+ "Can move here: " + hex.GetX() + " " + hex.GetY() );
				numOfMoves++;
			}
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
		return 9;
	}
}

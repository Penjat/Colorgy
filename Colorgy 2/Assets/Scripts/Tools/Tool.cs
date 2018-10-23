using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour {
	

	private static string TAG = "TOOL: ";

	protected SoundFXManager soundManager;


	protected bool isPlaced;
	protected int val;
	public Renderer rend;
	protected bool isDone;//don't process any more input
	protected bool isFading;
	protected bool isRising;
	protected int x,y;
	protected float timer;
	protected Color col;
	private Color fadeCol;
	private float fadeRate = 1.0f;
	protected float riseSpeed = 2.0f;
	protected float rotSpeed = 50.0f;


	public Material fadeMat;
	protected ButtonTool buttonTool;


	public virtual int SetUp(int v,ButtonTool bt,SoundFXManager sm){
		Debug.Log(TAG + "Setting up.");
		soundManager = sm;
		val = v;
		if(v == 7){
			//if the value passed is 7 (empty) just make it white
			val = 6;
		}
		buttonTool = bt;
		col = Calc.GetColor(val);
		//rend = GetComponent<Renderer>();
		//UpdateColor();
		return val;
	}
	public virtual void Update(){
		if(isFading){
			
			timer-= Time.deltaTime*fadeRate;

			rend.material.color = Color.Lerp(fadeCol,col,timer);
		}
		if(isRising){
			transform.Translate(new Vector3(0,1,0)*Time.deltaTime*riseSpeed, Space.World);
			transform.Rotate(new Vector3(0,1,0)*Time.deltaTime*rotSpeed);
		}
	}
	public void UpdateColor(){
		col = Calc.GetColor(val)*4.0f;
		rend.material.color = col;
	}
	public void SetVal(int v){
		val = v;
	}

	public virtual void Highlight(Hex oldHex,Hex newHex){
		//do the regular highlight by default
		if(oldHex){
			oldHex.LightUp(false);
		}
		if(newHex){
			 newHex.LightUp(true);
		}	
	}


	public int GetVal(){
		return val;
	}

	public virtual void Use(Hex hex){
		Debug.Log(TAG + "using.");
		if(isDone){
			return;
		}
	}
	public virtual void EndUse(){
		Debug.Log(TAG + "End use.");
		buttonTool.EndUse();
	}
	public bool IsPlaced(){
		return isPlaced;
	}
	public bool CanMix(int v1,int v2){
		if( (v1 < 3 && v2 < 3) && (v1!=v2)){
			//if they are both even but not equal
			return true;
		}
		return false;
	}
	public bool CanClear(int valHex, int valTool){
		//if it equals the same color or the tool is white, return true

		if(valHex == valTool ){
			return true;
		}
		if(valTool == 6 || valTool == 8){
			
			return true;
		}
		return false;
	}

	public bool CanAbsorb(int valHex, int valTool){
		//returns whether absorbing is possibe
		//does not deal with white tools

		//if they are the same value, return true
		if(valTool == valHex){
			return true;
		}


		//if the tool is a secondary color, return false
		if(valTool > 2){
			return false;
		}


		//if the tool is primary
		if(valTool == 0){
			if(valHex == 3 || valHex == 4){
				return true;
			}
		}
		if(valTool == 1){
			if(valHex == 3 || valHex == 5){
				return true;
			}
		}
		if(valTool == 2){
			if(valHex == 4 || valHex == 5){
				return true;
			}
		}
		return false;
	}
	public virtual GameObject GetTarget(){
		//gets the target for any hex effect
		return null;
	}

	public virtual void Activate(Hex hex){
		Debug.Log(TAG + "activating tool.");
	}
	public void Show(bool b){
		Debug.Log("SHOW = " + b);
		gameObject.SetActive(b);
	}
	public void SetGridPos(Hex hex){
		//Snaps the tool to the grid
		if(hex == null){
			//gameObject.SetActive(false);
			return;
		}
		if(isPlaced){
			return;
		}
		if(!gameObject.activeSelf){
			gameObject.SetActive(true);
		}
		transform.position = hex.transform.position;
	}

	public virtual void Place(Hex hex){
		isPlaced = true;
		gameObject.SetActive(true);
		x = hex.GetX();
		y = hex.GetY();
		if(val == 6){
			val=hex.GetVal()-1;
		}
		transform.rotation = hex.transform.rotation;
		transform.position = hex.transform.position + new Vector3(0,1.6f,0);


	}
	public virtual void EffectFinish(HexEffect hexEffect){
		Debug.Log(TAG + "effect finished.");
	}

	public void StartFade(float t, float rate, float destroyTime){
		if(isFading){
			//don't fade if already fading
			return;
		}
		rend.material = fadeMat;
		fadeCol = Calc.GetColor(val);
		fadeCol.a = 0;
		isFading = true;
		timer = t;
		fadeRate = rate;
		Destroy(gameObject,destroyTime);

	}
	public virtual int GetID(){
		return 0;
	}

}

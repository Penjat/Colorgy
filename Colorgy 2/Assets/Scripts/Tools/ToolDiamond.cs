using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolDiamond : Tool {

	private static string TAG = "TOOL DIAMOND: ";
	public Animator animator;
	protected Hex previousHex;
	public GameObject diamond;
	private float rotRate = 0.6f;

	private bool changingColor;
	private Color newColor;
	private Color color;
	private float colorTimer;
	public override int SetUp(int v,ButtonTool bt,SoundFXManager sm){
		//make sure it is a primary color for all diamond and cube type tools

		if(v == 3 || v == 4 || v == 5){
			Debug.Log("wrong value");
			val = 6;


			return base.SetUp(val,bt,sm);
		}
		return base.SetUp(v,bt,sm);

	}
	public override void Update(){
		
		base.Update();
		diamond.transform.Rotate(new Vector3(0,1,0)*rotRate);

		if(changingColor){
			colorTimer += Time.deltaTime;
			Color c = Color.Lerp(color,newColor,colorTimer);
			rend.material.color = c;
			if(colorTimer >= 1){
				changingColor = false;
			}
		}
	}
	public override void Use(Hex hex){
		Debug.Log(TAG + "using.");
		int hexVal = hex.GetVal() -1;

		if(val == 6 && hexVal < 3){
			animator.Play("SphereEnter");
			val = hexVal;
			isPlaced = true;
			x = hex.GetX();
			y = hex.GetY();
			gameObject.SetActive(true);
			hex.Press(true);
			Place(hex);
			changingColor = true;
			colorTimer = 0;
			newColor = Calc.GetColor(val)*2.0f;
			color = Color.white*2.0f;
			soundManager.PlayDiamond();
			return;
		}

		//if they can mix
		if(CanMix(hexVal,val)){
			Mix(hex);
			soundManager.PlayDiamond();
		}
		//if they are the same color
		if(hexVal == GetVal()){
			//just move but do nothing else
			Move(hex);
			soundManager.PlayDiamond();
		}
	}
	public void Mix(Hex hex){
		Debug.Log(TAG + "Mixing.");
		if(!isPlaced){
			isPlaced = true;
			animator.Play("SphereEnter");
			rend.material.color = Calc.GetColor(val)*2.0f;
			x = hex.GetX();
			y = hex.GetY();
			gameObject.SetActive(true);
			Place(hex);
			hex.SetVal(Calc.GetMix(hex.GetVal(),GetVal()));
			hex.SetAddColorVal(this);
			hex.SetWillMix(0.0f);

			hex.Press(true);
			if(previousHex != null){
				previousHex.Press(false);
			}
			previousHex= hex;
		}
		if(Calc.FindDistance(x,y,hex.GetX(),hex.GetY()) == 1){

			x = hex.GetX();
			y = hex.GetY();
			Place(hex);
			hex.SetVal(Calc.GetMix(hex.GetVal(),GetVal()));
			hex.SetAddColorVal(this);
			hex.SetWillMix(0.0f);
			hex.Press(true);
			if(previousHex != null){
				previousHex.Press(false);
			}
			previousHex= hex;
		}
	}
	public void Move(Hex hex){
		Debug.Log(TAG + "moving.");
		if(!isPlaced){
			isPlaced = true;
			animator.Play("SphereEnter");
			rend.material.color = Calc.GetColor(val)*2.0f;
			x = hex.GetX();
			y = hex.GetY();
			gameObject.SetActive(true);
			Place(hex);
			hex.Press(true);
		}
		if(Calc.FindDistance(x,y,hex.GetX(),hex.GetY()) == 1){

			x = hex.GetX();
			y = hex.GetY();
			Place(hex);
			hex.Press(true);

		}
	}

	public override void EndUse(){
		Debug.Log(TAG + "End use.");
		if(previousHex != null){
			previousHex.Press(false);
		}
		isRising = true;
		StartFade(1.0f,1.0f,1.2f);
		buttonTool.EndUse();
	}
	public override void Highlight(Hex oldHex,Hex newHex){
		//do the regular highlight by default


		if(oldHex){
			oldHex.LightUp(false);
		}

		if(!newHex){
			SetGridPos(null);
			return;
		}
		int hexVal = newHex.GetVal() -1;

		if(GetVal() == 6 && hexVal < 3){
			newHex.LightUp(true);

			SetGridPos(newHex);
			return;
		}
		if(hexVal < 3 && hexVal != GetVal()){
			newHex.LightUp(true);

			SetGridPos(newHex);
			return;
		}


	}
	public override int GetID(){
		return 10;
	}
}

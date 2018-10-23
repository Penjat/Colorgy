using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTool : MonoBehaviour {

	private static string TAG = "BUTTON TOOL: ";

	private SoundFXManager soundManager;
	public Tool tool;
	public Animator animator;
	public Renderer rend;
	public Material fadeMat;
	public Material rainbowMat;
	public Material normMat;
	private int val;
	private bool isOver;
	private bool isSelected;

	public ParticleSystem glowEffect;

	private float timer = 0.0f;
	private bool paused;
	private bool isFading;

	private int toolNum;
	private Color color;
	private bool isDone;
	public void SetUp(int v,int t,SoundFXManager sm){
		Debug.Log(TAG + "setting up.");
		soundManager = sm;

		//is a fal safe making sure an incorrect color isn't inputed
		val = tool.SetUp(v,this,sm);


		if(val == 8){
			rend.material = rainbowMat;
		}else{
			rend.material = normMat;
		}
		UpdateColor();
		glowEffect.Stop();
		var main = glowEffect.main;
		main.startColor = Calc.GetColor(val);

		//delay entry
		toolNum = t;
		timer = 1.8f + toolNum*0.3f;
		animator.speed = 0.0f;
		paused = true;
		tool.gameObject.SetActive(false);

	}

	void Update(){
		//pause before entering level
		if(paused){
			timer-= Time.deltaTime;
			if(timer <=0.0f){
				paused = false;
				animator.speed = 0.6f;
			}
		}
		//fadeout after use
		if(isFading){
			float fadeRate = 1.0f;
			timer-= Time.deltaTime*fadeRate;
			Color c = new Color(0,0,0,0); 
			rend.material.color = Color.Lerp(c,color,timer);
		}

	}
	public void UpdateColor(){
		
		rend.material.color = Calc.GetColor(val);
		color = Calc.GetColor(val);
	}
	public Tool GetTool(){
		return tool;
	}
	public void Highlight(bool b){
		if(isDone){
			return;
		}
		animator.SetBool("over",b);
	}
	public void SetSelected(bool b){
		Debug.Log(TAG + "selected = " + b);
		if(isDone){
			return;
		}
		isSelected = b;
		animator.SetBool("selected",b);
		//tool.Show(b);
		//toggle the glow effect
		if(b){
			glowEffect.Play();
			return;
		}
		glowEffect.Stop();
		glowEffect.Clear();

	}
	public void EndUse(){
		if(isDone){
			return;
		}
		Debug.Log(TAG + "ending use.");
		rend.material = fadeMat;
		rend.material.color = color;
		timer = 1.0f;
		isFading = true;
		isDone = true;
		glowEffect.Stop();
		Destroy(gameObject,4.0f);
	}
}

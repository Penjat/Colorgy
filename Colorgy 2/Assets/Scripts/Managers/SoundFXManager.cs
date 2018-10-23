using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundFXManager : MonoBehaviour {
	private static string TAG = "SOUND MANAGER: ";
	public MenuManager menuManager;

	public AudioSource[] toolSources;
	public AudioSource[] shapeSources;//give shapres there own array so they don't step on themeselves
	private int curSource;
	private int curShapeSource;

	public AudioMixer mixer;

	public AudioClip[] drumSounds;

	public AudioClip[] sphereSounds;
	public AudioClip[] cubeSounds;
	public AudioClip[] diamondSounds;
	public AudioClip[] vortexSounds;

	public AudioClip[] beamSounds;
	public AudioClip[] beamDownSounds;
	public AudioClip[] vortexBeamSounds;
	public AudioClip[] vortexBeamDownSounds;
	public AudioClip[] cubeBeamSounds;
	public AudioClip[] cubeBeamDownSounds;
	public AudioClip[] diamondBeamSounds;
	public AudioClip[] diamondBeamDownSounds;
	public AudioClip[] pyramidSounds;


	public AudioClip[] novaSounds;
	public AudioClip[] cubeNovaSounds;
	public AudioClip[] diamondNovaSounds;
	public AudioClip[] vortexNovaSounds;
	public AudioClip[] uiSounds;
	public AudioClip[] music;

	private int[] sphereMelody = new int[]{0,1,2,3,4,0,5,6};

	private int curMelodyNote = 0;

	private int curChord =0;

	private bool creditsTime = false;
	private int creditBeat;
	private bool creditsPlaying;
	private bool titleFading;

	void Update(){
		if(creditsPlaying){
			if(!toolSources[3].isPlaying){

				menuManager.BackToTitle();
				creditsPlaying = false;

			}
		}



	}
	public void CheckLastLevel(LevelManager levelManager,int chapterNum,Level level,bool isCustom){
		Debug.Log(TAG + level.GetName());
		if(chapterNum == 9 && level.GetName() == "zzz" && !isCustom){
			SetCreditsTime(true);
			Debug.Log(TAG + "last level");
			return;
		}
		SetCreditsTime(false);
	}
	public void SetCreditsTime(bool b){
		creditsTime = b;
	}

	public void PlayTitleBG(){
		toolSources[5].clip = music[4];
		toolSources[5].Play();

	}


	public AudioSource GetToolSource(int i){
		return toolSources[i];
	}
	private void AddCurSource(){
		curSource++;
		if(curSource >= 6){
			curSource = 0;
		}
	}
	private void AddShapeSource(){
		curShapeSource++;
		if(curShapeSource >= shapeSources.Length){
			curShapeSource = 0;
		}
	}
	public void FireBeam(int numCleared,int xDir){
		
		Debug.Log(TAG + "firing beam: " + numCleared);
		toolSources[curSource].clip = beamSounds[curChord];
		toolSources[curSource].Play();
		AddCurChord();
		AddCurSource();
		return;

		if(xDir >0){
			toolSources[curSource].clip = beamSounds[numCleared-1];
			toolSources[curSource].Play();
			AddCurSource();
			return;
		}
		toolSources[curSource].clip = beamDownSounds[numCleared-1];
		toolSources[curSource].Play();
		AddCurSource();

	}
	public void FireVortexBeam(int numCleared,int xDir){
		Debug.Log(TAG + "firing vortex beam: " + numCleared);
		toolSources[curSource].clip = vortexBeamSounds[curChord];
		toolSources[curSource].Play();
		AddCurChord();
		AddCurSource();
		return;

		if(xDir >0){
			toolSources[curSource].clip = vortexBeamSounds[numCleared-1];
			toolSources[curSource].Play();
			AddCurSource();
			return;
		}
		toolSources[curSource].clip = vortexBeamDownSounds[numCleared-1];
		toolSources[curSource].Play();
		AddCurSource();



	}
	public void FireDiamondBeam(int numCleared,int xDir){
		toolSources[curSource].clip = diamondBeamSounds[curChord];
		toolSources[curSource].Play();
		AddCurChord();
		AddCurSource();
		return;
		Debug.Log(TAG + "firing diamond beam: " + numCleared);
		if(xDir >0){
			toolSources[curSource].clip = diamondBeamSounds[numCleared-1];
			toolSources[curSource].Play();
			AddCurSource();
			return;
		}
		toolSources[curSource].clip = diamondBeamDownSounds[numCleared-1];
		toolSources[curSource].Play();
		AddCurSource();


	}
	public void FireCubeBeam(int numCleared,int xDir){
		toolSources[curSource].clip = cubeBeamSounds[curChord];
		toolSources[curSource].Play();
		AddCurChord();
		AddCurSource();
		return;

		Debug.Log(TAG + "firing cube beam: " + numCleared);
		if(xDir >0){
			toolSources[curSource].clip = cubeBeamSounds[numCleared-1];
			toolSources[curSource].Play();
			AddCurSource();
			return;
		}
		toolSources[curSource].clip = cubeBeamDownSounds[numCleared-1];
		toolSources[curSource].Play();
		AddCurSource();



	}
	public void PlayCredits(){
		Debug.Log(TAG + "playing the credits");
		//TODO need to handle reset
		if(creditBeat == 3){
			
			toolSources[creditBeat].clip = music[creditBeat];
			toolSources[creditBeat].Play();
			creditBeat = 0;
			for(int i=0;i<3;i++){
				toolSources[i].loop = false;

				toolSources[i].Stop();
			}
			creditsPlaying = true;
			return;
		}
		toolSources[creditBeat].loop = true;
		toolSources[creditBeat].clip = music[creditBeat];
		toolSources[creditBeat].Play();
		creditBeat++;
	}
	public void PlaySphere(bool placing){
		if(placing && creditsTime){
			//TODO change this
			PlayCredits();
			return;
		}
		if(creditsTime){
			return;
		}
		NextMelodyNote(sphereSounds);
		shapeSources[curShapeSource].clip = sphereSounds[curMelodyNote];
		shapeSources[curShapeSource].Play();
		AddShapeSource();


	}
	public void PlayCube(){
		NextMelodyNote(cubeSounds);
		shapeSources[curShapeSource].clip = cubeSounds[curMelodyNote];
		shapeSources[curShapeSource].Play();
		AddShapeSource();


	}
	public void PlayDiamond(){
		NextMelodyNote(diamondSounds);
		shapeSources[curShapeSource].clip = diamondSounds[curMelodyNote];
		shapeSources[curShapeSource].Play();
		AddShapeSource();


	}
	public void PlayVortex(){
		NextMelodyNote(vortexSounds);
		shapeSources[curShapeSource].clip = vortexSounds[curMelodyNote];
		shapeSources[curShapeSource].Play();
		AddShapeSource();


	}
	public void NextMelodyNote(Object[] array){
		bool b = (Random.value > 0.5f);
		if(b){
			curMelodyNote++;
		}else{
			curMelodyNote--;
		}
		if(curMelodyNote >= array.Length){
			curMelodyNote = array.Length-2;
		}
		if(curMelodyNote <0){
			curMelodyNote = 1;
		}
	}
	public void PlayNova(int val){
		if(creditsTime){
			PlayCredits();
			return;
		}
		toolSources[curSource].clip = novaSounds[curChord];
		toolSources[curSource].Play();
		AddCurSource();
		AddCurChord();
	}
	public void PlayCubeNova(int val){

		toolSources[curSource].clip = cubeNovaSounds[curChord];
		toolSources[curSource].Play();
		AddCurSource();
		AddCurChord();
	}
	public void PlayDiamondNova(int val){

		toolSources[curSource].clip = diamondNovaSounds[curChord];
		toolSources[curSource].Play();
		AddCurSource();
		AddCurChord();
	}
	public void PlayVortexNova(int val){

		toolSources[curSource].clip = vortexNovaSounds[curChord];
		toolSources[curSource].Play();
		AddCurSource();
		AddCurChord();

	}
	public void SetCurChord(int i){
		curChord = i;
	}
	public void SetStartChord(int numOfTools){
		//chooses a starting chord that will work with the current number of tools
		int maxVal = 6-numOfTools;
		if(maxVal == 0){
			curChord = 0;
			return;
		}
		curChord = Random.Range(0,maxVal+1);


	}
	private void AddCurChord(){
		//Could try going up or down
		curChord++;
	

	}
	public void RandomDrum(){
		int r = Random.Range(0,drumSounds.Length);
		toolSources[curSource].clip = drumSounds[r];
		toolSources[curSource].Play();
		AddCurSource();
	}

	public void SetVol(Slider slider){
		float val = slider.value;
		mixer.SetFloat("toolVol",val);
		shapeSources[0].clip = novaSounds[0];
		shapeSources[0].Play();
	}
	public void PlayUISound(int i){
		toolSources[curSource].clip = uiSounds[i];
		toolSources[curSource].Play();
		AddCurSource();
	}
	public void PlayUISound(int i,float delay){
		toolSources[curSource].clip = uiSounds[i];
		toolSources[curSource].PlayDelayed(delay);
		AddCurSource();
	}
	public void PlayPyramid(){
		NextMelodyNote(pyramidSounds);
		shapeSources[curShapeSource].clip = pyramidSounds[curMelodyNote];
		shapeSources[curShapeSource].Play();
		AddShapeSource();
	}
}

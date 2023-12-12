using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour 
{
	public static DialogueSystem instance;

	public ELEMENTS elements;

	void Awake()
	{
		instance = this;
	}

	/// <summary>
	/// Say something and show it on the speech box.
	/// </summary>
	public void Say(string speech, string speaker = "", string backgroundim = "", string chara1im = "", string chara2im = "")
	{
		if (backgroundim != "")
		{
			background.GetComponent<Image>().sprite = GetMoodSprite(backgroundim);
		}
		
		if (chara1im != "")
		{
			chara1.GetComponent<Image>().sprite = GetMoodSprite(chara1im);
		}
		
		if (chara2im != "")
		{
			chara2.GetComponent<Image>().sprite = GetMoodSprite(chara2im);
		}
			
		
		StopSpeaking();

		speaking = StartCoroutine(Speaking(speech, false, speaker));
		
	}
	
	private Sprite GetMoodSprite(string mood)
	{
		for (int i = 0 ; i < moodSprites.Length ; ++i)
		{
			if (moodSprites[i] != null && string.Equals(moodSprites[i].name, mood))
			{
				return moodSprites[i];
			}
			
		}
		Debug.LogWarningFormat("No sprite representing mood '{0}' has been found", mood);
		return empty;
	}

	/// <summary>
	/// Say something to be added to what is already on the speech box.
	/// </summary>
	public void SayAdd(string speech, string speaker = "")
	{
		StopSpeaking();

		speechText.text = targetSpeech;

		speaking = StartCoroutine(Speaking(speech, true, speaker));
	}

	public void StopSpeaking()
	{
		if (isSpeaking)
		{
			StopCoroutine(speaking);
		}
		if (textArchitect != null && textArchitect.isConstructing)
		{
			textArchitect.Stop();
		}
		speaking = null;
	}
		
	public bool isSpeaking {get{return speaking != null;}}
	[HideInInspector] public bool isWaitingForUserInput = false;

	public string targetSpeech = "";
	Coroutine speaking = null;
	TextArchitect textArchitect = null;

	IEnumerator Speaking(string speech, bool additive, string speaker = "")
	{
		speechPanel.SetActive(true);

		string additiveSpeech = additive ? speechText.text : "";
		targetSpeech = additiveSpeech + speech;

		textArchitect = new TextArchitect(speech, additiveSpeech);

		speakerNameText.text = DetermineSpeaker(speaker);//temporary

		isWaitingForUserInput = false;

		while(textArchitect.isConstructing)
		{
			if (Input.GetKey(KeyCode.Space))
				textArchitect.skip = true;

			speechText.text = textArchitect.currentText;
			
			yield return new WaitForEndOfFrame();
		}
		//if skipping stopped the display text from updating correctly, force it to update at the end.
		speechText.text = textArchitect.currentText;

		//text finished
		isWaitingForUserInput = true;
		while(isWaitingForUserInput)
			yield return new WaitForEndOfFrame();

		StopSpeaking();
	}

	string DetermineSpeaker(string s)
	{
		string retVal = speakerNameText.text;//default return is the current name
		if (s != speakerNameText.text && s != "")
			retVal = (s.ToLower().Contains("narrator")) ? "" : s;

		return retVal;
	}

	/// <summary>
	/// Close the entire speech panel. Stop all dialogue.
	/// </summary>
	public void Close()
	{
		StopSpeaking ();
		speechPanel.SetActive (false);
	}

	[System.Serializable]
	public class ELEMENTS
	{
		/// <summary>
		/// The main panel containing all dialogue related elements on the UI
		/// </summary>
		public GameObject speechPanel;
		public TextMeshProUGUI speakerNameText;
		public TextMeshProUGUI speechText;
		public GameObject background;
		public GameObject chara1;
		public GameObject chara2;
	}
	public GameObject speechPanel {get{return elements.speechPanel;}}
	public TextMeshProUGUI speakerNameText {get{return elements.speakerNameText;}}
	public TextMeshProUGUI speechText {get{return elements.speechText;}}
	public GameObject background {get{return elements.background;}}
	public GameObject chara1 {get{return elements.chara1;}}
	public GameObject chara2 {get{return elements.chara2;}}
	public Sprite empty;
	public Sprite[] moodSprites;
}

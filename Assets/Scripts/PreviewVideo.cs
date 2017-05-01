using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewVideo : MonoBehaviour {
	public static string folder = RecordButtonManager.folder;
	public static int frameRate = RecordButtonManager.frameRate;

	private string realFolder = "";

	public Button previewButton;
	public Renderer previewRenderer;
	private IEnumerator playPreviewCoroutine;
	private bool previewing;
	private Texture2D[] screenshots;


	void Start () {

		// Set the playback framerate!
		// (real time doesn't influence time anymore)
		// Time.captureFramerate = frameRate; // TODO: testing in progress

		realFolder = folder; // TODO: eventually allow the user to select which folder from a dropdown menu

		playPreviewCoroutine = PlayPreview();
		((Text)previewButton.transform.GetChild (0).GetComponent<Text>()).text = "Start Preview"; // set up preview button initial text
		previewButton.GetComponent<Button> ().onClick.AddListener (TogglePreview);	

	}

	void Update () {
		
	}

	void TogglePreview() {
		// Stop the preview playback if we are already playing it
		if (previewing) {
			StopCoroutine (playPreviewCoroutine);
			((Text)previewButton.transform.GetChild (0).GetComponent<Text>()).text = "Start Preview";
		} else {
			StartCoroutine (playPreviewCoroutine);
			((Text)previewButton.transform.GetChild (0).GetComponent<Text>()).text = "Stop Preview";
		}
		previewing = !previewing; // toggle the "recording" bool flag
	}

	private IEnumerator PlayPreview() {
		int index = 0;
		while(index < screenshots.Length) { 
			index = ((int) (Time.time * frameRate)) % screenshots.Length;
			previewRenderer.material.mainTexture = screenshots [index];
			yield return null; // Unity will crash without this line
		}
		yield return null;
	}
}

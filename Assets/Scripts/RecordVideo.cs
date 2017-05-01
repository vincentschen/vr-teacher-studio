using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// Reference: http://wiki.unity3d.com/index.php/ScreenShotMovie

public class RecordVideo : MonoBehaviour {

    // The folder we place all screenshots inside.
    // If the folder exists we will append numbers to create an empty folder.
    public static string folder = "ScreenshotMovieOutput";
    public static int frameRate = 25;
    public static int sizeMultiplier = 1;

    private string realFolder = "";

	private IEnumerator recordScreenshotsCoroutine;
	private bool recording;
	public Button recordButton;

    void Start()
    {
        // Set the playback framerate!
        // (real time doesn't influence time anymore)
        Time.captureFramerate = frameRate;

        // Find a folder that doesn't exist yet by appending numbers!
        realFolder = folder;
        int count = 1;
        while (System.IO.Directory.Exists(realFolder))
        {
            realFolder = folder + count;
            count++;
        }
        // Create the folder
        System.IO.Directory.CreateDirectory(realFolder);

		recordScreenshotsCoroutine = RecordScreenshots (); // set up the coroutine that records screenshots
		recording = false; // we won't start recording immediately
		((Text)recordButton.transform.GetChild (0).GetComponent<Text>()).text = "Start Recording"; // set up record button initial text
		recordButton.GetComponent<Button>().onClick.AddListener(ToggleRecord); // add listener to Record button

    }
		
	private void ToggleRecord() {
		// Stop the recording if we are already recording
		if (recording) {
			StopCoroutine (recordScreenshotsCoroutine);
			((Text)recordButton.transform.GetChild (0).GetComponent<Text>()).text = "Start Recording";
		} else {
			StartCoroutine (recordScreenshotsCoroutine);
			((Text)recordButton.transform.GetChild (0).GetComponent<Text>()).text = "Stop Recording";

		}
		recording = !recording; // toggle the "recording" bool flag
	}

	private IEnumerator RecordScreenshots() {
		while (true) {
			// name is "realFolder/shot 0005.png"
			var name = string.Format ("{0}/shot {1:D04}.png", realFolder, Time.frameCount);

			// Capture the screenshot
			Application.CaptureScreenshot (name, sizeMultiplier);
			yield return null; // Unity will crash without this line
		}
		yield return null;
	}
}

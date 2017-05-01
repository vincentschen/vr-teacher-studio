using UnityEngine;
using System.Collections;
using System;

public class CameraSnapshot : MonoBehaviour
{
    [SerializeField]
    RenderTexture securityCameraTexture;  // drag the render texture onto this field in the Inspector
    [SerializeField]
    Camera securityCamera; // drag the security camera onto this field in the inspector

    void LateUpdate()
    {

        if (Input.GetKeyDown("s"))
        {
            StartCoroutine(SaveCameraView());
        }

    }


    public IEnumerator SaveCameraView()
    {
        yield return new WaitForEndOfFrame();

        // get the camera's render texture
        RenderTexture rendText = RenderTexture.active;
        RenderTexture.active = securityCamera.targetTexture;

        // render the texture
        securityCamera.Render();

        // create a new Texture2D with the camera's texture, using its height and width
        Texture2D cameraImage = new Texture2D(securityCamera.targetTexture.width, securityCamera.targetTexture.height, TextureFormat.RGB24, false);
        cameraImage.ReadPixels(new Rect(0, 0, securityCamera.targetTexture.width, securityCamera.targetTexture.height), 0, 0);
        cameraImage.Apply();
        RenderTexture.active = rendText;

        // store the texture into a .PNG file
        byte[] bytes = cameraImage.EncodeToPNG();


        // save the encoded image to a file
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/camera_image.png", bytes);
    }
}

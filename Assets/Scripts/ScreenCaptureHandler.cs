using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenCaptureHandler : MonoBehaviour
{
    public static ScreenCaptureHandler instance;

    [SerializeField]
    private Rect captureArea;

    [HideInInspector]
    public WebCamTexture webcamTexture;

    private Texture2D screenShotTex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length > 0)
        {
            string camName = devices[0].name;
            ScreenCaptureHandler.instance.webcamTexture = new WebCamTexture(camName);

            // Start the camera
            ScreenCaptureHandler.instance.webcamTexture.Play();

            // Assign texture to RawImage
            UIManager.instance.CamFeed.texture = ScreenCaptureHandler.instance.webcamTexture;
        }
        else
        {
            Debug.LogWarning("No camera found!");
        }
    }

    public IEnumerator CaptureUI()
    {
        Debug.Log("Capturing webcam feed from UI area...");

        // Pause the webcam texture to avoid mid-frame capture
        webcamTexture.Pause();
        yield return new WaitForEndOfFrame();

        // Get the RawImage RectTransform area (UI space)
        Vector3[] worldCorners = new Vector3[4];
        UIManager.instance.ScreenShotArea.GetComponent<RectTransform>().GetWorldCorners(worldCorners);

        // Convert world corners to screen points
        Vector2 screenBottomLeft = RectTransformUtility.WorldToScreenPoint(Camera.main, worldCorners[0]);
        Vector2 screenTopRight = RectTransformUtility.WorldToScreenPoint(Camera.main, worldCorners[2]);

        float x = screenBottomLeft.x;
        float y = screenBottomLeft.y;
        float width = screenTopRight.x - x;
        float height = screenTopRight.y - y;

        Debug.Log($"Screen Area for Capture: x:{x}, y:{y}, width:{width}, height:{height}");

        // Convert screen coordinates to texture coordinates relative to RawImage
        RectTransform rawImageRect = UIManager.instance.CamFeed.rectTransform;
        Vector2 rawImageScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, rawImageRect.position);
        float rawImageWidth = rawImageRect.rect.width;
        float rawImageHeight = rawImageRect.rect.height;

        // Ratio of screen-to-texture pixels
        float texWidth = webcamTexture.width;
        float texHeight = webcamTexture.height;

        // Calculate crop region in webcam texture space
        float cropX = (x - rawImageScreenPos.x + rawImageWidth / 2f) * texWidth / rawImageWidth;
        float cropY = (y - rawImageScreenPos.y + rawImageHeight / 2f) * texHeight / rawImageHeight;
        float cropW = width * texWidth / rawImageWidth;
        float cropH = height * texHeight / rawImageHeight;

        cropX = Mathf.Clamp(cropX, 0, texWidth - cropW);
        cropY = Mathf.Clamp(cropY, 0, texHeight - cropH);

        Debug.Log($"WebcamTex crop: x:{cropX}, y:{cropY}, w:{cropW}, h:{cropH}");

        // Get pixels from the camera feed
        Color[] pixels = webcamTexture.GetPixels((int)cropX, (int)cropY, (int)cropW, (int)cropH);
        Texture2D cropped = new Texture2D((int)cropW, (int)cropH, TextureFormat.RGB24, false);
        cropped.SetPixels(pixels);
        cropped.Apply();

        // Assign to UI
        UIManager.instance.ScreenShotImage.texture = cropped;
        UIManager.instance.ScreenShotImage.color = new Color(1, 1, 1, 1);
        UIManager.instance.ScreenShotImage.gameObject.SetActive(true);

        // Resume webcam
        webcamTexture.Play();
    }
}

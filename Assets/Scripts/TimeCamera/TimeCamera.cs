using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TimeCamera : MonoBehaviour
{
    [Header("Time Cameras")]
    [SerializeField] private Camera displayCamera;
    [SerializeField] private Camera presentCamera;
    [SerializeField] private Camera pastCamera;
    [SerializeField] private Camera futureCamera;

    [Header("Time Camera 3D Models")]
    [SerializeField] private GameObject cameraModel;
    [SerializeField] private GameObject timelineSwitch;

    [Header("UI")]
    [SerializeField] private Image camFlashImage;
    [SerializeField] private TextMesh selectedTimelineText;

    private Dictionary<CameraType, Camera> cameras;
    private CameraType currentCameraType;
    public bool Active { get; set; }
    public LayerMask CurrentLayerMask => displayCamera.cullingMask;
    public int CurrentLayer => LayerMask.NameToLayer(currentCameraType.ToString());

    public delegate void ChangingDimensionEvent();
    public event ChangingDimensionEvent ChangingDimension;

    public delegate void ChangedDimensionEvent();
    public event ChangedDimensionEvent ChangedDimension;

    private void Awake()
    {
        presentCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        pastCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        futureCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

        cameras = new Dictionary<CameraType, Camera>();

        cameras.Add(CameraType.Present, presentCamera);
        cameras.Add(CameraType.Past, pastCamera);
        cameras.Add(CameraType.Future, futureCamera);

        currentCameraType = CameraType.Present;
        Shader.SetGlobalTexture("_TimeView", presentCamera.targetTexture);
        displayCamera.cullingMask = LayerMask.GetMask(currentCameraType.ToString());

        Active = false;
        cameraModel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetActive(bool active)
    {
        //TODO: Fix AudioManager issue
        //AudioManager.Instance.Play(gameObject, "Camera_Activate");
        Active = active;
        cameraModel.SetActive(active);
    }

    public void SetZoom(bool zoom)
    {
        if (!Active)
            return;

        Vector3 translation = zoom ? new Vector3(0, 0, 0.02f) : new Vector3(0, 0, -0.02f);
        cameraModel.transform.localPosition += translation;
    }

    public void SwapCamera(CameraType cameraType)
    {
        if (!Active || currentCameraType == cameraType)
            return;

        currentCameraType = cameraType;
        selectedTimelineText.text = currentCameraType.ToString();

        if (currentCameraType == CameraType.Present)
            selectedTimelineText.color = Color.green;

        else if (currentCameraType == CameraType.Past)
            selectedTimelineText.color = Color.yellow;

        else if (currentCameraType == CameraType.Future)
            selectedTimelineText.color = Color.red;

        StartCoroutine(TurnSwitch());
        Shader.SetGlobalTexture("_TimeView", cameras[currentCameraType].targetTexture);
    }

    public void ChangeDimension()
    {
        if (!Active)
            return;

        int layerMask = LayerMask.GetMask(currentCameraType.ToString());

        if (displayCamera.cullingMask == layerMask)
            return;

        ChangingDimension?.Invoke();
        StartCoroutine(FlashCamera());

        displayCamera.cullingMask = layerMask;
        int layer = LayerMask.NameToLayer(currentCameraType.ToString());
        gameObject.layer = layer;

        LayerUtils.SetChildLayerRecursivly(transform, layer);
    }

    private IEnumerator FlashCamera()
    {
        AudioManager.Instance.Play(gameObject, "Camera_Flash");
        camFlashImage.color = Color.white;

        yield return new WaitForSeconds(1);

        float duration = 1.5f;
        float time = 0;

        Color startCol = camFlashImage.color;
        Color endCol = new Color(1, 1, 1, 0);

        while (time < duration)
        {
            time += Time.deltaTime;

            Color newCol = Color.Lerp(startCol, endCol, time / duration);
            camFlashImage.color = newCol;

            yield return null;
        }

        ChangedDimension?.Invoke();
    }

    private IEnumerator TurnSwitch()
    {
        AudioManager.Instance.Play(gameObject, "Camera_Scroll");
        float duration = 0.3f;
        float time = 0;

        Vector3 startRot = timelineSwitch.transform.localEulerAngles;
        Vector3 endRot = timelineSwitch.transform.localEulerAngles + new Vector3(0, 90, 0);

        while (time < duration)
        {
            time += Time.deltaTime;

            Quaternion newRot = Quaternion.Lerp(Quaternion.Euler(startRot), Quaternion.Euler(endRot), time / duration);
            timelineSwitch.transform.localRotation = newRot;

            yield return null;
        }
    }
}

public enum CameraType
{
    Past,
    Present,
    Future
}

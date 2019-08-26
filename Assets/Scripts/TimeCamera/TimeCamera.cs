using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TimeCamera : MonoBehaviour
{
    [SerializeField] private Camera displayCamera;
    [SerializeField] private Camera presentCamera;
    [SerializeField] private Camera pastCamera;
    [SerializeField] private Camera futureCamera;
    [SerializeField] private Image camFlashImage;
    [SerializeField] private TextMesh selectedTimelineText;
    [SerializeField] private GameObject cameraModel;
    [SerializeField] private GameObject timelineSwitch;

    private Dictionary<CameraType, Camera> cameras;
    private CameraType currentCameraType;
    private bool active;

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

        SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            SwapCamera(CameraType.Past);
        }

        else if (Input.GetKey(KeyCode.Alpha2))
        {
            SwapCamera(CameraType.Present);
        }

        else if (Input.GetKey(KeyCode.Alpha3))
        {
            SwapCamera(CameraType.Future);
        }

        else if (Input.GetKey(KeyCode.Mouse0))
        {
            ChangeDimension();
        }

        else if(Input.GetKeyDown(KeyCode.Q))
        {
            SetActive(!active);
        }

        else if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            SetZoom(true);
        }

        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            SetZoom(false);
        }
    }
    
    public void SetActive(bool active)
    {
        this.active = active;
        cameraModel.SetActive(active);
    }

    public void SetZoom(bool zoom)
    {
        if (!active)
            return;

        Vector3 translation = zoom ? new Vector3(0, 0, 0.02f) : new Vector3(0, 0, -0.02f);
        cameraModel.transform.localPosition += translation;
    }

    public void SwapCamera(CameraType cameraType)
    {
        if (!active || currentCameraType == cameraType)
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
        if (!active)
            return;

        int layerMask = LayerMask.GetMask(currentCameraType.ToString());

        if (displayCamera.cullingMask == layerMask)
            return;

        StartCoroutine(FlashCamera());

        displayCamera.cullingMask = layerMask;
        int layer = LayerMask.NameToLayer(currentCameraType.ToString());
        gameObject.layer = layer;

        SetChildLayerRecursivly(transform, layer);
    }

    private void SetChildLayerRecursivly(Transform transform, int layer)
    {
        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
                SetChildLayerRecursivly(child, layer);

            child.gameObject.layer = layer;
        }
    }

    private IEnumerator FlashCamera()
    {
        camFlashImage.color = Color.white;

        yield return new WaitForSeconds(1);

        float duration = 2;
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
    }

    private IEnumerator TurnSwitch()
    {
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

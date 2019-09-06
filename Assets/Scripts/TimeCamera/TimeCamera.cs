using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TimeCamera : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Camera _displayCamera;
    [SerializeField] private Camera _OutlineCamera;
    [SerializeField] private Camera _presentCamera;
    [SerializeField] private Camera _pastCamera;
    [SerializeField] private Camera _futureCamera;

    [Header("Time Camera 3D Models")]
    [SerializeField] private GameObject _cameraModel;
    [SerializeField] private GameObject _timelineSwitch;

    [Header("UI")]
    [SerializeField] private Image _camFlashImage;
    [SerializeField] private TextMesh _selectedTimelineText;

    [Header("Others")]
    [SerializeField] private List<GameObject> _objectsToSwitchDimensions;

    private Dictionary<CameraType, Camera> cameras;
    private CameraType currentCameraType;
    private Dictionary<CameraType, bool> activeLenses;
    private AudioSource recAudioSource;

    public bool Active { get; set; }
    public LayerMask CurrentLayerMask => _displayCamera.cullingMask;
    public int CurrentLayer => LayerMask.NameToLayer(currentCameraType.ToString());

    public delegate void ChangingDimensionEvent();
    public event ChangingDimensionEvent ChangingDimension;

    public delegate void ChangedDimensionEvent();
    public event ChangedDimensionEvent ChangedDimension;

    private void Awake()
    {
        _presentCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        _pastCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        _futureCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

        cameras = new Dictionary<CameraType, Camera>();
        activeLenses = new Dictionary<CameraType, bool>();

        cameras.Add(CameraType.Present, _presentCamera);
        cameras.Add(CameraType.Past, _pastCamera);
        cameras.Add(CameraType.Future, _futureCamera);

        activeLenses.Add(CameraType.Present, true);
        activeLenses.Add(CameraType.Past, true);
        activeLenses.Add(CameraType.Future, true);

        currentCameraType = CameraType.Present;
        Shader.SetGlobalTexture("_TimeView", _presentCamera.targetTexture);
        _displayCamera.cullingMask = LayerMask.GetMask(currentCameraType.ToString());
        _OutlineCamera.cullingMask = LayerMask.GetMask(currentCameraType.ToString());

        Active = false;
        _cameraModel.SetActive(false);

        recAudioSource = GetComponent<AudioSource>();
        recAudioSource.loop = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetActive(bool active, bool playSound = true)
    {
        if(playSound)
            AudioManager.Instance.Play(gameObject, "Camera_Activate");

        if(active)
            recAudioSource.Play();
        else
            recAudioSource.Stop();

        Active = active;
        _cameraModel.SetActive(active);
    }

    public void SetZoom(bool zoom)
    {
        if (!Active)
            return;

        Vector3 translation = zoom ? new Vector3(0, 0, 0.02f) : new Vector3(0, 0, -0.02f);
        _cameraModel.transform.localPosition += translation;
    }

    public void SwapCamera(CameraType cameraType)
    {
        if (!Active || currentCameraType == cameraType || !activeLenses[cameraType])
            return;

        currentCameraType = cameraType;
        _selectedTimelineText.text = currentCameraType.ToString();

        if (currentCameraType == CameraType.Present)
            _selectedTimelineText.color = Color.green;

        else if (currentCameraType == CameraType.Past)
            _selectedTimelineText.color = Color.yellow;

        else if (currentCameraType == CameraType.Future)
            _selectedTimelineText.color = Color.red;

        StartCoroutine(TurnSwitch());
        Shader.SetGlobalTexture("_TimeView", cameras[currentCameraType].targetTexture);
    }

    public void ChangeDimension()
    {
        if (!Active)
            return;

        int layerMask = LayerMask.GetMask(currentCameraType.ToString());

        if (_displayCamera.cullingMask == layerMask)
            return;

        ChangingDimension?.Invoke();
        StartCoroutine(FlashCamera());

        _displayCamera.cullingMask = layerMask;
        _OutlineCamera.cullingMask = layerMask;
        int layer = LayerMask.NameToLayer(currentCameraType.ToString());
        gameObject.layer = layer;

        LayerUtils.SetChildLayerRecursivly(transform, layer);

        foreach(GameObject obj in _objectsToSwitchDimensions)
        {
            obj.layer = layer;
            LayerUtils.SetChildLayerRecursivly(obj.transform, layer);
        }

        SetActive(false, false);
    }

    public void SetLensActive(CameraType lensType, bool active)
    {
        activeLenses[lensType] = active;
    }

    private IEnumerator FlashCamera()
    {
        AudioManager.Instance.Play(gameObject, "Camera_Flash");
        _camFlashImage.color = Color.white;

        yield return new WaitForSeconds(1);

        float duration = 1.5f;
        float time = 0;

        Color startCol = _camFlashImage.color;
        Color endCol = new Color(1, 1, 1, 0);

        while (time < duration)
        {
            time += Time.deltaTime;

            Color newCol = Color.Lerp(startCol, endCol, time / duration);
            _camFlashImage.color = newCol;

            yield return null;
        }

        ChangedDimension?.Invoke();
    }

    private IEnumerator TurnSwitch()
    {
        AudioManager.Instance.Play(gameObject, "Camera_Scroll");
        float duration = 0.3f;
        float time = 0;

        Vector3 startRot = _timelineSwitch.transform.localEulerAngles;
        Vector3 endRot = _timelineSwitch.transform.localEulerAngles + new Vector3(0, 90, 0);

        while (time < duration)
        {
            time += Time.deltaTime;

            Quaternion newRot = Quaternion.Lerp(Quaternion.Euler(startRot), Quaternion.Euler(endRot), time / duration);
            _timelineSwitch.transform.localRotation = newRot;

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

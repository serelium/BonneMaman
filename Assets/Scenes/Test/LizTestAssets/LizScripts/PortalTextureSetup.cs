using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public Camera cameraPortalDest;
    public Material CameraPortalDestMat;

    public Camera cameraPortalOrigin;
    public Material CameraPortalOriginMat;

    void Start()
    {
        if(cameraPortalDest.targetTexture != null)
        {
            cameraPortalDest.targetTexture.Release();
        }
        cameraPortalDest.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        CameraPortalDestMat.mainTexture = cameraPortalDest.targetTexture;

        if (cameraPortalOrigin.targetTexture != null)
        {
            cameraPortalOrigin.targetTexture.Release();
        }
        cameraPortalOrigin.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        CameraPortalOriginMat.mainTexture = cameraPortalOrigin.targetTexture;
    }

   
}

using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
// This should always be attached to the main camera
public class OutlineAndBlurEffect : MonoBehaviour
{
	[Range (0, 10)]
	public float Intensity = 2;

	private Material outlineAndBlurMat;

	void OnEnable()
	{
        // Material that combines the blur and outline pass (outline pass - blur pass)
		outlineAndBlurMat = new Material(Shader.Find("Hidden/OutlineAndBlur"));
    }

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		outlineAndBlurMat.SetFloat("_Intensity", Intensity); // Set the intensity of the outline
        Graphics.Blit(src, dst, outlineAndBlurMat, 0); // Copy the result to the main display camera
	}
}

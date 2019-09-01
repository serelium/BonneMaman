using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class OutlinePrePass : MonoBehaviour
{
	private static RenderTexture outlinePass;
	private static RenderTexture blurredPass;

	private Material blurMat;

	void OnEnable()
	{
        outlinePass = new RenderTexture(Screen.width, Screen.height, 24); // Outline texture pass
		outlinePass.antiAliasing = QualitySettings.antiAliasing;
		blurredPass = new RenderTexture(Screen.width, Screen.height, 0); // Blur texture pass (blur of outline pass)

        Camera camera = GetComponent<Camera>();
		Shader outlineShader = Shader.Find("Hidden/OutlineReplace");
		camera.targetTexture = outlinePass;
		camera.SetReplacementShader(outlineShader, "Outline"); // Set the camera to render using the outline shader as a replacement
		Shader.SetGlobalTexture("_OutlinePrePassTex", outlinePass);
		Shader.SetGlobalTexture("_OutlineBlurredTex", blurredPass);

		blurMat = new Material(Shader.Find("Hidden/Blur"));
		blurMat.SetVector("_BlurSize", new Vector2(blurredPass.texelSize.x * 1.3f, blurredPass.texelSize.y * 1.3f));
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		Graphics.Blit(src, dst); // Copy the texture rendered by the camera to the end result (we keep it the same and only modify a copy of the texture)

        // Clear the blurredPass buffer
        Graphics.SetRenderTarget(blurredPass);
		GL.Clear(false, true, Color.clear);

		Graphics.Blit(src, blurredPass);
		
        // Blur the outlinePass 4 times using the blur shader and copy the result to the blur Pass
		for (int i = 0; i < 4; i++)
		{
            RenderTexture temp = RenderTexture.GetTemporary(blurredPass.width, blurredPass.height);
			Graphics.Blit(blurredPass, temp, blurMat, 0);
			Graphics.Blit(temp, blurredPass, blurMat, 1);
			RenderTexture.ReleaseTemporary(temp);
		}
	}
}

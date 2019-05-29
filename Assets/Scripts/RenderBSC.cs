using UnityEngine;

[ExecuteInEditMode]
public class RenderBSC : MonoBehaviour {
    Material ScreenMat {
        get {
            if (screenMat == null) {
                screenMat = new Material(curShader);
                screenMat.hideFlags = HideFlags.HideAndDontSave;
            }
            return screenMat;
        }
    }

    public Shader curShader = null;
    public float brightness = 1.0f;
    public float saturation = 1.0f;
    public float contrast = 1.0f;
    private Material screenMat;

    // Start is called before the first frame update
    void Start() {
        if (!curShader && !curShader.isSupported) {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update() {
        brightness = Mathf.Clamp(brightness, 0.0f, 2.0f);
        saturation = Mathf.Clamp(saturation, 0.0f, 2.0f);
        contrast = Mathf.Clamp(contrast, 0.0f, 2.0f);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (curShader != null) {
            ScreenMat.SetFloat("_Brightness", brightness);
            ScreenMat.SetFloat("_Saturation", saturation);
            ScreenMat.SetFloat("_Contrast", contrast);

            Graphics.Blit(source, destination, screenMat);
        } else {
            Graphics.Blit(source, destination);
        }
    }

    private void OnDisable() {
        if (screenMat) {
            DestroyImmediate(screenMat);
        }
    }
}

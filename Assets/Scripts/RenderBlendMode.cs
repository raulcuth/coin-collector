using UnityEngine;

[ExecuteInEditMode]
public class RenderBlendMode : MonoBehaviour {
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
    public Texture2D blendTexture;
    public float blendOpacity = 1.0f;
    private Material screenMat;

    // Start is called before the first frame update
    void Start() {
        if (!curShader && !curShader.isSupported) {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update() {
        blendOpacity = Mathf.Clamp(blendOpacity, 0.0f, 1.0f);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (curShader != null) {
            ScreenMat.SetTexture("_BlendTex", blendTexture);
            ScreenMat.SetFloat("_Opacity", blendOpacity);
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

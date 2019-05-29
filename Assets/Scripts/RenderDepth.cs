using UnityEngine;

[ExecuteInEditMode]
public class RenderDepth : MonoBehaviour {
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
    public float depthPower = 0.2f;
    private Material screenMat;

    // Start is called before the first frame update
    void Start() {
        if (!curShader && !curShader.isSupported) {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update() {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
        depthPower = Mathf.Clamp(depthPower, 0, 1);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (curShader != null) {
            ScreenMat.SetFloat("_DepthPower", depthPower);
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

using UnityEngine;

[ExecuteInEditMode]
public class TestRenderImage : MonoBehaviour
{
    Material ScreenMat {
        get {
            if (screenMat == null) {
                screenMat = new Material(curShader);
                screenMat.hideFlags = HideFlags.HideAndDontSave;
            }
            return screenMat;
        }
    }

    public Shader curShader;
    public float greyscaleAmount = 1.0f;
    private Material screenMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceRenderTextureController : MonoBehaviour
{
    Material elementSurfaceMaterial;
    public RenderTexture splatControl;
    int textureHeight = 512;
    int textureWidth = 512;
    Vector2Int textureSize { get { return new Vector2Int(textureWidth,textureHeight); } }
    public float brushSize=64;
    public Shader brushShader;
    
    [Range(0,1)]
    public float threshold;
    public bool debugEnalble;
    public Color debugColor;
    public Shader debugShader;
    Material debugMaterial;

    private void Awake()
    {
        elementSurfaceMaterial = GetComponent<MeshRenderer>().material;
        splatControl = new RenderTexture(textureWidth,textureHeight,0);
        splatControl.Create();
        elementSurfaceMaterial.SetTexture("_Control", splatControl);
        debugMaterial = new Material(debugShader);
        
    }
    public void ClearTexture()
    {
        RenderTexture.active = splatControl;
        GL.Clear(true, true, new Color(0, 0, 0, 0));
        RenderTexture.active = null;
    }
    
    public void Paint(Vector2 uv,  float brushSize, Material material)
    {
        Graphics.SetRenderTarget(splatControl);
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, textureWidth, textureHeight, 0);
        Vector2 center = uv * textureSize;
        Graphics.DrawTexture(new Rect(center - (Vector2)textureSize * brushSize , 2 * (Vector2)textureSize * brushSize), Texture2D.whiteTexture, material); ;
        
        GL.PopMatrix();
        Graphics.SetRenderTarget(Camera.main.targetTexture);
    }
    private void OnGUI()
    {
        if (debugEnalble)
        {
            debugMaterial.SetColor("_Color", debugColor);
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, Screen.width, Screen.height, 0);
            Graphics.DrawTexture(new Rect(0, 0, 300, 300), splatControl, debugMaterial);
            GL.PopMatrix();
        }
    }

}

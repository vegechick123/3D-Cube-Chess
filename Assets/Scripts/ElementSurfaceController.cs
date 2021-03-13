using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSurfaceController : MonoBehaviour
{
    Material elementSurfaceMaterial;
    RenderTexture splatControl;
    int textureHeight = 512;
    int textureWidth = 512;
    Vector2Int textureSize { get { return new Vector2Int(textureWidth,textureHeight); } }
    public float brushSize=64;
    public Shader brushShader;
    public Color color;
    [Range(0,1)]
    public float threshold;
    Material brushMaterial;
    private void Awake()
    {
        elementSurfaceMaterial = GetComponent<MeshRenderer>().material;
        splatControl = new RenderTexture(textureWidth,textureHeight,0);
        splatControl.Create();
        elementSurfaceMaterial.SetTexture("_Control", splatControl);
        RenderTexture.active = splatControl;
        GL.Clear(true, true, new Color(1,0,0,0));
        RenderTexture.active = null;
        brushMaterial = new Material(brushShader);
        
        //Graphics.SetRenderTarget(splatControl);
        //Graphics.DrawTexture(new Rect(0, 0, 100, 100), brushTexture);
    }
    private void Update()
    {

        //Paint(new Vector2(0.5f, 0.5f), color,64);
    }
    public void Paint(Vector2 uv, Color color, float brushSize, Texture2D brushTexture)
    {
        brushMaterial.SetTexture("_MainTex", brushTexture);
        brushMaterial.SetFloat("_Threshold", threshold);
        brushMaterial.SetColor("_Color",color);
        Graphics.SetRenderTarget(splatControl);
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, textureWidth, textureHeight, 0);
        Vector2 center = uv * textureSize;
        Graphics.DrawTexture(new Rect(center - (Vector2)textureSize * brushSize , 2 * (Vector2)textureSize * brushSize), brushTexture, brushMaterial); ;
        
        GL.PopMatrix();
        Graphics.SetRenderTarget(Camera.main.targetTexture);
    }
    private void OnGUI()
    {
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, Screen.width, Screen.height, 0);
        Graphics.DrawTexture(new Rect(0, 0, 100, 100), splatControl);
        GL.PopMatrix();
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementSurface
{
    public class ElementGrid : SingletonMonoBehaviour<ElementGrid>
    {
        public Texture2D texture;
        public ElementQuad[,] grid;
        public Vector2Int size;
        public Shader brushShader;
        SurfaceRenderTexturePainter painter;
        protected override void Awake()
        {
            base.Awake();
            Create();
        }
        public void Create()
        {
            ElementQuad.tex = texture;
            painter = ElementQuad.painter = GetComponent<SurfaceRenderTexturePainter>();
            ElementQuad.brushShader = brushShader;
            grid = new ElementQuad[size.x, size.y];
            int length = size.x;
            ElementQuad.size = 1f / length ;
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Vector2 center = new Vector2(j + 0.5f, i + 0.5f) / length;
                    grid[i, j].Init(center);
                }
        }
        private void Update()
        {
            painter.ClearTexture();
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j].FrameUpdate();
                }
        }
        public void SetType(Vector2Int location, int type)
        {
            grid[location.x, location.y].activeType = type;
        }
        public static int ElementTotype(FloorStateEnum element)
        {
            switch (element)
            {
                case FloorStateEnum.FireCover:
                    return 1;
                case FloorStateEnum.WaterCover:
                    return 2;
                case FloorStateEnum.OilCover:
                    return 3;
                case FloorStateEnum.NoneCover:
                    return 0;

            }
            throw new Exception();

        }
    }

    public struct ElementQuad
    {
        int m_activeType;
        Material brushMaterial;
        float time;

        public Vector2 center;
        public float changeTime;
        public int activeType { get { return m_activeType; } set { ChangeType(value); } }
        public static SurfaceRenderTexturePainter painter;
        public static Texture2D tex;
        public static float size;
        public static Shader brushShader;
        public static Color[] typeColor = new Color[] { new Color(1, 0, 0, 0), new Color(0, 1, 0, 0), new Color(0, 0, 1, 0), new Color(0, 0, 0, 1) };
        public void Init(Vector2 center)
        {
            this.center = center;
            brushMaterial = new Material(brushShader);
            brushMaterial.SetTexture("_MainTex", tex);
            ChangeType(0);
        }
        void ChangeType(int newType,float initialTime = 0f)
        {
            time = initialTime;
            brushMaterial.SetColor("_OldColor", typeColor[activeType]);
            m_activeType = newType;
            brushMaterial.SetColor("_NewColor", typeColor[activeType]);

        }
        public void FrameUpdate()
        {
            time += Time.deltaTime;
            brushMaterial.SetFloat("_Lerp", GetLerp(time));
            painter.Paint(center, size, brushMaterial,tex);
        }
        public float GetLerp(float t)
        {

            return 1-Mathf.Exp(-t);
        }
    }
}

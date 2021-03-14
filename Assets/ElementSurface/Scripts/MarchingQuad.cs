using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementSurface
{
    public class MarchingQuad : SingletonMonoBehaviour<MarchingQuad>
    {
        public Texture2D[] texture = new Texture2D[14];
        public BigGrid grid;
        public Vector2Int size;
        public Shader brushShader;
        protected override void Awake()
        {
            base.Awake();
            Create();
        }
        public void Create()
        {
            SmallGrid.tex = texture;
            SmallGrid.painter = GetComponent<SurfaceRenderTextureController>();
            SmallGrid.brushShader = brushShader;
            grid = new BigGrid();
            grid.Init(size);
            grid.UpdateTexture();
        }
        private void Update()
        {
            SmallGrid.painter.ClearTexture();
            grid.FrameUpdate();
        }
        public void SetType(Vector2Int location, int type)
        {
            grid.grid[location.x, location.y].activeType = type;
        }
        public void SetTypeAndUpdateTexture(Vector2Int location, int type)
        {
            SetType(location, type);
            grid.UpdateTexture();
        }
        public void UpdateAllTexture()
        {
            grid.UpdateTexture();
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
    public class BigGrid
    {
        public MiddleGrid[,] grid;
        public int length;
        public void Init(Vector2Int size)
        {
            length = 3 * size.x;
            SmallGrid.size = 1.0f / length;
            grid = new MiddleGrid[size.x, size.y];
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Vector2 center = new Vector2(3 * j + 1.5f, 3 * i + 1.5f) / length;
                    grid[i, j] = new MiddleGrid(center);
                    grid[i, j].activeType = 0;
                }

        }
        public MiddleGrid SaveGetGrid(int i, int j)
        {
            if (i >= 0 && j >= 0 && i < grid.GetLength(0) && j < grid.GetLength(1))
                return grid[i, j];
            else
                return null;
        }
        public int CyclicShift(int value, int offset = 1, int size = 4)
        {
            return ((value << offset) | (value >> (size - offset))) & ((1 << size) - 1);
        }
        int[] GetCode(int i, int j)
        {
            int activeType = grid[i, j].activeType;
            if (activeType == 0)
                return new int[] { 0, 0, 0, 0 };
            int[] dx = { 1, 0, -1, 0 };
            int[] dy = { 0, 1, 0, -1 };
            int[] ans = new int[4];
            for (int ki = 0; ki < 4; ki++)
            {
                int res = 8;
                int currentBegin = ki;

                int nowi = i, nowj = j;
                for (int kj = 1; kj < 4; kj++)
                {
                    nowi += dy[currentBegin];
                    nowj += dx[currentBegin];
                    if (SaveGetGrid(nowi, nowj)?.activeType == activeType)
                    {
                        res |= (1 << (3 - kj));
                    }
                    currentBegin++;
                    currentBegin %= 4;
                }
                ans[ki] = res;

            }
            return ans;
        }
        public void UpdateTexture()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    int[] code = GetCode(i, j);
                    grid[i, j].UpdateTexture(code);
                }
            }
        }
        public void FrameUpdate()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                    grid[i, j].FrameUpdate();
        }
    }
    public class MiddleGrid
    {
        public int activeType;
        SmallGrid centerGrid = new SmallGrid();
        SmallGrid[] edgeGrid = new SmallGrid[4];
        SmallGrid[] cornerGrid = new SmallGrid[4];
        public MiddleGrid(Vector2 center)
        {
            centerGrid.type = 0;
            for (int i = 0; i < 4; i++)
                edgeGrid[i].type = 1;
            for (int i = 0; i < 4; i++)
                cornerGrid[i].type = 2;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    SmallGrid copy = this[i, j];
                    copy.Create(this, center, i, j);
                    this[i, j] = copy;
                }
            }

        }
        public SmallGrid this[int i, int j]
        {
            get
            {
                if (j == 1 && i == 1)
                {
                    return centerGrid;
                }
                else if (j == 2 && i == 1)
                {
                    return edgeGrid[0];
                }
                else if (j == 1 && i == 2)
                {
                    return edgeGrid[1];
                }
                else if (j == 0 && i == 1)
                {
                    return edgeGrid[2];
                }
                else if (j == 1 && i == 0)
                {
                    return edgeGrid[3];
                }
                else if (j == 2 && i == 2)
                {
                    return cornerGrid[0];
                }
                else if (j == 0 && i == 2)
                {
                    return cornerGrid[1];
                }
                else if (j == 0 && i == 0)
                {
                    return cornerGrid[2];
                }
                else if (j == 2 && i == 0)
                {
                    return cornerGrid[3];
                }
                else
                    throw new Exception();
            }
            set
            {
                if (j == 1 && i == 1)
                {
                    centerGrid = value;
                }
                else if (j == 2 && i == 1)
                {
                    edgeGrid[0] = value;
                }
                else if (j == 1 && i == 2)
                {
                    edgeGrid[1] = value;
                }
                else if (j == 0 && i == 1)
                {
                    edgeGrid[2] = value;
                }
                else if (j == 1 && i == 0)
                {
                    edgeGrid[3] = value;
                }
                else if (j == 2 && i == 2)
                {
                    cornerGrid[0] = value;
                }
                else if (j == 0 && i == 2)
                {
                    cornerGrid[1] = value;
                }
                else if (j == 0 && i == 0)
                {
                    cornerGrid[2] = value;
                }
                else if (j == 2 && i == 0)
                {
                    cornerGrid[3] = value;
                }
                else
                    throw new Exception();
            }

        }
        public void UpdateSpeceficRotation(int code, int rot)
        {
            centerGrid.UpdateTexture(code, 0);
            edgeGrid[rot].UpdateTexture(code, rot);
            cornerGrid[rot].UpdateTexture(code, rot);
        }
        public void UpdateTexture(int[] code)
        {
            centerGrid.UpdateTexture(code[0], 0);
            for (int i = 0; i < 4; i++)
            {
                edgeGrid[i].UpdateTexture(code[i], i);
                cornerGrid[i].UpdateTexture(code[i], i);
            }
        }
        public void FrameUpdate()
        {
            centerGrid.FrameUpdate();
            for (int i = 0; i < 4; i++)
                edgeGrid[i].FrameUpdate();
            for (int i = 0; i < 4; i++)
                cornerGrid[i].FrameUpdate();
        }
    }
    public struct SmallGrid
    {
        public MiddleGrid parent;
        public static SurfaceRenderTextureController painter;
        public static Texture2D[] tex;
        public static float size;
        public static Shader brushShader;
        public static Color[] typeColor = new Color[] { new Color(1, 0, 0, 0), new Color(0, 1, 0, 0), new Color(0, 0, 1, 0), new Color(0, 0, 0, 1) };
        public Vector2 center;
        public int type;
        public int texType;
        public float changeTime; 
        Material brushMaterial;
        float time;
        //public Color color=Color.red;
        public void Create(MiddleGrid parent, Vector2 center, int i, int j)
        {
            this.center = center + size * new Vector2(j - 1, i - 1);
            this.parent = parent;
            brushMaterial = new Material(brushShader);
        }
        public int GetTextureType(int code)
        {
            if ((code & 8) == 0)
                return 0;
            else
            {
                switch (type)
                {
                    case 0:
                        return 1;
                    case 1:
                        if ((code & 4) != 0)
                        {
                            return 1;
                        }
                        else
                        {
                            return 2;
                        }
                    case 2:
                        switch (code)
                        {
                            case 0b1010:
                            case 0b1000:
                                return 6;
                            case 0b1001:
                            case 0b1011:
                                return 2;
                            case 0b1100:
                            case 0b1110:
                                return 3;
                            case 0b1101:
                                return 10;
                            case 0b1111:
                                return 1;
                            default:
                                throw new Exception();
                        }
                    default:
                        throw new Exception();
                }
            }
        }
        public static int RotateTextureType(int texType, int rotation)
        {
            if (texType <= 1)
            {
                return texType;
            }
            else if (texType <= 5)
            {
                return ((texType - 2) + rotation) % 4 + 2;
            }
            else if (texType <= 9)
            {
                return ((texType - 6) + rotation) % 4 + 6;
            }
            else if (texType <= 13)
            {
                return ((texType - 10) + rotation) % 4 + 10;
            }
            else
                throw new Exception();
        }
        public int GetTextureType(int code, int rotation)
        {
            int t = GetTextureType(code);
            int res = RotateTextureType(t, rotation);
            return res;
        }
        public void UpdateTexture(int code, int rotation)
        {
            time = 0;
            brushMaterial.SetTexture("_Last", tex[texType]);
            texType = GetTextureType(code, rotation);
            brushMaterial.SetTexture("_New", tex[1]);
            brushMaterial.SetColor("_Color", typeColor[parent.activeType]);
        }
        public void FrameUpdate()
        {
            time += Time.deltaTime;
            time = Mathf.Min(time, 1);
            brushMaterial.SetColor("_Lerp", typeColor[parent.activeType]);
            painter.Paint(center, size, brushMaterial);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class GridAttribute : PropertyAttribute
    {
        public float m_cellSize;
        public int m_gridCount;

        public GridAttribute(int gridCount = 5, float cellSize = 20f)
        {
            m_cellSize = cellSize;
            m_gridCount = gridCount;
        }
    }

    [Serializable]
    public class GridList<T>
    {
        public List<GridContent<T>> m_gridContent;
    }

    [Serializable]
    public struct GridContent<T>
    {
        public T m_content;
        public Vector2Int m_position;
    }
}


using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GridAttribute
{
    [CustomPropertyDrawer(typeof(GridAttribute))]
    public class GridDrawer : PropertyDrawer
    {
        private int m_selectedRow = -1;
        private int m_selectedCol = -1;
        private SerializedProperty m_contentList;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.type != typeof(GridList<>).Name)
            {
                EditorGUI.LabelField(position, "GridAttribute only works with GridList<T>.");
                return;
            }

            Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, label.text);
            position.y += EditorGUIUtility.singleLineHeight;

            GridAttribute gridAttribute = (GridAttribute)attribute;
            float cellSize = gridAttribute.m_cellSize;
            int gridSize = gridAttribute.m_gridCount;

            m_contentList = property.FindPropertyRelative("m_gridContent");
            
            Rect gridRect = new Rect(position.x, position.y, position.width, cellSize * gridSize);
            position.y += cellSize * gridSize;

            // Draw grid and select cell
            for (int row = 0; row < gridSize; row++)
            {
                for (int column = 0; column < gridSize; column++)
                {
                    Rect cellRect = new Rect(
                        gridRect.x + column * cellSize,
                        gridRect.y + row * cellSize,
                        cellSize,
                        cellSize
                    );

                    string btnText;

                    // Ajouter une indication de la cellule s�lectionn�e
                    if (row == m_selectedRow && column == m_selectedCol)
                    {
                        btnText = "X";
                    }
                    else if (CellHasContent(row, column, out SerializedProperty ct))
                    {
                        btnText = "0";
                    }
                    else
                        btnText = "";

                    // Afficher le bouton et g�rer la s�lection
                    if (GUI.Button(cellRect, btnText, new GUIStyle(GUI.skin.button)))
                    {
                        m_selectedRow = row;
                        m_selectedCol = column;
                    }
                }
            }

            if (m_contentList.arraySize == 0 || !CellHasContent(m_selectedRow, m_selectedCol, out SerializedProperty content))
            {
                Rect editRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                position.y += EditorGUIUtility.singleLineHeight;

                if (m_selectedRow >= 0 && m_selectedCol >= 0 && GUI.Button(editRect, "Edit That Shit", new GUIStyle(GUI.skin.button)))
                {
                    AddElement();
                }
            }
            else
            {
                SerializedProperty valueProperty = content.FindPropertyRelative("m_content");
                float propertyHeight = EditorGUI.GetPropertyHeight(valueProperty, true);

                Rect contentRect = new Rect(position.x, position.y, position.width, propertyHeight);
                
                EditorGUI.PropertyField(contentRect, valueProperty, new GUIContent(valueProperty.type.ToString()), true);
                position.y += propertyHeight;
            }

            
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            GridAttribute gridAttribute = (GridAttribute)attribute;
            float cellSize = gridAttribute.m_cellSize;
            int gridSize = gridAttribute.m_gridCount;

            float height = EditorGUIUtility.singleLineHeight; // Pour le label
            height += cellSize * gridSize; // Hauteur de la grille

            // Ajouter la hauteur pour le bouton ou le champ s�lectionn�
            if (m_selectedRow >= 0 && m_selectedCol >= 0)
            {
                if (CellHasContent(m_selectedRow, m_selectedCol, out SerializedProperty content))
                {
                    float structHeight = 0;

                    SerializedProperty iterator = content.Copy();
                    SerializedProperty endProperty = iterator.GetEndProperty();

                    while (iterator.NextVisible(true) && !SerializedProperty.EqualContents(iterator, endProperty))
                    {
                        structHeight += EditorGUI.GetPropertyHeight(iterator, true);
                    }

                    height += structHeight;
                }
                else if (m_selectedRow >= 0 && m_selectedCol >= 0)
                {
                    height += EditorGUIUtility.singleLineHeight; // Pour le bouton 
                }
            }


            return height;
        }

        private bool CellHasContent(int row, int column, out SerializedProperty content)
        {
            Vector2 selectedTile = new Vector2(column, row);
            for (int i = 0; i < m_contentList.arraySize; i++)
            {
                SerializedProperty data = m_contentList.GetArrayElementAtIndex(i).FindPropertyRelative("m_position");

                if (data.vector2Value == selectedTile)
                {
                    content = m_contentList.GetArrayElementAtIndex(i);
                    return true;
                }
            }

            content = null;
            return false;
        }

        private void AddElement()
        {
            m_contentList.InsertArrayElementAtIndex(m_contentList.arraySize);
            //Debug.Log(m_contentList.arraySize);
            SerializedProperty element = m_contentList.GetArrayElementAtIndex(m_contentList.arraySize - 1);

            SerializedProperty valueProperty = element.FindPropertyRelative("m_value");
            SerializedProperty positionProperty = element.FindPropertyRelative("m_position");

            // Initialiser les valeurs
            positionProperty.vector2Value = new Vector2(m_selectedCol, m_selectedRow);
        }
    }

}
using UnityEditor;
using UnityEngine;
using Grid = Enemies.Grid;

namespace Editor
{
    [CustomEditor(typeof(Grid))]
    public class GridComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Create Grid"))
            {
                Grid myComponent = (Grid) target;
                myComponent.CreateGrid();
            }
        }
    }
}
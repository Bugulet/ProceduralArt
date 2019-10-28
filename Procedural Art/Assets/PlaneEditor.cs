using UnityEngine;

using UnityEditor;



namespace Demo {

    [CanEditMultipleObjects]
    [CustomEditor(typeof(GenerationPlane))]
    public class PlaneEditor : Editor
    {

        bool customHeight = false;

        public override void OnInspectorGUI()
        {

            GenerationPlane targetCleaner = (GenerationPlane)target;

            if (targetCleaner.GenerateWhileEditing == true && GUILayout.Button("Generate House"))
            {
                targetCleaner.Generate();
            }


            customHeight = EditorGUILayout.Toggle("Custom height", customHeight);

            if (customHeight) {
                targetCleaner.height = (int)EditorGUILayout.Slider(targetCleaner.height, 1, 50);

            }
            else
            {
                targetCleaner.height = (targetCleaner.width + targetCleaner.depth) / 2 + (targetCleaner.TriggerObject.GetComponent<BuildingParameters>().Rand.Next(-1, 1));
            }



            DrawDefaultInspector();
        }
    }
}
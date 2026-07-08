using Sebanne.GripFit;
using UnityEditor;
using UnityEngine;

namespace Sebanne.GripFit.Editor
{
    [CustomEditor(typeof(GripFitOffset))]
    internal sealed class GripFitOffsetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var offset = (GripFitOffset)target;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("記録は「Grip Fit 記録」ウィンドウから行います。", EditorStyles.miniLabel);
            if (GUILayout.Button("記録ウィンドウを開く"))
            {
                GripFitRecorderWindow.Open();
            }

            if (offset.HasRecordedValue)
            {
                EditorGUILayout.LabelField(
                    $"確定済み: pos {offset.OffsetPosition:F4} / rot {offset.OffsetRotation.eulerAngles:F1}",
                    EditorStyles.miniLabel);
            }
            else
            {
                EditorGUILayout.LabelField("未記録", EditorStyles.miniLabel);
            }

            EditorGUILayout.EndVertical();
        }
    }
}

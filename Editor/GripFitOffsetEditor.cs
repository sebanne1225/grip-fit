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

            using (new EditorGUI.DisabledScope(!EditorApplication.isPlaying))
            {
                if (GUILayout.Button("現在の姿勢を記録"))
                {
                    GripFitRecorder.RecordCurrentPose(offset);
                }
            }

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("Play Mode 中のみ記録できます", EditorStyles.miniLabel);
            }
            else if (GripFitRecorder.HasPendingRecord(offset))
            {
                EditorGUILayout.LabelField("記録済み（Play Mode 終了時に確定されます）", EditorStyles.miniLabel);
            }

            if (offset.HasRecordedValue)
            {
                EditorGUILayout.LabelField(
                    $"確定済み: pos {offset.OffsetPosition:F4} / rot {offset.OffsetRotation.eulerAngles:F1}",
                    EditorStyles.miniLabel);
            }

            EditorGUILayout.EndVertical();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace BurstImageProcessing
{
    [CustomEditor(typeof(EffectComposer))]
    public class ComposedEffectEditor : Editor
    {
        SerializedProperty redOperator;
        SerializedProperty redComparator;
        SerializedProperty redOperand;

        SerializedProperty blueOperator;
        SerializedProperty blueComparator;
        SerializedProperty blueOperand;

        EffectComposer m_Composer;

        void OnEnable()
        {
            m_Composer = (EffectComposer)target;
            redOperator = serializedObject.FindProperty("m_RedOperator");
            redComparator = serializedObject.FindProperty("m_RedComparator");
            redOperand = serializedObject.FindProperty("m_RedOperand");
            blueOperator = serializedObject.FindProperty("m_BlueOperator");
            blueComparator = serializedObject.FindProperty("m_BlueComparator");
            blueOperand = serializedObject.FindProperty("m_BlueOperand");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Per-Channel Effect Components", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(redOperator);
            EditorGUILayout.PropertyField(redComparator);
            EditorGUILayout.PropertyField(redOperand);
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(blueOperator);
            EditorGUILayout.PropertyField(blueComparator);
            EditorGUILayout.PropertyField(blueOperand);
            EditorGUILayout.Separator();
        }
    }

    
}

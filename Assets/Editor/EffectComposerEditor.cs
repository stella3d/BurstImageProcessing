using UnityEditor;
using UnityEngine;

namespace BurstImageProcessing
{
    [CustomEditor(typeof(EffectComposer))]
    public class ComposedEffectEditor : Editor
    {
        SerializedProperty enableRed;
        SerializedProperty redOperator;
        SerializedProperty redComparator;
        SerializedProperty redOperand;

        SerializedProperty enableGreen;
        SerializedProperty greenOperator;
        SerializedProperty greenComparator;
        SerializedProperty greenOperand;

        SerializedProperty enableBlue;
        SerializedProperty blueOperator;
        SerializedProperty blueComparator;
        SerializedProperty blueOperand;

        SerializedProperty colorThreshold;

        SerializedProperty additionValue;

        void OnEnable()
        {
            enableRed = serializedObject.FindProperty("m_EnableRed");
            redOperator = serializedObject.FindProperty("m_RedOperator");
            redComparator = serializedObject.FindProperty("m_RedComparator");
            redOperand = serializedObject.FindProperty("m_RedOperand");
            enableGreen = serializedObject.FindProperty("m_EnableGreen");
            greenOperator = serializedObject.FindProperty("m_GreenOperator");
            greenComparator = serializedObject.FindProperty("m_GreenComparator");
            greenOperand = serializedObject.FindProperty("m_GreenOperand");
            enableBlue = serializedObject.FindProperty("m_EnableBlue");
            blueOperator = serializedObject.FindProperty("m_BlueOperator");
            blueComparator = serializedObject.FindProperty("m_BlueComparator");
            blueOperand = serializedObject.FindProperty("m_BlueOperand");

            colorThreshold = serializedObject.FindProperty("m_ColorThreshold");

            additionValue = serializedObject.FindProperty("m_AdditionValue");
        }

        const string k_PerChannelHelp = "For each color channel, there are 3 things to select:\n" +
                "the 'Operator' defines the mathematical or bitwise operation\n" +
                "the 'Comparator' defines how the pixel value is compared against the Operand\n" +
                "the 'Operand' defines what the Operator runs against to effect the pixel's value";

        static readonly GUIContent k_ColorThresholdLabel = new GUIContent("Color Threshold");

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Per-Channel Effect Components", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(k_PerChannelHelp, MessageType.Info);

            Channel("Process Red Channel", enableRed, redOperator, redComparator, redOperand);
            Channel("Process Green Channel", enableGreen, greenOperator, greenComparator, greenOperand);
            Channel("Process Blue Channel", enableBlue, blueOperator, blueComparator, blueOperand);

            EditorGUILayout.Space();
            colorThreshold.colorValue = EditorGUILayout.ColorField(k_ColorThresholdLabel, colorThreshold.colorValue, false, false, false);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(additionValue);

            serializedObject.ApplyModifiedProperties();
        }

        void Channel(string label, SerializedProperty enable, SerializedProperty op, SerializedProperty comparator, SerializedProperty operand)
        {
            enable.boolValue = EditorGUILayout.BeginToggleGroup(label, enable.boolValue);
            EditorGUILayout.PropertyField(op);
            EditorGUILayout.PropertyField(comparator);
            EditorGUILayout.PropertyField(operand);
            EditorGUILayout.EndToggleGroup();
            EditorGUILayout.Separator();
        }
    }

    
}

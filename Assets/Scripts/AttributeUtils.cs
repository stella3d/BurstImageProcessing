using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace BurstImageProcessing.Utils
{
    public static class AttributeUtils
    {

        [MenuItem("BIP/Find All Composer Inputs")]
        public static Dictionary<ComposerInputsAttribute, Type> FindAllComposerInputs()
        {
            var ass = Assembly.GetExecutingAssembly();
            var types = GetTypesWithComposerInputsAttribute(ass);

            foreach (var kvp in types)
                Debug.Log(kvp.Key + " : " + kvp.Value);

            return types;
        }

        static Dictionary<ComposerInputsAttribute, Type> GetTypesWithComposerInputsAttribute(Assembly assembly)
        {
            var dict = new Dictionary<ComposerInputsAttribute, Type>();
            foreach (Type type in assembly.GetTypes())
            {
                var inputs = type.GetCustomAttributes(typeof(ComposerInputsAttribute), true);
                if (inputs.Length > 0)
                    foreach (var input in inputs)
                        if (input is ComposerInputsAttribute)
                            dict.Add((ComposerInputsAttribute)input, type);
            }

            return dict;
        }

    }
}

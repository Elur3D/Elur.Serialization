using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

using FullSerializer;

namespace Serialization
{
    public static class JSON
    {
        private static readonly fsSerializer _serializer = new fsSerializer();

        public static string Serialize(Type type, object value, bool pretty = false)
        {
            fsData data;
            _serializer.TrySerialize(type, value, out data).AssertSuccessWithoutWarnings();

            if (pretty)
            {
                return fsJsonPrinter.PrettyJson(data);
            }
            else
            {
                return fsJsonPrinter.CompressedJson(data);
            }
        }

        public static string Serialize<T>(T value, bool pretty = false)
        {
            fsData data;
            _serializer.TrySerialize(value, out data).AssertSuccessWithoutWarnings();
            if (pretty)
            {
                return fsJsonPrinter.PrettyJson(data);
            }
            else
            {
                return fsJsonPrinter.CompressedJson(data);
            }
        }

        public static object Deserialize(Type type, string serializedState)
        {
            fsData data = fsJsonParser.Parse(serializedState);
            object deserialized = null;
            _serializer.TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();

            return deserialized;
        }

        public static T Deserialize<T>(string serializedState)
        {
            fsData data = fsJsonParser.Parse(serializedState);
            T deserialized = default(T);
            _serializer.TryDeserialize<T>(data, ref deserialized).AssertSuccessWithoutWarnings();
            return deserialized;
        }


        public static T Deserialize<T>(this TextAsset textAsset)
        {
            return JSON.Deserialize<T>(textAsset.text);
        }

        public static void Serialize<T>(this TextAsset textAsset, T fsm)
        {
            #if UNITY_EDITOR
            var path = AssetDatabase.GetAssetPath(textAsset);
            var dir = Application.dataPath.Replace("Assets", "");
            path = Path.Combine(dir, path);

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(JSON.Serialize<T>(fsm, true));
                writer.Close();
            }
            #endif
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (!Instance)
            {
                Instance = this as T;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public struct Flag
    {
        public bool State { get; private set; }

        public void Set()
        {
            State = true;
        }

        public bool Pop()
        {
            bool app = State;
            State = false;
            return app;
        }
    }

    [Serializable]
    public struct Range
    {
        [field: SerializeField] public float Min { get; private set; }
        [field: SerializeField] public float Max { get; private set; }

        public Range(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float GetRandom()
        {
            return UnityEngine.Random.Range(Min, Max);
        }
    }

    public static class Extensions
    {
        #region Vector2 Extensions

        public static Vector2 WithX(this Vector2 v, float x)
        {
            return new Vector2(x, v.y);
        }

        public static Vector2 WithY(this Vector2 v, float y)
        {
            return new Vector2(v.x, y);
        }

        public static Vector2 SnapToGrid(this Vector2 v, float gridSize)
        {
            v /= gridSize;
            v.x = Mathf.RoundToInt(v.x);
            v.y = Mathf.RoundToInt(v.y);
            return v * gridSize;
        }

        public static Vector2 SnapToGrid(this Vector2 v, Vector2 gridSize)
        {
            v.x = Mathf.RoundToInt(v.x / gridSize.x) * gridSize.x;
            v.y = Mathf.RoundToInt(v.y / gridSize.y) * gridSize.y;
            return v;
        }

        #endregion

        #region Vector3 Extensions

        public static Vector3 WithX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        public static Vector3 WithY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }

        public static Vector3 WithZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        public static Vector3 SnapToGrid(this Vector3 v, float gridSize)
        {
            v /= gridSize;
            v.x = Mathf.RoundToInt(v.x);
            v.y = Mathf.RoundToInt(v.y);
            v.z = Mathf.RoundToInt(v.z);
            return v * gridSize;
        }

        public static Vector3 SnapToGrid(this Vector3 v, Vector3 gridSize)
        {
            v.x = Mathf.RoundToInt(v.x / gridSize.x) * gridSize.x;
            v.y = Mathf.RoundToInt(v.y / gridSize.y) * gridSize.y;
            v.z = Mathf.RoundToInt(v.z / gridSize.z) * gridSize.z;
            return v;
        }

        #endregion

        #region Quaternion Extensions

        public static Quaternion SnapToStandardAngles(this Quaternion rot)
        {
            var r = rot.eulerAngles;
            r.x = Helpers.FindNearestStandardAngle(r.x);
            r.y = Helpers.FindNearestStandardAngle(r.y);
            r.z = Helpers.FindNearestStandardAngle(r.z);
            return Quaternion.Euler(r);
        }

        #endregion
    }

    public static class Helpers
    {
        public static bool IsInLayerMask(int layer, LayerMask layermask) { return layermask == (layermask | (1 << layer)); }

        public static int CircularClamp(int value, int min, int max)
        {
            int range = Mathf.Abs(max) - Mathf.Abs(min) + 1;
            if (value < min) return max + 1 - (min - value) % range;
            if (value > max) return min - 1 + (value - max) % range;
            return value;
        }

        public static string ConvertCamelToSnake(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([a-z])([A-Z])", "$1_$2").ToLower();
        }

        public static float MapRange01(float value, float min, float max)
        {
            return MapRange(value, min, max, 0f, 1f);
        }

        public static float MapRange(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
        }

        public static float FindNearestStandardAngle(float angle)
        {
            angle = angle % 360f;           // Normalize to (-360, 360)
            if (angle < 0) angle += 360f;   // Normalize to [0, 360)

            int nearestMultipleOf90 = Mathf.RoundToInt(angle / 90f) * 90;

            // Ensure result is in [0, 360)
            return nearestMultipleOf90 % 360f;
        }
    }

}

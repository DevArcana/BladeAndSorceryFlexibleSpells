using UnityEngine;

namespace FlexibleSpells.Operators.Math
{
    public class Vector : Operator
    {
        public Vector3 Apply(int a, int b, int c) => new Vector3(a, b, c);
        public Vector3 Apply(int a, int b) => new Vector3(a, b);
        public Vector3 Apply(float a, float b, float c) => new Vector3(a, b, c);
        public Vector3 Apply(float a, float b) => new Vector3(a, b);
    }
}
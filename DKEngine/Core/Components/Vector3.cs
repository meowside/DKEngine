﻿namespace DKEngine.Core.Components
{
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()

    /// <summary>
    /// Three-dimensional vector
    /// </summary>
    public struct Vector3
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        /// <summary>
        /// The X vector
        /// </summary>
        public float X;

        /// <summary>
        /// The Y vector
        /// </summary>
        public float Y;

        /// <summary>
        /// The Z vector
        /// </summary>
        public float Z;

        public Vector3(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Vector3 operator -(Vector3 left, float right)
        {
            return new Vector3(left.X - right, left.Y - right, left.Z - right);
        }

        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Vector3 operator +(Vector3 left, float right)
        {
            return new Vector3(left.X + right, left.Y + right, left.Z + right);
        }

        public static Vector3 operator *(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        public static Vector3 operator *(Vector3 left, float right)
        {
            return new Vector3(left.X * right, left.Y * right, left.Z * right);
        }

        public static Vector3 operator /(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        }

        public static Vector3 operator /(Vector3 left, float right)
        {
            return new Vector3(left.X / right, left.Y / right, left.Z / right);
        }

        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }

        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return left.X != right.X || left.Y != right.Y || left.Z != right.Z;
        }

        public Vector3 Add(Vector3 Value)
        {
            return this + Value;
        }

        public Vector3 Add(float X, float Y, float Z)
        {
            return this + new Vector3(X, Y, Z);
        }

        public Vector3 Add(float Value)
        {
            return this + Value;
        }

        public Vector3 Decrease(Vector3 Value)
        {
            return this - Value;
        }

        public Vector3 Decrease(float X, float Y, float Z)
        {
            return this - new Vector3(X, Y, Z);
        }

        public Vector3 Decrease(float Value)
        {
            return this - Value;
        }

        public Vector3 Multiply(Vector3 Value)
        {
            return this * Value;
        }

        public Vector3 Multiply(float X, float Y, float Z)
        {
            return this * new Vector3(X, Y, Z);
        }

        public Vector3 Multiply(float Value)
        {
            return this * Value;
        }

        public Vector3 Divide(Vector3 Value)
        {
            return this / Value;
        }

        public Vector3 Divide(float X, float Y, float Z)
        {
            return this / new Vector3(X, Y, Z);
        }

        public Vector3 Divide(float Value)
        {
            return this / Value;
        }

        private static Vector3 _zero = new Vector3(0, 0, 0);

        public static Vector3 Zero
        {
            get { return _zero; }
        }
    }
}
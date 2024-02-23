using System;

namespace Common
{
    public static class MathUtil
    {
        public enum WindingOrder
        {
            Collinear,
            Clockwise,
            Counterclockwise
        }

        public const float Pi = 3.14159265359f;
        public const float TwoPi = 6.28318530718f;
        public const float PiOverFour = 0.78539816339f;
        public const float PiOverTwo = 1.57079632679f;

        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;

            return value > max ? max : value;
        }

        public static bool NearlyEquals(float lhs, float rhs)
        {
            return Math.Abs(lhs - rhs) < 0.001f;
        }

        public static WindingOrder GetWindingOrder(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            var val = (p2.Y - p1.Y) * (p3.X - p2.X) -
                      (p2.X - p1.X) * (p3.Y - p2.Y);

            if (Math.Abs(val) < 0.00001) return WindingOrder.Collinear;

            return (val > 0)
                ? WindingOrder.Clockwise
                : WindingOrder.Counterclockwise;
        }

        public static double Lerp(double from, double to, double amount)
        {
            return (1 - amount) * from + amount * to;
        }

        public static float Lerp(float from, float to, float amount)
        {
            return (1 - amount) * from + amount * to;
        }

        public static byte Lerp(byte from, byte to, float amount)
        {
            return Lerp(from, to, amount);
        }

        public static bool PointInTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 q)
        {
            var denominator = ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));
            var a = ((p2.Y - p3.Y) * (q.X - p3.X) + (p3.X - p2.X) * (q.Y - p3.Y)) / denominator;
            var b = ((p3.Y - p1.Y) * (q.X - p3.X) + (p1.X - p3.X) * (q.Y - p3.Y)) / denominator;
            var c = 1 - a - b;

            return 0 <= a && a <= 1 && 0 <= b && b <= 1 && 0 <= c && c <= 1;
        }

        public static bool RayIntersectsPlane(Ray ray, Plane plane, out Vector3 point)
        {
            float distance;
            if (!RayIntersectsPlane(ray, plane, out distance))
            {
                point = Vector3.Zero;
                return false;
            }
            point = ray.Position + ray.Direction * distance;
            return true;
        }

        public static bool RayIntersectsPlane(Ray ray, Plane plane, out float distance)
        {
            var result1 = Vector3.Dot(plane.Normal, ray.Direction);
            if (NearlyEquals(result1, 0))
            {
                distance = 0.0f;
                return false;
            }
            var result2 = Vector3.Dot(plane.Normal, ray.Position);
            distance = (-plane.D - result2) / result1;
            if (distance >= 0.0)
                return true;

            distance = 0.0f;
            return false;
        }

        public static float? RayIntersectsSphere(Vector2 position, Vector2 direction, Vector2 origo, float radius)
        {
            var result = position - origo;
            //Vector3.Subtract(ref ray.Position, ref sphere.Center, out result);
            float num1 = Vector2.Dot(result, direction);
            float num2 = Vector2.Dot(result, result) - radius*radius;
            if (num2 > 0.0 && num1 > 0.0)
                return null;

            var num3 = num1*num1 - num2;
            if (num3 < 0.0)
                return null;
            var distance = -num1 - (float) Math.Sqrt(num3);
            //if (distance < 0.0)
            //    distance = 0.0f;
                //distance = 0.0f;
            return distance;
        }

        public static float? RayIntersectsSphere1(Vector2 position, Vector2 direction, Vector2 origo, float radius)
        {
            var endPoint = position + direction;

            var h = origo.X;
            var k = origo.Y;
            var r = radius;

            var x0 = position.X;
            var y0 = position.Y;

            var x1 = endPoint.X;
            var y1 = endPoint.Y;

            var x1Mx0 = x1 - x0;
            var y1My0 = y1 - y0;

            var x0Mh = x0 - h;
            var y0Mk = y0 - k;

            var a = x1Mx0*x1Mx0 + y1My0*y1My0;
            var b = 2f*x1Mx0*x0Mh + 2f*y1My0*y0Mk;
            var c = x0Mh*x0Mh + y0Mk*y0Mk - r*r;

            var det = b*b - 4*a*c;
            //if (det > 0)
            {
                var detSqrt = (float)Math.Sqrt(det);
                var t1 = (-b + detSqrt)/(2*a);
                var t2 = (-b - detSqrt)/(2*a);

                if (t1 < 0)
                    return t2;
                
                return t1;

                return Math.Min(t1, t2);
            }

            return -654;
        }

        public static Vector2[] CircleCircleIntersection(Vector2 o1, float r1, Vector2 o2, float r2)
        {
            var d = Vector2.Distance(o1, o2);
            var a = (r1*r1 - r2*r2 + d*d)/(2*d);
            var b = (r2*r2 - r1*r1 + d*d)/(2*d);
            var h = (float)Math.Sqrt(r1*r1 - a*a);
            var q1 = (h*(o2.Y - o1.Y))/d;
            var q2 = (h*(o2.X - o1.X))/d;

            var p1 = o2 + new Vector2(q1, -q2);
            var p2 = o2 - new Vector2(q1, -q2);

            return new[] {p1, p2};
        }

        public static float ToTwoPi(float alpha)
        {
            var c = alpha/TwoPi;
            var result = alpha - ((int) c)*TwoPi;

            if (result >= 0)
                return result;

            return TwoPi + result;
        }

    }
}

using System;
using System.Numerics;

namespace Wolf_Hack.SDK.Mathematics
{
    public static class Vector
    {
        public static double RadTwoDegrees(double Yaw) => Yaw * (180f / Math.PI);

        public static double DegreesTwoRad(double Yaw) => Yaw * (Math.PI / 180f);

        /// <summary>
        /// Метод, вычисляющий расстояние до цели в градусах
        /// </summary>
        /// <param name="ViewAngel">Угол обзора локального игрока</param>
        /// <param name="Dst">Противник</param>
        /// <returns></returns>
        public static float GetFov(Vector3 ViewAngel, Vector3 Dst) => (float)Math.Sqrt(Math.Pow(Dst.X - ViewAngel.X, 2) + Math.Pow(Dst.Y - ViewAngel.Y, 2));

        /// <summary>
        /// Метод, вычисляющий расстояние до цели в градусах
        /// </summary>
        /// <param name="ViewAngel">Угол обзора локального игрока</param>
        /// <param name="Dst">Противник</param>
        /// <returns></returns>
        public static float GetFov(Vector3 ViewAngel, Vector3 Dst, float Distance) => (float)Math.Sqrt(Math.Pow(Math.Sin(DegreesTwoRad(ViewAngel.X - Dst.X)) * Distance, 2) + Math.Pow(Math.Sin(DegreesTwoRad(ViewAngel.Y - Dst.Y)) * Distance, 2));

        /// <summary>
        /// Захват угла
        /// </summary>
        /// <param name="Angles">Угол</param>
        /// <returns></returns>
        public static Vector3 ClampAngle(this Vector3 Angles)
        {
            if (Angles.X > 89.0f)
            {
                Angles.X = 89.0f;
            }

            if (Angles.X < -89.0f)
            {
                Angles.X = -89.0f;
            }

            if (Angles.Y > 180.0f)
            {
                Angles.Y = 180.0f;
            }

            if (Angles.Y < -180.0f)
            {
                Angles.Y = -180.0f;
            }

            Angles.Z = 0;

            return Angles;
        }

        /// <summary>
        /// Нормализация угла
        /// </summary>
        /// <param name="Angle">Угол</param>
        /// <returns></returns>
        public static Vector3 NormalizeAngle(this Vector3 Angle)
        {
            while (Angle.X >= 89.0f)
            {
                Angle.X -= 180f;
            }

            while (Angle.X <= -89.0f)
            {
                Angle.X += 180f;
            }

            while (Angle.Y >= 180.0f)
            {
                Angle.Y -= 360.0f;
            }

            while (Angle.Y <= -180.0f)
            {
                Angle.Y += 360.0f;
            }

            return Angle;
        }

        /// <summary>
        /// Высчитывание угла
        /// </summary>
        /// <param name="Src">От локального игрока</param>
        /// <param name="Dst">До противника</param>
        /// <returns></returns>
        public static Vector3 CalcAngle(this Vector3 Src, Vector3 Dst) => new Vector3()
        {
            X = (float)(Math.Atan((Src - Dst).Z / Math.Sqrt((Src - Dst).X * (Src - Dst).X + (Src - Dst).Y * (Src - Dst).Y)) * 57.295779513082f),
            Y = (float)(Math.Atan2((Src - Dst).Y, (Src - Dst).X) * 57.295779513082f) + 180.0f,
            Z = 0.0f
        };

        /// <summary>
        /// Плавная доводка до нужного угла
        /// </summary>
        /// <param name="Src">От</param>
        /// <param name="Dst">До</param>
        /// <param name="SmoothAmount">Скорость доводки</param>
        /// <returns></returns>
        public static Vector3 SmoothAngle(Vector3 Src, Vector3 Dst, float SmoothAmount) => (Src + NormalizeAngle(Dst - Src) / 100f * SmoothAmount).ClampAngle();
    }
}
using UnityEngine;

namespace ETModel
{
    public class MathHelp
    {

        public static Vector3 GetIntersection(Vector3 lineAStart, Vector3 lineAEnd, Vector3 lineBStart, Vector3 lineBEnd)
        {
            float x1 = lineAStart.x, y1 = lineAStart.y;
            float x2 = lineAEnd.x, y2 = lineAEnd.y;

            float x3 = lineBStart.x, y3 = lineBStart.y;
            float x4 = lineBEnd.x, y4 = lineBEnd.y;

            //equations of the form x=c (two vertical lines)
            if (x1 == x2 && x3 == x4 && x1 == x3)
            {
                return Vector3.zero;
            }

            //equations of the form y=c (two horizontal lines)
            if (y1 == y2 && y3 == y4 && y1 == y3)
            {
                return Vector3.zero;
            }

            //equations of the form x=c (two vertical lines)
            if (x1 == x2 && x3 == x4)
            {
                return Vector3.zero;
            }

            //equations of the form y=c (two horizontal lines)
            if (y1 == y2 && y3 == y4)
            {
                return Vector3.zero;
            }

            float x, y;

            if (x1 == x2)
            {
                float m2 = (y4 - y3) / (x4 - x3);
                float c2 = -m2 * x3 + y3;

                x = x1;
                y = c2 + m2 * x1;
            }
            else if (x3 == x4)
            {
                float m1 = (y2 - y1) / (x2 - x1);
                float c1 = -m1 * x1 + y1;

                x = x3;
                y = c1 + m1 * x3;
            }
            else
            {
                //compute slope of line 1 (m1) and c2
                float m1 = (y2 - y1) / (x2 - x1);
                float c1 = -m1 * x1 + y1;

                //compute slope of line 2 (m2) and c2
                float m2 = (y4 - y3) / (x4 - x3);
                float c2 = -m2 * x3 + y3;

                //solving equations (3) & (4) => x = (c1-c2)/(m2-m1)
                //plugging x value in equation (4) => y = c2 + m2 * x
                x = (c1 - c2) / (m2 - m1);
                y = c2 + m2 * x;

                //          if (!(-m1 * x + y == c1
                //              && -m2 * x + y == c2))
                //          {
                //              return Vector3.zero;
                //          }
            }

            if (IsInsideLine(lineAStart, lineAEnd, x, y) &&
                IsInsideLine(lineBStart, lineBEnd, x, y))
            {
                return new Vector3(x, y, 0);
            }

            //return default null (no intersection)
            return Vector3.zero;

        }

        private static bool IsInsideLine(Vector3 start, Vector3 end, float x, float y)
        {
            return ((x >= start.x && x <= end.x)
                        || (x >= end.x && x <= start.x))
                    && ((y >= start.y && y <= end.y)
                        || (y >= end.y && y <= start.y));
        }
    }
}
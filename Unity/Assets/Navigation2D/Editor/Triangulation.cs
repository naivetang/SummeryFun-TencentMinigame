// Triangulation implementation via ear clipping algorithm (c) noobtuts.com 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Triangulation {
    // triangle helper functions ///////////////////////////////////////////////
    // calculate the determinant for 3 points
    public static float Determinant(Vector2 a, Vector2 b, Vector2 c)
    {
        return (a.x - c.x) * (b.y - c.y) - (b.x - c.x) * (a.y - c.y);
    }

    // is the triangle convex (if traversing anti clockwise)
    public static bool IsConvex(Vector2 a, Vector2 b, Vector2 c)
    {
        return Determinant(a, b, c) >= 0;
    }

    // is the triangle concav (if traversing anti clockwise)
    public static bool IsConcav(Vector2 a, Vector2 b, Vector2 c)
    {
        return Determinant(a, b, c) < 0;
    }

    // calculate triangle area via 1/2 determinant method
    // test:
    //   Vector2 a = new Vector2(0, 0);
    //   Vector2 b = new Vector2(1, 0);
    //   Vector2 c = new Vector2(0, 1);
    //   Triangulation.TriangleArea(a, b, c)) => 0.5
    public static float TriangleArea(Vector2 a, Vector2 b, Vector2 c)
    {
        return Mathf.Abs(Determinant(a, b, c)) / 2f;
    }

    // check if a point is in a triangle
    // the algorithm is very simple, all we do is calculate the triangle's area
    // and then compare it with the areas of (a,b,p) (b,c,p), (a,c,p). if the
    // areas is bigger, then the point is outside of the triangle
    //
    // area = 0.5; area of combinations with p = 0.25+0.125+0.125 = 0.5
    // |\
    // |  \
    // |  p \  
    // |______\
    //
    // area = 0.5; area of combinations with p = 0.5+0.5+0.5 = 1.5
    // .......p
    // |\     .
    // |  \   .
    // |    \ .
    // |______\
    //
    // test:
    //   Vector2 a = new Vector2(0, 0);
    //   Vector2 b = new Vector2(1, 0);
    //   Vector2 c = new Vector2(0, 1);
    //   Triangulation.InTriangle(a, b, c, new Vector2(0.25f, 0.25f)) => True
    //   Triangulation.InTriangle(a, b, c, new Vector2(1, 1)) => False
    //   Triangulation.InTriangle(a, b, c, new Vector2(0, 0)) => True
    public static bool InTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
    {
        float area1 = TriangleArea(p, b, c);
        float area2 = TriangleArea(p, a, c);
        float area3 = TriangleArea(p, a, b);
        return area1 + area2 + area3 <= TriangleArea(a, b, c);
    }

    // polygon helper functions ////////////////////////////////////////////////
    // polygon area calculation
    // source: http://local.wasp.uwa.edu.au/~pbourke/geometry/polyarea/
    /*public static float PolygonAreaSigned(List<Vector2> polygon)
    {
       float area = 0; 

       // go through each of them, combine last one with first one too
       for (int i = 0; i < polygon.Count; i++)
       {
          int j = (i + 1) % polygon.Count;

          area += polygon[i].x * polygon[j].y;
          area -= polygon[i].y * polygon[j].x;
       }

       area /= 2;
       return area; // can be + and -
    }

    public static float PolygonArea(List<Vector2> polygon)
    {
        return Mathf.Abs(PolygonAreaSigned(polygon));
    }

    // is the polygon clockwise?
    public static bool IsClockwise(List<Vector2> polygon)
    {
        return PolygonArea(polygon) >= 0;
    }*/

    // triangulation ///////////////////////////////////////////////////////////
    public static List<int> TryTriangulate(List<Vector2> polygon)
    {
        var triangles = new List<int>();

        // we need an array of indices
        List<int> indices = Enumerable.Range(0, polygon.Count).ToList();
        int current = polygon.Count - 1;

        // we need to avoid deadlocks that may happen for crossing polygons
        int dead = polygon.Count * 2; // max allowed loops
        
        while (indices.Count > 2)
        {
            // avoid deadlock
            if (--dead == 0) return triangles;

            // three consecutive indices
            int previous = (current) % indices.Count;
            current = (current + 1) % indices.Count;
            int next = (current + 1) % indices.Count;
 
            if (IsEar(previous, current, next, polygon, indices))
            {
                // add triangle indices
                triangles.Add(indices[previous]);
                triangles.Add(indices[current]);
                triangles.Add(indices[next]);

                // remove current from remaining polygon
                indices.RemoveAt(current);
            }
        }

        return triangles;
    }

    public static List<int> Triangulate(List<Vector2> polygon)  
    {
        // only if there is at least one triangle
        if (polygon.Count < 3) return new List<int>();

        // triangulation only works if the polygon is counter-clockwise. this is
        // not always the case, and the current detection mode doesn't seem to
        // work all the time. the quick and easy solution is to just triangulate
        // and reverse if it didn't work. this catches 100% of the cases.
        //if (IsClockwise(polygon)) polygon.Reverse(); <- doesn't always work.
        var triangles = TryTriangulate(polygon);
        if (triangles.Count == 0)
        {
            Debug.Log("trying reversed polygon");
            polygon.Reverse();
            triangles = TryTriangulate(polygon);
        }
        return triangles;
    }

    // is the current triangle an ear?
    static bool IsEar(int u, int v, int w, List<Vector2> polygon, List<int> indices)
    {
        Vector2 a = polygon[indices[u]];
        Vector2 b = polygon[indices[v]];
        Vector2 c = polygon[indices[w]];
        
        // is the angle concave? then it cannot be a an ear
        if (IsConcav(b, c, a)) return false;

        // are any of the other points in the triangle a,b,c?
        for (int i = 0; i < indices.Count; ++i)
        {
            if (i != u && i != v && i != w)
            {
                Vector2 point = polygon[indices[i]];
                if (InTriangle(a, b, c, point)) return false;
            }
        }

        // must be an ear then
        return true;
    }
}

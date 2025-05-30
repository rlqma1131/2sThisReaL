using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Gradient Image")]
public class GradientImage : Graphic
{
    public Color leftColor = new Color(0, 0, 0, 0.7f);
    public Color rightColor = new Color(0, 0, 0, 0f);

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        Rect rect = rectTransform.rect;

        Vector2 bottomLeft = new Vector2(rect.xMin, rect.yMin);
        Vector2 topLeft = new Vector2(rect.xMin, rect.yMax);
        Vector2 topRight = new Vector2(rect.xMax, rect.yMax);
        Vector2 bottomRight = new Vector2(rect.xMax, rect.yMin);

        UIVertex vert = UIVertex.simpleVert;

        // bottom left
        vert.position = bottomLeft;
        vert.color = leftColor;
        vh.AddVert(vert);

        // top left
        vert.position = topLeft;
        vert.color = leftColor;
        vh.AddVert(vert);

        // top right
        vert.position = topRight;
        vert.color = rightColor;
        vh.AddVert(vert);

        // bottom right
        vert.position = bottomRight;
        vert.color = rightColor;
        vh.AddVert(vert);

        // Two triangles
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }
}

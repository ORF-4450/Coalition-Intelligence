using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectGrid : MonoBehaviour
{
    public Clamp clamps;
    public float gridScale;
    public float minimumGap;
    public List<GameObject> gridObjects;

    [HideInInspector] public bool horizontalSpaceIsInfinity;
    public bool reverseHorizontalSide;
    [HideInInspector] public float horizontalSpace;
    [HideInInspector] public int maxHorizontalGridObjects;
    [HideInInspector] public float xAdjustedGap;
    [HideInInspector] public bool verticalSpaceIsInfinity;
    public bool reverseVerticalSide;
    [HideInInspector] public float verticalSpace;
    [HideInInspector] public int maxVerticalGridObjects;
    [HideInInspector] public float yAdjustedGap;

    public bool visualizeBounds;
    [HideInInspector] public LineRenderer lineRenderer;

    public void SetGridScale(float scale)
    {
        gridScale = scale;
        transform.localScale = new Vector2(scale, scale);
    }

    public bool IsThereALimit()
    {
        if ((!clamps.leftClamp.clamped && !clamps.rightClamp.clamped) || (!clamps.topClamp.clamped && !clamps.bottomClamp.clamped))
        {
            Debug.Log($"The grid did not have a limit either vertically or horizontally");
            return true;
        }
        return false;
    }
    public void SetGridSpacing()
    {
        if (IsThereALimit()) return;

        reverseHorizontalSide = !clamps.leftClamp.clamped ? true : reverseHorizontalSide;
        reverseVerticalSide = !clamps.topClamp.clamped ? true : reverseVerticalSide;
        
        transform.localScale = new Vector2(gridScale, gridScale);

        horizontalSpaceIsInfinity = !(clamps.leftClamp.clamped && clamps.rightClamp.clamped);
        verticalSpaceIsInfinity = !(clamps.topClamp.clamped && clamps.bottomClamp.clamped);

        if (!horizontalSpaceIsInfinity)
        {
            horizontalSpace = Mathf.Abs(clamps.rightClamp.worldPosition - clamps.leftClamp.worldPosition);
            maxHorizontalGridObjects = (int)((horizontalSpace - minimumGap) / (gridScale + minimumGap));
            xAdjustedGap = (horizontalSpace - maxHorizontalGridObjects * gridScale) / (maxHorizontalGridObjects + 1);
        }

        if (!verticalSpaceIsInfinity)
        {
            verticalSpace = Mathf.Abs(clamps.topClamp.worldPosition - clamps.bottomClamp.worldPosition);
            maxVerticalGridObjects = (int)((verticalSpace - minimumGap) / (gridScale + minimumGap));
            yAdjustedGap = (verticalSpace - maxVerticalGridObjects * gridScale) / (maxVerticalGridObjects + 1);
        }

        if (horizontalSpaceIsInfinity) xAdjustedGap = yAdjustedGap;
        if (verticalSpaceIsInfinity) yAdjustedGap = xAdjustedGap;
    }
    [EditorCools.Button]
    public void OrganizeGrid()
    {
        if (IsThereALimit()) return;

        SetGridSpacing();

        foreach (GameObject gameObject in gridObjects)
            OrganizeGridObject(gameObject);
    }
    public void OrganizeGridObject(GameObject gameObject)
    {
        float xOffset;
        float yOffset;
        int index = gridObjects.IndexOf(gameObject);

        xOffset = reverseHorizontalSide
                ? clamps.rightClamp.worldPosition - (gridScale + xAdjustedGap) * ((index % maxHorizontalGridObjects) + 1) + gridScale / 2
                : clamps.leftClamp.worldPosition + (gridScale + xAdjustedGap) * ((index % maxHorizontalGridObjects) + 1) - gridScale / 2;

        yOffset = reverseVerticalSide
            ? clamps.bottomClamp.worldPosition + (gridScale + yAdjustedGap) * ((index / maxHorizontalGridObjects) + 1)
            : clamps.topClamp.worldPosition - (gridScale + yAdjustedGap) * ((index / maxHorizontalGridObjects) + 1) + gridScale / 2;

        gameObject.transform.position = new Vector3(xOffset, yOffset, 0);
    }
    
    public void AddGridObject(GameObject gameObject)
    {
        gridObjects.Add(gameObject);
        OrganizeGridObject(gameObject);
    }
    public void RemoveGridObject(GameObject gameObject)
    {
        if (gridObjects.Contains(gameObject))
        {
            bool reorganize = gridObjects.IndexOf(gameObject) != gridObjects.Count - 1;

            gridObjects.Remove(gameObject);

            if (reorganize)
                OrganizeGrid();
        }
    }

    [EditorCools.Button]
    public void SetGridObjectsToChildren()
    {
        gridObjects.Clear();
        for (int i = 0; i < transform.childCount; i++)
            AddGridObject(transform.GetChild(i).gameObject);
    }

    [Serializable]
    public class Clamp
    {
        public SideOfClamp leftClamp;
        public SideOfClamp rightClamp;
        public SideOfClamp topClamp;
        public SideOfClamp bottomClamp;

        [Serializable]
        public class SideOfClamp
        {
            public bool clamped = true;
            public float worldPosition;
        }
    }
}
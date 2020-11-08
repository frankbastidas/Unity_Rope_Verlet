using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeClass : MonoBehaviour
{
    // Start is called before the first frame update
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private float SegLineRope = 0.25f;
    private int SegLength = 35;
    private float lineWidth = 0.1f;
    void Start()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropePointInit = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        for (int i = 0; i < SegLength; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropePointInit));
            ropePointInit.y -= SegLineRope;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        Vector3[] ropePositions = new Vector3[this.SegLength];


    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;
        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }

    }
}

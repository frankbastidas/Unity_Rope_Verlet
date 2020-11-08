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
        this.DrawRope();
    }

    private void FixUpdate()
    {
        this.Simulate();
    }

    private void Simulate()
    {
        //SIMULATION
        Vector2 forceGravity = new Vector2(0f, -1);
        for(int i = 0; i < this.SegLength; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            Vector2 velocity = firstSeg.posNow - firstSeg.posOld;
            firstSeg.posOld = firstSeg.posNow;
            firstSeg.posNow += velocity;
            firstSeg.posNow += forceGravity * Time.deltaTime;
            this.ropeSegments[i] = firstSeg;
        }
        //CONSTRAINTS
        for(int i = 0; i < 50; i++)
        {
            this.ApplyConstraint();
        }

    }

    private void ApplyConstraint()
    {
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.ropeSegments[0] = firstSegment;
        for (int i = 0; i < this.SegLength; i++)
        {
            RopeSegment firstseg = this.ropeSegments[i];
            RopeSegment secondseg = this.ropeSegments[i + 1];
            float dist = (firstseg.posNow - secondseg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.SegLineRope);
            Vector2 changeDir = Vector2.zero;
            if (dist > SegLineRope)
            {
                changeDir = (firstseg.posNow - secondseg.posNow).normalized;
            }else if (dist < SegLineRope)
            {
                changeDir = (secondseg.posNow - firstseg.posNow).normalized;
            }
            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstseg.posNow -= changeAmount * error;
                this.ropeSegments[i] = firstseg;
                secondseg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondseg;
            }
            else
            {
                secondseg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondseg;
            }
        }
    }

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        Vector3[] ropePositions = new Vector3[this.SegLength];
        for(int i = 0; i < this.SegLength; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }
        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);

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

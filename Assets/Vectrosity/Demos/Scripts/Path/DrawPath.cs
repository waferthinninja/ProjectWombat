// Makes a textured path that follows a 3D object

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vectrosity.Demos.Scripts.Path
{
    public class DrawPath : MonoBehaviour
    {
        private GameObject ball;
        public GameObject ballPrefab;
        public bool continuousUpdate = true;
        public float force = 16.0f;
        public Color lineColor = Color.green;

        public Texture lineTex;
        public int maxPoints = 500;
        private int pathIndex;

        private VectorLine pathLine;

        private void Start()
        {
            pathLine = new VectorLine("Path", new List<Vector3>(), lineTex, 12.0f, LineType.Continuous);
            pathLine.color = Color.green;
            pathLine.textureScale = 1.0f;

            MakeBall();
            StartCoroutine(SamplePoints(ball.transform));
        }

        private void MakeBall()
        {
            if (ball) Destroy(ball);
            ball = Instantiate(ballPrefab, new Vector3(-2.25f, -4.4f, -1.9f), Quaternion.Euler(300.0f, 70.0f, 310.0f));
            ball.GetComponent<Rigidbody>().useGravity = true;
            ball.GetComponent<Rigidbody>().AddForce(ball.transform.forward * force, ForceMode.Impulse);
        }

        private IEnumerator SamplePoints(Transform thisTransform)
        {
            // Gets the position of the 3D object at intervals (20 times/second)
            var running = true;
            while (running)
            {
                pathLine.points3.Add(thisTransform.position);
                if (++pathIndex == maxPoints) running = false;
                yield return new WaitForSeconds(.05f);

                if (continuousUpdate) pathLine.Draw();
            }
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 30), "Reset")) Reset();
            if (!continuousUpdate && GUI.Button(new Rect(10, 45, 100, 30), "Draw Path")) pathLine.Draw();
        }

        private void Reset()
        {
            StopAllCoroutines();
            MakeBall();
            pathLine.points3.Clear();
            pathLine.Draw(); // Re-draw the cleared line in order to erase all previously drawn segments
            pathIndex = 0;
            StartCoroutine(SamplePoints(ball.transform));
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZaccCharv
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform characterPosition;
        public GameObject levelBounds;
        private Vector3 levelBoundsBottom;
        private Vector3 levelBoundsTop;
        private float cameraExtentsYForward;
        private float cameraExtentsXForward;
        private float cameraExtentsYBackward;
        private float cameraExtentsXBackward;
        private Vector3 target;

        [SerializeField] private Camera aCamera;

        private void Start()
        {
            levelBoundsBottom = levelBounds.gameObject.GetComponent<Renderer>().bounds.min;
            levelBoundsTop = levelBounds.gameObject.GetComponent<Renderer>().bounds.max;

            aCamera = gameObject.GetComponent<Camera>();
            Debug.Log("Camera started");
        }

        // Update is called once per frame
        void Update()
        {
            cameraExtentsYForward = transform.position.y + aCamera.orthographicSize;
            cameraExtentsXForward = transform.position.x + ((aCamera.orthographicSize * (1.777f)));
            cameraExtentsYBackward = aCamera.orthographicSize - transform.position.y;
            cameraExtentsXBackward = ((aCamera.orthographicSize * (1.777f)) - transform.position.x);

            Debug.DrawLine(levelBoundsBottom, levelBoundsTop, Color.yellow);
            Debug.DrawLine(new Vector3(cameraExtentsXBackward, cameraExtentsYBackward), new Vector3(cameraExtentsXForward, cameraExtentsYForward), Color.red);

            float targetX = Mathf.Clamp(characterPosition.position.x, levelBoundsBottom.x + cameraExtentsXBackward, levelBoundsTop.x + cameraExtentsXForward);
            float targetY = Mathf.Clamp(characterPosition.position.y, levelBoundsBottom.y + cameraExtentsYBackward, levelBoundsTop.y + cameraExtentsYForward);

            target = new Vector3 (targetX, targetY, 0);
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
        }
    }
}

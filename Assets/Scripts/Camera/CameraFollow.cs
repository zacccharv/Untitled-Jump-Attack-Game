using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace ZaccCharv
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform characterPosition;
        public GameObject levelBounds;
        private Vector3 levelBoundsBottom;
        private Vector3 levelBoundsTop;
        private float cameraExtentsY;
        private float cameraExtentsX;

        private float targetX;
        private float targetY;
        private Vector3 target;

        [SerializeField] private Camera aCamera;

        private void Start()
        {
            levelBoundsBottom = levelBounds.gameObject.GetComponent<Renderer>().bounds.min;
            levelBoundsTop = levelBounds.gameObject.GetComponent<Renderer>().bounds.max;

            aCamera = gameObject.GetComponent<Camera>();

            transform.position = target;
        }

        // Update is called once per frame
        void Update()
        {
            cameraExtentsY = aCamera.orthographicSize;
            cameraExtentsX = (aCamera.orthographicSize * 1.778f);

            Debug.DrawLine(levelBoundsBottom, levelBoundsTop, Color.yellow);
            Debug.DrawLine(new Vector3(-cameraExtentsX + transform.position.x, -cameraExtentsY + transform.position.y), new Vector3(cameraExtentsX + transform.position.x, cameraExtentsY + transform.position.y), Color.red);

            targetX = Mathf.Clamp(characterPosition.position.x, levelBoundsBottom.x + cameraExtentsX, levelBoundsTop.x - cameraExtentsX);
            targetY = Mathf.Clamp(characterPosition.position.y, levelBoundsBottom.y + cameraExtentsY, levelBoundsTop.y - cameraExtentsY);

            target = new Vector3 (targetX, targetY, 0);
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
        }
    }
}

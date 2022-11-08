using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ZaccCharv
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform characterPosition;
        public GameObject levelBounds;
        private float cameraExtentsY;
        private float cameraExtentsX;
        private Vector3 target;

        [SerializeField] private Camera aCamera;

        private void Start()
        {
            aCamera = gameObject.GetComponent<Camera>();
            cameraExtentsY = aCamera.orthographicSize + (transform.position.y/2);
            cameraExtentsX = (aCamera.orthographicSize + (transform.position.x * 2)) * Screen.width / Screen.height;
        }

        // Update is called once per frame
        void Update()
        {

            Vector3 levelBoundsBottom = levelBounds.gameObject.GetComponent<Renderer>().bounds.min;
            Vector3 levelBoundsTop = levelBounds.gameObject.GetComponent<Renderer>().bounds.max;

            float targetX = Mathf.Clamp(characterPosition.position.x, levelBoundsBottom.x + cameraExtentsX, levelBoundsTop.x);
            float targetY = Mathf.Clamp(characterPosition.position.y, levelBoundsBottom.y + cameraExtentsY, levelBoundsTop.y);


            Debug.DrawLine(levelBoundsBottom, levelBoundsTop);

            target = new Vector3 (targetX, targetY, 0);
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
        }
    }
}

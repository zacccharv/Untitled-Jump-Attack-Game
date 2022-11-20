using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace ZaccCharv
{
    public class WeaponAnimation : MonoBehaviour
    {
        private LineRenderer lr;
        public GameObject[] lazerArray;
        private Camera mainCam;
        private Vector3[] points = new Vector3[2];
        public bool isntRunning = true;
        public float length;

        // Start is called before the first frame update
        void Start()
        {
            mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            lr = GetComponent<LineRenderer>();
            for (int i = 0; i < lazerArray.Length; i++)
            {
                points[i] = lazerArray[0].transform.position;
            }
        }

        private IEnumerator AnimateLine()
        {
            int num = 0;
            float startTime = Time.time;

            Vector3 startPosition = points[0];
            Vector3 endPosition = points[1];

            lr.SetPosition(0, startPosition);

            Vector3 pos = startPosition;
            while (pos != endPosition)
            {
                float t = Time.time - startTime;
                pos = Vector3.Lerp(startPosition, endPosition, t);
                Vector3 posTwo = Vector3.Lerp(startPosition, endPosition, t + .1f);

                lr.SetPosition(1, pos);
                lr.SetPosition(0, posTwo);

                isntRunning = false;

                Debug.Log(num++);

                yield return new WaitForSeconds(.1f);
            }

            SwitchOff();
        }

        void SwitchOff()
        {
            Debug.Log("I/m Switched off");
            isntRunning = true;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 enemyPos = lazerArray[1].transform.position;

            Vector3 rotation = enemyPos - transform.position;

            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            if (isntRunning == true)
            {
                lr.SetPosition(0, points[0]);
                lr.SetPosition(1, points[0]);
            }

            if (Input.GetMouseButtonDown(0) && isntRunning == true)
            {
                for (int i = 0; i < lazerArray.Length; i++)
                {
                    points[i] = new Vector3(lazerArray[i].transform.position.x, lazerArray[i].transform.position.y, 0);
                }

                Time.timeScale = 2.5f;
                StartCoroutine(AnimateLine());
            }

        }
    }
}
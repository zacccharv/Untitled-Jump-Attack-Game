using UnityEngine;

namespace ZaccCharv
{
    public class CharCollisions : MonoBehaviour
    {
        public bool _touchingBottom, _earlyJump;
        public bool _leftWallHit, _rightWallhit;
        public Transform _rayCenter;
        public void CharCollisionCheck(Transform currentObj)
        {
            // Bit shift the index of the layer (7) to get a bit mask
            int layerMask = 1 << 7;
            // This would cast rays only against colliders in layer 7.
            var RightCast = Physics2D.Raycast(currentObj.position, currentObj.TransformDirection(Vector2.right), .4f, layerMask);
            var LeftCast = Physics2D.Raycast(currentObj.position, currentObj.TransformDirection(Vector2.left), .4f, layerMask);
            var UpCast = Physics2D.Raycast(currentObj.position, currentObj.TransformDirection(Vector2.up), .5f, layerMask);
            var DownCast = Physics2D.BoxCast(currentObj.position, new Vector2(.4f, .2f), 0f, currentObj.TransformDirection(Vector2.down), 0, layerMask);
            var DownCast2 = Physics2D.BoxCast(currentObj.position, new Vector2(.4f, .2f), 0f, currentObj.TransformDirection(Vector2.down), 0, layerMask);

            if (RightCast && !LeftCast && !UpCast && !DownCast)
            {
                Debug.DrawRay(_rayCenter.position + new Vector3(0, 1, 0), _rayCenter.TransformDirection(Vector2.right), Color.yellow);

                _rightWallhit = true;
            }
            else if (LeftCast && !RightCast && !UpCast && !DownCast)
            {
                Debug.DrawRay(_rayCenter.position + new Vector3(0, 1, 0), _rayCenter.TransformDirection(Vector2.left), Color.blue);

                _leftWallHit = true;
            }
            else
            {
                _leftWallHit = false;
                _rightWallhit = false;
            }
            if (DownCast)
            {
                _touchingBottom = true;
                // use child to draw ray from
                Debug.DrawRay(_rayCenter.position, _rayCenter.TransformDirection(new Vector2(0, -.2f)), Color.yellow);
            }
            else
            {
                _touchingBottom = false;
            }

            if (!DownCast && DownCast2)
            {
                _earlyJump = true;
            }
        }
    }
}
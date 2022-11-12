using UnityEngine;
public class CharCollisions : MonoBehaviour
 {
        public bool _leftWallHit, _rightWallhit;
        public Transform _rayCenter;
        public void CharCollisionCheck()
    {
        // Bit shift the index of the layer (7) to get a bit mask
        int layerMask = 1 << 7;
        // This would cast rays only against colliders in layer 7.
        var RightCast = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), .4f, layerMask);
        var LeftCast = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), .4f, layerMask);
        var UpCast = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), .5f, layerMask);
        var DownCast = Physics2D.BoxCast(transform.position, new Vector2(.4f,.2f), 0f, transform.TransformDirection(Vector2.down), 0, layerMask);
        var DownCast2 = Physics2D.BoxCast(transform.position, new Vector2(.4f, .25f), 0f, transform.TransformDirection(Vector2.down), 0, layerMask);

        if (RightCast && (!UpCast && !DownCast))
        {
            Debug.DrawRay(_rayCenter.position + new Vector3(0, 1, 0), transform.TransformDirection(Vector2.right), Color.yellow);

            _rightWallhit = true;
        }
        else if (LeftCast && (!UpCast && !DownCast))
        {
            Debug.DrawRay(_rayCenter.position + new Vector3(0,1,0), transform.TransformDirection(Vector2.left), Color.blue);

            _leftWallHit = true;
        }
        else
        {
            _leftWallHit = false;
            _rightWallhit = false;

            // use child to draw ray from
            Debug.DrawRay(_rayCenter.position, transform.TransformDirection(Vector2.right) * 1000, Color.white);
            Debug.DrawRay(_rayCenter.position, transform.TransformDirection(Vector2.left) * 1000, Color.white);
        }
        if (DownCast)
        {
            _touchingBottom = true;
            // use child to draw ray from
            Debug.DrawRay(_rayCenter.position, transform.TransformDirection(new Vector2(0, -.2f)), Color.yellow);
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
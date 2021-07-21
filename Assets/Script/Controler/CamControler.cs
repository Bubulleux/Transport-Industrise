using System.Linq;
using Script.Mapping;
using UnityEngine;

namespace Script.Controler
{
    public class CamControler : MonoBehaviour
    {
        [SerializeField]
        private float upSpeed = 1f;

        private Vector3 futurPos;

        public static Vector3 camPos;

        private void Start()
        {
            futurPos = transform.position;
        }

        void Update()
        {
            if (!PlayerControler.PointerIsOverUI())
            {
                futurPos +=  Vector3.up * (Input.mouseScrollDelta.y * -1 * futurPos.y * 0.1f);
            }
            if (futurPos.y > 500)
                futurPos = new Vector3(futurPos.x, 500f, futurPos.z);
            futurPos += (Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal")) * 
                        (Time.deltaTime * futurPos.y * 1f * (Input.GetKey(KeyCode.LeftShift) ? 3 : 1));

            float minCam = MapManager.map.GetParcel(futurPos.ToVec2Int()).corner.Max() + 1;
            if (futurPos.y < minCam)
            {
                futurPos = new Vector3(futurPos.x, minCam, futurPos.z);
            }

            var position = transform.position;
            var diffPos = futurPos - position;
            Vector3 velocity = diffPos.normalized *
                               ((1 + diffPos.magnitude * 5f) * Time.deltaTime);
            if (diffPos.magnitude < velocity.magnitude)
                transform.position = futurPos;
            else
                transform.position += velocity;
            camPos = transform.position;

        }
    }
}

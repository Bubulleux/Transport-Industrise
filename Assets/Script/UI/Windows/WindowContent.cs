using UnityEngine;

namespace Script.UI.Windows
{
    public class WindowContent : MonoBehaviour
    {
        public Window WindowParente
        {
            get
            {
                return transform.parent.GetComponent<Window>();
            }
        }

        public void Close()
        {
            WindowParente.Close();
        }
    }
}

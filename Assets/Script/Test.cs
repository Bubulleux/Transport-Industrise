using UnityEngine;

namespace Script
{
	public class Test : MonoBehaviour
	{
		public bool PrintTest(bool value, string text)
		{
			Debug.Log(text);
			return value;
		}

		private void Start()
		{
			if (PrintTest(true, "First") || PrintTest(false, "First"))
			{
				Debug.Log("OK");
			}
		}
	}
	
	
}



using System.Collections;
using System.Collections.Generic;
using Script.Mapping;
using Script.Mapping.ParcelType;
using Script.UI.Windows;
using UnityEngine;

namespace Script.Controler
{
	public class PlayerControler : MonoBehaviour
	{
		public static PlayerControler instance;
		public Camera cam;
		public Tools curTool;
		public delegate void MainToolRedir(Vector2Int posMouse);
		public MainToolRedir toolRedirection = null;

		private void Awake()
		{
			instance = this;
		}
	
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				toolRedirection = null;
			}
			if (Input.GetMouseButtonDown(0) && GetMoussePos() != Vector3.zero)
			{
				if (toolRedirection == null)
				{
					switch (curTool)
					{
						case Tools.none:
							MapManager.map.GetParcel(GetMoussePos().ToVec2Int()).Interact();
							break;
						case Tools.road:
							StartCoroutine(MakeRoad());
							break;
						case Tools.depot:
							MapManager.map.AddConstruction(GetMoussePos().ToVec2Int(), new Depot());
							break;
						case Tools.loadingBay:
							MapManager.map.AddConstruction(GetMoussePos().ToVec2Int(), new LoadingBay());
							break;
						case Tools.busStop:
							MapManager.map.AddConstruction(GetMoussePos().ToVec2Int(), new BusStop());
							break;
						case Tools.destroy:
							MapManager.map.Destroy(GetMoussePos().ToVec2Int());
							break;
					}
				}
				else
				{
					toolRedirection(GetMoussePos().ToVec2Int());
				}
			
			}

		
		}

		public static Vector3 GetMoussePos()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit) && !PointerIsOverUI())
			{
				return hit.point;
			}
			return Vector3.zero;
		}

		public IEnumerator MakeRoad()
		{
			Vector2Int startMouse = GetMoussePos().ToVec2Int();
			Vector2Int endMouse = GetMoussePos().ToVec2Int();
			while (Input.GetMouseButton(0))
			{
				yield return new WaitForFixedUpdate();

				if (endMouse == GetMoussePos().ToVec2Int())
					continue;
				
				endMouse = GetMoussePos().ToVec2Int();
				MapManager.Selector.ClearSelection();
				foreach (var pathCell in GetPathBetweenTwoPoint(startMouse, endMouse, 50))
				{
					MapManager.Selector.SelectionParcel(pathCell.Key, Color.black);
				}
			}

			endMouse = GetMoussePos().ToVec2Int();
			MapManager.Selector.ClearSelection();
			foreach (var pathCell in GetPathBetweenTwoPoint(startMouse, endMouse, 50))
			{;
				MapManager.map.AddRoad(pathCell.Key);
			}
			
		}

		public static Dictionary<Vector2Int, bool> GetPathBetweenTwoPoint(Vector2Int start, Vector2Int stop, int minSizeRoad)
		{
			
			Vector2Int lastPos = start;
			var path = new Dictionary<Vector2Int, bool>();
			path.Add(start, true);
			while (true)
			{
				if (lastPos == stop)
				{
					break;
				}
				float xProgress = Mathf.Abs(lastPos.x - start.x) / (float)Mathf.Abs(start.x - stop.x);
				float yProgress = Mathf.Abs(lastPos.y - start.y) / (float)Mathf.Abs(start.y - stop.y);
				xProgress = (float.IsNaN(xProgress) || float.IsInfinity(xProgress)) ? 1 : xProgress;
				yProgress = (float.IsNaN(yProgress) || float.IsInfinity(yProgress)) ? 1 : yProgress;
				if (xProgress < yProgress)
				{
					for (int i = 0; i < minSizeRoad; i++)
					{
						lastPos += new Vector2Int(lastPos.x < stop.x ? 1 : -1, 0);
						path.Add(lastPos, true);
						if (lastPos.x == stop.x) { break; }
					}
				}
				else
				{
					for (int i = 0; i < minSizeRoad; i++)
					{
						lastPos += new Vector2Int(0, lastPos.y < stop.y ? 1 : -1);
						path.Add(lastPos, true);
						if (lastPos.y == stop.y) { break; }
					}
				}
		   
			}
			
			return path;
		}

		public void SetTool(int tool)
		{
			curTool = (Tools)tool;
			toolRedirection = null;
		}

		public void RedirectTool( MainToolRedir func)
		{
			toolRedirection = func;
			Debug.Log("Redirec Main Tool");
		}
		public enum Tools
		{
			none,
			road,
			depot,
			loadingBay,
			busStop,
			destroy,
		}

		public static bool PointerIsOverUI()
		{
			foreach (GameObject curWindow in GameObject.FindGameObjectsWithTag("Window"))
			{
				if (curWindow.GetComponent<Window>().pointerOverMe)
				{
					return true;
				}
			}
			return false;
		}
	}
}

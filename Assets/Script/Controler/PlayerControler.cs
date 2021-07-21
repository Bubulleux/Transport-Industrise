using System;
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
		public delegate void MainToolRedir(Vector2Int posMouse);
		public MainToolRedir toolRedirection = null;
		
		public Tool curTool;
		public Tool[] Tools = new[]
		{
			new Tool(),
			new RoadTool(),
			new ConstructorTool(),
			new DestroyTool(),
		};

		private Vector2Int _startDrag = Vector2Int.one * -1;
		private Vector2Int _lastMoussePos;
		private Vector2Int _futureLastMoussePos;

		private void Awake()
		{
			instance = this;
		}
	
		void Update()
		{
			var moussePos = GetMoussePos().ToVec2Int();
			var mousseValid = moussePos != Vector2Int.one * -1;
			
			
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				toolRedirection = null;
			}
			if (Input.GetMouseButtonDown(0) && mousseValid)
			{
				if (toolRedirection == null)
				{
					_startDrag = moussePos;
				}
				else
				{
					toolRedirection(moussePos);
				}
			
			}

			if (Input.GetMouseButton(0) && MouseMove() && mousseValid)
			{
				if (_startDrag != Vector2Int.one * -1)
				{
					curTool.Drag(_startDrag, moussePos);
				}
			}

			if (Input.GetMouseButtonUp(0))
			{
				if (mousseValid)
				{
					if (moussePos != _startDrag && _startDrag != Vector2Int.one * -1)
					{
						curTool.StopDrag(_startDrag, moussePos);
					}
					else if (moussePos == _startDrag)
					{
						curTool.OneClick(moussePos);
					}
				}
				_startDrag = Vector2Int.one * -1;
			}

			if (Input.GetMouseButtonDown(1))
			{
				_startDrag = Vector2Int.one * -1;
			}

			if (MouseMove() && mousseValid && !Input.GetMouseButton(0))
			{
				curTool.MousseOverMap(moussePos);
			}
			//Debug.Log($"{moussePos} {MouseMove()} {mousseValid} {_lastMoussePos}");
			_futureLastMoussePos = GetMoussePos().ToVec2Int();
		}

		private void LateUpdate()
		{
			if (MousseValide())
			{
				_lastMoussePos = _futureLastMoussePos;
			}
		}

		public static Vector3 GetMoussePos()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit) && !PointerIsOverUI())
			{
				return hit.point;
			}
			return Vector3.one * -1;
		}

		public static bool MousseValide()
		{
			return GetMoussePos() != Vector3.one * -1;
		}

		public static bool MouseMove()
		{
			return instance._lastMoussePos != GetMoussePos().ToVec2Int();
		}
		public void SetTool(int tool)
		{
			curTool = Tools[tool];
			toolRedirection = null;
		}

		public void RedirectTool( MainToolRedir func)
		{
			toolRedirection = func;
			Debug.Log("Redirec Main Tool");
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

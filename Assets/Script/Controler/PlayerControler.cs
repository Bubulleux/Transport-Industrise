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
		
		public Tool CurTool
		{
			get => toolRedirection != null ? toolRedirection : tools[curToolIndex];
			set
			{
				if (Array.IndexOf(tools, value) != -1)
				{
					toolRedirection = null;
					curToolIndex = Array.IndexOf(tools, value);
				}
				else
					toolRedirection = value;
			}
		}
		private int curToolIndex = 0;
		private Tool toolRedirection = null;
		public readonly Tool[] tools = new[]
		{
			new Tool(),
			new RoadTool(),
			new ConstructorTool(),
			new DestroyTool(),
			//new Terraformer(),
		};

		private Vector2Int startDrag = Vector2Int.one * -1;
		private Vector2Int lastMoussePos;
		private Vector2Int futureLastMoussePos;

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
				startDrag = moussePos;
			}

			if (Input.GetMouseButton(0) && MouseMove() && mousseValid)
			{
				if (startDrag != Vector2Int.one * -1)
				{
					CurTool.Drag(startDrag, moussePos);
				}
			}

			if (Input.GetMouseButtonUp(0))
			{
				if (mousseValid)
				{
					if (moussePos != startDrag && startDrag != Vector2Int.one * -1)
					{
						CurTool.StopDrag(startDrag, moussePos);
					}
					else if (moussePos == startDrag)
					{
						CurTool.OneClick(moussePos);
					}
				}
				startDrag = Vector2Int.one * -1;
			}

			if (Input.GetMouseButtonDown(1))
			{
				startDrag = Vector2Int.one * -1;
			}

			if (MouseMove() && mousseValid && !Input.GetMouseButton(0))
			{
				CurTool.MousseOverMap(moussePos);
			}

			if (Input.GetMouseButtonDown(2))
			{
				CurTool.MidelMousseBtn();
				if (mousseValid)
				{
					CurTool.MousseOverMap(moussePos);
					if (Input.GetMouseButton(0))
						CurTool.Drag(startDrag, moussePos);
				}
			}

			if (Input.mouseScrollDelta.y != 0)
			{
				CurTool.MouseScrool(Mathf.FloorToInt(Input.mouseScrollDelta.y));
				CurTool.MousseOverMap(moussePos);
			}
			//Debug.Log($"{moussePos} {MouseMove()} {mousseValid} {_lastMoussePos}");
			futureLastMoussePos = GetMoussePos().ToVec2Int();
		}

		private void LateUpdate()
		{
			if (MousseValide())
			{
				lastMoussePos = futureLastMoussePos;
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
			return instance.lastMoussePos != GetMoussePos().ToVec2Int();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class VehicleContoler : MonoBehaviour
{
	
	public VehicleData vehicleData;
	
	public float damage = 0f;

	private float driveCooldown = 0f;

	private float restartCooldown = 0f;
	private bool vehicleNeedRestar = false;

	public bool engineEnable = false;
	public bool vehicleStoped = true;
	public Animator vehicleAnimation;

	public List<Vector2Int> path = new List<Vector2Int>();
	private int routePointGo;
	public int RoutePointGo { get => routePointGo; set { routePointGo = value; Debug.Log(MyRoute.points.Count); routePointGo %= MyRoute.points.Count; } }

	private Group myGroup = null;
	public Group MyGroup
	{
		get => myGroup;
		set
		{
			if (myGroup != null)
			{
				myGroup.vehicles.Remove(this);
			}
			if (value != null)
			{
				value.vehicles.Add(this);
			}
			myGroup = value;
		}
	}

	public string Id
	{
		get
		{
			string _group = "";
			if (myGroup != null)
			{
				_group = Group.groups.IndexOf(MyGroup) + "-";
			}
			return _group + transform.GetSiblingIndex();
		}
	}

	private Route myRoute = null;
	public Route MyRoute
	{
		get
		{
			if (MyGroup != null && MyGroup.forceRoute)
			{
				return MyGroup.route;
			}
			else
			{
				return myRoute;
			}

		}
		set => myRoute = value;
	}

	public Vector2Int VehiclePos { get { return transform.position.ToVec2Int(); } 
		set
		{
			float y = MapManager.map.parcels[value.x, value.y].corner.Max();
			transform.position = new Vector3(value.x, y, value.y);
		}
	}

	public Task asyncUpdateOperation;
	void Start()
	{
		transform.parent = GameObject.Find("Vehicles").transform;
		AnimeVehicle();
	}

	void Update()
	{
		if (engineEnable)
		{
			DriveAlong();
		}
		if (restartCooldown > 0f)
			restartCooldown -= Time.deltaTime;
		else if (vehicleNeedRestar == true)
		{
			engineEnable = true;
			vehicleNeedRestar = false;
		}
	}

	public virtual void StartVehicle()
	{
		if (MyRoute == null)
		{
			return;
		}
		path = null;
		engineEnable = true;
		vehicleStoped = false;
	}

	public virtual void DriveAlong()
	{
		if (driveCooldown > 0f)
		{
			driveCooldown -= Time.deltaTime;
			return;
		}

		if (!vehicleStoped && (path == null || path.Count == 0))
		{
			if (path != null)
				DoSomething();
			path = GetPath();
			if (path == null)
				engineEnable = false;
			return;
		}
		else if (path == null || path.Count == 0)
			return;

		AnimeVehicle();
		VehiclePos = path[0];
		path.RemoveAt(0);
		damage += 0.02f;
		driveCooldown = 1f / (float)vehicleData.speed;
	}

	public virtual List<Vector2Int> GetPath()
	{
		RoutePointGo += 1;
		return PathFinder.FindPath(VehiclePos, MyGroup.route.points[routePointGo]);
	}

	public virtual Depot GetCloserDepot()
	{
		Depot closerDepot = null;
		float distance = 0f;
		foreach (Vector2Int curPoint in MyGroup.route.points)
		{
			if (MapManager.map.GetparcelType(curPoint) == typeof(Depot))
			{
				if (closerDepot == null || Vector2Int.Distance(VehiclePos, MapManager.map.GetParcel<Depot>(curPoint).pos) < distance)
				{
					closerDepot = MapManager.map.GetParcel<Depot>(curPoint);
					distance = Vector2Int.Distance(VehiclePos, MapManager.map.GetParcel<Depot>(curPoint).pos);
				}
			}
		}
		return closerDepot;
	}

	public virtual void DoSomething()
	{
		if (MapManager.map.GetparcelType(VehiclePos) == typeof(Depot) && damage > 0.1f)
		{
			damage = 0f;
			SetRestart(1f);
		}
	}

	public void SetRestart(float time)
	{
		if (time == 0f || vehicleStoped)
			return;
		restartCooldown = time;
		vehicleNeedRestar = true;
		engineEnable = false;
	}

	public virtual void ReturnInDepot() { }
	public virtual void AnimeVehicle() { }
	public virtual void GoToAnotherPoint() { }
}
public static class Vector3Extension
{
	public static Vector2Int ToVec2Int(this Vector3 vec)
	{
		return new Vector2Int(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.z));
	}
}
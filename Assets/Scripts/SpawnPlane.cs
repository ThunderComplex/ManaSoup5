using UnityEngine;

public class SpawnPlane : MonoBehaviour
{
	[Header("Grid Settings")]
	[Tooltip("Number of points per line in X direction")]
	public int pointCountInLine = 5;

	[Header("Enemy Settings")]
	public GameObject EnemyPrefab;

	[SerializeField, HideInInspector]
	public Vector3[] pointPositions;
	public Vector3[] GetPointPositions() => pointPositions;

	private void OnDrawGizmos()
	{
		if (pointCountInLine < 2)
		{
			pointPositions = null;
			return;
		}

		Gizmos.color = Color.red;
		float sizeX = 1f;
		float sizeZ = 1f;
		MeshFilter mf = GetComponent<MeshFilter>();
		if (mf != null && mf.sharedMesh != null)
		{
			Vector3 meshSize = mf.sharedMesh.bounds.size;
			Vector3 lossyScale = transform.lossyScale;
			sizeX = meshSize.x * lossyScale.x;
			sizeZ = meshSize.z * lossyScale.z;
		}

		// Calculate spacing so that X and Z are always the same, and fill the plane
		int countX = Mathf.Max(2, pointCountInLine);
		float spacing = sizeX / (countX - 1);
		int countZ = Mathf.Max(2, Mathf.RoundToInt(sizeZ / spacing) + 1);
		float usedZ = spacing * (countZ - 1);
		Vector3 origin = transform.position - new Vector3(sizeX, 0, usedZ) * 0.5f;

		pointPositions = new Vector3[countX * countZ];
		int idx = 0;
		for (int x = 0; x < countX; x++)
		{
			for (int z = 0; z < countZ; z++)
			{
				Vector3 point = origin + new Vector3(x * spacing, 0, z * spacing);
				pointPositions[idx++] = point;
				Gizmos.DrawSphere(point, 0.1f);
			}
		}
	}

	public void SpawnEnemies()
	{
		var playerLocation = GameObject.FindWithTag("Player").transform;

		for (int i = 0; i < pointPositions.Length; i++)
		{
			if (i % 2 > 0)
			{
				var point = pointPositions[i];
				point.y += 1f;
				var obj = PoolingSystem.Instance.SpawnObject(EnemyPrefab, point, Quaternion.identity);
				obj.GetComponent<EnemyController>().PlayerLocation = playerLocation;
			}
		}
	}
}

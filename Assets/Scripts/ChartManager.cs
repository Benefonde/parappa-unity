using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartManager : MonoBehaviour
{
	// thanks Enjoy Tommorow for the code
	public void LoadMap()
	{
		if (File.Exists(MapPath))
		{
			StartCoroutine("LoadChart2");
		}
		else
		{
			Debug.LogWarning("No chart found!");
		}
	}

	private IEnumerator LoadChart2()
	{
		ChartName = MapPath.Split('/')[MapPath.Split('/').Length - 1].Replace(".txt", "");
		List<string> lines = new List<string>();
		int currentContainter = 0;
		for (int i = 0; i < File.ReadAllLines(MapPath).Length; i++)
		{
			lines.Add(File.ReadAllLines(MapPath)[i]);
		}
		for (int i = 0; i < lines.Count; i++)
		{
			if (lines[i] != "")
			{
				if (lines[i].StartsWith("#"))
				{
					currentContainter = int.Parse(lines[i].Replace("#", ""));
				}
				else
				{
					if (currentContainter == 0)
					{
						ChartName = lines[i];
					}
					else if (currentContainter == 1)
					{
						difficulty = int.Parse(lines[i]);
					}
					else if (currentContainter == 2)
					{
						List<string> info1 = new List<string>();
						foreach (string str in lines[i].Split('.'))
						{
							if (lines[i] != "")
							{
								info1.Add(str);
							}
						}
						/*rooms.Add(int.Parse(info1[0]));
						roomTypes.Add(int.Parse(info1[1]));
						GameObject roomClone = Instantiate(roomTemplate, new Vector3(0f, 0f, 0f), Quaternion.identity);
						roomClone.transform.parent = MapParent;
						roomClone.name = "Room-" + info1[0];*/
					}
				}
			}
			yield return null;
		}
		if (MapPath.EndsWith(".txt"))
		{
			string MapPath3 = "";
			for (int i = 0; i < MapPath.Split('/').Length; i++)
			{
				if (i + 1 < MapPath.Split('/').Length)
				{
					MapPath3 += MapPath.Split('/')[i] + "/";
				}
			}
			MapPath = MapPath3;
		}
	}

	public string MapPath;
	string ChartName;
	int difficulty;

	public GameObject chart;
}

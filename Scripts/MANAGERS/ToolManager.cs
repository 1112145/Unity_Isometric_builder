using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToolManager : MonoBehaviour {

	public static List<Tool> tools = new List<Tool>();
	public static Tool currentTool = null;
	public static Tool defaultTool;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Select a tool as current tool
	public static void SelectTool(Tool tool)
	{
		currentTool = tool;
		UseCurrentTool();
	}

	// Deselect current tool.
	public static void DeselectTool()
	{
		currentTool = defaultTool;
		UseCurrentTool();
	}

	// Set a tool as default tool. 
	public static void SetDefaultTool(Tool tool)
	{
		defaultTool = tool;
		ActiveTool(tool);
	}


	private static void ActiveTool (Tool currentTool)
	{
		for (int i = 0; i < tools.Count; i++) {
			tools [i].active = (currentTool.ToolName == tools [i].ToolName)? true: false;
		}
	}



	private static void UseCurrentTool()
	{
		ActiveTool(currentTool);
	}


}

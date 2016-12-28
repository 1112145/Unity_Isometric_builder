using UnityEngine;
using System.Collections;
using System.Windows.Forms;
using System.IO;


// To export my level to JSON Files.

public class ExportFileManager : MonoBehaviour {

	public static IsoMetricRootModel OutputRootModel = new IsoMetricRootModel();
	public static string currentPath = "";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
			&& (Input.GetKeyDown(KeyCode.S)))
		{
			if(currentPath == "")
			{
				ExportToJSONDialog();
			}
			else
			{
				OutputRootModel.ConvertAllLayer();
				string jsonContent = JsonUtility.ToJson(OutputRootModel);
				File.WriteAllText(currentPath,jsonContent);
			}
		}
	
	}

	public void ExportToJSONDialog()
	{
		SaveFileDialog saveDialog = new SaveFileDialog();
		saveDialog.Filter = "JSON Files |*.json";
		if(saveDialog.ShowDialog() == DialogResult.OK)
		{
			OutputRootModel.ConvertAllLayer();
			string jsonContent = JsonUtility.ToJson(OutputRootModel);
			currentPath = saveDialog.FileName;
			File.WriteAllText(saveDialog.FileName,jsonContent);
			CameraTool._isDragging = false;
		}
	}
}

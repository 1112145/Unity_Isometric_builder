using UnityEngine;
using System.Collections;
using System.Windows.Forms;
using System.IO;


// To export my level to JSON Files.

public class ExportFileManager : MonoBehaviour {

	public static IsoMetricRootModel OutputRootModel = new IsoMetricRootModel();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ExportToJSON()
	{
		SaveFileDialog saveDialog = new SaveFileDialog();
		saveDialog.Filter = "JSON Files |*.json";
		if(saveDialog.ShowDialog() == DialogResult.OK)
		{
			OutputRootModel.ConvertAllLayer();
			string jsonContent = JsonUtility.ToJson(OutputRootModel);
			File.WriteAllText(saveDialog.FileName,jsonContent);
			CameraTool._isDragging = false;
		}
	}
}

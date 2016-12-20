using UnityEngine;
using System.Collections;
using System.Windows.Forms;
using System;

public class PropertiesFormManager : MonoBehaviour {

	public static Form properties;

	public static TextBox txtTileHeight;
	public static TextBox txtTileWidth;


	// Create form components.
	public static void CreateFormComponents()
	{
		properties = new Form();
		properties.Text = "Properties";
	

		txtTileHeight = new TextBox();
		txtTileHeight.SetBounds(0,100,100,20);

		txtTileWidth = new TextBox();
		txtTileHeight.SetBounds(0,150,100,20);

		System.Windows.Forms.Button btnOK = new Button();
		btnOK.Text = "OK";
		btnOK.Click += new System.EventHandler(OnClickButtonOK);

		// Add controls to this form.
		properties.Controls.Add(txtTileHeight);
		properties.Controls.Add(btnOK);
	}

	public static void OnClickButtonOK(object sender, EventArgs e)
	{
//		Debug.Log(txtTileWidth.Text);
	}
}

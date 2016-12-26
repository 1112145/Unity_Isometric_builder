using UnityEngine;
using System.Collections;

public class Ultils{

	public static Sprite SetIsoPivot(Texture2D texture)
	{
		Vector2 size = new Vector2 (texture.width, texture.height);
		Rect rect = new Rect (0, 0, texture.width, texture.height);
		Vector2 offset = new Vector2 (0.5f, (size.y - 0.5f * size.x) / size.y);
//		Vector2 offset = new Vector2 (0.5f, 0);
		Sprite sprite = Sprite.Create (texture, rect, offset);
		return sprite;
	}
}

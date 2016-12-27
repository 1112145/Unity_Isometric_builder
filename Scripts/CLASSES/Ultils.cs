using UnityEngine;
using System.Collections;

public class Ultils{

	public static Sprite SetIsoPivot(Texture2D texture)
	{
		Vector2 size = new Vector2 (texture.width, texture.height);
		Rect rect = new Rect (0, 0, texture.width, texture.height);
		Sprite sprite = Sprite.Create (texture, rect, PivotEditForm.realSpriteOffset);
		return sprite;
	}
}

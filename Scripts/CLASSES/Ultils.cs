using UnityEngine;
using System.Collections;
using System;

public class Ultils{

	public static Sprite ChangeOffset(Texture2D texture)
	{
		return ChangeOffset(texture,PivotEditForm.realSpriteOffset);
	}

	public static Sprite ChangeOffset(Texture2D texture, Vector2 offset)
	{
		Vector2 size = new Vector2 (texture.width, texture.height);
		Rect rect = new Rect (0, 0, texture.width, texture.height);
		Sprite sprite = Sprite.Create (texture, rect, offset);
		return sprite;
	}

	public static Sprite ChangeOffset(Sprite spriteImage, Vector2 offset)
	{
		Vector2 size = new Vector2 (spriteImage.texture.width, spriteImage.texture.height);
		Rect rect = new Rect (0, 0, spriteImage.texture.width, spriteImage.texture.height);
		Sprite sprite = Sprite.Create (spriteImage.texture, rect, offset);
		return sprite;
	}

	public static IEnumerator LoadItemMenuImage (string url, Action<Sprite> callback)
	{
		Texture2D texture = null;
		WWW www = new WWW ("file:///" + url);
		yield return www;
		texture = www.texture;

		Sprite sprite = Ultils.ChangeOffset(texture);
		callback (sprite);
	}
		
}

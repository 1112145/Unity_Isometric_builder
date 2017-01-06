using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITransition: MonoBehaviour {
	
	private Toggle toggle;

	public void whatToggle(Transform target){
		toggle = target.GetComponent<Toggle>();
	}

	public void Toggle(Transform target){
		if (toggle.isOn){
			PopIn(target);
		}else{
			PopOut(target);
		}
	}



	public static void PopIn(Transform target){
		target.localScale = new Vector3(0,0,0);
		target.gameObject.SetActive(true);
		Tween.Begin(target);
		Tween.Easing = Ease.Berp;
		Tween.Scale( new Vector3(1,1,1), .333f);
		Tween.Run ();
	}

	public static void PopOutThenPopIn(Transform target)
	{
		Tween.Begin(target);
		Tween.Easing = Ease.QuinticIn;
		Tween.Scale (new Vector3(0.1f,1f,0.1f), .333f);
		Tween.Run(onFinished: callback => {
			target.gameObject.SetActive(false);
			target.localScale = new Vector3(0.1f,1f,0.1f);
			target.gameObject.SetActive(true);
			Tween.Begin(target);
			Tween.Easing = Ease.Berp;
			Tween.Scale( new Vector3(1,1,1), .333f);
			Tween.Run ();
		});
	}

	public static void ZoomInAndRotate(Transform target)
	{
		target.localScale = new Vector3(1,1,1);
		target.gameObject.SetActive(true);
		Tween.Begin(target);
		Tween.Easing = Ease.CubicIn;
		Tween.Scale( new Vector3(2f,2f,1), 2f);
		Tween.Run ();
	}

	public static void PopInAndZoom(Transform target){
		target.localScale = new Vector3(0,0,0);
		target.gameObject.SetActive(true);
		Tween.Begin(target);
		Tween.Easing = Ease.CubicIn;
		Tween.Scale( new Vector3(2f,2f,1), .777f);
		Tween.Run ();
		Tween.Begin(target);
		Tween.Scale( new Vector3(1f,1f,1), .777f);
		Tween.Run ();
	}

	public void Hide(Transform target){
		target.gameObject.SetActive(false);
	}

	public void Show(Transform target){
		target.gameObject.SetActive(true);
	}

	public static void PopOut(Transform target){
		Tween.Begin(target);
		Tween.Easing = Ease.QuinticIn;
		Tween.Scale (new Vector3(0f,0f,0f), .333f);
		Tween.Run(onFinished: callback => {
			target.gameObject.SetActive(false);
		});
	}

	public static void UI_FadeSlideUp(Transform target, float distance, float duration, bool includeChildren){
		target.position = new Vector3(target.transform.position.x, target.transform.position.y - distance, target.transform.position.z);
		target.gameObject.SetActive(true);
		Image target_img = target.gameObject.GetComponent<Image>();
		Image[] targets_img = (includeChildren) ? target.GetComponentsInChildren<Image>(true) : null;
		Text [] targets_text = (includeChildren) ? target.GetComponentsInChildren<Text>(true) : null;
		Tween.Begin(target);
		Tween.Duration = duration;
		Tween.Move(new Vector3(target.transform.position.x, target.transform.position.y + distance, target.transform.position.z), Space.World, Ease.Berp);
		Tween.Custom(value => {
			target_img.color = new Color(target_img.color.r, target_img.color.g, target_img.color.b, value);
			if (includeChildren){
				foreach(Image img in targets_img){
					img.color = new Color(img.color.r, img.color.g, img.color.b, value);
				}

				foreach(Text txt in targets_text){
					txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, value);
				}
			}
		});
		Tween.Run();
	}

	public static void UI_FadeIn(Transform target, float duration = .3f, bool includeChildren = false){
		target.gameObject.SetActive(true);
		Image target_img = target.gameObject.GetComponent<Image>();
		Image[] targets_img = (includeChildren) ? target.GetComponentsInChildren<Image>(true) : null;
		Text [] targets_text = (includeChildren) ? target.GetComponentsInChildren<Text>(true) : null;
		target_img.color = new Color(target_img.color.r, target_img.color.g, target_img.color.b, 0);
		Tween.Begin(target);
		Tween.Duration = duration;
		Tween.Custom(value => {
			target_img.color = new Color(target_img.color.r, target_img.color.g, target_img.color.b, value);
			if (includeChildren){
				foreach(Image img in targets_img){
					img.color = new Color(img.color.r, img.color.g, img.color.b, value);
				}
				
				foreach(Text txt in targets_text){
					txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, value);
				}
			}
		});
		Tween.Run();
	}

	public static void UI_FadeIn_Sprite(Transform target, float duration = .3f, bool includeChildren = false){
		target.gameObject.SetActive(true);
		SpriteRenderer target_img = target.gameObject.GetComponent<SpriteRenderer>();
		SpriteRenderer[] targets_img = (includeChildren) ? target.GetComponentsInChildren<SpriteRenderer>(true) : null;
		Text [] targets_text = (includeChildren) ? target.GetComponentsInChildren<Text>(true) : null;
		target_img.color = new Color(target_img.color.r, target_img.color.g, target_img.color.b, 0);
		Tween.Begin(target);
		Tween.Duration = duration;
		Tween.Custom(value => {
			target_img.color = new Color(target_img.color.r, target_img.color.g, target_img.color.b, value);
			if (includeChildren){
				foreach(SpriteRenderer img in targets_img){
					img.color = new Color(img.color.r, img.color.g, img.color.b, value);
				}

				foreach(Text txt in targets_text){
					txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, value);
				}
			}
		});
		Tween.Run();
	}

	public static void UI_FadeOut(Transform target, float duration = .3f, bool includeChildren = false)
	{
		target.gameObject.SetActive(true);
		Image target_img = target.gameObject.GetComponent<Image>();
		Image[] targets_img = (includeChildren) ? target.GetComponentsInChildren<Image>(true) : null;
		Text [] targets_text = (includeChildren) ? target.GetComponentsInChildren<Text>(true) : null;
		target_img.color = new Color(target_img.color.r, target_img.color.g, target_img.color.b, 0);
		Tween.Begin(target);
		Tween.Duration = duration;
		Tween.Custom(value => {
			target_img.color = new Color(target_img.color.r, target_img.color.g, target_img.color.b,1 - value);
			if (includeChildren){
				foreach(Image img in targets_img){
					img.color = new Color(img.color.r, img.color.g, img.color.b,1 - value);
				}

				foreach(Text txt in targets_text){
					txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, value);
				}
			}
		});
		Tween.Run();
	}

	public static void UI_FadeOut_Sprite(Transform target, float duration = .3f, bool includeChildren = false)
	{
		target.gameObject.SetActive(true);
		SpriteRenderer target_img = target.gameObject.GetComponent<SpriteRenderer>();
		SpriteRenderer[] targets_img = (includeChildren) ? target.GetComponentsInChildren<SpriteRenderer>(true) : null;
		Text [] targets_text = (includeChildren) ? target.GetComponentsInChildren<Text>(true) : null;
		target_img.color = new Color(target_img.color.r, target_img.color.g, target_img.color.b, 0);
		Tween.Begin(target);
		Tween.Duration = duration;
		Tween.Custom(value => {
			target_img.color = new Color(target_img.color.r, target_img.color.g, target_img.color.b,1 - value);
			if (includeChildren){
				foreach(SpriteRenderer img in targets_img){
					img.color = new Color(img.color.r, img.color.g, img.color.b,1 - value);
				}

				foreach(Text txt in targets_text){
					txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, value);
				}
			}
		});
		Tween.Run();
	}

    public static void ScaleIn(Transform target,Vector3 Max,Vector3 Min, float time)
    {
        Tween.Begin(target);
        Tween.Easing = Ease.Linear;
        Tween.Scale(Max, time);
        Tween.Run(onFinished: callback =>
        {
            ScaleOut(target,Max,Min,time);
        });
    }

    public static void ScaleOut(Transform target,Vector3 Max, Vector3 Min, float time)
    {
        Tween.Begin(target);
        Tween.Easing = Ease.Linear;
        Tween.Scale(Min, time);
        Tween.Run(onFinished: callback =>
        {
            ScaleIn(target,Max,Min,time);
        });
    }

    public static void ScaleLoop(Transform target,Vector3 Max,Vector3 Min, float time)
    {
        ScaleIn(target,Max,Min,time);
    }

    public static void Rotation(Transform target, Vector3 Positon, float time)
    {
        Tween.Begin(target);
        Tween.Easing = Ease.Linear;
        Tween.Rotate(Positon, time);
        Tween.Run();

    }

    public static void MoveIn(Transform target, Vector3 Position)
    {
        Tween.Begin(target);
        Tween.Easing = Ease.QuadraticOut;
        Tween.Move(Position);
        Tween.Run();
    }

    public static void MoveIn(Transform target, Vector3 Position, float duration)
    {
        Tween.Begin(target);
        Tween.Easing = Ease.QuadraticOut;
        Tween.Move(Position, duration);
        Tween.Run();
    }

    public static void MoveOut(Transform target, Vector3 Position)
    {
        Tween.Begin(target);
        Tween.Easing = Ease.QuadraticIn;
        Tween.Move(Position);
        Tween.Run();
    }

	public static void DropMenuLeftToRight(Transform target, float deltaMove)
	{
		Tween.Begin(target);
		Tween.Easing = Ease.QuadraticIn;
		Tween.Move(target.localPosition - new Vector3(deltaMove,0,0),Ease.Quadratic);
		Tween.Run();
	}


	public static void DropMenuRightToLeft(Transform target, float deltaMove)
	{
		Tween.Begin(target);
		Tween.Easing = Ease.QuadraticIn;
		Tween.Move(target.localPosition + new Vector3(deltaMove,0,0),Ease.Quadratic);
		Tween.Run();
	}


	public static void MoveLeft(Transform target, float deltaX)
	{
		Tween.Begin(target);
		Tween.Easing = Ease.QuadraticIn;
		Tween.Move(target.localPosition + new Vector3(deltaX,0,0),Space.Self);
		Tween.Run();
	}

	public static void MoveRight(Transform target, float deltaX)
	{
		Tween.Begin(target);
		Tween.Easing = Ease.QuadraticIn;
		Tween.Move(target.localPosition - new Vector3(deltaX,0,0),Space.Self);
		Tween.Run();
	}

	public static void MoveUp(Transform target, float deltaY)
	{
		Tween.Begin(target);
		Tween.Easing = Ease.QuadraticIn;
		Tween.Move(target.localPosition + new Vector3(0,deltaY,0),Space.Self);
		Tween.Run();
	}

	public static void MoveDown(Transform target, float deltaY)
	{
		Tween.Begin(target);
		Tween.Easing = Ease.QuadraticIn;
		Tween.Move(target.localPosition - new Vector3(0,deltaY,0),Space.Self);
		Tween.Run();
	}

}

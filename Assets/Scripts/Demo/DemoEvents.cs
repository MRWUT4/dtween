using UnityEngine;
using DavidOchmann.Animation;
    	
namespace Demo
{
	[ RequireComponent (typeof ( Mutate )) ]
	public class DemoEvents : MonoBehaviour
	{
		// Create private dTween instance.
		private DTween dTween = new DTween();


		public void Start()
		{ 
			tweenToDestination();
		}


		/** Tween to destintation position. */
		private void tweenToDestination()
		{
			Tween tween = dTween.To( GetComponent<Mutate>(), 1, new { x = 200, rotationZ = 180 }, Quad.EaseInOut );
			tween.OnStart += desinationTweenOnStartHandler;
			tween.OnComplete += desinationTweenOnCompleteHandler;
		}

		private void desinationTweenOnStartHandler(Tween tween)
		{
			Debug.Log( "desinationTweenOnStartHandler" );
		}

		private void desinationTweenOnCompleteHandler(Tween tween)
		{
			tweenToStart();
		}


		/** Tween to start position. */
		private void tweenToStart()
		{
			Tween tween = dTween.To( GetComponent<Mutate>(), 1, new { x = -200, rotationZ = 0 }, Quad.EaseInOut );
			tween.OnComplete += startTweenOnCompleteHandler;
		}

		private void startTweenOnCompleteHandler(Tween tween)
		{
			tweenToDestination();
		}


		/** Update loop. */
		public void FixedUpdate()
		{
			// Update dTween on the FixedUpdate interval. 
			// DTween only updates if you call this function.
			dTween.Update();
		}
	}
}
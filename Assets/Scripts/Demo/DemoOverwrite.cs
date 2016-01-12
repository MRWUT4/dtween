using UnityEngine;
using DavidOchmann.Animation;
    	
namespace Demo
{
	[ RequireComponent (typeof ( Mutate )) ]
	public class DemoOverwrite : MonoBehaviour
	{
		// Create private dTween instance. With overwrite set to true.
		private DTween dTween = new DTween( true );


		public void Start()
		{
			startTickAnimation();
		}

		private void startTickAnimation()
		{
			Tween tweenTick = dTween.To( 1 );
			tweenTick.OnComplete += tweenTickCompleteHandler;
		}

		private void tweenTickCompleteHandler(Tween tween)
		{
			Debug.Log( "jo" );
			startTickAnimation();
		}

		public void FixedUpdate()
		{
			// Update dTween on the FixedUpdate interval. 
			// DTween only updates if you call this function.
			dTween.Update();
		}
	}
}
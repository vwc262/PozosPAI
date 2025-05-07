using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("VWC")]
	public class DateTimeToSunRotation : FsmStateAction
	{

		public FsmString DateTime;
		public FsmFloat offset;
		public FsmFloat SunRotation;
		
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			
		}

		public override void OnEnter()
		{
			DoAction();
		    
			if (!everyFrame)
			{
				Finish();
			}
		}
	    
		public override void OnUpdate()
		{
			DoAction();
		}
	    
		public void DoAction()
		{
			var timeData = DateTime.Value.Split(":");
			var hour = float.Parse(timeData[0]);
			var minute = float.Parse(timeData[1]);
			var time = hour + minute / 60;

			SunRotation.Value = OffsetSpan(time, 0, 24, 0 + offset.Value, 360 + offset.Value);
		}


		public float OffsetSpan(float x, float a, float b, float c, float d)
		{
			float y = (x - a) * ((d-c) / (b-a)) + c;

			return y;
		}
	}

}

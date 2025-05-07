using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("VWC")]
	public class CarruselPozo : FsmStateAction
	{
		[ArrayEditor(VariableType.GameObject)]
		public FsmArray carruselElements;
		private List<TransformData> _transformDatas = new List<TransformData>();

		public FsmFloat lamda;

		public FsmInt direction;

		private int _shrinkIndex;
		private int _expandIndex;

		public FsmFloat backScale;
		
		public override void OnEnter()
		{
			_transformDatas.Clear();
			for (int i = 0; i < carruselElements.Values.Length; i++)
			{
				_transformDatas.Add(
					new TransformData()
					{
						pos = ((GameObject)carruselElements.Values[i]).transform.localPosition,
						rotX = SetAngle180(((GameObject)carruselElements.Values[i]).transform.localEulerAngles.x),
						rotY = SetAngle180(((GameObject)carruselElements.Values[i]).transform.localEulerAngles.y),
						rotZ = SetAngle180(((GameObject)carruselElements.Values[i]).transform.localEulerAngles.z),
						alpha = ((GameObject)carruselElements.Values[i]).
							GetComponent<BoyCarruselElement>().alpha,
						elementIndex = ((GameObject)carruselElements.Values[i]).
							GetComponent<BoyCarruselElement>().carruselElementIndex,
					});
			}

			
			
			var otherIndex = 0;
			_expandIndex = carruselElements.Values.Length - 1;
			
			if (direction.Value < 0)
			{
				_shrinkIndex = 0;
				otherIndex = 0;
			}
			if(direction.Value > 0)
			{
				_shrinkIndex = carruselElements.Values.Length - 2;
				otherIndex = carruselElements.Values.Length - 2;
			}

			((GameObject)carruselElements.Values[_expandIndex]).
				GetComponent<BoyCarruselElement>().carruselElementIndex = 
				((GameObject)carruselElements.Values[otherIndex]).
				GetComponent<BoyCarruselElement>().carruselElementIndex;

		}

		
		public override void OnUpdate()
		{
			for (int i = 0; i < carruselElements.Values.Length; i++)
			{
				var next_i = i;
				if(direction.Value > 0)
					next_i = (i + 1)%carruselElements.Values.Length;
				if(direction.Value < 0)
					next_i = (i - 1) % carruselElements.Values.Length;
				if (next_i < 0)
					next_i += carruselElements.Values.Length;
				
				
				((GameObject)carruselElements.Values[i]).transform.localPosition =
					Vector3.Lerp(_transformDatas[i].pos,_transformDatas[next_i].pos, lamda.Value);
				Vector3 rotVect = new Vector3(
					Mathf.Lerp(_transformDatas[i].rotX, _transformDatas[next_i].rotX, lamda.Value),
					Mathf.Lerp(_transformDatas[i].rotY, _transformDatas[next_i].rotY, lamda.Value),
					Mathf.Lerp(_transformDatas[i].rotZ, _transformDatas[next_i].rotZ, lamda.Value));
				((GameObject)carruselElements.Values[i]).transform.localEulerAngles = rotVect;
				((GameObject)carruselElements.Values[i]).
					GetComponent<BoyCarruselElement>().alpha =
					Mathf.Lerp(_transformDatas[i].alpha,_transformDatas[next_i].alpha, lamda.Value);

			}
			
			((GameObject)carruselElements.Values[_shrinkIndex]).
				GetComponent<BoyCarruselElement>().
				SetScaleFactor(Mathf.Lerp(1,backScale.Value,lamda.Value));
			((GameObject)carruselElements.Values[_expandIndex]).
				GetComponent<BoyCarruselElement>().
				SetScaleFactor(Mathf.Lerp(backScale.Value, 1,lamda.Value));
		}

		
		public override void OnExit()
		{
			var index0 = ((GameObject)carruselElements.Values[0]).GetComponent<BoyCarruselElement>()
				.carruselElementIndex;
			var indexLast = ((GameObject)carruselElements.Values[^1]).GetComponent<BoyCarruselElement>()
				.carruselElementIndex;
			for (int i = 0; i < carruselElements.Values.Length; i++)
			{
				((GameObject)carruselElements.Values[i]).transform.localPosition = _transformDatas[i].pos;
				Vector3 rotVect = new Vector3(
					_transformDatas[i].rotX, 
					_transformDatas[i].rotY,
					_transformDatas[i].rotZ);
				((GameObject)carruselElements.Values[i]).transform.localEulerAngles = rotVect;
				((GameObject)carruselElements.Values[i]).
					GetComponent<BoyCarruselElement>().alpha = _transformDatas[i].alpha;
				
				var prev_i = i;
				if(direction.Value < 0)// al reves
					prev_i = (i + 1)%carruselElements.Values.Length;
				if(direction.Value > 0)
					prev_i = (i - 1) % carruselElements.Values.Length;
				if (prev_i < 0)
					prev_i += carruselElements.Values.Length;
				
				((GameObject)carruselElements.Values[i]).
					GetComponent<BoyCarruselElement>().carruselElementIndex =
					_transformDatas[prev_i].elementIndex;

			}

			if (direction.Value < 0)
				((GameObject)carruselElements.Values[^1]).GetComponent<BoyCarruselElement>()
					.carruselElementIndex = index0;
			if (direction.Value > 0)
				((GameObject)carruselElements.Values[0]).GetComponent<BoyCarruselElement>()
					.carruselElementIndex = indexLast;
			
			((GameObject)carruselElements.Values[_shrinkIndex]).
				GetComponent<BoyCarruselElement>().
				SetScaleFactor(1);
			((GameObject)carruselElements.Values[_expandIndex]).
				GetComponent<BoyCarruselElement>().
				SetScaleFactor(backScale.Value);
			
			
			
			
		}

		private float SetAngle180(float angle)
		{
			if (angle > 180)
				angle -= 360;
			return angle;
		}
	}
}

public class TransformData
{
	public Vector3 pos;
	public float rotX;
	public float rotY;
	public float rotZ;
	public float alpha;
	public int elementIndex;
	public bool mainWindow;
}
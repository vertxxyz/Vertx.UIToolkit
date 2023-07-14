using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using PointerType = UnityEngine.UIElements.PointerType;

namespace Vertx.UIToolkit
{
	internal static class BetterFieldMouseDraggerExtensions
	{
		// private void EnableLabelDragger(bool enable)
		private static MethodInfo _enableLabelDraggerMethod;
		private static MethodInfo EnableLabelDraggerMethod
			=> _enableLabelDraggerMethod ??= typeof(TextValueField<>).GetMethod("EnableLabelDragger", BindingFlags.NonPublic | BindingFlags.Instance);

		private static readonly object[] falseParameter = { false };
		private static readonly object[] trueParameter = { true };

		internal static void ApplyBetterFieldMouseDragger<TValueType>(TextValueField<TValueType> field)
		{
			if (field.isDelayed)
			{
				Debug.LogWarning($"Sorry, {nameof(BetterFieldMouseDragger<TValueType>)} does not support delayed fields due to it being a hacky implementation.\n" +
				          "Hopefully unity will soon fix their implementation and we can live better lives.");
				return;
			}
			
			_enableLabelDraggerMethod = typeof(TextValueField<TValueType>).GetMethod("EnableLabelDragger", BindingFlags.NonPublic | BindingFlags.Instance);
			
			EnableLabelDraggerMethod.Invoke(field, falseParameter);
			var draggerField = typeof(TextValueField<TValueType>).GetField("m_Dragger", BindingFlags.NonPublic | BindingFlags.Instance);
			draggerField!.SetValue(field, new BetterFieldMouseDragger<TValueType>(field));
			EnableLabelDraggerMethod.Invoke(field, trueParameter);
		}
	}

	/// <summary>
	/// Provides dragging on a visual element to change a value field.
	/// This dragger uses the distance travelled, not the current mouse delta to modify the values.
	/// This means it can be used in combination with a field that supports rounding.
	/// https://forum.unity.com/threads/fieldmousedragger-improvement.1225911/
	/// </summary>
	/// <description>
	/// <para>To create a field mouse dragger use <see cref="Vertx.UIToolkit.BetterFieldMouseDragger{T}.Apply{T}"/></para>
	/// and then set the drag zone using <see cref="UnityEngine.UIElements.BaseFieldMouseDragger.SetDragZone(VisualElement)"/>
	/// To create a field mouse dragger use <see cref="Vertx.UIToolkit.BetterFieldMouseDragger{T}"/>
	/// </description>
	public class BetterFieldMouseDragger<T> : BaseFieldMouseDragger
	{
		/// <summary>
		/// BetterFieldMouseDragger's constructor.
		/// </summary>
		/// <param name="drivenField">The field.</param>
		internal BetterFieldMouseDragger(IValueField<T> drivenField)
		{
			_drivenField = drivenField;
			_dragElement = null;
			_dragHotZone = new Rect(0, 0, -1, -1);
			Dragging = false;
		}
		
		/// <summary>
		/// <para>
		/// A helper for applying <see cref="BetterFieldMouseDragger{T}"/> to a composite field.
		/// </para>
		/// ApplyBetterFieldMouseDragger&lt;Vector3Field, Vector3, FloatField, float&gt;(); would apply to a <see cref="Vector3Field"/> for example.
		/// </summary>
		/// <param name="field"></param>
		/// <typeparam name="TField">The VisualElement field the composite field contains.</typeparam>
		public static void Apply<TField>(TField field)
			where TField : TextValueField<T>, new() =>
			BetterFieldMouseDraggerExtensions.ApplyBetterFieldMouseDragger(field);

		/// <summary>
		/// <para>
		/// A helper for applying <see cref="BetterFieldMouseDragger{T}"/> to a composite field.
		/// </para>
		/// Apply&lt;Vector3Field, Vector3, FloatField&gt;(); would apply to a <see cref="Vector3Field"/> for example.
		/// </summary>
		/// <param name="compositeField">A composite field to apply an instance of BetterFieldMouseDragger to.</param>
		/// <typeparam name="TCompositeField">The type of composite field.</typeparam>
		/// <typeparam name="TValueType">The value type associated with the field.</typeparam>
		/// <typeparam name="TField">The VisualElement field the composite field contains.</typeparam>
		public static void Apply<TCompositeField, TValueType, TField>(TCompositeField compositeField)
			where TCompositeField : BaseCompositeField<TValueType, TField, T>
			where TField : TextValueField<T>, new()
		{
			// ReSharper disable once ConvertClosureToMethodGroup - as to not allocate.
			compositeField.Query<TField>().ForEach(a => BetterFieldMouseDraggerExtensions.ApplyBetterFieldMouseDragger(a));
		}

		private readonly IValueField<T> _drivenField;
		private VisualElement _dragElement;
		private Rect _dragHotZone;

		/// <summary>
		/// Is dragging.
		/// </summary>
		public bool Dragging { get; set; }
		/// <summary>
		/// Start value before drag.
		/// </summary>
		public T StartValue { get; set; }

		/// <inheritdoc />
		public sealed override void SetDragZone(VisualElement dragElement, Rect hotZone)
		{
			if (_dragElement != null)
			{
				_dragElement.UnregisterCallback<PointerDownEvent>(UpdateValueOnPointerDown);
				_dragElement.UnregisterCallback<PointerUpEvent>(UpdateValueOnPointerUp);
				_dragElement.UnregisterCallback<KeyDownEvent>(UpdateValueOnKeyDown);
			}

			_dragElement = dragElement;
			_dragHotZone = hotZone;

			if (_dragElement != null)
			{
				Dragging = false;
				_dragElement.RegisterCallback<PointerDownEvent>(UpdateValueOnPointerDown);
				_dragElement.RegisterCallback<PointerUpEvent>(UpdateValueOnPointerUp);
				_dragElement.RegisterCallback<KeyDownEvent>(UpdateValueOnKeyDown);
			}
		}

		private bool CanStartDrag(int button, Vector2 localPosition) =>
			button == 0 && (_dragHotZone.width < 0 || _dragHotZone.height < 0 ||
			                _dragHotZone.Contains(_dragElement.WorldToLocal(localPosition)));

		private void UpdateValueOnPointerDown(PointerDownEvent evt)
		{
			if (!CanStartDrag(evt.button, evt.localPosition))
				return;

			// We want to allow dragging when using a mouse in any context and when in an Editor context with any pointer type.
			if (evt.pointerType == PointerType.mouse)
			{
				_dragElement.CaptureMouse();
				ProcessDownEvent(evt);
			}
			else if (_dragElement.panel.contextType == ContextType.Editor)
			{
				evt.StopPropagation();
				_dragElement.CapturePointer(evt.pointerId);
				ProcessDownEvent(evt);
			}
		}

		private Type _baseVisualElementPanelType;
		private Type BaseVisualElementPanelType => _baseVisualElementPanelType ??= Type.GetType("UnityEngine.UIElements.BaseVisualElementPanel,UnityEngine");

		private Type _uiElementsBridgeType;
		private Type UIElementsBridgeType => _uiElementsBridgeType ??= Type.GetType("UnityEngine.UIElements.UIElementsBridge,UnityEngine");

		private MethodInfo _setWantsMouseJumping;
		private MethodInfo SetWantsMouseJumping => _setWantsMouseJumping ??= UIElementsBridgeType.GetMethod("SetWantsMouseJumping", BindingFlags.Public | BindingFlags.Instance);

		private Action<IPanel, int> _processPointerCapture;
		private Action<IPanel, int> ProcessPointerCapture
			=> _processPointerCapture ??= (Action<IPanel, int>)Delegate.CreateDelegate(
				typeof(Action<IPanel, int>),
				typeof(PointerCaptureHelper).GetMethod("ProcessPointerCapture", BindingFlags.NonPublic | BindingFlags.Static)!
			);

		private PropertyInfo _uiElementsBridgeProperty;
		private PropertyInfo UiElementsBridgeProperty =>
			_uiElementsBridgeProperty ??= BaseVisualElementPanelType.GetProperty("uiElementsBridge", BindingFlags.NonPublic | BindingFlags.Instance);

		private static readonly object[] zeroParameter = { 0 };
		private static readonly object[] oneParameter = { 1 };

		private Vector2 _totalDelta;

		private void ProcessDownEvent(EventBase evt)
		{
			// Make sure no other elements can capture the mouse!
			evt.StopPropagation();

			Dragging = true;
			_dragElement.RegisterCallback<PointerMoveEvent>(UpdateValueOnPointerMove);
			StartValue = _drivenField.value;
			_totalDelta = Vector2.zero;

			_drivenField.StartDragging();
			InvokeSetWantsMouseJumping(oneParameter);
		}

		private void UpdateValueOnPointerMove(PointerMoveEvent evt) => ProcessMoveEvent(evt.shiftKey, evt.altKey, evt.deltaPosition);

		private void ProcessMoveEvent(bool shiftKey, bool altKey, Vector2 deltaPosition)
		{
			if (!Dragging)
				return;
			DeltaSpeed s = shiftKey ? DeltaSpeed.Fast : (altKey ? DeltaSpeed.Slow : DeltaSpeed.Normal);
			_totalDelta += deltaPosition;
			_totalDelta.y = 0;
			if (_drivenField is BaseField<T> field)
			{
				field.SetValueWithoutNotify(StartValue);
				_drivenField.ApplyInputDeviceDelta(_totalDelta, s, StartValue);
			}
			else
			{
				Debug.Log($"Field type {_drivenField.GetType()} is not supported. Falling back to original behaviour.");
				_drivenField.ApplyInputDeviceDelta(deltaPosition, s, StartValue);
			}
		}

		private void UpdateValueOnPointerUp(PointerUpEvent evt)
		{
			ProcessUpEvent(evt, evt.pointerId);
		}

		private void ProcessUpEvent(EventBase evt, int pointerId)
		{
			if (!Dragging)
				return;
			Dragging = false;
			_dragElement.UnregisterCallback<PointerMoveEvent>(UpdateValueOnPointerMove);
			_dragElement.ReleasePointer(pointerId);
			if (evt is IMouseEvent)
				ProcessPointerCapture(_dragElement.panel, PointerId.mousePointerId);

			InvokeSetWantsMouseJumping(zeroParameter);
			_drivenField.StopDragging();
		}

		private void UpdateValueOnKeyDown(KeyDownEvent evt)
		{
			if (!Dragging || evt.keyCode != KeyCode.Escape)
				return;
			Dragging = false;
			_drivenField.value = StartValue;
			_drivenField.StopDragging();
			IPanel panel = (evt.target as VisualElement)?.panel;
			panel.ReleasePointer(PointerId.mousePointerId);
			InvokeSetWantsMouseJumping(zeroParameter);
		}

		private void InvokeSetWantsMouseJumping(object[] parameter)
		{
			if (_dragElement.panel == null || !_dragElement.panel.GetType().IsSubclassOf(BaseVisualElementPanelType))
				return;
			// (_dragElement.panel as BaseVisualElementPanel)?.uiElementsBridge?.SetWantsMouseJumping(0);
			object bridge = UiElementsBridgeProperty.GetValue(_dragElement.panel);
			if (bridge != null)
				SetWantsMouseJumping.Invoke(bridge, parameter);
		}
	}
}
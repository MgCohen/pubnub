// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.22
// 

using Colyseus.Schema;
using Action = System.Action;

namespace Server {
	public partial class PlayerAction : Schema {
		[Type(0, "number")]
		public float actionID = default(float);

		[Type(1, "number")]
		public float target = default(float);

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<float> __actionIDChange;
		public Action OnActionIDChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.actionID));
			__actionIDChange += __handler;
			if (__immediate && this.actionID != default(float)) { __handler(this.actionID, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(actionID));
				__actionIDChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __targetChange;
		public Action OnTargetChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.target));
			__targetChange += __handler;
			if (__immediate && this.target != default(float)) { __handler(this.target, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(target));
				__targetChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(actionID): __actionIDChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(target): __targetChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				default: break;
			}
		}
	}
}

// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.22
// 

using Colyseus.Schema;
using Action = System.Action;

namespace Server {
	public partial class PlayerTurn : Schema {
		[Type(0, "array", typeof(ArraySchema<PlayerAction>))]
		public ArraySchema<PlayerAction> actions = new ArraySchema<PlayerAction>();

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<ArraySchema<PlayerAction>> __actionsChange;
		public Action OnActionsChange(PropertyChangeHandler<ArraySchema<PlayerAction>> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.actions));
			__actionsChange += __handler;
			if (__immediate && this.actions != null) { __handler(this.actions, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(actions));
				__actionsChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(actions): __actionsChange?.Invoke((ArraySchema<PlayerAction>) change.Value, (ArraySchema<PlayerAction>) change.PreviousValue); break;
				default: break;
			}
		}
	}
}

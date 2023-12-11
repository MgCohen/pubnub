// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.22
// 

using Colyseus.Schema;
using Action = System.Action;

namespace Server {
	public partial class Player : Schema {
		[Type(0, "string")]
		public string playerID = default(string);

		[Type(1, "number")]
		public float teamID = default(float);

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<string> __playerIDChange;
		public Action OnPlayerIDChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.playerID));
			__playerIDChange += __handler;
			if (__immediate && this.playerID != default(string)) { __handler(this.playerID, default(string)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(playerID));
				__playerIDChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __teamIDChange;
		public Action OnTeamIDChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.teamID));
			__teamIDChange += __handler;
			if (__immediate && this.teamID != default(float)) { __handler(this.teamID, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(teamID));
				__teamIDChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(playerID): __playerIDChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
				case nameof(teamID): __teamIDChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				default: break;
			}
		}
	}
}

// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.22
// 

using Colyseus.Schema;
using Action = System.Action;

namespace Server {
	public partial class Team : Schema {
		[Type(0, "number")]
		public float teamID = default(float);

		[Type(1, "array", typeof(ArraySchema<Player>))]
		public ArraySchema<Player> players = new ArraySchema<Player>();

		/*
		 * Support for individual property change callbacks below...
		 */

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

		protected event PropertyChangeHandler<ArraySchema<Player>> __playersChange;
		public Action OnPlayersChange(PropertyChangeHandler<ArraySchema<Player>> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.players));
			__playersChange += __handler;
			if (__immediate && this.players != null) { __handler(this.players, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(players));
				__playersChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(teamID): __teamIDChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(players): __playersChange?.Invoke((ArraySchema<Player>) change.Value, (ArraySchema<Player>) change.PreviousValue); break;
				default: break;
			}
		}
	}
}

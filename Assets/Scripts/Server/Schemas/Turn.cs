// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.22
// 

using Colyseus.Schema;
using Action = System.Action;

namespace Server {
	public partial class Turn : Schema {
		[Type(0, "number")]
		public float turnNumber = default(float);

		[Type(1, "map", typeof(MapSchema<PlayerTurn>))]
		public MapSchema<PlayerTurn> playerTurns = new MapSchema<PlayerTurn>();

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<float> __turnNumberChange;
		public Action OnTurnNumberChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.turnNumber));
			__turnNumberChange += __handler;
			if (__immediate && this.turnNumber != default(float)) { __handler(this.turnNumber, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(turnNumber));
				__turnNumberChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<MapSchema<PlayerTurn>> __playerTurnsChange;
		public Action OnPlayerTurnsChange(PropertyChangeHandler<MapSchema<PlayerTurn>> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.playerTurns));
			__playerTurnsChange += __handler;
			if (__immediate && this.playerTurns != null) { __handler(this.playerTurns, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(playerTurns));
				__playerTurnsChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(turnNumber): __turnNumberChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(playerTurns): __playerTurnsChange?.Invoke((MapSchema<PlayerTurn>) change.Value, (MapSchema<PlayerTurn>) change.PreviousValue); break;
				default: break;
			}
		}
	}
}

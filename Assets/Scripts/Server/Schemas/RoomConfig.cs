// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.22
// 

using Colyseus.Schema;
using Action = System.Action;

namespace Server {
	public partial class RoomConfig : Schema {
		[Type(0, "number")]
		public float turnLength = default(float);

		[Type(1, "number")]
		public float playLength = default(float);

		[Type(2, "number")]
		public float seed = default(float);

		[Type(3, "number")]
		public float maxPlayers = default(float);

		[Type(4, "number")]
		public float teamCount = default(float);

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<float> __turnLengthChange;
		public Action OnTurnLengthChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.turnLength));
			__turnLengthChange += __handler;
			if (__immediate && this.turnLength != default(float)) { __handler(this.turnLength, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(turnLength));
				__turnLengthChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __playLengthChange;
		public Action OnPlayLengthChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.playLength));
			__playLengthChange += __handler;
			if (__immediate && this.playLength != default(float)) { __handler(this.playLength, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(playLength));
				__playLengthChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __seedChange;
		public Action OnSeedChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.seed));
			__seedChange += __handler;
			if (__immediate && this.seed != default(float)) { __handler(this.seed, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(seed));
				__seedChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __maxPlayersChange;
		public Action OnMaxPlayersChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.maxPlayers));
			__maxPlayersChange += __handler;
			if (__immediate && this.maxPlayers != default(float)) { __handler(this.maxPlayers, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(maxPlayers));
				__maxPlayersChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __teamCountChange;
		public Action OnTeamCountChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.teamCount));
			__teamCountChange += __handler;
			if (__immediate && this.teamCount != default(float)) { __handler(this.teamCount, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(teamCount));
				__teamCountChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(turnLength): __turnLengthChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(playLength): __playLengthChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(seed): __seedChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(maxPlayers): __maxPlayersChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(teamCount): __teamCountChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				default: break;
			}
		}
	}
}

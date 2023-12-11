// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.22
// 

using Colyseus.Schema;
using Action = System.Action;

namespace Server {
	public partial class MyRoomState : Schema {
		[Type(0, "map", typeof(MapSchema<Player>))]
		public MapSchema<Player> players = new MapSchema<Player>();

		[Type(1, "map", typeof(MapSchema<Team>))]
		public MapSchema<Team> teams = new MapSchema<Team>();

		[Type(2, "array", typeof(ArraySchema<Turn>))]
		public ArraySchema<Turn> turns = new ArraySchema<Turn>();

		[Type(3, "ref", typeof(RoomConfig))]
		public RoomConfig config = new RoomConfig();

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<MapSchema<Player>> __playersChange;
		public Action OnPlayersChange(PropertyChangeHandler<MapSchema<Player>> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.players));
			__playersChange += __handler;
			if (__immediate && this.players != null) { __handler(this.players, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(players));
				__playersChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<MapSchema<Team>> __teamsChange;
		public Action OnTeamsChange(PropertyChangeHandler<MapSchema<Team>> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.teams));
			__teamsChange += __handler;
			if (__immediate && this.teams != null) { __handler(this.teams, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(teams));
				__teamsChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<ArraySchema<Turn>> __turnsChange;
		public Action OnTurnsChange(PropertyChangeHandler<ArraySchema<Turn>> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.turns));
			__turnsChange += __handler;
			if (__immediate && this.turns != null) { __handler(this.turns, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(turns));
				__turnsChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<RoomConfig> __configChange;
		public Action OnConfigChange(PropertyChangeHandler<RoomConfig> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.config));
			__configChange += __handler;
			if (__immediate && this.config != null) { __handler(this.config, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(config));
				__configChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(players): __playersChange?.Invoke((MapSchema<Player>) change.Value, (MapSchema<Player>) change.PreviousValue); break;
				case nameof(teams): __teamsChange?.Invoke((MapSchema<Team>) change.Value, (MapSchema<Team>) change.PreviousValue); break;
				case nameof(turns): __turnsChange?.Invoke((ArraySchema<Turn>) change.Value, (ArraySchema<Turn>) change.PreviousValue); break;
				case nameof(config): __configChange?.Invoke((RoomConfig) change.Value, (RoomConfig) change.PreviousValue); break;
				default: break;
			}
		}
	}
}

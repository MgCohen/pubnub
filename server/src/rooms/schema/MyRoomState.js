const schema = require('@colyseus/schema');
const {Player} = require('./Player');
const {Team} = require('./Team');
const {Turn, PlayerTurn, Action} = require('./Turn');
const { RoomConfig } = require('./RoomConfig');
const MapSchema = schema.MapSchema;
const ArraySchema = schema.ArraySchema;


class MyRoomState extends schema.Schema {
  constructor(){
    super();
    this.players = new MapSchema();
    this.teams = new MapSchema();
    this.turns = new ArraySchema();
    this.currentTurn = new Turn();
    this.config = new RoomConfig();
  }
}

schema.defineTypes(MyRoomState, {
  players: {map : Player},
  teams: {map: Team},
  turns: [Turn],
  config: RoomConfig,
  //currentTurn: Turn,
});

exports.MyRoomState = MyRoomState;
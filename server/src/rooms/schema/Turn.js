const schema = require('@colyseus/schema');
const MapSchema = schema.MapSchema;
const ArraySchema = schema.ArraySchema;
const { Player } = require('./Player');

class PlayerAction extends schema.Schema {
  constructor() {
    super();
  }
}

schema.defineTypes(PlayerAction, {
  actionID: "number",
  target: "number",
});

class PlayerTurn extends schema.Schema {
  constructor() {
    super();
    this.actions = new ArraySchema();
  }

  Parse(actions){
    if(actions){
      actions.forEach(element => {
        var action = new PlayerAction().assign({
          actionID: element.action,
          target: element.target
        });
        this.actions.push(action);
      });
    }
    console.log("parsed actions: " + this.actions.length);
    return this;
  }
}

schema.defineTypes(PlayerTurn, {
  actions: [PlayerAction],
});

class Turn extends schema.Schema {
  constructor() {
    super();
    this.turnNumber = 0;
    this.playerTurns = new MapSchema();
  }

  RegisterPlayerTurn(player, actions){
    var turn = new PlayerTurn().Parse(actions);
    this.playerTurns.set(player.playerID, turn);
  }

  CheckTurns(){
    return this.playerTurns.size;
  }

  WithNumber(turnNumber){
    this.turnNumber = turnNumber;
    return this;
  }
}

schema.defineTypes(Turn, {
  turnNumber: "number",
  playerTurns: {map: PlayerTurn},
});


exports.Turn = Turn;
exports.PlayerTurn = PlayerTurn;
exports.PlayerAction = PlayerAction;

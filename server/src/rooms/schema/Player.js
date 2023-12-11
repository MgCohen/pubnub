const schema = require('@colyseus/schema');

class Player extends schema.Schema {
  constructor(){
    super();
  }
  }
  
  schema.defineTypes(Player, {
    playerID: "string",
    teamID: "number",
  });

exports.Player = Player;


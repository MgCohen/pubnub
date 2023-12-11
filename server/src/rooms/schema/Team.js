const schema = require('@colyseus/schema');
const Schema = schema.Schema;
const ArraySchema = schema.ArraySchema;
const { Player } = require('./Player');

class Team extends schema.Schema {
    constructor() {
        super();
        this.players = new ArraySchema();
    }
}

schema.defineTypes(Team, {
    teamID: "number",
    players: [Player]
});

exports.Team = Team;


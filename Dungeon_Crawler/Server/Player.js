var shortID = require("shortid");
var Vector3 = require("./Vector3.js");

module.exports = class Player{
    constructor(){
        this.id = shortID.generate();
        this.position = new Vector3(0, 0, 0);
        this.rotation = new Vector3(0, 0, 0);
    }
}

module.exports = class Vector3{
    constructor(x,y,z){
        this.x = x;
        this.y = y;
        this.z = z;
        this.sub = (vec) =>{
            this.x -= vec.x;
            this.y -= vec.y;
            this.z -= vec.z;
        }
        this.add = (vec) =>{
            this.x += vec.x;
            this.y += vec.y;
            this.z += vec.z;
        }
    }
}
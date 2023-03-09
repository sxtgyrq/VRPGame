var targetShow = {
    // objCount: 10,
    startT: 0,
    p: new THREE.Vector3(0, 0, 0),//three.js 左手坐标系
    leftY: 0,//three.js 左手坐标系
    leftZ: 0,//three.js 左手坐标系
    draw: function (x, y, h) {
        // sum = sum * 2 + 1;
        this.p = new THREE.Vector3(x, h, -y);
        if (objMain.targetGroup.children.length == 0) {

            var geometry = new THREE.RingGeometry(1, 0.5, 32, 1);
            var material = new THREE.MeshBasicMaterial(
                {
                    color: 0xffff00,
                    side: THREE.DoubleSide,
                    opacity: 0.3,
                    transparent: true,

                });
            var mesh = new THREE.Mesh(geometry, material);
            mesh.position.set(x, h, -y);
            mesh.rotation.set(Math.PI / 2, 0, 0, 'XYZ');
            objMain.targetGroup.add(mesh);
        }
        else {
            objMain.targetGroup.children[0].position.set(x, h, -y);
        }

    },
    animate: function () {
        const deltaT = 5000;
        var l = objMain.absorbGroup.children.length;
        var selfCar = objMain.carGroup.getObjectByName('car_' + objMain.indexKey);
        if (selfCar == undefined) { }
        else if (objMain.targetGroup.children.length == 1) {
            var t = Date.now() % deltaT / deltaT;
            if (t < 0.5) {
                //https://threejs.org/docs/#api/en/math/Vector3
                var percent = t / 0.5;
                //var newP = selfCar.position.add(this.p.add(selfCar.position.negate()).multiplyScalar(percent));
                //this.p.addScaledVector(selfCar.position, percent);
                //selfCar.position.add(this.p.)
                objMain.targetGroup.children[0].scale.set(1, 1, 1);
                // objMain.targetGroup.children[0].position.set(newP.x, newP.y, newP.z);

                var newX = this.p.x - (this.p.x - selfCar.position.x) * percent;
                var newY = this.p.y - (this.p.y - selfCar.position.y) * percent;
                var newZ = this.p.z - (this.p.z - selfCar.position.z) * percent;
                objMain.targetGroup.children[0].position.set(newX, newY, newZ);
                //var x = selfCar.position.x +
                //    objMain.targetGroup.children[0].position.set()
            }
            else {
                var l = objMain.mainF.getLength(this.p, selfCar.position);
                var percent = (t - 0.5) / 0.5;
                var scale = l / 2 * (percent) + 1;
                objMain.targetGroup.children[0].scale.set(scale, scale, scale);
                objMain.targetGroup.children[0].position.set(this.p.x, this.p.y, this.p.z);
            }
            //  objMain.targetGroup.children[0].position.set(x, 0, -y)
        }
        // this.animate2();
    },

}


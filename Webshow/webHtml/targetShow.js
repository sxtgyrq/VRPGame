var targetShow = {
    // objCount: 10,
    startT: 0,
    draw: function (x, y) {
        // sum = sum * 2 + 1;
        if (objMain.targetGroup.children.length == 0) {

            var geometry = new THREE.RingGeometry(1, 0.98, 32, 1);
            var material = new THREE.MeshBasicMaterial({ color: 0xffff00, side: THREE.DoubleSide });
            var mesh = new THREE.Mesh(geometry, material);
            mesh.position.set(x, 0, -y);
            mesh.rotation.set(Math.PI / 2, 0, 0, 'XYZ');
            objMain.targetGroup.add(mesh);
        }
        else {
            objMain.targetGroup.children[0].position.set(x, 0, -y);
        }

    },
    animate: function () {
        const deltaT = 10000;
        var l = objMain.absorbGroup.children.length;
        var selfCar = objMain.carGroup.getObjectByName('car_' + objMain.indexKey);
        if (selfCar == undefined) { }
        else if (objMain.targetGroup.children.length == 1) {
            var l = objMain.mainF.getLength(objMain.targetGroup.children[0].position, selfCar.position);
            var scale = l / 2 * (Date.now() % deltaT / deltaT);
            objMain.targetGroup.children[0].scale.set(scale, scale, scale);
            //  objMain.targetGroup.children[0].position.set(x, 0, -y)
        }
        // this.animate2();
    },

}


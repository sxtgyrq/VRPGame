var moneyAbsorb = {
    // objCount: 10,
    startT: 0,
    copyModel: function (sum) {
        // sum = sum * 2 + 1;
        //m = objMain.rmbModel['rmb1'].clone();
        var rmb = 'rmb1';
        if (sum < 1) {
            sum = 1;
            rmb = 'rmb1';
        }
        else if (sum < 10) {
            sum = 2;
            rmb = 'rmb1';
        }
        else if (sum < 20) {
            sum = 3;
            rmb = 'rmb5';
        }
        else if (sum < 50) {
            sum = 4;
            rmb = 'rmb5';
        }
        else if (sum < 100) {
            sum = 5;
            rmb = 'rmb5';
        }
        else if (sum < 200) {
            sum = 6;
            rmb = 'rmb10';
        }
        else if (sum < 500) {
            sum = 7;
            rmb = 'rmb10';
        }
        else if (sum < 1000) {
            sum = 8;
            rmb = 'rmb10';
        }
        else if (sum < 2000) {
            sum = 9;
            rmb = 'rmb20';
        }
        else if (sum < 5000) {
            sum = 10;
            rmb = 'rmb20';
        }
        else if (sum < 10000) {
            sum = 11;
            rmb = 'rmb20';
        }
        else if (sum < 20000) {
            sum = 12;
            rmb = 'rmb50';
        }
        else if (sum < 50000) {
            sum = 13;
            rmb = 'rmb50';
        }
        else if (sum < 100000) {
            sum = 14;
            rmb = 'rmb50';
        }
        else {
            sum = 15;
            rmb = 'rmb100';
        }
        var indexOfModel = 0;
        var obj = [];
        this.startT = Date.now();
        while (sum > 0) {


            var m;
            //if (sum >= 10000) {
            //    m = objMain.rmbModel['rmb100'].clone();
            //    sum -= 10000;

            //}
            //else if (sum >= 5000) {
            //    m = objMain.rmbModel['rmb50'].clone();
            //    sum -= 5000;
            //}
            //else if (sum >= 2000) {
            //    m = objMain.rmbModel['rmb20'].clone();
            //    sum -= 2000;
            //}
            //else if (sum >= 1000) {
            //    m = objMain.rmbModel['rmb10'].clone();
            //    sum -= 1000;
            //}
            //else
            //if (sum >= 0)
            //{
            m = objMain.rmbModel[rmb].clone();

            // }
            var selfCar = objMain.carGroup.getObjectByName('car_' + objMain.indexKey);
            if (selfCar == undefined) { }
            else {
                m.userData = { 'index': indexOfModel + 0, 't': Date.now(), 'x': selfCar.position.x, 'z': selfCar.position.z };
                objMain.absorbGroup.add(m);
                m.position.set(selfCar.position.x, selfCar.position.y, selfCar.position.z);
                indexOfModel++;
                obj.push(m);
            }
            sum -= 1;
        }
        for (var i = 0; i < obj.length; i++) {
            obj[i].userData['sum'] = indexOfModel;
        }
    },
    animate: function () {
        const deltaT = 10000;
        var l = objMain.absorbGroup.children.length;
        var selfCar = objMain.carGroup.getObjectByName('car_' + objMain.indexKey);
        if (selfCar == undefined) { }
        else
            for (var i = l - 1; i >= 0; i--) {
                //  var objCount=
                var sumY = Math.abs(objMain.camera.position.y) * 0.5;
                sumY = Math.max(sumY, 1);
                // var percent=

                //  objMain.absorbGroup.children[i].userData.t
                var userData = objMain.absorbGroup.children[i].userData;
                var objCount = userData['sum'];
                //var percent = (sumY / objCount) * (0 - userData.index) + (Date.now() - userData.t) / deltaT * 2 * sumY;
                //objMain.absorbGroup.children[i].rotation.z = i / this.objCount * 2 * Math.PI + (Date.now() - userData.t) / deltaT * 4 * Math.PI;
                objMain.absorbGroup.children[i].position.y = (sumY / objCount) * (0 - userData.index) + (Date.now() - userData.t) / deltaT * 2 * sumY;
                if (objMain.absorbGroup.children[i].position.y < 0) {
                    //objMain.absorbGroup.children[i].position.x = i / this.objCount * sumY / 2;
                    objMain.absorbGroup.children[i].position.y = 0;
                    objMain.absorbGroup.children[i].scale.set(selfCar.scale.x, selfCar.scale.y, selfCar.scale.z);
                    objMain.absorbGroup.children[i].rotation.z = 0;
                    objMain.absorbGroup.children[i].visible = false;
                }

                else if (objMain.absorbGroup.children[i].position.y > sumY) {
                    objMain.absorbGroup.remove(objMain.absorbGroup.children[i]);
                }
                else {
                    objMain.absorbGroup.children[i].visible = true;
                    var percent = - objMain.absorbGroup.children[i].position.y / sumY + 1;

                    objMain.absorbGroup.children[i].position.y = Math.sqrt(Math.sqrt(1 - percent)) * sumY;

                    objMain.absorbGroup.children[i].position.x = userData.x + sumY / 1.3 * Math.sqrt(Math.sqrt(1 - percent)) * Math.cos(percent * 2 * Math.PI);
                    objMain.absorbGroup.children[i].position.z = userData.z + sumY / 1.3 * Math.sqrt(Math.sqrt(1 - percent)) * Math.sin(percent * 2 * Math.PI);
                    objMain.absorbGroup.children[i].scale.set(selfCar.scale.x * Math.sqrt(percent), selfCar.scale.y * Math.sqrt(percent), selfCar.scale.z * Math.sqrt(percent));
                    objMain.absorbGroup.children[i].rotation.z = i / objCount * 2 * Math.PI + percent * 2 * Math.PI;
                }
            }
        this.animate2();
    },
    animate2: function () {
        const deltaT = 10000 / 2;
        if (Date.now() - this.startT < 500) {
            objMain.light1.color.r = 1;
            objMain.light1.color.g = 1;
            objMain.light1.color.b = 1;
        }
        else if (Date.now() - this.startT < deltaT) {

            if (Date.now() % 450 < 200) {
                var percent = 1 - (Date.now() - this.startT - 500) / (deltaT - 500);
                objMain.light1.color.r = 1 + percent * 1;
                objMain.light1.color.g = 1 + percent * 1.60;
                objMain.light1.color.b = 1 + percent * 1;
            }
            else {
                objMain.light1.color.r = 1;
                objMain.light1.color.g = 1;
                objMain.light1.color.b = 1;
            }
        }
        else {
            objMain.light1.color.r = 1;
            objMain.light1.color.g = 1;
            objMain.light1.color.b = 1;
        }
    }
}


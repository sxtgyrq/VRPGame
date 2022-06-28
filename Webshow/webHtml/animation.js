var carAnimation = function () {
    for (let key of Object.keys(objMain.carsAnimateData)) {
        let animateDataForeach = objMain.carsAnimateData[key];
        var previous = animateDataForeach.previous;
        var current = animateDataForeach.current;
        var isSelf = (key == 'car_' + objMain.indexKey);
        var now = Date.now();
        var animationF = function (key, obj, now, isSelf) {
            var isAnimation = false;
            if (isSelf) {
                isAnimation = true;
            }
            for (var j = 0; j < obj.animateData.length; j++) {
                //objMain.carsAnimateData[carId].current.animateData[0].initialData =
                //{
                //    'recordTime': recordTime,
                //    'animateData': animateData,
                //    'speedImproved': false,
                //    'start': recordTime,
                //    'end': recordTime + startT
                //}
                //  var recordTime = obj.animateData[j].recordTime;
                if (
                    now < obj.animateData[j].initialData.end &&
                    now > obj.animateData[j].initialData.start) {
                    var animateData = obj.animateData[j].initialData.animateData;
                    for (var i = 0; i < animateData.length; i++) {
                        var start = animateData[i].start;
                        var percent = (now - start - animateData[i].t0) / (animateData[i].t1 - animateData[i].t0);
                        if (percent < 0) {
                            continue;
                        }
                        else if (percent < 1) {
                            var x = animateData[i].x0 + percent * (animateData[i].x1 - animateData[i].x0);
                            var y = animateData[i].y0 + percent * (animateData[i].y1 - animateData[i].y0);
                            if (objMain.carGroup.getObjectByName(key) != undefined) {
                                objMain.carGroup.getObjectByName(key).position.set(x, 0, -y);

                                var scale = objMain.mainF.getLength(objMain.camera.position, objMain.controls.target) * 0.001;
                                if (scale < 0.002) {
                                    scale = 0.002;
                                }
                                objMain.carGroup.getObjectByName(key).scale.set(scale, scale, scale);

                                var complexV = new Complex(animateData[i].x1 - animateData[i].x0, -(animateData[i].y1 - animateData[i].y0));
                                ;
                                if (!complexV.isZero()) {
                                    objMain.carGroup.getObjectByName(key).rotation.set(0, -complexV.toAngle() + Math.PI, 0);
                                    if (isSelf) {
                                        if (isAnimation) {
                                            objMain.controls.target.set(x, 0, -y);
                                            var angle = objMain.controls.getPolarAngle();
                                            //if(
                                            var dCal = objMain.mainF.getLength(objMain.camera.position, objMain.controls.target);
                                            var distance = 32;
                                            if (dCal >= 33) {
                                                distance = dCal * 0.99 - 0.01;
                                            }
                                            else if (dCal <= 31) {
                                                distance = dCal * 1.01 + 0.01;
                                            }
                                            var unitY = distance * Math.cos(angle);
                                            var unitZX = distance * Math.sin(angle);

                                            var angleOfCamara = objMain.controls.getAzimuthalAngle();
                                            var unitX = unitZX * Math.sin(angleOfCamara);
                                            var unitZ = unitZX * Math.cos(angleOfCamara);
                                            //var unitX = unitZX * Math.sin(-complexV.toAngle() - Math.PI / 2);
                                            //var unitZ = unitZX * Math.cos(-complexV.toAngle() - Math.PI / 2);

                                            objMain.camera.position.set(x + unitX, unitY, -y + unitZ);
                                            objMain.camera.lookAt(x, 0, -y);
                                        }
                                        // objMain.controls.maxDistance
                                    }
                                }

                                break;
                            }
                        }
                        else {
                            var x = animateData[i].x0 + 1 * (animateData[i].x1 - animateData[i].x0);
                            var y = animateData[i].y0 + 1 * (animateData[i].y1 - animateData[i].y0);
                            if (objMain.carGroup.getObjectByName(key) != undefined) {
                                objMain.carGroup.getObjectByName(key).position.set(x, 0, -y);

                                var scale = objMain.mainF.getLength(objMain.camera.position, objMain.controls.target) * 0.001;
                                if (scale < 0.002) {
                                    scale = 0.002;
                                }
                                objMain.carGroup.getObjectByName(key).scale.set(scale, scale, scale);

                                var complexV = new Complex(animateData[i].x1 - animateData[i].x0, -(animateData[i].y1 - animateData[i].y0));
                                ;
                                if (!complexV.isZero()) {
                                    objMain.carGroup.getObjectByName(key).rotation.set(0, -complexV.toAngle() + Math.PI, 0);
                                }
                            }
                        }
                    }
                }
            }
        }

        if (previous == null) {
            animationF(key, previous, now, isSelf);
        }
        if (current != null) {
            animationF(key, current, now, isSelf);
        }
    }
};

var carAnimationData = function (passObj) {
    var culDetail = function (dataObj, recordTime) {
        if (dataObj.animateData.length > 0) {

            for (var j = 0; j < dataObj.animateData.length; j++) {
                if (dataObj.animateData[j].privateKey >= 0) {
                    if (j == 0) {
                        var initialData = Decrypt_90021457(dataObj.animateData[0].privateKey, dataObj.animateData[0].dataEncrypted);
                        var start = { 'x': initialData[0], 'y': initialData[1] };
                        start.x /= 256;
                        start.y /= 256;

                        var animateData = [];
                        var startT = 0;
                        for (var i = 2; i < initialData.length; i += 3) {
                            animateData.push(
                                {
                                    x0: start.x,
                                    y0: start.y,
                                    t0: startT,
                                    x1: initialData[i] / 256 + start.x,
                                    y1: initialData[i + 1] / 256 + start.y,
                                    t1: initialData[i + 2] + startT
                                });
                            start.x += initialData[i] / 256;
                            start.y += initialData[i + 1] / 256;
                            startT += initialData[i + 2];
                        };
                        dataObj.animateData[0].initialData =
                        {
                            'recordTime': recordTime,
                            'animateData': animateData,
                            'speedImproved': false,
                            'start': recordTime + 0,
                            'end': recordTime + startT
                        };
                    }
                    else {
                        var initialData = Decrypt_90021457(dataObj.animateData[j].privateKey, dataObj.animateData[j].dataEncrypted);
                        var start = { 'x': initialData[0], 'y': initialData[1] };
                        start.x /= 256;
                        start.y /= 256;

                        var animateData = [];
                        var startT = 0;
                        for (var i = 2; i < initialData.length; i += 3) {
                            animateData.push(
                                {
                                    x0: start.x,
                                    y0: start.y,
                                    t0: startT,
                                    x1: initialData[i] / 256 + start.x,
                                    y1: initialData[i + 1] / 256 + start.y,
                                    t1: initialData[i + 2] + startT
                                });
                            start.x += initialData[i] / 256;
                            start.y += initialData[i + 1] / 256;
                            startT += initialData[i + 2];
                        };
                        //var start, end;
                        var lastRecord = dataObj.animateData[j - 1].initialData;

                        if (lastRecord.end > Date.now()) {
                            dataObj.animateData[j].initialData =
                            {
                                'recordTime': recordTime,
                                'animateData': animateData,
                                'speedImproved': false,
                                'start': lastRecord.end,
                                'end': lastRecord.end + startT
                            };
                        }
                        else {
                            dataObj.animateData[j].initialData =
                            {
                                'recordTime': recordTime,
                                'animateData': animateData,
                                'speedImproved': false,
                                'start': Date.now(),
                                'end': Date.now() + startT
                            };
                        }
                    }
                }
            }

        }
        return dataObj;
    }
    var afterPrivateKeysPatch = function (objOperate, patch) {
        //console.log('objOperate', objOperate);
        //console.log('patch', patch);
        if (objOperate.animateData.length > 0) {
            var recordTime = objOperate.animateData[0].recordTime;
            for (var j = 0; j < objOperate.animateData.length; j++) {
                if (objOperate.animateData[j].privateKey < 0 && patch[j] >= 0) {
                    if (j == 0) {
                        throw '逻辑错误';
                    }
                    else {
                        objOperate.animateData[j].privateKey = patch[j];
                        var initialData = Decrypt_90021457(objOperate.animateData[j].privateKey, objOperate.animateData[j].dataEncrypted);
                        var start = { 'x': initialData[0], 'y': initialData[1] };
                        start.x /= 256;
                        start.y /= 256;

                        var animateData = [];
                        var startT = 0;
                        for (var i = 2; i < initialData.length; i += 3) {
                            animateData.push(
                                {
                                    x0: start.x,
                                    y0: start.y,
                                    t0: startT,
                                    x1: initialData[i] / 256 + start.x,
                                    y1: initialData[i + 1] / 256 + start.y,
                                    t1: initialData[i + 2] + startT
                                });
                            start.x += initialData[i] / 256;
                            start.y += initialData[i + 1] / 256;
                            startT += initialData[i + 2];
                        };
                        //var start, end;
                        var lastRecord = objMain.carsAnimateData[carId].current.animateData[j - 1].initialData;

                        if (lastRecord.end > Date.now()) {
                            objMain.carsAnimateData[carId].current.animateData[j].initialData =
                            {
                                'recordTime': recordTime,
                                'animateData': animateData,
                                'speedImproved': false,
                                'start': lastRecord.end,
                                'end': lastRecord.end + startT
                            };
                        }
                        else {
                            objMain.carsAnimateData[carId].current.animateData[j].initialData =
                            {
                                'recordTime': recordTime,
                                'animateData': animateData,
                                'speedImproved': false,
                                'start': Date.now(),
                                'end': Date.now() + startT
                            };
                        }
                    }
                }
            }

        }
        return objOperate;
    }
     
    if (passObj.passPrivateKeysOnly) {
        var carId = passObj.carID;
        if (objMain.carsAnimateData[carId] == undefined) { }
        else {
            var afterPrivateKeysAdded = function (objOperate, patch) {
                var privateKeys = [];
                for (var i = 0; i < objOperate.animateData.length; i++) {
                    privateKeys.push(-1);
                }
                var privateKeyIndex = patch.privateKeyIndex;
                privateKeys[privateKeyIndex] = patch.privateKeyValue;
                for (var i = privateKeyIndex; i > 0; i--) {
                    privateKeys[i - 1] = calHash(privateKeys[i]);
                } 
                return afterPrivateKeysPatch(objOperate, privateKeys);
            }
            if (objMain.carsAnimateData[carId].previous != null) {
                if (objMain.carsAnimateData[carId].previous.currentMd5 == passObj.Animate.currentMd5) {
                    objMain.carsAnimateData[carId].previous = afterPrivateKeysAdded(objMain.carsAnimateData[carId].previous, passObj.Animate);
                }
            }
            if (objMain.carsAnimateData[carId].current != null) {
                if (objMain.carsAnimateData[carId].current.currentMd5 == passObj.Animate.currentMd5) {
                    objMain.carsAnimateData[carId].current = afterPrivateKeysAdded(objMain.carsAnimateData[carId].current, passObj.Animate);
                }
            }
        }
    }
    else {
        var deltaT = passObj.Animate.deltaT;
        var carId = passObj.carID;
        var recordTime = Date.now() - deltaT;
        if (objMain.carsAnimateData[carId] == undefined) {
            //  var deltaT = passObj.Animate.deltaT;
            objMain.carsAnimateData[carId] = { 'current': null, 'previous': null };
            // console.log('passObj', passObj);
            objMain.carsAnimateData[carId].current = passObj.Animate;
            var current = objMain.carsAnimateData[carId].current;
            objMain.carsAnimateData[carId].current = culDetail(current, recordTime);
        }
        else {
            if (objMain.carsAnimateData[carId].current.currentMd5 == passObj.Animate.previousMd5) {
                var afterPrivateKeysAdded = function (objOperate, patch) {
                    return afterPrivateKeysPatch(objOperate, patch);
                }
                var newPrevious = afterPrivateKeysAdded(objMain.carsAnimateData[carId].current, passObj.Animate.privateKeys);
                objMain.carsAnimateData[carId].previous = newPrevious;

                var isParkingObj = function (objOperate) {
                    if (objOperate.animateData.length == 1) {
                        return objOperate.animateData[0].isParking;
                    }
                    else {
                        return false;
                    }
                };

                if (isParkingObj(objMain.carsAnimateData[carId].previous)) {
                    //    objMain.carsAnimateData[carId].previous.deltaT
                    objMain.carsAnimateData[carId].current = passObj.Animate;
                    var current = objMain.carsAnimateData[carId].current;
                    recordTime = Math.max(objMain.carsAnimateData[carId].previous.animateData[0].initialData.recordTime, recordTime);
                    objMain.carsAnimateData[carId].previous.animateData[0].initialData.animateData[0].t1 = recordTime - objMain.carsAnimateData[carId].previous.animateData[0].initialData.recordTime;
                    objMain.carsAnimateData[carId].previous.animateData[0].initialData.end
                        =
                        objMain.carsAnimateData[carId].previous.animateData[0].initialData.start +
                        objMain.carsAnimateData[carId].previous.animateData[0].initialData.animateData[0].t1;
                    var current = objMain.carsAnimateData[carId].current;
                    objMain.carsAnimateData[carId].current = culDetail(current, recordTime);
                }
                else {
                    var j = objMain.carsAnimateData[carId].previous.animateData.length;
                    var lastRecord = objMain.carsAnimateData[carId].previous.animateData[j - 1].initialData;
                    if (recordTime < lastRecord.end) {
                        recordTime = lastRecord.end;
                    }
                    objMain.carsAnimateData[carId].current = passObj.Animate;
                    var current = objMain.carsAnimateData[carId].current;
                    objMain.carsAnimateData[carId].current = culDetail(current, recordTime);
                }
            }
            else {
                delete objMain.carsAnimateData[carId];
                carAnimationData(passObj);
            }
        }
    }
}

var test = function () {
    var a = { 'b': 1 };
    var f = function (p) {
        p.b++;
    }
    f(a);
    console.log('r', a.b);
}
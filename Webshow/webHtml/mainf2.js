function TaskClass(s, c) {
    this._state = s;
    this._carSelect = c;
}
TaskClass.prototype.__defineGetter__("state", function () {
    //ShowGetInfo("Age"); 
    return this._state;
});

TaskClass.prototype.__defineSetter__("state", function (val) {
    if (this._state == 'getTax' && val != 'getTax') {
        Tax.trunOffAnimate();
    }
    else if (this._state != 'getTax' && val == 'getTax') {
        Tax.trunOnAnimate();
    }
    this._state = val;

    //ShowSetInfo("Age");
});

TaskClass.prototype.__defineGetter__("carSelect", function () {
    //ShowGetInfo("Age"); 
    return this._carSelect;
});

TaskClass.prototype.__defineSetter__("carSelect", function (val) {
    this._carSelect = val;
    //ShowSetInfo("Age");
});
var objMain =
{
    debug: true,
    indexKey: '',
    displayName: '',
    positionInStation: 0,
    MoneyForSave: 0,
    Money: 0,
    state: '',
    receivedState: '',
    scene: null,
    renderer: null,
    labelRenderer: null,
    centerPosition: { lon: 112.573463, lat: 37.891474 },
    roadGroup: null,
    basePoint: null,
    fpIndex: -1,
    othersBasePoint: {},
    playerGroup: null,
    carGroup: null,
    collectGroup: null,
    getOutGroup: null,
    robotModel: null,
    buildingModel: {},
    buildingGroup: null,
    cars: {},
    rmbModel: {},
    ModelInput: {
        speed: null,
        attack: null,
        shield: null,
        confusePrepare: null,
        lostPrepare: null,
        ambushPrepare: null,
        water: null,
        direction: null,
        directionArrow: null
    },
    shieldGroup: null,
    confusePrepareGroup: null,
    lostPrepareGroup: null,
    ambushPrepareGroup: null,
    waterGroup: null,
    fireGroup: null,
    lightningGroup: null,
    directionGroup: null,
    clock: null,
    leaveGameModel: null,
    profileModel: null,
    light1: null,
    controls: null,
    raycaster: null,
    mouse: null,
    raycasterOfSelector: null,
    selectorPosition: { x: 0, y: 0.5 },
    selectObj: { obj: null, type: '' },
    canSelect: false,
    carsNames: null,
    carsAnimateData: {},
    PromoteState: -1,
    PromotePositions:
    {
        mile: null,
        business: null,
        volume: null,
        speed: null
    },
    PromoteList: ['mile', 'business', 'volume', 'speed'],
    PromoteDiamondCount:
    {
        mile: 0,
        business: 0,
        volume: 0,
        speed: 0
    },
    CollectPosition: {},
    diamondGeometry: null,
    mirrorCubeCamera: null,
    promoteDiamond: null,
    columnGroup: null,
    mainF:
    {
        drawLineOfFpToRoad: function (fp, group, color, lineName) {
            {
                var start = new THREE.Vector3(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde));
                var end = new THREE.Vector3(MercatorGetXbyLongitude(fp.positionLongitudeOnRoad), 0, -MercatorGetYbyLatitude(fp.positionLatitudeOnRoad));
                var lineGeometry = new THREE.Geometry();
                lineGeometry.vertices.push(start);
                lineGeometry.vertices.push(end);
                var lineMaterial = new THREE.LineBasicMaterial({ color: color });
                var line = new THREE.Line(lineGeometry, lineMaterial);
                line.name = 'approach_' + lineName;
                group.add(line);
            }
        },
        lookAtPosition: function (fp) {

            var start = new THREE.Vector3(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde));
            var end = new THREE.Vector3(MercatorGetXbyLongitude(fp.positionLongitudeOnRoad), 0, -MercatorGetYbyLatitude(fp.positionLatitudeOnRoad));

            var cc = new Complex(end.x - start.x, end.z - start.z);
            cc.toOne();
            var minDistance = objMain.controls.minDistance * 1.1;
            var maxPolarAngle = objMain.controls.maxPolarAngle - Math.PI / 30;
            {
                var planePosition = new THREE.Vector3(start.x + cc.r * minDistance * Math.sin(maxPolarAngle), start.y + minDistance * Math.cos(maxPolarAngle), start.z + cc.i * minDistance * Math.sin(maxPolarAngle));
                objMain.camera.position.set(planePosition.x, planePosition.y, planePosition.z);

                objMain.controls.target.set(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde));
                objMain.camera.lookAt(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde));
            }
        },
        initilizeCars: function (fp, color, key, isSelf, postionInStation) {
            if (isSelf == undefined) {
                isSelf = true;
            }
            var start = new THREE.Vector3(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde))
            var end = new THREE.Vector3(MercatorGetXbyLongitude(fp.positionLongitudeOnRoad), 0, -MercatorGetYbyLatitude(fp.positionLatitudeOnRoad))
            var cc = new Complex(end.x - start.x, end.z - start.z);
            cc.toOne();

            var positon1 = cc.multiply(new Complex(-0.309016994, 0.951056516));
            var positon2 = positon1.multiply(new Complex(0.809016994, 0.587785252));
            var positon3 = positon2.multiply(new Complex(0.809016994, 0.587785252));
            var positon4 = positon3.multiply(new Complex(0.809016994, 0.587785252));
            var positon5 = positon4.multiply(new Complex(0.809016994, 0.587785252));

            var positons = [positon1, positon2, positon3, positon4, positon5];

            var names = ['carA', 'carB', 'carC', 'carD', 'carE'];
            //   console.log('positons', positons);
            var percentOfPosition = 0.25;
            //for (var i = 0; i < positons.length; i++)
            {
                var i = postionInStation;
                var start = new THREE.Vector3(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde));
                var end = new THREE.Vector3(start.x + positons[i].r * percentOfPosition, 0, start.z + positons[i].i * percentOfPosition);
                var lineGeometry = new THREE.Geometry();
                lineGeometry.vertices.push(start);
                lineGeometry.vertices.push(end);
                var lineMaterial = new THREE.LineBasicMaterial({ color: color });
                var line = new THREE.Line(lineGeometry, lineMaterial);
                line.name = 'carRoad' + '_' + key;
                if (key === objMain.indexKey) {
                    isSelf = true;
                }

                line.userData = { objectType: 'carRoad', parent: key, };
                objMain.carGroup.add(line);
                var model = null;
                if (isSelf) {
                    model = objMain.cars[names[i]].clone();
                }
                else {
                    model = objMain.cars['carO'].clone();
                }
                model.name = 'car' + '_' + key;
                model.position.set(end.x, 0, end.z);
                model.scale.set(0.002, 0.002, 0.002);
                model.rotateY(-positons[i].toAngle());
                model.userData = { objectType: 'car', parent: key, };
                objMain.carGroup.add(model);

            }
        },
        lookTwoPositionCenter: function (p1, p2) {

            var start = p1;
            var end = p2;
            var lengthTwoPoint = objMain.mainF.getLength(start, end);
            if (lengthTwoPoint > 0.2) {
                var cc = new Complex(end.x - start.x, end.z - start.z);
                cc.toOne();
                var x1 = lengthTwoPoint / (1 / Math.tan(Math.PI / 6) - 1 / Math.tan(Math.PI * 42 / 180));
                var x2 = x1 / Math.tan(Math.PI * 42 / 180);
                var x3 = x2 + lengthTwoPoint / 2;
                var minDistance = x3 / Math.cos(Math.PI * 36 / 180);
            }
            //var minDistance = objMain.controls.minDistance * 1.1;
            var maxPolarAngle = Math.PI * 54 / 180;
            {
                var planePosition = new THREE.Vector3(start.x + cc.r * minDistance * Math.sin(maxPolarAngle), start.y + minDistance * Math.cos(maxPolarAngle), start.z + cc.i * minDistance * Math.sin(maxPolarAngle));
                objMain.camera.position.set(planePosition.x, planePosition.y, planePosition.z);

                objMain.controls.target.set((start.x + end.x) / 2, 0, (start.z + end.z) / 2);
                objMain.camera.lookAt((start.x + end.x) / 2, 0, (start.z + end.z) / 2);
            }
        },
        getLength: function (p1, p2) {
            return Math.sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y) + (p1.z - p2.z) * (p1.z - p2.z));
        },
        removeF:
        {
            removePanle: function (id) {
                //carsSelectionPanel
                while (document.getElementById(id) != null) {
                    document.getElementById(id).remove();
                }
            },
            clearGroup: function (group) {
                var startIndex = group.children.length - 1;
                for (var i = startIndex; i >= 0; i--) {
                    group.remove(group.children[i]);
                }
            }
        },
        refreshPromotionDiamondAndPanle: function (received_obj, endF) {
            //if (received_obj.resultType == objMain.Task.state)
            {
                /*
                 * 这里进行了Task的状态验证，确保3D资源没有加载前，不会调用此方法
                 */
                if (objMain.state == "OnLine") {
                    var diamondName = "diamond_" + received_obj.resultType;
                    if (objMain.promoteDiamond.getObjectByName(diamondName) != undefined) {
                        objMain.promoteDiamond.remove(objMain.promoteDiamond.getObjectByName(diamondName));
                    }
                    var lineName = "approach_diamond_" + received_obj.resultType;
                    if (objMain.promoteDiamond.getObjectByName(lineName) != undefined) {
                        objMain.promoteDiamond.remove(objMain.promoteDiamond.getObjectByName(lineName));
                    }
                    // var lineName=
                    var color = 0x000000;
                    switch (received_obj.resultType) {
                        case 'mile':
                            {
                                color = 0xff0000;
                            }; break;
                        case 'business':
                            {
                                color = 0x00ff00;
                            }; break;
                        case 'volume':
                            {
                                color = 0x0000ff;
                            }; break;
                        case 'speed':
                            {
                                color = 0x000000;
                            }; break;
                    }
                    //var mirrorCubeCamera = new THREE.CubeCamera(0.1, 5000, 512);
                    //objMain.scene.add(mirrorCubeCamera);
                    var geometry = objMain.diamondGeometry;
                    var material = new THREE.MeshBasicMaterial({ color: color, transparent: true, opacity: 0.5, depthWrite: true });

                    var diamond = new THREE.Mesh(geometry, material);
                    diamond.userData.endF = endF;
                    diamond.name = 'diamond' + '_' + received_obj.resultType;
                    diamond.scale.set(0.2, 0.22, 0.2);
                    diamond.position.set(MercatorGetXbyLongitude(objMain.PromotePositions[received_obj.resultType].Fp.Longitude), 0, -MercatorGetYbyLatitude(objMain.PromotePositions[received_obj.resultType].Fp.Latitde));

                    objMain.promoteDiamond.add(diamond);

                    //  objMain.mainF.lookTwoPositionCenter(objMain.promoteDiamond.children[0].position, objMain.carGroup.getObjectByName('car_' + objMain.indexKey).position);

                    objMain.mainF.drawLineOfFpToRoad(objMain.PromotePositions[received_obj.resultType].Fp, objMain.promoteDiamond, color, "diamond_" + received_obj.resultType);

                    //objMain.mainF.drawPanelOfPromotion(objMain.Task.state, endF);
                }
            }
        },
        refreshCollectAndPanle: function (collectIndex, endF) {
            if (objMain.state == "OnLine") {
                var objName = 'moneymodel' + collectIndex;
                var obj = objMain.collectGroup.getObjectByName(objName);
                if (obj == undefined) { }
                else {
                    if (endF == undefined && obj.userData.endF != undefined) {
                        endF = obj.userData.endF;
                    }
                    objMain.collectGroup.remove(obj);
                }
                //'approach_' + lineName;
                var lineName = 'approach_' + 'money' + collectIndex;
                var line = objMain.collectGroup.getObjectByName(lineName);
                if (line == undefined) { }
                else {
                    objMain.collectGroup.remove(line);
                }

                var changeIndexToMoney = function (inputIndex) {
                    switch (inputIndex) {
                        case 0:
                            {
                                return 100;
                            };
                        case 1:
                        case 2:
                            {
                                return 50;
                            };
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            {
                                return 20;
                            };
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                            {
                                return 10;
                            };
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                        case 27:
                        case 28:
                        case 29:
                        case 30:
                        case 31:
                        case 32:
                        case 33:
                        case 34:
                        case 35:
                        case 36:
                        case 37:
                            {
                                return 5;
                            };
                        default: return 0;
                    }
                };
                var model;

                switch (changeIndexToMoney(collectIndex)) {
                    case 5:
                        {
                            model = objMain.rmbModel['rmb5'].clone();
                        }; break;
                    case 10:
                        {
                            model = objMain.rmbModel['rmb10'].clone();
                        }; break;
                    case 20:
                        {
                            model = objMain.rmbModel['rmb20'].clone();
                        }; break;
                    case 50:
                        {
                            model = objMain.rmbModel['rmb50'].clone();
                        }; break;
                    case 100:
                        {
                            model = objMain.rmbModel['rmb100'].clone();
                        }; break;
                    default: return;
                }

                if (objMain.CollectPosition[collectIndex] == undefined) {
                    return;
                }
                // model.name = names[i] + '_' + key;
                //model.position.set(end.x, 0, end.z);
                // model.scale.set(0.002, 0.002, 0.002);
                //model.rotateY(-positons[i].toAngle());
                //model.userData = { objectType: 'car', parent: key, index: names[i] };
                model.position.set(MercatorGetXbyLongitude(objMain.CollectPosition[collectIndex].Fp.Longitude), 0, -MercatorGetYbyLatitude(objMain.CollectPosition[collectIndex].Fp.Latitde));
                model.name = objName;
                model.userData.collectPosition = objMain.CollectPosition[collectIndex];
                objMain.collectGroup.add(model);

                var color = 0xFFD700;

                objMain.mainF.drawLineOfFpToRoad(objMain.CollectPosition[collectIndex].Fp, objMain.collectGroup, color, 'money' + collectIndex);
                //if (objMain.Task.state == 'collect') {
                //    if (objMain.Task.carSelect == '') {
                //        objMain.mainF.lookAtPosition(objMain.CollectPosition.Fp);
                //    }
                //    else {
                //        objMain.mainF.lookTwoPositionCenter(objMain.collectGroup.children[0].position, objMain.carGroup.getObjectByName(objMain.Task.carSelect).position);
                //    }
                //}
                if (objMain.Task.state == 'collect') {
                    objMain.mainF.drawPanelOfCollect(endF);
                }
            }
        },
        drawPanelOfCollect: function (endF) {

            var lengthOfObjs = objMain.groupOfOperatePanle.children.length;
            for (var i = lengthOfObjs - 1; i >= 0; i--) {
                objMain.groupOfOperatePanle.remove(objMain.groupOfOperatePanle.children[i]);
            }
            for (var i = 0; i < 38; i++) {
                if (objMain.CollectPosition[i] == undefined) {
                    continue;
                }
                var element = document.createElement('div');
                element.style.width = '10em';
                //element.style.marginLeft = 'calc(5em + 20px)';
                element.style.marginTop = '3em';
                var color = '#ff0000';
                //var colorName = '红';
                //switch (type) {
                //    case 'mile':
                //        {
                //            color = '#ff0000';
                //            colorName = '红';
                //        }; break;
                //    case 'business': {
                //        color = '#00ff00';
                //        colorName = '绿';
                //    }; break;
                //    case 'volume': {

                //        color = '#0000ff';
                //        colorName = '蓝';
                //    }; break;
                //    case 'speed': {

                //        color = '#000000';
                //        colorName = '黑';
                //    }; break;

                //}
                element.style.border = '2px solid ' + color;
                element.style.borderTopLeftRadius = '0.5em';
                element.style.backgroundColor = 'rgba(155, 55, 255, 0.3)';
                element.style.color = '#1504f6';

                var div2 = document.createElement('div');
                div2.style.fontSize = '0.5em';

                var b = document.createElement('b');
                b.innerHTML = '到[<span style="color:#05ffba">' + objMain.CollectPosition[i].Fp.FastenPositionName + '</span>]回收<span style="color:#05ffba">' + (objMain.CollectPosition[i].collectMoney).toFixed(2) + '元</span>现金。';
                div2.appendChild(b);

                element.appendChild(div2);

                var object = new THREE.CSS2DObject(element);
                var fp = objMain.CollectPosition[i].Fp;
                object.position.set(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde));

                objMain.groupOfOperatePanle.add(object);
            }



        },
        removeRole: function (roleID) {
            var carRoad_ID = 'carRoad_' + roleID;

            objMain.carGroup.remove(objMain.carGroup.getObjectByName(carRoad_ID));

            var car_ID = 'car_' + roleID;

            objMain.carGroup.remove(objMain.carGroup.getObjectByName(car_ID));


            var approachId = 'approach_' + roleID;//;216596b5fddf7bc24f05bfebb2b1f10d
            objMain.playerGroup.remove(objMain.playerGroup.getObjectByName(approachId));

            var flagId = 'flag_' + roleID;//;216596b5fddf7bc24f05bfebb2b1f10d
            objMain.playerGroup.remove(objMain.playerGroup.getObjectByName(flagId));

            delete objMain.othersBasePoint[roleID];
        },
        drawDiamondCollected: function () {
            var fp = objMain.basePoint;
            var key = objMain.indexKey;
            var start = new THREE.Vector3(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde))
            var end = new THREE.Vector3(MercatorGetXbyLongitude(fp.positionLongitudeOnRoad), 0, -MercatorGetYbyLatitude(fp.positionLatitudeOnRoad))
            var cc = new Complex(end.x - start.x, end.z - start.z);
            cc.toOne();

            var positon1 = cc.multiply(new Complex(0, 1));
            var positon2 = positon1.multiply(new Complex(0.5, 0.86602));
            var positon3 = positon2.multiply(new Complex(0.5, 0.86602));
            var positon4 = positon3.multiply(new Complex(0.5, 0.86602));

            var positons = [positon1, positon2, positon3, positon4];

            var names = ['BatteryMile', 'BatteryBusiness', 'BatteryVolume', 'BatterySpeed'];
            var index = ['mile', 'business', 'volume', 'speed'];
            var colors = [0xff0000, 0x00ff00, 0x0000ff, 0x000000];
            //   console.log('positons', positons);
            var percentOfPosition = 0.5;
            for (var i = 0; i < positons.length; i++) {
                var start = new THREE.Vector3(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde));
                var end = new THREE.Vector3(start.x + positons[i].r * percentOfPosition, 0, start.z + positons[i].i * percentOfPosition);
                //var lineGeometry = new THREE.Geometry();
                //lineGeometry.vertices.push(start);
                //lineGeometry.vertices.push(end);
                //var lineMaterial = new THREE.LineBasicMaterial({ color: color });
                //var line = new THREE.Line(lineGeometry, lineMaterial);
                //line.name = 'carRoad11' + 'ABCDE'[i] + '_' + key;
                //line.userData = { objectType: 'carRoad', parent: key, index: (i + 0) };
                //objMain.carGroup.add(line);
                var mesh;
                if (objMain.columnGroup.getObjectByName(names[i]) == undefined) {
                    var geometryCylinder = new THREE.CylinderGeometry(0.05, 0.05, 0.05, 16);
                    var color = colors[i];
                    var materialCylinder = new THREE.MeshPhongMaterial({ color: color, transparent: true, opacity: 0.6 });
                    mesh = new THREE.Mesh(geometryCylinder, materialCylinder);
                    mesh.castShadow = true;
                    mesh.receiveShadow = true;
                    mesh.name = names[i];
                    mesh.scale.setY(0.01);
                    mesh.position.set(end.x, end.y, end.z);
                    mesh.userData.index = index[i];
                    objMain.columnGroup.add(mesh);
                }
                else {
                    mesh = objMain.columnGroup.getObjectByName(names[i]);
                }

                if (objMain.PromoteDiamondCount[index[i]] != undefined) {
                    var scale = objMain.PromoteDiamondCount[index[i]];
                    scale = Math.max(0.01, scale);
                    mesh.scale.setY(scale);
                }
                //var model = objMain.cars[names[i]].clone();
                //model.name = names[i] + '_' + key;
                //model.position.set(end.x, 0, end.z);
                //model.scale.set(0.002, 0.002, 0.002);
                //model.rotateY(-positons[i].toAngle());
                //model.userData = { objectType: 'car', parent: key, index: names[i] };
                //objMain.carGroup.add(model);
            }
        },
        updateCollectGroup: function () {
            theLagestHoderKey.updateCollectGroup();
        },
        refreshBuyPanel: function () {
            objMain.mainF.removeF.clearGroup(objMain.groupOfOperatePanle);
            {
                {
                    var element = document.createElement('div');

                    element.style.width = '10em';
                    element.style.marginTop = '3em';
                    var color = '#ff0000';
                    //var colorName = '红';
                    //switch (type) {
                    //    case 'mile':
                    //        {
                    //            color = '#ff0000';
                    //            colorName = '红';
                    //        }; break;
                    //    case 'business': {
                    //        color = '#00ff00';
                    //        colorName = '绿';
                    //    }; break;
                    //    case 'volume': { 
                    //        color = '#0000ff';
                    //        colorName = '蓝';
                    //    }; break;
                    //    case 'speed': { 
                    //        color = '#000000';
                    //        colorName = '黑';
                    //    }; break; 
                    //}
                    element.style.border = '2px solid ' + color;
                    element.style.borderTopLeftRadius = '0.5em';
                    element.style.backgroundColor = 'rgba(255, 255, 255, 0.5)';
                    element.style.color = '#1504f6';

                    var div2 = document.createElement('div');

                    var b = document.createElement('b');
                    b.innerHTML = '现在售价如下红宝石10.00，蓝宝石10.00，蓝宝石10.00，黑宝石10.00！点击柱状仓库，进行购买！';
                    div2.appendChild(b);



                    element.appendChild(div2);

                    var object = new THREE.CSS2DObject(element);
                    var fp = objMain.basePoint;
                    object.position.set(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde));

                    objMain.groupOfOperatePanle.add(object);
                }


            }
        }
    },
    Task: new TaskClass('', ''),
    animation:
    {
        animateCameraByCarAndTask: function () {
            if (objMain.Task.state == 'mile') {
                if (objMain.Task.carSelect != '') {
                    var p1 = objMain.promoteDiamond.children[0].position;
                    var p2 = objMain.carGroup.getObjectByName(objMain.Task.carSelect).position;

                }
            }
        }
    },
    groupOfOperatePanle: null,
    GetPositionNotify: {
        data: null, F: function (data) {
            console.log(data);
            var objInput = JSON.parse(data);
            objMain.basePoint = objInput.fp;
            objMain.carsNames = objInput.carsNames;
            objMain.indexKey = objInput.key;
            objMain.displayName = objInput.PlayerName;
            objMain.fpIndex = objInput.fPIndex;

            objMain.positionInStation = objInput.positionInStation;
            //if (objMain.receivedState == 'WaitingToGetTeam') {
            //    objMain.ws.send(received_msg);
            //}
            //小车用 https://threejs.org/examples/#webgl_animation_skinning_morph
            //小车用 基地用 https://threejs.org/examples/#webgl_animation_cloth
            // drawFlag(); 
            drawPoint('green', objMain.basePoint, objMain.indexKey);
            /*画引线*/
            objMain.mainF.drawLineOfFpToRoad(objMain.basePoint, objMain.playerGroup, 'green', objMain.indexKey);
            objMain.mainF.drawDiamondCollected();
            objMain.mainF.lookAtPosition(objMain.basePoint);
            objMain.mainF.initilizeCars(objMain.basePoint, 'green', objMain.indexKey, true, objMain.positionInStation);
            drawCarBtns();


            objMain.GetPositionNotify.data = null;
            SysOperatePanel.draw();
            frequencyShow.show();
            operateStateShow.show();
        }
    },
    Tax: {},
    taxGroup: null,
    msg:
    {

    },
    panOrRotate: 'rotate',
    carState: {},
    music:
    {
        theme: '',
        change: function () {
            var bgm = document.getElementById('backGroudMusick');
            if (bgm.currentTime === 0 || bgm.ended) {
                switch (this.theme) {
                    case '':
                        {
                            var itemCount = bgm.children.length - 1;
                            for (var i = itemCount; i >= 0; i--) {
                                bgm.children[i].remove();
                            }
                            var source1 = document.createElement('source');
                            source1.src = 'bgm/changshoucunwai.ogg';
                            source1.type = 'audio/ogg';

                            var source2 = document.createElement('source');
                            source2.src = 'bgm/changshoucunwai.mp3';
                            source2.type = 'audio/mpeg';

                            bgm.appendChild(source1);
                            bgm.appendChild(source2);
                            bgm.load();
                            bgm.play();
                        }; break;
                    default:
                        {
                            var itemCount = bgm.children.length - 1;
                            for (var i = itemCount; i >= 0; i--) {
                                bgm.children[i].remove();
                            }
                            var source1 = document.createElement('source');
                            source1.src = 'bgm/' + this.theme + '.ogg';
                            source1.type = 'audio/ogg';

                            var source2 = document.createElement('source');
                            source2.src = 'bgm/' + this.theme + '.mp3';
                            source2.type = 'audio/mpeg';

                            bgm.appendChild(source1);
                            bgm.appendChild(source2);

                            bgm.load();
                            bgm.play();
                        }; break;
                }

            }
        }
    },
    background:
    {
        path: '',
        change: function () {
            switch (this.path) {
                case '':
                    {
                        var cubeTextureLoader = new THREE.CubeTextureLoader();
                        cubeTextureLoader.setPath('Pic/');
                        //var cubeTexture = cubeTextureLoader.load([
                        //    "xi_r.jpg", "dong_r.jpg",
                        //    "ding_r.jpg", "di_r.jpg",
                        //    "nan_r.jpg", "bei_r.jpg"
                        //]);
                        var cubeTexture = cubeTextureLoader.load([
                            "px.jpg", "nx.jpg",
                            "py.jpg", "ny.jpg",
                            "pz.jpg", "nz.jpg"
                        ]);
                        //var cubeTexture = cubeTextureLoader.load([
                        //    "px.png", "nx.png",
                        //    "py.jpg", "ny.jpg",
                        //    "pz.png", "nz.png"
                        //]);
                        objMain.scene.background = cubeTexture;
                    }; break;
                default:
                    {
                        var cubeTextureLoader = new THREE.CubeTextureLoader();
                        cubeTextureLoader.setPath('Pic/' + this.path + '/');
                        var cubeTexture = cubeTextureLoader.load([
                            "px.jpg", "nx.jpg",
                            "py.jpg", "ny.jpg",
                            "pz.jpg", "nz.jpg"
                        ]);
                        objMain.scene.background = cubeTexture;
                    }; break;
            }
        }
    },
    rightAndDuty:
    {
        data: {},
        update: function () { }
    },
    dealWithReceivedObj: function (received_obj, evt, received_msg) {
        switch (received_obj.c) {
            case 'setState':
                {
                    objMain.receivedState = received_obj.state;
                    switch (objMain.receivedState) {
                        case 'selectSingleTeamJoin':
                            {
                                selectSingleTeamJoinHtml();
                            }; break;
                        case 'OnLine':
                            {
                                set3DHtml();
                                //  objMain.state = objMain.receivedState;
                                objMain.ws.send('SetOnLine');
                                for (var key in objMain.othersBasePoint) {
                                    var indexKey = key;
                                    var basePoint = objMain.othersBasePoint[key].basePoint;
                                    drawPoint('orange', basePoint, indexKey);
                                    objMain.mainF.drawLineOfFpToRoad(basePoint, objMain.playerGroup, 'green', indexKey);
                                    objMain.mainF.initilizeCars(basePoint, 'orange', indexKey, false, objMain.othersBasePoint[key].positionInStation);
                                    console.log('哦哦', '出现了预料的情况！！！');
                                    //alert();
                                }
                                objMain.state = objMain.receivedState;
                            }; break;
                        case 'WaitingToStart':
                            {
                                setWaitingToStart();
                            }; break;
                        case 'WaitingToGetTeam':
                            {
                                setWaitingToGetTeam();
                            }; break;
                    }
                }; break;
            case 'setSession':
                {
                    sessionStorage['session'] = received_obj.session;
                }; break;
            case 'TeamCreateFinish':
                {
                    //  alert();
                    console.log('提示', '队伍创建成功');
                    if (objMain.receivedState == 'WaitingToStart') {
                        //{"CommandStart":"182be0c5cdcd5072bb1864cdee4d3d6e","WebSocketID":3,"TeamNum":0,"c":"TeamCreateFinish"}
                        token.CommandStart = received_obj.CommandStart;
                        createTeam(received_obj)
                    }
                    //  sessionStorage['session'] = received_obj.session;
                }; break;
            case 'TeamJoinFinish':
                {
                    console.log('提示', '加入队伍成功');
                    if (objMain.receivedState == 'WaitingToGetTeam') {
                        joinTeamDetail(received_obj);
                    }
                }; break;
            case 'Alert':
                {
                    alert(received_obj.msg);
                }; break;
            case 'TeamJoinBroadInfo':
                {
                    //  broadTeamJoin(received_obj);
                    if (objMain.receivedState == 'WaitingToGetTeam' || objMain.receivedState == 'WaitingToStart') {
                        broadTeamJoin(received_obj);
                    }
                }; break;
            case 'TeamNumWithSecret':
                {
                    if (objMain.receivedState == 'WaitingToGetTeam') {
                        objMain.ws.send(received_msg);
                    }
                }; break;
            case 'GetOthersPositionNotify_v2':
                {
                    /*
                     * 其他玩家的状态刷新
                     */
                    console.log(evt.data);
                    var objInput = JSON.parse(evt.data);
                    var basePoint = objInput.fp;
                    //var carsNames = objInput.carsNames;
                    var indexKey = objInput.key;
                    var PlayerName = objInput.PlayerName;
                    var fPIndex = objInput.fPIndex;
                    var positionInStation = objInput.positionInStation;
                    var isNPC = objInput.isNPC;
                    var isPlayer = objInput.isPlayer;
                    var Level = objInput.Level;
                    objMain.othersBasePoint[indexKey] =
                    {
                        'basePoint': basePoint,
                        'indexKey': indexKey,
                        'playerName': PlayerName,
                        'fPIndex': fPIndex,
                        'isNPC': isNPC,
                        'isPlayer': isPlayer,
                        'Level': Level
                    };
                    if (objMain.state == "OnLine") {
                        drawPoint('orange', basePoint, indexKey);
                        /*画引线*/
                        objMain.mainF.drawLineOfFpToRoad(basePoint, objMain.playerGroup, 'green', indexKey);
                        //  objMain.mainF.lookAtPosition(objMain.basePoint);
                        objMain.mainF.initilizeCars(basePoint, 'orange', indexKey, false, positionInStation);
                    }
                    else {
                        /*
                         * 两个用户同时刷新，在update websocketID与 setOnline 这段时间可能出现这种情况。
                         */
                        //var msg = 'GetPositionNotify出入时，状态为' + objMain.state;
                        //throw (msg);
                        //return;
                    }

                    //    objMain.othersBasePoint[indexKey] = { 'basePoint': basePoint, 'carsNames': carsNames, 'indexKey': indexKey, 'playerName': PlayerName, 'fPIndex': fPIndex };
                    //if (objMain.receivedState == 'WaitingToGetTeam') {
                    //    objMain.ws.send(received_msg);
                    //}
                    //小车用 https://threejs.org/examples/#webgl_animation_skinning_morph
                    //小车用 基地用 https://threejs.org/examples/#webgl_animation_cloth
                    // drawFlag(); 

                    // drawCarBtns(objMain.carsNames);
                }; break;
            case 'GetPositionNotify_v2':
                {
                    /*
                     * 命令GetPositionNotify 与setState objMain.state="OnLine"发生顺序不定。但是画数据，需要在3D初始化之后。
                     */
                    if (objMain.state == "OnLine") {
                        var msg = '先执行了 OnLine命令';
                        console.log('提示', msg);
                        objMain.GetPositionNotify.F(evt.data);
                    }
                    else {
                        /*
                         * 
                         */
                        var msg = 'GetPositionNotify出入时，状态为' + objMain.state;
                        console.log('提示', msg);
                        throw (msg);
                        //  objMain.GetPositionNotify.data = evt.data;
                        return;
                    }

                    //var model = objMain.robotModel.clone();
                    //model.position.set(objMain.basePoint.MacatuoX, 0, -objMain.basePoint.MacatuoY);
                    //objMain.roadGroup.add(model);
                }; break;
            case 'MapRoadAndCrossJson':
                {
                    switch (received_obj.action) {
                        case 'start':
                            {
                                Map.roadAndCrossJson = '';
                                objMain.ws.send('MapRoadAndCrossJson,start');
                            }; break;
                        case 'mid':
                            {
                                Map.roadAndCrossJson += received_obj.passStr;
                                objMain.ws.send('MapRoadAndCrossJson,mid');
                            }; break;
                        case 'end':
                            {
                                Map.roadAndCross = JSON.parse(Map.roadAndCrossJson);
                                Map.roadAndCrossJson = '';
                                objMain.ws.send('MapRoadAndCrossJson,end');
                            }; break;
                    }
                }; break;
            case 'SetRobot':
                {
                    //  console.log(evt.data);
                    var f = function (received_obj, mIndex, field) {
                        var manager = new THREE.LoadingManager();
                        new THREE.MTLLoader(manager)
                            .loadTextOnly(received_obj.modelBase64[1], 'data:image/png;base64,' + received_obj.modelBase64[mIndex], function (materials) {
                                materials.preload();
                                // materials.depthTest = false;
                                new THREE.OBJLoader(manager)
                                    .setMaterials(materials)
                                    //.setPath('/Pic/')
                                    .loadTextOnly(received_obj.modelBase64[0], function (object) {
                                        console.log('o', object);
                                        for (var iOfO = 0; iOfO < object.children.length; iOfO++) {
                                            if (object.children[iOfO].isMesh) {
                                                for (var mi = 0; mi < object.children[iOfO].material.length; mi++) {
                                                    //object.children[iOfO].material[mi].depthTest = false;
                                                    object.children[iOfO].material[mi].transparent = true;
                                                    object.children[iOfO].material[mi].opacity = 1;
                                                    object.children[iOfO].material[mi].side = THREE.FrontSide;
                                                    // console.log('color', object.children[iOfO].material[mi].color);
                                                    //switch (level) {
                                                    //    case 'high':
                                                    //        {
                                                    //            //FR-2HZB-033
                                                    //            object.children[iOfO].material[mi].color = new THREE.Color(1.5, 1, 1);
                                                    //        }; break;
                                                    //    case 'mid':
                                                    //        {
                                                    //            object.children[iOfO].material[mi].color = new THREE.Color(1, 1.5, 1);
                                                    //        }; break;
                                                    //    case 'low':
                                                    //        {
                                                    //            object.children[iOfO].material[mi].color = new THREE.Color(1, 1, 1.5);
                                                    //        }; break;
                                                    //}
                                                    //object.children[iOfO].material[mi].color = new THREE.Color(1, 1, 1);
                                                }
                                            }
                                        }
                                        console.log('o', object);
                                        objMain.cars[field] = object;
                                        //var model = objMain.car1.clone();
                                        //model.position.set(objMain.basePoint.MacatuoX, 0, -objMain.basePoint.MacatuoY);
                                        //model.scale.set(0.002, 0.002, 0.002);
                                        //objMain.roadGroup.add(model);

                                    }, function () { }, function () { });
                            });
                    };
                    f(received_obj, 2, 'carA');
                    f(received_obj, 3, 'carB');
                    f(received_obj, 4, 'carC');
                    f(received_obj, 5, 'carD');
                    f(received_obj, 6, 'carE');
                    f(received_obj, 7, 'carO');
                    f(received_obj, 8, 'carO2');
                    objMain.ws.send('SetRobot');
                }; break;
            case 'SetRMB':
                {
                    var f = function (received_obj, field) {
                        var manager = new THREE.LoadingManager();
                        new THREE.MTLLoader(manager)
                            .loadTextOnly(received_obj.modelBase64[0], 'data:image/jpeg;base64,' + received_obj.modelBase64[1], function (materials) {
                                materials.preload();
                                // materials.depthTest = false;
                                new THREE.OBJLoader(manager)
                                    .setMaterials(materials)
                                    //.setPath('/Pic/')
                                    .loadTextOnly(objMain.rmbModel.geometry, function (object) {
                                        console.log('o', object);
                                        for (var iOfO = 0; iOfO < object.children.length; iOfO++) {
                                            if (object.children[iOfO].isMesh) {
                                                for (var mi = 0; mi < object.children[iOfO].material.length; mi++) {
                                                    object.children[iOfO].material[mi].transparent = true;
                                                    object.children[iOfO].material[mi].opacity = 1;
                                                    object.children[iOfO].material[mi].side = THREE.FrontSide;
                                                    object.children[iOfO].material[mi].color = new THREE.Color(0.45, 0.45, 0.45);
                                                }
                                            }
                                        }
                                        console.log('o', object);
                                        object.scale.set(0.003, 0.003, 0.003);
                                        object.rotateX(-Math.PI / 2);
                                        objMain.rmbModel[field] = object;

                                    }, function () { }, function () { });
                            });
                    };
                    //  f(received_obj, received_obj.faceValue);
                    switch (received_obj.faceValue) {
                        case 'model':
                            {
                                objMain.rmbModel.geometry = received_obj.modelBase64;
                                objMain.ws.send('SetRMB');
                            }; break;
                        case 'rmb100':
                        case 'rmb50':
                        case 'rmb20':
                        case 'rmb10':
                            {
                                f(received_obj, received_obj.faceValue);
                                objMain.ws.send('SetRMB');
                            }; break;
                        case 'rmb5':
                            {
                                f(received_obj, received_obj.faceValue);
                                objMain.rmbModel.geometry = undefined;
                                objMain.ws.send('SetRMB');
                            }; break;
                    };
                }; break;
            case 'SetSpeedIcon':
                {
                    ModelOperateF.f(received_obj, {
                        bind: function (objectInput) {
                            objMain.ModelInput.speed = objectInput;

                        },
                        transparent: { opacity: 0.7 },
                        color: { r: 2, g: 1, b: 2 }
                    });
                    objMain.ws.send('SetSpeedIcon');
                }; break;
            case 'SetAttackIcon':
                {
                    ModelOperateF.f(received_obj, {
                        bind: function (objectInput) {
                            objMain.ModelInput.attack = objectInput;

                        },
                        transparent: { opacity: 0.5 },
                        color: { r: 2, g: 2, b: 0.4 }
                        //transparent: { opacity: 0 }
                    });
                    objMain.ws.send('SetAttackIcon');
                }; break;
            case 'SetShield':
                {
                    ModelOperateF.f(received_obj, {
                        bind: function (objectInput) {
                            objMain.ModelInput.shield = objectInput;

                        },
                        transparent: { opacity: 0.8 },
                        color: { r: 2, g: 1, b: 1 }
                        //transparent: { opacity: 0 }
                    });
                    objMain.ws.send('SetShield');
                }; break;
            case 'SetConfusePrepareIcon':
                {
                    ModelOperateF.f(received_obj, {
                        bind: function (objectInput) {
                            objMain.ModelInput.confusePrepare = objectInput;

                        },
                        transparent: { opacity: 0.8 },
                        color: { r: 1.5, g: 1.5, b: 1.5 }
                        //transparent: { opacity: 0 }
                    });
                    objMain.ws.send('SetConfusePrepareIcon');
                }; break;
            case 'SetLostPrepareIcon':
                {
                    ModelOperateF.f(received_obj, {
                        bind: function (objectInput) {
                            objMain.ModelInput.lostPrepare = objectInput;

                        },
                        transparent: { opacity: 1 },
                        color: { r: 1, g: 1.2, b: 1.2 }
                        //transparent: { opacity: 0 }
                    });
                    objMain.ws.send('SetLostPrepareIcon');
                }; break;
            case 'SetAmbushPrepareIcon':
                {
                    ModelOperateF.f(received_obj, {
                        bind: function (objectInput) {
                            objMain.ModelInput.ambushPrepare = objectInput;

                        },
                        transparent: { opacity: 1 },
                        color: { r: 1.3, g: 0.9, b: 0.9 }
                        //transparent: { opacity: 0 }
                    });
                    objMain.ws.send('SetAmbushPrepareIcon');
                }; break;
            case 'SetWaterIcon':
                {
                    ModelOperateF.f(received_obj, {
                        bind: function (objectInput) {
                            //objMain.ModelInput.ambushPrepare = objectInput;
                            objMain.ModelInput.water = objectInput;
                        },
                        //transparent: { opacity: 0.2 },
                        //color: { r: 1, g: 1, b: 1 }
                        //transparent: { opacity: 0 }
                    });
                    objMain.ws.send('SetWaterIcon');
                }; break;
            case 'SetDirectionIcon':
                {
                    ModelOperateF.f(received_obj, {
                        bind: function (objectInput) {
                            //objMain.ModelInput.ambushPrepare = objectInput;
                            objMain.ModelInput.direction = objectInput;
                        },
                        transparent: { opacity: 0.3 },
                        //color: { r: 1, g: 1, b: 1 }
                        //transparent: { opacity: 0 }
                    });
                    objMain.ws.send('SetDirectionIcon');
                }; break;
            case 'SetDirectionArrowIcon':
                {
                    ModelOperateF.f(received_obj, {
                        bind: function (objectInput) {
                            //objMain.ModelInput.ambushPrepare = objectInput;
                            objMain.ModelInput.directionArrow = objectInput;
                        },
                        transparent: { opacity: 0.3 },
                        //color: { r: 1, g: 1, b: 1 }
                        //transparent: { opacity: 0 }
                    });
                    objMain.ws.send('SetDirectionArrowIcon');
                }; break;
            case 'BradCastAnimateOfCar':
                {
                    //已作废,用BradCastAnimateOfSelfCar与BradCastAnimateOfOthersCar代替
                    throw (received_obj.c + '已作废');
                    return;
                    console.log(evt.data);
                    var passObj = JSON.parse(evt.data);
                    var animateData = passObj.Animate;
                    var carId = passObj.carID;

                    var recordTime = Date.now() + 5000;
                    //   animateData.recordTime = recordTime;
                    objMain.carsAnimateData[carId] = { 'recordTime': recordTime, 'animateData': animateData };
                    /*
                     * 之前这里需要广播，前台验证。这里采用服务器session机制，在服务器进行判断。
                     * 
                     */
                }; break;
            case 'BradCastAnimateOfSelfCar':
            case 'BradCastAnimateOfOthersCar2':
                {
                    var passObj = JSON.parse(evt.data);
                    console.log('新对象', passObj);
                    var deltaT = passObj.Animate.deltaT;
                    var carId = passObj.carID;
                    var recordTime = Date.now() - deltaT;
                    var animateData = [];
                    var start = passObj.Animate.start;
                    start.x /= 256;
                    start.y /= 256;
                    var startT = 0;
                    var isParking = passObj.Animate.isParking;
                    for (var i = 0; i < passObj.Animate.animateData.length; i += 3) {
                        animateData.push(
                            {
                                x0: start.x,
                                y0: start.y,
                                t0: startT,
                                x1: passObj.Animate.animateData[i] / 256 + start.x,
                                y1: passObj.Animate.animateData[i + 1] / 256 + start.y,
                                t1: passObj.Animate.animateData[i + 2] + startT
                            });
                        start.x += passObj.Animate.animateData[i] / 256;
                        start.y += passObj.Animate.animateData[i + 1] / 256;
                        startT += passObj.Animate.animateData[i + 2];
                    };
                    if (objMain.carsAnimateData[carId] == undefined) {
                        objMain.carsAnimateData[carId] = { 'recordTime': recordTime, 'animateData': animateData };
                    }
                    else {
                        if (isParking) { }
                        else {
                            objMain.carsAnimateData[carId] = { 'recordTime': recordTime, 'animateData': animateData };
                        }
                    }
                    return;
                    //var animateData = passObj.Animate.animateData;
                    //var deltaT = passObj.Animate.deltaT;
                    //var carId = passObj.carID;

                    //var recordTime = Date.now() - deltaT;
                    ////   animateData.recordTime = recordTime;
                    //objMain.carsAnimateData[carId] = { 'recordTime': recordTime, 'animateData': animateData };
                }; break;
            case 'BradCastPromoteInfoDetail':
                {
                    //alert('1');
                    console.log('显示', received_obj);
                    //  switch (received_obj.
                    objMain.PromotePositions[received_obj.resultType] = received_obj;
                    objMain.mainF.refreshPromotionDiamondAndPanle(received_obj);


                }; break;
            case 'BradCastCollectInfoDetail':
                {
                    console.log('显示', received_obj);
                    //  switch (received_obj.
                    objMain.CollectPosition = received_obj;
                    //objMain.mainF.refreshCollectAndPanle(received_obj.);
                    //  objMain.mainF.refreshCollectPositionAndPanle(received_obj);
                    // objMain.mainF.refreshPromotionDiamondAndPanle(received_obj);
                }; break;
            case 'SetDiamond':
                {
                    var manager = new THREE.LoadingManager();
                    new THREE.OBJLoader(manager)
                        .loadTextOnly(received_obj.DiamondObj, function (object) {
                            console.log('SetDiamond', object.children[0].geometry);
                            var geometry = object.children[0].geometry;
                            objMain.diamondGeometry = geometry;
                        }, function () { }, function () { });

                    objMain.ws.send('SetDiamond');
                }; break;
            case 'DialogMsg':
                {
                    dialogSys.dealWithMsg(received_obj);
                }; break;
            case 'BradCastMoneyForSave':
                {
                    objMain.MoneyForSave = received_obj.Money;
                }; break;
            case 'BradCastPromoteDiamondCount':
                {
                    objMain.PromoteDiamondCount[received_obj.pType] = received_obj.count;
                    objMain.mainF.drawDiamondCollected();
                }; break;
            case 'BradCastAbility':
                {
                    carAbility.drawPanel('car');
                    carAbility.setData(received_obj);

                    carAbility.drawChanel(received_obj.carIndexStr);
                    carAbility.refreshPosition();
                    //carAbility.updateNotify();
                }; break;
            case 'MoneyForSaveNotify':
                {
                    moneyOperator.MoneyForSave = received_obj.MoneyForSave;
                    moneyOperator.updateMoneyForSave();
                }; break;
            case 'LeftMoneyInDB':
                {
                    subsidizeSys.LeftMoneyInDB[received_obj.address] = received_obj.Money;
                    subsidizeSys.updateMoneyOfSumSubsidizing();
                }; break;
            case 'SupportNotify':
                {
                    subsidizeSys.SupportMoney = received_obj.Money;
                    subsidizeSys.updateMoneyOfSumSubsidized();
                }; break;
            case 'TheLargestHolderChangedNotify':
                {
                    theLagestHoderKey.data[received_obj.operateKey] = received_obj;
                    objMain.mainF.updateCollectGroup();
                }; break;
            case 'OthersRemove':
                {
                    objMain.mainF.removeRole(received_obj.othersKey);
                    theLagestHoderKey.removeData(received_obj.othersKey);

                    delete SocialResponsibility.data[received_obj.othersKey];
                    delete objMain.rightAndDuty.data[received_obj.othersKey];
                    // SocialResponsibility.data[received_obj.otherKey] = received_obj.socialResponsibility;
                }; break;
            case 'SingleRoadPathData':
                {
                    // MapData.meshPoints.push(received_obj.meshPoints);
                    var basePoint = received_obj.basePoint;
                    for (var i = 0; i < received_obj.meshPoints.length; i += 8) {
                        var itemData = [
                            (received_obj.meshPoints[i + 0] + basePoint[0]) / 1000000,
                            (received_obj.meshPoints[i + 1] + basePoint[1]) / 1000000,
                            (received_obj.meshPoints[i + 2] + basePoint[0]) / 1000000,
                            (received_obj.meshPoints[i + 3] + basePoint[1]) / 1000000,
                            (received_obj.meshPoints[i + 4] + basePoint[0]) / 1000000,
                            (received_obj.meshPoints[i + 5] + basePoint[1]) / 1000000,
                            (received_obj.meshPoints[i + 6] + basePoint[0]) / 1000000,
                            (received_obj.meshPoints[i + 7] + basePoint[1]) / 1000000,
                        ];
                        MapData.meshPoints.push(itemData);
                        // MapData.meshPoints.push(received_obj.meshPoints[i]);
                    }
                    //for (var i = 0; i < received_obj.meshPoints.length; i++) {
                    //    MapData.meshPoints.push(received_obj.meshPoints[i]);
                    //}
                    var drawRoadInfomation = function () {

                        objMain.mainF.removeF.clearGroup(objMain.roadGroup);
                        //  objMain.F.clearGroup(
                        var obj = MapData.meshPoints;

                        var positions = [];
                        var colors = [];
                        for (var i = 0; i < obj.length; i++) {
                            positions.push(
                                MercatorGetXbyLongitude(obj[i][0]), 0, -MercatorGetYbyLatitude(obj[i][1]),
                                MercatorGetXbyLongitude(obj[i][2]), 0, -MercatorGetYbyLatitude(obj[i][3]),
                                MercatorGetXbyLongitude(obj[i][4]), 0, -MercatorGetYbyLatitude(obj[i][5]),
                                MercatorGetXbyLongitude(obj[i][4]), 0, -MercatorGetYbyLatitude(obj[i][5]),
                                MercatorGetXbyLongitude(obj[i][6]), 0, -MercatorGetYbyLatitude(obj[i][7]),
                                MercatorGetXbyLongitude(obj[i][0]), 0, -MercatorGetYbyLatitude(obj[i][1]),

                            );
                        }
                        function disposeArray() {

                            this.array = null;

                        }
                        //  console.log('p', positions);
                        //var vertices = new Float32Array(positions);
                        var geometry = new THREE.BufferGeometry();
                        geometry.addAttribute('position', new THREE.Float32BufferAttribute(positions, 3).onUpload(disposeArray));
                        //geometry.addAttribute('color', new THREE.BufferAttribute(colors, 3).onUpload(disposeArray));
                        geometry.computeBoundingSphere();
                        //var material = new THREE.MeshBasicMaterial({ vertexColors: THREE.VertexColors });
                        var material = new THREE.MeshBasicMaterial({ color: 0xff0000 });
                        var mesh = new THREE.Mesh(geometry, material);

                        objMain.roadGroup.add(mesh);


                        var edges = new THREE.EdgesGeometry(geometry);
                        var line = new THREE.LineSegments(edges, new THREE.LineBasicMaterial({ color: 0x0000FF }));
                        objMain.roadGroup.add(line);
                    };
                    drawRoadInfomation();
                }; break;
            case 'SetLeaveGameIcon':
                {
                    objMain.ws.send('SetLeaveGameIcon');
                    //alert('SetLeaveGameIcon');
                    var obj = received_obj.data[0];
                    var mtl = received_obj.data[1];
                    var imageBase64s = {};
                    for (var i = 2; i < received_obj.data.length; i += 2) {
                        var index = received_obj.data[i];
                        var data = received_obj.data[i + 1];
                        imageBase64s[index] = 'data:image/jpeg;base64,' + data;
                    }
                    console.log('imageBase64s', imageBase64s);
                    // objMain.leaveGameModel = received_obj.
                    var f = function (obj, mtl, imageBase64s) {
                        var manager = new THREE.LoadingManager();
                        new THREE.MTLLoader(manager)
                            .loadTextWithMulImg(mtl, imageBase64s, function (materials) {
                                materials.preload();
                                // materials.depthTest = false;
                                new THREE.OBJLoader(manager)
                                    .setMaterials(materials)
                                    //.setPath('/Pic/')
                                    .loadTextOnly(obj, function (object) {
                                        console.log('o', object);
                                        //for (var iOfO = 0; iOfO < object.children.length; iOfO++) {
                                        //    if (object.children[iOfO].isMesh) {
                                        //        for (var mi = 0; mi < object.children[iOfO].material.length; mi++) {
                                        //            object.children[iOfO].material[mi].transparent = true;
                                        //            object.children[iOfO].material[mi].opacity = 1;
                                        //            object.children[iOfO].material[mi].side = THREE.FrontSide;
                                        //            object.children[iOfO].material[mi].color = new THREE.Color(0.45, 0.45, 0.45);
                                        //        }
                                        //    }
                                        //}
                                        console.log('o', object);
                                        object.scale.set(0.003, 0.003, 0.003);
                                        object.rotateX(-Math.PI / 2);
                                        objMain.leaveGameModel = object;
                                        // objMain.rmbModel[field] = object;

                                    }, function () { }, function () { });
                            });
                    };
                    f(obj, mtl, imageBase64s);
                }; break;
            case 'SetProfileIcon':
                {
                    objMain.ws.send('ProfileIcon');
                    //alert('SetLeaveGameIcon');
                    var obj = received_obj.data[0];
                    var mtl = received_obj.data[1];
                    var imageBase64s = {};
                    for (var i = 2; i < received_obj.data.length; i += 2) {
                        var index = received_obj.data[i];
                        var data = received_obj.data[i + 1];
                        imageBase64s[index] = 'data:image/jpeg;base64,' + data;
                    }
                    console.log('imageBase64s', imageBase64s);
                    // objMain.leaveGameModel = received_obj.
                    var f = function (obj, mtl, imageBase64s) {
                        var manager = new THREE.LoadingManager();
                        new THREE.MTLLoader(manager)
                            .loadTextWithMulImg(mtl, imageBase64s, function (materials) {
                                materials.preload();
                                // materials.depthTest = false;
                                new THREE.OBJLoader(manager)
                                    .setMaterials(materials)
                                    //.setPath('/Pic/')
                                    .loadTextOnly(obj, function (object) {
                                        console.log('o', object);
                                        //for (var iOfO = 0; iOfO < object.children.length; iOfO++) {
                                        //    if (object.children[iOfO].isMesh) {
                                        //        for (var mi = 0; mi < object.children[iOfO].material.length; mi++) {
                                        //            object.children[iOfO].material[mi].transparent = true;
                                        //            object.children[iOfO].material[mi].opacity = 1;
                                        //            object.children[iOfO].material[mi].side = THREE.FrontSide;
                                        //            object.children[iOfO].material[mi].color = new THREE.Color(0.45, 0.45, 0.45);
                                        //        }
                                        //    }
                                        //}
                                        console.log('o', object);
                                        object.scale.set(0.003, 0.003, 0.003);
                                        object.rotateX(-Math.PI / 2);
                                        objMain.profileModel = object;
                                        // objMain.rmbModel[field] = object;

                                    }, function () { }, function () { });
                            });
                    };
                    f(obj, mtl, imageBase64s);
                }; break;
            case 'MoneyNotify':
                {
                    objMain.Money = received_obj.Money;
                    moneyShow.show();
                }; break;
            case 'BradCastSocialResponsibility':
                {
                    SocialResponsibility.data[received_obj.otherKey] = received_obj.socialResponsibility;
                    //SocialResponsibility.data.add
                    if (objMain.indexKey == received_obj.otherKey) {
                        SocialResponsibility.show();
                    }
                }; break;
            case 'GetName':
                {
                    if (document.getElementById('playerNameTextArea') != undefined) {
                        document.getElementById('playerNameTextArea').value = received_obj.name;
                    }
                }; break;
            case 'GetCarsName':
                {
                    for (var i = 0; i < 5; i++) {
                        var iName = 'car' + (i + 1) + 'NameTextArea';
                        if (document.getElementById(iName) != undefined) {
                            document.getElementById(iName).value = received_obj.names[i];
                        }
                    }

                }; break;
            case 'BradCarState':
                {
                    objMain.carState[received_obj.carID] = received_obj.State;
                    objNotify.notifyCar(received_obj.carID, received_obj.State);
                    operatePanel.refresh();
                    //  marketOperate.refresh();
                }; break;
            case 'BradCastCollectInfoDetail_v2':
                {

                    console.log('显示', received_obj);
                    objMain.CollectPosition[received_obj.collectIndex] = received_obj;
                    ////  switch (received_obj.
                    //objMain.CollectPosition = received_obj;
                    objMain.mainF.refreshCollectAndPanle(received_obj.collectIndex, undefined);
                }; break;
            case 'BradCarPurpose':
                {
                    console.log('显示', 'BradCarPurpose');
                }; break;
            case 'BradCastRightAndDuty':
                {
                    objMain.rightAndDuty.data[received_obj.playerKey] =
                    {
                        right: received_obj.right,
                        duty: received_obj.duty,
                        rightPercent: received_obj.rightPercent,
                        dutyPercent: received_obj.dutyPercent
                    }
                }; break;
            case 'BradCastMusicTheme':
                {
                    objMain.music.theme = received_obj.theme;
                }; break;
            case 'BradCastBackground':
                {
                    objMain.background.path = received_obj.path;
                    objMain.background.change();
                }; break;
            case 'WMsg':
                {
                    $.notify(received_obj.Msg, { style: "happyblue" })
                }; break;
            case 'BradDiamondPrice':
                {
                    objMain.diamondPrice[received_obj.priceType] = received_obj.price;
                }; break;
            case 'DriverNotify':
                {
                    objMain.driver.driverIndex = received_obj.index;
                    objMain.driver.name = received_obj.name;
                    objMain.driver.skill1.name = received_obj.skill1Name;
                    objMain.driver.skill1.skillIndex = received_obj.skill1Index;
                    objMain.driver.skill2.name = received_obj.skill2Name;
                    objMain.driver.skill2.skillIndex = received_obj.skill2Index;
                    objMain.driver.sex = received_obj.sex;
                    objMain.driver.race = received_obj.race;
                    driverSys.drawIcon(objMain.driver);
                    operatePanel.refresh();
                }; break;
            case 'SpeedNotify':
                {
                    if (received_obj.On) { stateSet.speed.add(received_obj.Key); }
                    else { stateSet.speed.clear(received_obj.Key); }
                }; break;
            case 'AttackNotify':
                {
                    if (received_obj.On) { stateSet.attck.add(received_obj.Key); }
                    else { stateSet.attck.clear(received_obj.Key); }
                }; break;
            case 'DefenceNotify':
                {
                    if (received_obj.On) { stateSet.defend.add(received_obj.Key); }
                    else { stateSet.defend.clear(received_obj.Key); }
                }; break;
            case 'ViewSearch':
                {
                    var animationData =
                    {
                        old: {
                            x: objMain.controls.target.x,
                            y: objMain.controls.target.y,
                            z: objMain.controls.target.z,
                            t: Date.now()
                        },
                        newT:
                        {
                            x: received_obj.mctX,
                            y: objMain.controls.target.y,
                            z: 0 - received_obj.mctY,
                            t: Date.now() + 3000
                        }
                    };
                    objMain.camaraAnimateData = animationData;
                }; break;
            case 'ConfusePrepareNotify':
                {
                    if (received_obj.On) {
                        stateSet.control.clear(received_obj.Key);
                        var animateData = { startX: received_obj.StartX / 256, startY: objMain.controls.target.y, startZ: received_obj.StartY / -256, start: Date.now(), endX: received_obj.EndX / 256, endY: objMain.controls.target.y, endZ: received_obj.EndY / -256 };
                        stateSet.confusePrepare.add(received_obj.Key, animateData);
                    }
                }; break;
            case 'LostPrepareNotify':
                {
                    if (received_obj.On) {
                        stateSet.control.clear(received_obj.Key);
                        var animateData = { startX: received_obj.StartX / 256, startY: objMain.controls.target.y, startZ: received_obj.StartY / -256, start: Date.now(), endX: received_obj.EndX / 256, endY: objMain.controls.target.y, endZ: received_obj.EndY / -256 };
                        stateSet.lostPrepare.add(received_obj.Key, animateData);
                    }
                }; break;
            case 'AmbushPrepareNotify':
                {
                    if (received_obj.On) {
                        stateSet.control.clear(received_obj.Key);
                        var animateData = { startX: received_obj.StartX / 256, startY: objMain.controls.target.y, startZ: received_obj.StartY / -256, start: Date.now(), endX: received_obj.EndX / 256, endY: objMain.controls.target.y, endZ: received_obj.EndY / -256 };
                        stateSet.ambusePrepare.add(received_obj.Key, animateData);
                    }
                }; break;
            case 'ControlPrepareNotify':
                {
                    if (received_obj.On) {
                    }
                    else {
                        // stateSet.confusePrepare.clear(received_obj.Key);
                        stateSet.control.clear(received_obj.Key);
                    }
                }; break;
            case 'LoseNotify':
                {
                    if (received_obj.On) { stateSet.lost.add(received_obj.Key); }
                    else { stateSet.lost.clear(received_obj.Key); }
                }; break;
            case 'ConfuseNotify':
                {
                    if (received_obj.On) { stateSet.confuse.add(received_obj.Key); }
                    else { stateSet.confuse.clear(received_obj.Key); }
                }; break;
            case 'FireNotify':
                {
                    stateSet.fire.add(received_obj.targetRoleID, received_obj.actionRoleID);
                }; break;
            case 'WaterNotify':
                {
                    stateSet.water.add(received_obj.targetRoleID, received_obj.actionRoleID);
                }; break;
            case 'ElectricNotify':
                {
                    stateSet.lightning.add(received_obj.targetRoleID, received_obj.actionRoleID);
                }; break;
            case 'ShowDirectionOperator':
                {
                    DirectionOperator.show(received_obj);
                }; break;
            case 'ModelDataShow':
                {
                    var modelDataShow = received_obj;
                    BuildingModelObj.f(modelDataShow);
                    if (modelDataShow.existed) {

                    }

                }; break;
            default:
                {
                    console.log('命令未注册', received_obj.c + "__没有注册。");
                }; break;
        }
    },
    diamondPrice:
    {
        'mile': 0,
        'business': 0,
        'volume': 0,
        'speed': 0
    },
    driver:
    {
        driverIndex: -1,
        sex: null,
        name: '',
        skill1: { name: '', skillIndex: -1 },
        skill2: { name: '', skillIndex: -1 },
        race: ''
    },
    camaraAnimateData: null
};
var startA = function () {
    var connected = false;
    var wsConnect = '';
    if (objMain.debug)
        wsConnect = 'ws://127.0.0.1:11001/websocket';
    else
        wsConnect = 'wss://www.nyrq123.com/websocket' + window.location.pathname.split('/')[1] + '/';
    var ws = new WebSocket(wsConnect);
    ws.onopen = function () {
        {
            var session = '';
            if (sessionStorage['session'] == undefined) {

            }
            else {
                session = sessionStorage['session'];
            }
            ws.send(JSON.stringify({ c: 'CheckSession', session: session }));
        }
        //   alert("数据发送中...");
    };
    ws.onmessage = function (evt) {
        var received_msg = evt.data;
        //console.log(evt.data);
        var received_obj = JSON.parse(evt.data);
        objMain.dealWithReceivedObj(received_obj, evt, received_msg);
    };
    ws.onclose = function () {
        // 关闭 websocket
        alert("连接已关闭...");
    };
    objMain.ws = ws;
    $.notify.addStyle('happyblue', {
        html: "<div><span data-notify-text/></div>",
        classes: {
            base: {
                "opacity": "0.85",
                "width": "90%",
                "max-width": "90%",
                "background": "#F5F5F5",
                "padding": "5px",
                "border- radius": "10px"
            },
            superblue: {
                "opacity": "0.85",
                "width": "90%",
                "max-width": "90%",
                "background": "#F5F5F5",
                "padding": "5px",
                "border- radius": "10px"
            }
        }
    });
}
startA();

function animate() {
    {
        requestAnimationFrame(animate);
        if (objMain.state != objMain.receivedState) {
            objMain.state = objMain.receivedState;
        }
        if (objMain.state == 'OnLine') {

            var lengthOfCC = objMain.mainF.getLength(objMain.camera.position, objMain.controls.target);
            {
                if (objMain.camaraAnimateData != null) {
                    if (objMain.camaraAnimateData.newT.t < Date.now()) {
                        objMain.camaraAnimateData = null;
                    }
                    else {
                        var percent1 = (Date.now() - objMain.camaraAnimateData.old.t) / 3000 * Math.PI;
                        var percent2 = (Math.sin(percent1 - Math.PI / 2) + 1) / 2;
                        var x = objMain.camaraAnimateData.old.x + (objMain.camaraAnimateData.newT.x - objMain.camaraAnimateData.old.x) * percent2;
                        var y = objMain.camaraAnimateData.old.y + (objMain.camaraAnimateData.newT.y - objMain.camaraAnimateData.old.y) * percent2;
                        var z = objMain.camaraAnimateData.old.z + (objMain.camaraAnimateData.newT.z - objMain.camaraAnimateData.old.z) * percent2;
                        objMain.controls.target.set(x, y, z);
                        var angle = objMain.controls.getPolarAngle();
                        //if(
                        var dCal = objMain.mainF.getLength(objMain.camera.position, objMain.controls.target);
                        var distance = lengthOfCC;
                        var unitY = distance * Math.cos(angle);
                        var unitZX = distance * Math.sin(angle);

                        var angleOfCamara = objMain.controls.getAzimuthalAngle();
                        var unitX = unitZX * Math.sin(angleOfCamara);
                        var unitZ = unitZX * Math.cos(angleOfCamara);
                        //var unitX = unitZX * Math.sin(-complexV.toAngle() - Math.PI / 2);
                        //var unitZ = unitZX * Math.cos(-complexV.toAngle() - Math.PI / 2);

                        objMain.camera.position.set(x + unitX, y + unitY, z + unitZ);
                        objMain.camera.lookAt(x, y, z);
                    }
                }

            }

            for (var i = 0; i < objMain.collectGroup.children.length; i++) {
                /*
                 * 初始化人民币的大小
                 */

                if (objMain.collectGroup.children[i].isGroup) {
                    //if (objMain.Task.state == 'collect') {
                    //    var scale = objMain.mainF.getLength(objMain.camera.position, objMain.controls.target) / 1226;
                    //    objMain.collectGroup.children[i].scale.set(scale, scale, scale);
                    //}
                    //else
                    {
                        var scale = 0.006;//; objMain.mainF.getLength(objMain.camera.position, objMain.controls.target) / 1840;
                        objMain.collectGroup.children[i].scale.set(scale, scale, scale);
                    }
                    objMain.collectGroup.children[i].rotation.set(-Math.PI / 2, 0, Date.now() % 3000 / 3000 * Math.PI * 2);
                    objMain.collectGroup.children[i].position.y = 0;
                }
            }

            for (var i = 0; i < objMain.playerGroup.children.length; i++) {
                /*
                 * 初始化 旗帜的大小
                 */

                if (objMain.playerGroup.children[i].isMesh) {
                    {
                        objMain.playerGroup.children[i].scale.set(0.0015, 0.0015, 0.0015);
                        objMain.playerGroup.children[i].position.y = 0.1;
                        stateSet.defend.Animate(objMain.playerGroup.children[i].name.split('_')[1]);
                        stateSet.confusePrepare.Animate(objMain.playerGroup.children[i].name.split('_')[1]);
                        stateSet.lostPrepare.Animate(objMain.playerGroup.children[i].name.split('_')[1]);
                        stateSet.ambusePrepare.Animate(objMain.playerGroup.children[i].name.split('_')[1]);
                        stateSet.water.Animate(objMain.playerGroup.children[i].name.split('_')[1]);
                        stateSet.fire.Animate(objMain.playerGroup.children[i].name.split('_')[1]);
                        stateSet.lightning.Animate(objMain.playerGroup.children[i].name.split('_')[1]);
                    }
                }
            }

            for (var i = 0; i < objMain.promoteDiamond.children.length; i++) {
                /*
                 * 初始化 砖石/人民币的大小
                 */
                if (objMain.promoteDiamond.children[i].isMesh) {
                    objMain.promoteDiamond.children[i].scale.set(0.2, 0.22, 0.2);

                }
            }

            for (var i = 0; i < objMain.columnGroup.children.length; i++) {
                /*
                 * 初始化 砖石/人民币的大小
                 */
                if (objMain.columnGroup.children[i].isMesh) {
                    objMain.columnGroup.children[i].scale.setX(1);
                    objMain.columnGroup.children[i].scale.setZ(1);
                }
            }
            {
                var lengthOfObjs = objMain.groupOfOperatePanle.children.length;
                for (var i = lengthOfObjs - 1; i >= 0; i--) {
                    objMain.groupOfOperatePanle.remove(objMain.groupOfOperatePanle.children[i]);
                }
            }
            if (objMain.canSelect) {

                objMain.Task.state = '';
                var scale = 0.01;
                objMain.raycasterOfSelector.setFromCamera(objMain.selectorPosition, objMain.camera);
                var selectObj = null;
                var maxCosA = -1;
                {
                    for (var i = 0; i < objMain.collectGroup.children.length; i++) {
                        if (objMain.collectGroup.children[i].isGroup) {
                            var position = objMain.collectGroup.children[i].position;
                            var d = new THREE.Vector3(position.x - objMain.camera.position.x, position.y - objMain.camera.position.y, position.z - objMain.camera.position.z);
                            var cosA = objMain.raycasterOfSelector.ray.direction.dot(d) / d.length() / objMain.raycasterOfSelector.ray.direction.length();
                            if (cosA > 0.984807753)
                                if (cosA > maxCosA) {
                                    maxCosA = cosA;
                                    objMain.Task.state = 'collect';
                                    selectObj = objMain.collectGroup.children[i];
                                    scale = 0.01 * objMain.mainF.getLength(objMain.camera.position, position) / 10;
                                    scale = Math.max(scale, 0.01);
                                }
                        }
                    }

                    for (var i = 0; i < objMain.playerGroup.children.length; i++) {
                        if (objMain.playerGroup.children[i].isMesh) {
                            //flag_d59195aa49213765a72fdff81b1e18c6
                            if (objMain.playerGroup.children[i].name.split('_')[1] == objMain.indexKey) {
                                var position = objMain.playerGroup.children[i].position;
                                var d = new THREE.Vector3(position.x - objMain.camera.position.x, position.y - objMain.camera.position.y, position.z - objMain.camera.position.z);
                                var cosA = objMain.raycasterOfSelector.ray.direction.dot(d) / d.length() / objMain.raycasterOfSelector.ray.direction.length();
                                if (cosA > 0.984807753)
                                    if (cosA > maxCosA) {
                                        maxCosA = cosA;
                                        objMain.Task.state = 'setReturn';
                                        selectObj = objMain.playerGroup.children[i];
                                        scale = 0.0025 * objMain.mainF.getLength(objMain.camera.position, position) / 10;
                                        scale = Math.max(scale, 0.0015);
                                    }
                            }
                            else {
                                var position = objMain.playerGroup.children[i].position;
                                var d = new THREE.Vector3(position.x - objMain.camera.position.x, position.y - objMain.camera.position.y, position.z - objMain.camera.position.z);
                                var cosA = objMain.raycasterOfSelector.ray.direction.dot(d) / d.length() / objMain.raycasterOfSelector.ray.direction.length();
                                if (cosA > 0.984807753)
                                    if (cosA > maxCosA) {
                                        maxCosA = cosA;
                                        objMain.Task.state = 'attack';
                                        selectObj = objMain.playerGroup.children[i];
                                        scale = 0.0025 * objMain.mainF.getLength(objMain.camera.position, position) / 10;
                                        scale = Math.max(scale, 0.0015);
                                    }
                            }
                        }
                    }

                    for (var i = 0; i < objMain.promoteDiamond.children.length; i++) {
                        if (objMain.promoteDiamond.children[i].isMesh) {
                            {
                                var position = objMain.promoteDiamond.children[i].position;
                                var d = new THREE.Vector3(position.x - objMain.camera.position.x, position.y - objMain.camera.position.y, position.z - objMain.camera.position.z);
                                var cosA = objMain.raycasterOfSelector.ray.direction.dot(d) / d.length() / objMain.raycasterOfSelector.ray.direction.length();
                                if (cosA > 0.984807753)
                                    if (cosA > maxCosA) {
                                        maxCosA = cosA;

                                        objMain.Task.state = '';
                                        selectObj = objMain.promoteDiamond.children[i];
                                        scale = 2 * objMain.mainF.getLength(objMain.camera.position, position) / 10;
                                        scale = Math.max(scale, 0.2);
                                        switch (selectObj.name) {
                                            case 'diamond_mile':
                                                {
                                                    objMain.Task.state = 'mile';
                                                }; break;
                                            case 'diamond_business':
                                                {
                                                    objMain.Task.state = 'business';
                                                }; break;
                                            case 'diamond_volume':
                                                {
                                                    objMain.Task.state = 'volume';
                                                }; break;
                                            case 'diamond_speed':
                                                {
                                                    objMain.Task.state = 'speed';
                                                }; break;
                                            default: { }; break;
                                        }
                                    }
                            }
                        }
                    }

                    if (objMain.carState.car == 'waitAtBaseStation')
                        for (var i = 0; i < objMain.columnGroup.children.length; i++) {
                            if (objMain.columnGroup.children[i].isMesh) {
                                {
                                    var position = objMain.columnGroup.children[i].position;
                                    var d = new THREE.Vector3(position.x - objMain.camera.position.x, position.y - objMain.camera.position.y, position.z - objMain.camera.position.z);
                                    var cosA = objMain.raycasterOfSelector.ray.direction.dot(d) / d.length() / objMain.raycasterOfSelector.ray.direction.length();
                                    if (cosA > 0.984807753)
                                        if (cosA > maxCosA) {
                                            maxCosA = cosA;

                                            objMain.Task.state = '';
                                            selectObj = objMain.columnGroup.children[i];
                                            //scale = 2 * objMain.mainF.getLength(objMain.camera.position, position) / 10;
                                            //scale = Math.max(scale, 0.2);
                                            switch (selectObj.name) {
                                                case 'BatteryMile':
                                                    {
                                                        objMain.Task.state = 'ability';
                                                    }; break;
                                                case 'BatteryBusiness':
                                                    {
                                                        objMain.Task.state = 'ability';
                                                    }; break;
                                                case 'BatteryVolume':
                                                    {
                                                        objMain.Task.state = 'ability';
                                                    }; break;
                                                case 'BatterySpeed':
                                                    {
                                                        objMain.Task.state = 'ability';
                                                    }; break;
                                                default: { }; break;
                                            }
                                        }
                                }
                            }
                        }

                    switch (objMain.Task.state) {
                        case 'collect':
                            {
                                selectObj.scale.set(scale, scale, scale);
                                objMain.selectObj.obj = selectObj;
                                objMain.selectObj.type = objMain.Task.state;
                                selectObj.rotation.set(-Math.PI / 2, 0, Date.now() % 600 / 600 * Math.PI * 2);
                                {
                                    var collectPosition = selectObj.userData.collectPosition;
                                    var element = document.createElement('div');
                                    element.style.width = '10em';
                                    element.style.marginTop = '3em';
                                    var color = '#ff0000';
                                    element.style.border = '2px solid ' + color;
                                    element.style.borderTopLeftRadius = '0.5em';
                                    element.style.backgroundColor = 'rgba(245, 255, 179, 0.9)';
                                    element.style.color = '#1504f6';

                                    var div2 = document.createElement('div');
                                    div2.style.fontSize = '0.5em';

                                    var b = document.createElement('b');
                                    b.innerHTML = '到' + collectPosition.Fp.region + '[<span style="color:#05ffba">' + collectPosition.Fp.FastenPositionName + '</span>]回收<span style="color:#05ffba">' + (collectPosition.collectMoney).toFixed(2) + '元</span>现金。';
                                    div2.appendChild(b);

                                    element.appendChild(div2);

                                    var object = new THREE.CSS2DObject(element);
                                    var fp = collectPosition.Fp;
                                    object.position.set(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde));

                                    objMain.groupOfOperatePanle.add(object);
                                }
                            }; break;
                        case 'setReturn':
                            {
                                selectObj.scale.set(scale, scale, scale);
                                objMain.selectObj.obj = selectObj;
                                objMain.selectObj.type = objMain.Task.state;
                                selectObj.rotation.set(-Math.PI / 2, 0, Date.now() % 300 / 300 * Math.PI * 2);
                                {
                                    var element = document.createElement('div');
                                    element.style.width = '10em';
                                    element.style.marginTop = '3em';
                                    var color = '#ff0000';
                                    element.style.border = '2px solid ' + color;
                                    element.style.borderTopLeftRadius = '0.5em';
                                    element.style.backgroundColor = 'rgba(245, 255, 179, 0.9)';
                                    element.style.color = '#1504f6';

                                    var div2 = document.createElement('div');
                                    div2.style.fontSize = '0.5em';

                                    var b = document.createElement('b');
                                    b.innerHTML = '回到基地->' + objMain.basePoint.FastenPositionName + '(' + objMain.basePoint.Longitude.toFixed(2) + ',' + objMain.basePoint.Latitde.toFixed(2) + ')';
                                    div2.appendChild(b);

                                    element.appendChild(div2);

                                    var object = new THREE.CSS2DObject(element);
                                    //  var fp = collectPosition.Fp;
                                    object.position.set(selectObj.position.x, 0, selectObj.position.z);

                                    objMain.groupOfOperatePanle.add(object);
                                }
                            }; break;
                        case 'attack':
                            {
                                selectObj.scale.set(scale, scale, scale);
                                objMain.selectObj.obj = selectObj;
                                objMain.selectObj.type = objMain.Task.state;
                                selectObj.rotation.set(-Math.PI / 2, 0, Date.now() % 300 / 300 * Math.PI * 2);
                                {
                                    var element = document.createElement('div');
                                    element.style.width = '10em';
                                    element.style.marginTop = '3em';
                                    var color = '#ff0000';
                                    element.style.border = '2px solid ' + color;
                                    element.style.borderTopLeftRadius = '0.5em';
                                    element.style.backgroundColor = 'rgba(245, 255, 179, 0.9)';
                                    element.style.color = '#1504f6';

                                    var div2 = document.createElement('div');
                                    div2.style.fontSize = '0.5em';

                                    var b = document.createElement('b');

                                    var customTagIndexKey = selectObj.name.substring(5);
                                    if (objMain.othersBasePoint[customTagIndexKey] == undefined)
                                        b.innerHTML = '攻击';
                                    else {
                                        if (objMain.othersBasePoint[customTagIndexKey].isNPC) {
                                            //b.innerHTML = '[NPC]->到';
                                            b.innerHTML = b.innerHTML = '到(' + objMain.othersBasePoint[customTagIndexKey].basePoint.region + ')'
                                                + objMain.othersBasePoint[customTagIndexKey].basePoint.FastenPositionName
                                                + '(' + objMain.othersBasePoint[customTagIndexKey].basePoint.Longitude.toFixed(2) + ','
                                                + objMain.othersBasePoint[customTagIndexKey].basePoint.Latitde.toFixed(2) + ')攻击NPC'
                                                + '【' + objMain.othersBasePoint[customTagIndexKey].playerName + '】'
                                                + '(' + objMain.othersBasePoint[customTagIndexKey].Level + '级)';

                                        }
                                        else if (objMain.othersBasePoint[customTagIndexKey].isPlayer) {
                                            b.innerHTML = '到(' + objMain.othersBasePoint[customTagIndexKey].basePoint.region + ')'
                                                + objMain.othersBasePoint[customTagIndexKey].basePoint.FastenPositionName
                                                + '(' + objMain.othersBasePoint[customTagIndexKey].basePoint.Longitude.toFixed(2) + ','
                                                + objMain.othersBasePoint[customTagIndexKey].basePoint.Latitde.toFixed(2) + ')攻击玩家'
                                                + '【' + objMain.othersBasePoint[customTagIndexKey].playerName + '】'
                                                + '(' + objMain.othersBasePoint[customTagIndexKey].Level + '级)';
                                        }
                                        else {
                                            b.innerHTML = '攻击';
                                        }
                                    }
                                    div2.appendChild(b);

                                    element.appendChild(div2);

                                    var object = new THREE.CSS2DObject(element);
                                    //  var fp = collectPosition.Fp;
                                    object.position.set(selectObj.position.x, 0, selectObj.position.z);

                                    objMain.groupOfOperatePanle.add(object);
                                }
                            }; break;
                        case 'mile':
                        case 'business':
                        case 'volume':
                        case 'speed':
                            {
                                objMain.selectObj.obj = selectObj;
                                objMain.selectObj.type = objMain.Task.state;
                                // objMain.promoteDiamond.getChildByName('diamond_' + objMain.Task.state).scale.set(scale, scale * 1.1, scale);
                                //
                                selectObj.scale.set(scale, scale * 1.1, scale);


                                //objMain.ws.send(JSON.stringify({ 'c': 'Promote', 'pType': objMain.Task.state }));
                            }; break;
                        case 'ability':
                            {
                                objMain.selectObj.obj = selectObj;
                                objMain.selectObj.type = objMain.Task.state;
                                selectObj.scale.setX((Date.now() % 2000 / 2000) + 1);
                                selectObj.scale.setZ((Date.now() % 2000 / 2000) + 1);

                                {
                                    var element = document.createElement('div');
                                    element.style.width = '10em';
                                    element.style.marginTop = '3em';
                                    var color = '#ff0000';
                                    element.style.border = '2px solid ' + color;
                                    element.style.borderTopLeftRadius = '0.5em';
                                    element.style.backgroundColor = 'rgba(245, 255, 179, 0.9)';
                                    element.style.color = '#1504f6';

                                    var div2 = document.createElement('div');
                                    div2.style.fontSize = '0.5em';

                                    var b = document.createElement('b');
                                    switch (selectObj.name) {
                                        case 'BatteryMile':
                                            {
                                                b.innerHTML = '红宝石市价:' + (objMain.diamondPrice.mile / 100.0).toFixed(2) + "银两。用其提升最大里程。你有"
                                                    + objMain.PromoteDiamondCount.mile + '块';

                                            }; break;
                                        case 'BatteryBusiness':
                                            {
                                                b.innerHTML = '绿宝石市价:' + (objMain.diamondPrice.business / 100.0).toFixed(2) + "银两。用其提升最大业务能力。你有"
                                                    + objMain.PromoteDiamondCount.business + '块';
                                            }; break;
                                        case 'BatteryVolume':
                                            {
                                                b.innerHTML = '蓝宝石市价:' + (objMain.diamondPrice.volume / 100.0).toFixed(2) + "银两。用其提升最大收集能力。你有"
                                                    + objMain.PromoteDiamondCount.volume + '块';
                                            }; break;
                                        case 'BatterySpeed':
                                            {
                                                b.innerHTML = '黑宝石市价:' + (objMain.diamondPrice.speed / 100.0).toFixed(2) + "银两。用其提升速度。你有"
                                                    + objMain.PromoteDiamondCount.speed + '块';
                                            }; break;
                                    }

                                    div2.appendChild(b);

                                    element.appendChild(div2);

                                    var object = new THREE.CSS2DObject(element);
                                    //  var fp = collectPosition.Fp;
                                    object.position.set(selectObj.position.x, 0, selectObj.position.z);

                                    objMain.groupOfOperatePanle.add(object);
                                }
                            }; break;
                    }
                }
            }
            for (var i = 0; i < objMain.playerGroup.children.length; i++) {
                if (objMain.playerGroup.children[i].isMesh) {
                    objMain.playerGroup.children[i].userData.animateDataYrq.simulate(Date.now());
                    objMain.playerGroup.children[i].userData.animateDataYrq.refresh(Date.now());
                }
            }
            for (var i = 0; i < objMain.carGroup.children.length; i++) {
                /*
                 * 初始化汽车的大小
                 */
                if (objMain.carGroup.children[i].isGroup) {
                    objMain.carGroup.children[i].scale.set(0.002, 0.002, 0.002);
                    objMain.carGroup.children[1].name.split('_')[1]
                    stateSet.speed.Animate(objMain.carGroup.children[i].name.split('_')[1]);
                    stateSet.attck.Animate(objMain.carGroup.children[i].name.split('_')[1]);
                    stateSet.confuse.Animate(objMain.carGroup.children[i].name.split('_')[1]);
                    stateSet.lost.Animate(objMain.carGroup.children[i].name.split('_')[1]);
                }
            }



            //for (var i = 0; i < objMain.playerGroup.children.length; i++) {
            //    /*
            //     * 初始化 旗帜的大小
            //     */


            //}

            var lengthOfPAndC = objMain.mainF.getLength(objMain.camera.position, objMain.controls.target);


            {
                /*放大选中的汽车*/
                var scale = objMain.mainF.getLength(objMain.camera.position, objMain.controls.target) * 0.001;
                if (scale < 0.002) {
                    scale = 0.002;
                }
                if (objMain.Task.carSelect != '') {
                    if (objMain.carGroup.getObjectByName(objMain.Task.carSelect) != undefined) {
                        objMain.carGroup.getObjectByName(objMain.Task.carSelect).scale.set(scale, scale, scale);
                    }
                }

            }
            {
                /*放大选中的砖石*/
                var scale = objMain.mainF.getLength(objMain.camera.position, objMain.controls.target) / 35;
                if (scale < 0.2) {
                    scale = 0.2;
                }
                if (objMain.PromoteList.indexOf(objMain.Task.state) >= 0) {
                    objMain.promoteDiamond.children[0].scale.set(scale, scale * 1.1, scale);
                }
            }
            {
                /*放大选中的RMB*/

            }
            {

            }

            {
                /*汽车的移动动画*/
                for (let key of Object.keys(objMain.carsAnimateData)) {
                    let animateData = objMain.carsAnimateData[key].animateData;
                    let recordTime = objMain.carsAnimateData[key].recordTime;
                    var now = Date.now();
                    var isSelf = (key == 'car_' + objMain.indexKey);
                    var isAnimation = false;
                    if (isSelf) {
                        isAnimation = true;
                        //isAnimation = objMain.carState['car'] == 'working' || objMain.carState['car'] == 'waitAtBaseStation'
                    }
                    //var angleOfCamara = 0;
                    //if (isSelf)
                    //{
                    //   
                    //}
                    for (var i = 0; i < animateData.length; i++) {
                        var percent = (now - recordTime - animateData[i].t0) / (animateData[i].t1 - animateData[i].t0);
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
                            //if (isSelf && isAnimation && objMain.carState.car == 'selecting') {
                            //    //  console.log('');
                            //    //  alert('selecting');
                            //    objMain.controls.target.set(x, 0, -y);
                            //    var angle = objMain.controls.getPolarAngle();
                            //    //if(
                            //    var dCal = objMain.mainF.getLength(objMain.camera.position, objMain.controls.target);
                            //    var distance = 8;
                            //    if (dCal >= 9) {
                            //        distance = dCal * 0.99 - 0.01;
                            //    }
                            //    else if (dCal <= 7) {
                            //        distance = dCal * 1.01 + 0.01;
                            //    }
                            //    var unitY = distance * Math.cos(angle);
                            //    var unitZX = distance * Math.sin(angle);

                            //    var angleOfCamara = objMain.controls.getAzimuthalAngle();
                            //    var unitX = unitZX * Math.sin(angleOfCamara);
                            //    var unitZ = unitZX * Math.cos(angleOfCamara);
                            //    //var unitX = unitZX * Math.sin(-complexV.toAngle() - Math.PI / 2);
                            //    //var unitZ = unitZX * Math.cos(-complexV.toAngle() - Math.PI / 2);

                            //    objMain.camera.position.set(x + unitX, unitY, -y + unitZ);
                            //    objMain.camera.lookAt(x, 0, -y);
                            //}
                        }
                    }
                    //if (key == 'car_' + objMain.indexKey)
                    //{

                    //}
                }
            }

            if (objMain.carState.car == 'selecting') {
                objMain.directionGroup.visible = true;
                if (objMain.directionGroup.children.length > 0) {
                    var p = objMain.directionGroup.children[0].position;
                    var x = p.x;
                    var y = -p.z;
                    objMain.controls.target.set(x, 0, -y);
                    var angle = objMain.controls.getPolarAngle();
                    //if(
                    var dCal = objMain.mainF.getLength(objMain.camera.position, objMain.controls.target);
                    var distance = 3;
                    if (dCal >= 3.5) {
                        distance = dCal * 0.99 - 0.01;
                    }
                    else if (dCal <= 2.5) {
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
            }
            else {
                objMain.directionGroup.visible = false;
            }
            objMain.animation.animateCameraByCarAndTask();

            theLagestHoderKey.animate();
            objMain.renderer.render(objMain.scene, objMain.camera);
            objMain.labelRenderer.render(objMain.scene, objMain.camera);
            objMain.light1.position.set(objMain.camera.position.x, objMain.camera.position.y, objMain.camera.position.z);
        }
    }
}
animate();
var selectSingleTeamJoinHtml = function () {
    document.getElementById('rootContainer').innerHTML = selectSingleTeamJoinHtmlF.drawHtml();
}
var buttonClick = function (v) {
    if (objMain.receivedState == 'selectSingleTeamJoin') {
        switch (v) {
            case 'single':
                {
                    objMain.ws.send(JSON.stringify({ c: 'JoinGameSingle' }));
                    objMain.receivedState = '';
                }; break;
            case 'team':
                {
                    objMain.ws.send(JSON.stringify({ c: 'CreateTeam' }));
                    objMain.receivedState = '';
                }; break;
            case 'join':
                {
                    objMain.ws.send(JSON.stringify({ c: 'JoinTeam' }));
                    objMain.receivedState = '';
                }; break;
            case 'setName':
                {
                    // objMain.ws.send(JSON.stringify({ c: 'JoinTeam' }));
                    objMain.receivedState = 'setName';
                    selectSingleTeamJoinHtmlF.setNameHtmlShow();
                    objMain.ws.send(JSON.stringify({ c: 'GetName' }));
                }; break;
            case 'setCarsName':
                {
                    // objMain.ws.send(JSON.stringify({ c: 'JoinTeam' }));
                    objMain.receivedState = 'setCarsName';
                    selectSingleTeamJoinHtmlF.setCarsNameHtmlShow();
                    objMain.ws.send(JSON.stringify({ c: 'GetCarsName' }));
                }; break;
        }
        // objMain.receivedState = '';
    }

}

var set3DHtml = function () {
    //var text = "";
    //text += "  <div>";
    //text += "            3D界面";
    //text += "        </div>";
    //document.getElementById('rootContainer').innerHTML = text;
    document.getElementById('rootContainer').innerHTML = '';

    //<div id="mainC" class="container" onclick="testTop();">
    //    <!--<img />-->
    //    <!--<a href="DAL/MapImage.ashx">DAL/MapImage.ashx</a>-->
    //    <img src="Pic/11.png" />
    //</div>
    var mainC = document.createElement('div');
    mainC.id = 'mainC';

    mainC.className = 'container';
    document.getElementById('rootContainer').appendChild(mainC);

    objMain.scene = new THREE.Scene();
    //objMain.scene.background = new THREE.Color(0x7c9dd4);
    //objMain.scene.fog = new THREE.FogExp2(0x7c9dd4, 0.2);

    var cubeTextureLoader = new THREE.CubeTextureLoader();
    cubeTextureLoader.setPath('Pic/');
    //var cubeTexture = cubeTextureLoader.load([
    //    "xi_r.jpg", "dong_r.jpg",
    //    "ding_r.jpg", "di_r.jpg",
    //    "nan_r.jpg", "bei_r.jpg"
    //]);
    var cubeTexture = cubeTextureLoader.load([
        "px.jpg", "nx.jpg",
        "py.jpg", "ny.jpg",
        "pz.jpg", "nz.jpg"
    ]);
    objMain.scene.background = cubeTexture;

    objMain.renderer = new THREE.WebGLRenderer({ alpha: true });
    objMain.renderer.setClearColor(0x000000, 0); // the default
    objMain.renderer.setPixelRatio(window.devicePixelRatio);
    objMain.renderer.setSize(window.innerWidth, window.innerHeight);
    objMain.renderer.domElement.className = 'renderDom';
    document.getElementById('mainC').appendChild(objMain.renderer.domElement);
    //  document.body

    objMain.labelRenderer = new THREE.CSS2DRenderer();
    objMain.labelRenderer.setSize(window.innerWidth, window.innerHeight);
    objMain.labelRenderer.domElement.className = 'labelRenderer';
    //objMain.labelRenderer.domElement.style.curs
    document.getElementById('mainC').appendChild(objMain.labelRenderer.domElement);

    objMain.camera = new THREE.PerspectiveCamera(35, window.innerWidth / window.innerHeight, 0.1, 30000);
    objMain.camera.position.set(4000, 2000, 0);
    objMain.camera.position.set(MercatorGetXbyLongitude(objMain.centerPosition.lon), 20, -MercatorGetYbyLatitude(objMain.centerPosition.lat));

    objMain.controls = new THREE.OrbitControls(objMain.camera, objMain.labelRenderer.domElement);
    objMain.controls.center.set(MercatorGetXbyLongitude(objMain.centerPosition.lon), 0, -MercatorGetYbyLatitude(objMain.centerPosition.lat));

    objMain.roadGroup = new THREE.Group();
    objMain.scene.add(objMain.roadGroup);

    objMain.playerGroup = new THREE.Group();
    objMain.scene.add(objMain.playerGroup);

    objMain.promoteDiamond = new THREE.Group();
    objMain.scene.add(objMain.promoteDiamond);

    objMain.columnGroup = new THREE.Group();
    objMain.scene.add(objMain.columnGroup);

    objMain.carGroup = new THREE.Group();
    objMain.scene.add(objMain.carGroup);

    objMain.groupOfOperatePanle = new THREE.Group();
    objMain.scene.add(objMain.groupOfOperatePanle);

    objMain.collectGroup = new THREE.Group();
    objMain.scene.add(objMain.collectGroup);

    objMain.getOutGroup = new THREE.Group();
    objMain.scene.add(objMain.getOutGroup);

    objMain.taxGroup = new THREE.Group();
    objMain.scene.add(objMain.taxGroup);

    objMain.shieldGroup = new THREE.Group();
    objMain.scene.add(objMain.shieldGroup);

    objMain.confusePrepareGroup = new THREE.Group();
    objMain.scene.add(objMain.confusePrepareGroup);

    objMain.lostPrepareGroup = new THREE.Group();
    objMain.scene.add(objMain.lostPrepareGroup);

    objMain.ambushPrepareGroup = new THREE.Group();
    objMain.scene.add(objMain.ambushPrepareGroup);

    objMain.waterGroup = new THREE.Group();
    objMain.scene.add(objMain.waterGroup);

    objMain.fireGroup = new THREE.Group();
    objMain.scene.add(objMain.fireGroup);

    objMain.lightningGroup = new THREE.Group();
    objMain.scene.add(objMain.lightningGroup);

    objMain.directionGroup = new THREE.Group();
    objMain.scene.add(objMain.directionGroup);

    objMain.buildingGroup = new THREE.Group();
    objMain.scene.add(objMain.buildingGroup);

    objMain.clock = new THREE.Clock();

    {
        objMain.light1 = new THREE.PointLight(0xffffff);
        objMain.light1.position.set(-100, 300, -100);
        objMain.light1.intensity = 2;
        objMain.scene.add(objMain.light1);
    }

    {
        //objMain.controls.minDistance = 3;
        // objMain.controls.maxPolarAngle = Math.PI;
        objMain.controls.minPolarAngle = Math.PI / 600;
        objMain.controls.maxPolarAngle = Math.PI / 2 - Math.PI / 36;
        objMain.controls.minDistance = 2;
        objMain.controls.maxDistance = 256;
    }

    objMain.raycaster = new THREE.Raycaster();
    objMain.raycaster.linePrecision = 0.2;

    objMain.raycasterOfSelector = new THREE.Raycaster();
    //objMain.raycasterOfSelector.linePrecision = 100;

    objMain.mouse = new THREE.Vector2();

    //objMain.labelRenderer.domElement.addEventListener

    var operateEnd = function (event) {
        operatePanel.refresh();

        var json = JSON.stringify({ c: 'ViewAngle', x1: objMain.camera.position.x, y1: -objMain.camera.position.z, x2: objMain.controls.target.x, y2: -objMain.controls.target.z });
        objMain.ws.send(json);
        //objMain.ws
        return;
    }
    var operateStart = function (event) {
        objMain.canSelect = true;
        objMain.music.change();
    }
    objMain.labelRenderer.domElement.addEventListener('mouseup', operateEnd, false);
    objMain.labelRenderer.domElement.addEventListener('mousedown', operateStart, false);


    objMain.labelRenderer.domElement.addEventListener('touchstart', operateStart, false);
    objMain.labelRenderer.domElement.addEventListener('touchend', operateEnd, false);
    //scope.domElement.removeEventListener('touchstart', onTouchStart, false);
    //scope.domElement.removeEventListener('touchend', onTouchEnd, false);
    //drawCarBtnsFrame();
    //objNotify.carNotifyShow();
    window.addEventListener('resize', onWindowResize, false);
}
function onWindowResize() {

    objMain.camera.aspect = window.innerWidth / window.innerHeight;
    objMain.camera.updateProjectionMatrix();

    objMain.labelRenderer.setSize(window.innerWidth, window.innerHeight);
    objMain.renderer.setSize(window.innerWidth, window.innerHeight);
    carAbility.refreshPosition();
}


var MapData =
{
    roadAndCrossJson: '',
    roadAndCross: null,
    meshPoints: []
};
var drawPoint = function (color, fp, indexKey) {
    var createFlag = function (color) {
        var that = this;
        this.windStrengthDelta = 0;
        this.DAMPING = 0.03;
        this.DRAG = 1 - this.DAMPING;
        this.MASS = 0.1;
        this.restDistance = 25;
        this.xSegs = 10;
        this.ySegs = 10;
        function plane(width, height) {

            return function (u, v, target) {

                var x = (u - 0.5) * width;
                var y = (v + 0.5) * height;
                var z = 0;

                target.set(x, y, z);

            };

        }
        var clothFunction = plane(this.restDistance * this.xSegs, this.restDistance * this.ySegs);
        function Cloth(w, h) {

            w = w || 10;
            h = h || 10;
            this.w = w;
            this.h = h;

            var particles = [];
            var constraints = [];

            // Create particles
            for (let v = 0; v <= h; v++) {

                for (let u = 0; u <= w; u++) {

                    particles.push(
                        new Particle(u / w, v / h, 0, that.MASS)
                    );

                }

            }

            // Structural

            for (let v = 0; v < h; v++) {

                for (let u = 0; u < w; u++) {

                    constraints.push([
                        particles[index(u, v)],
                        particles[index(u, v + 1)],
                        that.restDistance
                    ]);

                    constraints.push([
                        particles[index(u, v)],
                        particles[index(u + 1, v)],
                        that.restDistance
                    ]);

                }

            }

            for (let u = w, v = 0; v < h; v++) {

                constraints.push([
                    particles[index(u, v)],
                    particles[index(u, v + 1)],
                    that.restDistance

                ]);

            }

            for (let v = h, u = 0; u < w; u++) {

                constraints.push([
                    particles[index(u, v)],
                    particles[index(u + 1, v)],
                    that.restDistance
                ]);

            }
            this.particles = particles;
            this.constraints = constraints;

            function index(u, v) {

                return u + v * (w + 1);

            }

            this.index = index;

        }
        var cloth = new Cloth(this.xSegs, this.ySegs);
        this.cloth = cloth;

        this.GRAVITY = 981 * 1.4;
        this.gravity = new THREE.Vector3(0, -  this.GRAVITY, 0).multiplyScalar(this.MASS);

        this.TIMESTEP = 18 / 1000;
        this.TIMESTEP_SQ = this.TIMESTEP * this.TIMESTEP;

        this.pins = [];

        this.windForce = new THREE.Vector3(0, 0, 0);

        //this. ballPosition = new THREE.Vector3(0, - 45, 0);
        //this. ballSize = 60; //40

        this.tmpForce = new THREE.Vector3();

        function Particle(x, y, z, mass) {

            this.position = new THREE.Vector3();
            this.previous = new THREE.Vector3();
            this.original = new THREE.Vector3();
            this.a = new THREE.Vector3(0, 0, 0); // acceleration
            this.mass = mass;
            this.invMass = 1 / mass;
            this.tmp = new THREE.Vector3();
            this.tmp2 = new THREE.Vector3();

            // init

            clothFunction(x, y, this.position); // position
            clothFunction(x, y, this.previous); // previous
            clothFunction(x, y, this.original);

        }

        // Force -> Acceleration

        Particle.prototype.addForce = function (force) {

            this.a.add(
                this.tmp2.copy(force).multiplyScalar(this.invMass)
            );

        };


        // Performs Verlet integration

        Particle.prototype.integrate = function (timesq) {

            var newPos = this.tmp.subVectors(this.position, this.previous);
            newPos.multiplyScalar(that.DRAG).add(this.position);
            newPos.add(this.a.multiplyScalar(timesq));

            this.tmp = this.previous;
            this.previous = this.position;
            this.position = newPos;

            this.a.set(0, 0, 0);

        };

        this.diff = new THREE.Vector3();
        function satisfyConstraints(p1, p2, distance) {

            that.diff.subVectors(p2.position, p1.position);
            var currentDist = that.diff.length();
            if (currentDist === 0) return; // prevents division by 0
            var correction = that.diff.multiplyScalar(1 - distance / currentDist);
            var correctionHalf = correction.multiplyScalar(0.5);
            p1.position.add(correctionHalf);
            p2.position.sub(correctionHalf);

        }
        this.simulate = function (now) {
            //这里进行动画
            var windStrength = Math.cos(now / 7000) * 20 + 40 + that.windStrengthDelta;

            that.windForce.set(Math.sin(now / 2000), Math.cos(now / 3000), Math.sin(now / 1000));
            that.windForce.normalize();
            that.windForce.multiplyScalar(windStrength);

            // Aerodynamics forces

            var particles = cloth.particles;

            {

                let indx;
                var normal = new THREE.Vector3();
                var indices = clothGeometry.index;
                var normals = clothGeometry.attributes.normal;

                for (let i = 0, il = indices.count; i < il; i += 3) {

                    for (let j = 0; j < 3; j++) {

                        indx = indices.getX(i + j);
                        normal.fromBufferAttribute(normals, indx);
                        that.tmpForce.copy(normal).normalize().multiplyScalar(normal.dot(that.windForce));
                        particles[indx].addForce(that.tmpForce);

                    }

                }

            }

            for (let i = 0, il = particles.length; i < il; i++) {

                var particle = particles[i];
                particle.addForce(that.gravity);

                particle.integrate(that.TIMESTEP_SQ);

            }

            // Start Constraints

            var constraints = cloth.constraints;
            var il = constraints.length;

            for (let i = 0; i < il; i++) {

                var constraint = constraints[i];
                satisfyConstraints(constraint[0], constraint[1], constraint[2]);

            }

            // Ball Constraints




            // Floor Constraints

            for (let i = 0, il = particles.length; i < il; i++) {

                var particle = particles[i];
                var pos = particle.position;
                if (pos.y < - 250) {

                    pos.y = - 250;

                }

            }

            // Pin Constraints

            for (let i = 0, il = that.pins.length; i < il; i++) {

                var xy = that.pins[i];
                var p = particles[xy];
                p.position.copy(p.original);
                p.previous.copy(p.original);

            }


        }

        this.pinsFormation = [];
        this.pins = [6];

        this.pinsFormation.push(this.pins);

        this.pins = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        this.pinsFormation.push(this.pins);

        this.pins = [0];
        this.pinsFormation.push(this.pins);

        this.pins = []; // cut the rope ;)
        this.pinsFormation.push(this.pins);

        this.pins = [0, cloth.w]; // classic 2 pins
        this.pinsFormation.push(this.pins);

        this.pins = this.pinsFormation[1];

        function togglePins() {

            pins = pinsFormation[~ ~(Math.random() * pinsFormation.length)];

        }
        this.clothMaterial = new THREE.MeshLambertMaterial({
            side: THREE.DoubleSide,
            alphaTest: 0.5,
            color: color,
            emissive: color
        });
        var clothGeometry = new THREE.ParametricBufferGeometry(clothFunction, cloth.w, cloth.h);
        this.clothGeometry = clothGeometry;


        this.refresh = function () {
            var p = that.cloth.particles;
            for (let i = 0, il = p.length; i < il; i++) {

                var v = p[i].position;

                that.clothGeometry.attributes.position.setXYZ(i, v.x, v.y, v.z);

            }
            that.clothGeometry.attributes.position.needsUpdate = true;
            that.clothGeometry.computeVertexNormals();
        };

        return this;
    }

    var objToShow = new createFlag(color);
    // return [clothGeometry,]
    object = new THREE.Mesh(objToShow.clothGeometry, objToShow.clothMaterial);
    object.userData['animateDataYrq'] = objToShow;
    object.position.set(MercatorGetXbyLongitude(fp.Longitude), 0.1, -MercatorGetYbyLatitude(fp.Latitde));
    object.scale.set(0.0005, 0.0005, 0.0005);
    objMain.playerGroup.add(object);

    var start = new THREE.Vector3(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde))
    var end = new THREE.Vector3(MercatorGetXbyLongitude(fp.positionLongitudeOnRoad), 0, -MercatorGetYbyLatitude(fp.positionLatitudeOnRoad))
    var cc = new Complex(end.x - start.x, end.z - start.z);
    cc.toOne();
    object.rotateY(-cc.toAngle() + Math.PI / 2);
    //alert('1');
    object.name = 'flag_' + indexKey;
    return;
    var initializeCars = function (fp) {
        var start = new THREE.Vector3(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(objMain.basePoint.Latitde))
        var end = new THREE.Vector3(MercatorGetXbyLongitude(objMain.basePoint.positionLongitudeOnRoad), 0, -MercatorGetYbyLatitude(objMain.basePoint.positionLatitudeOnRoad))
        var cc = new Complex(end.x - start.x, end.z - start.z);
        cc.toOne();

        var positon1 = cc.multiply(new Complex(-0.309016994, 0.951056516));
        var positon2 = positon1.multiply(new Complex(0.809016994, 0.587785252));
        var positon3 = positon2.multiply(new Complex(0.809016994, 0.587785252));
        var positon4 = positon3.multiply(new Complex(0.809016994, 0.587785252));
        var positon5 = positon4.multiply(new Complex(0.809016994, 0.587785252));

        var positons = [positon1, positon2, positon3, positon4, positon5];

        var names = ['carA', 'carB', 'carC', 'carD', 'carE'];
        console.log('positons', positons);
        var percentOfPosition = 0.25;
        for (var i = 0; i < positons.length; i++) {
            var start = new THREE.Vector3(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde));
            var end = new THREE.Vector3(start.x + positons[i].r * percentOfPosition, 0, start.z + positons[i].i * percentOfPosition);
            var lineGeometry = new THREE.Geometry();
            lineGeometry.vertices.push(start);
            lineGeometry.vertices.push(end);
            var lineMaterial = new THREE.LineBasicMaterial({ color: color });
            var line = new THREE.Line(lineGeometry, lineMaterial);
            objMain.scene.add(line);

            var model = objMain.cars[names[i]].clone();
            model.position.set(end.x, 0, end.z);
            model.scale.set(0.002, 0.002, 0.002);
            model.rotateY(-positons[i].toAngle());
            objMain.roadGroup.add(model);
        }
        var minDistance = objMain.controls.minDistance * 1.1;
        var maxPolarAngle = objMain.controls.maxPolarAngle - Math.PI / 30;
    }




    //  cc.multiply(6);


    var positon1 = cc.multiply(new Complex(-0.309016994, 0.951056516));
    var positon2 = positon1.multiply(new Complex(0.809016994, 0.587785252));
    var positon3 = positon2.multiply(new Complex(0.809016994, 0.587785252));
    var positon4 = positon3.multiply(new Complex(0.809016994, 0.587785252));
    var positon5 = positon4.multiply(new Complex(0.809016994, 0.587785252));

    var positons = [positon1, positon2, positon3, positon4, positon5];
    var names = ['carA', 'carB', 'carC', 'carD', 'carE'];
    console.log('positons', positons);
    var percentOfPosition = 0.25;
    for (var i = 0; i < positons.length; i++) {
        var start = new THREE.Vector3(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde));
        var end = new THREE.Vector3(start.x + positons[i].r * percentOfPosition, 0, start.z + positons[i].i * percentOfPosition);
        var lineGeometry = new THREE.Geometry();
        lineGeometry.vertices.push(start);
        lineGeometry.vertices.push(end);
        var lineMaterial = new THREE.LineBasicMaterial({ color: color });
        var line = new THREE.Line(lineGeometry, lineMaterial);
        objMain.scene.add(line);

        var model = objMain.cars[names[i]].clone();
        model.position.set(end.x, 0, end.z);
        model.scale.set(0.002, 0.002, 0.002);
        model.rotateY(-positons[i].toAngle());
        objMain.roadGroup.add(model);
    }
    var minDistance = objMain.controls.minDistance * 1.1;
    var maxPolarAngle = objMain.controls.maxPolarAngle - Math.PI / 30;
    //var kValue = planeLength
    {
        var planePosition = new THREE.Vector3(start.x + cc.r * minDistance * Math.sin(maxPolarAngle), start.y + minDistance * Math.cos(maxPolarAngle), start.z + cc.i * minDistance * Math.sin(maxPolarAngle));
        objMain.camera.position.set(planePosition.x, planePosition.y, planePosition.z);

        objMain.controls.target.set(objMain.basePoint.MacatuoX, 0, -objMain.basePoint.MacatuoY);
        objMain.camera.lookAt(objMain.basePoint.MacatuoX, 0, -objMain.basePoint.MacatuoY);

        // var polarAngle = objMain.controls.getPolarAngle();
        //   objMain.camera.position.set(objMain.basePoint.MacatuoX, 10, -objMain.basePoint.MacatuoY - Math.tan(Math.PI / 6) * 10);

        objMain.camera.lookAt(objMain.basePoint.MacatuoX, 0, -objMain.basePoint.MacatuoY);
    }
}

var drawCarBtns = function () {
}

var SysOperatePanel =
{
    draw: function () {
        while (document.getElementById('sysOperatePanel') != null) {
            document.getElementById('sysOperatePanel').remove();
        }
        var divSysOperatePanel = document.createElement('div');
        divSysOperatePanel.id = 'sysOperatePanel';
        divSysOperatePanel.style.position = 'absolute';
        divSysOperatePanel.style.zIndex = '7';
        divSysOperatePanel.style.top = 'calc(100% - 2.5em - 8px)';
        divSysOperatePanel.style.left = '8px';
        {
            var img = document.createElement('img');
            img.id = 'msgToNotify'
            img.src = 'Pic/chatPng.png';
            img.classList.add('chatdialog');
            img.style.border = 'solid 1px yellowgreen';
            img.style.borderRadius = '5px';
            img.style.height = 'calc(2.5em - 2px)';
            img.style.width = 'auto';

            img.onclick = function () {
                // alert('打开聊天框');
                dialogSys.show();
            };

            divSysOperatePanel.appendChild(img);
        }
        {
            var img = document.createElement('img');
            img.id = 'moneyServe'
            img.src = 'Pic/trade.png';
            img.classList.add('chatdialog');
            img.style.border = 'solid 1px yellowgreen';
            img.style.borderRadius = '5px';
            img.style.height = 'calc(2.5em - 2px)';
            img.style.width = 'auto';
            img.style.marginLeft = "0.5em";
            img.onclick = function () {
                // alert('打开聊天框');
                //      dialogSys.show();
                //alert('弹出对话框');
                moneyOperator.add();
            };

            divSysOperatePanel.appendChild(img);
        }
        {
            var img = document.createElement('img');
            img.id = 'moneySubsidize'
            img.src = 'Pic/subsidize.png';
            img.classList.add('chatdialog');
            img.style.border = 'solid 1px yellowgreen';
            img.style.borderRadius = '5px';
            img.style.height = 'calc(2.5em - 2px)';
            img.style.width = 'auto';
            img.style.marginLeft = "0.5em";
            img.onclick = function () {
                // alert('打开聊天框');
                //      dialogSys.show();
                //alert('弹出对话框');
                subsidizeSys.add();
                //moneyOperator.add();
            };

            divSysOperatePanel.appendChild(img);
        }
        {
            var img = document.createElement('img');
            img.id = 'moneySubsidize'
            img.src = 'Pic/subsidize.png';
            img.classList.add('chatdialog');
            img.style.border = 'solid 1px yellowgreen';
            img.style.borderRadius = '5px';
            img.style.height = 'calc(2.5em - 2px)';
            img.style.width = 'auto';
            img.style.marginLeft = "0.5em";
            img.onclick = function () {
                alert('债务框！');
                //      dialogSys.show();
                //alert('弹出对话框');
                //subsidizeSys.add();
                //moneyOperator.add();
            };

            divSysOperatePanel.appendChild(img);
        }
        document.body.appendChild(divSysOperatePanel);
    },
    notifyMsg: function () {
        var element = document.getElementById('msgToNotify');
        element.classList.add('msg');
    },
    cancelNotifyMsg: function () {
        var element = document.getElementById('msgToNotify');
        element.classList.remove('msg');
    }
};

var setWaitingToStart = function () {
    var text = "";
    text += "  <div>";
    text += "          请等待";
    text += "        </div>";
    document.getElementById('rootContainer').innerHTML = text;
}
var token =
{
    CommandStart: '',

}


var createTeam = function (teamCreateFinish) {
    document.getElementById('rootContainer').innerHTML = '';
    var div1 = document.createElement('div');
    div1.style.textAlign = 'center';
    var addDiv = function (title, content) {
        var div = document.createElement('div');
        var label = document.createElement('label');
        var b = document.createElement('b');
        label.innerText = title;
        b.innerText = content;
        div.appendChild(label);
        div.appendChild(b);
        return div;
    }
    div1.appendChild(addDiv('房间号：', teamCreateFinish.TeamNum));
    div1.appendChild(addDiv('队长：', teamCreateFinish.PlayerName));

    document.getElementById('rootContainer').appendChild(div1);

    var div2 = document.createElement('div');
    div2.style.textAlign = 'center';

    var button = document.createElement("button");
    button.innerText = '开始';
    button.style.width = "5em";
    button.style.height = "3em";
    button.style.marginTop = "1em";
    button.onclick = function () {
        objMain.ws.send(token.CommandStart);
    };
    div2.appendChild(button);
    document.getElementById('rootContainer').appendChild(div2);
}

var setWaitingToGetTeam = function () {
    document.getElementById('rootContainer').innerHTML = '';
    var div1 = document.createElement('div');
    div1.style.textAlign = 'center';
    div1.style.marginTop = '2em';
    var label = document.createElement('label');
    label.innerText = '房间号';
    var input = document.createElement('input');
    input.id = 'roomNumInput';
    input.type = 'number';
    div1.appendChild(label);
    div1.appendChild(input);
    document.getElementById('rootContainer').appendChild(div1);

    var div2 = document.createElement('div');
    div2.style.textAlign = 'center';

    var button = document.createElement("button");
    button.innerText = '加入';
    button.style.width = "5em";
    button.style.height = "3em";
    button.style.marginTop = "1em";
    button.onclick = function () {
        console.log('提示', '加入事件还没有写写哦');
        var roomNumInput = document.getElementById('roomNumInput').value;
        if (roomNumInput == '') {
            alert('不要输入空');
        }
        else {
            objMain.ws.send(roomNumInput);
            this.onclick = function () { };
        }
    };
    div2.appendChild(button);
    document.getElementById('rootContainer').appendChild(div2);
}


var joinTeamDetail = function (teamJoinFinish) {
    document.getElementById('rootContainer').innerHTML = '';
    var div1 = document.createElement('div');
    div1.style.textAlign = 'center';

    var addDiv = function (title, content) {
        var div = document.createElement('div');
        var label = document.createElement('label');
        var b = document.createElement('b');
        label.innerText = title;
        b.innerText = content;
        div.appendChild(label);
        div.appendChild(b);
        return div;
    }
    div1.appendChild(addDiv('房间号：', teamJoinFinish.TeamNum));
    div1.appendChild(addDiv('队长：', teamJoinFinish.PlayerNames[0]));

    for (var i = 1; i < teamJoinFinish.PlayerNames.length; i++) {
        div1.appendChild(addDiv('队员：', teamJoinFinish.PlayerNames[i]));
    }

    document.getElementById('rootContainer').appendChild(div1);

    var div2 = document.createElement('div');
    div2.style.textAlign = 'center';

    document.getElementById('rootContainer').appendChild(div2);
}

var broadTeamJoin = function (teamJoinBroadInfo) {
    var addDiv = function (title, content) {
        var div = document.createElement('div');
        var label = document.createElement('label');
        var b = document.createElement('b');
        label.innerText = title;
        b.innerText = content;
        div.appendChild(label);
        div.appendChild(b);
        return div;
    }
    document.getElementById('rootContainer').children[0].appendChild(addDiv('队员：', teamJoinBroadInfo.PlayerName));
}

var marketOperate =
{
    refresh: function () {
        marketOperate.clearPanel();
        marketOperate.state = '';
        switch (objMain.carState["car"]) {
            case 'waitAtBaseStation':
                {
                    marketOperate.drawCarBtns();
                }; break;
            default:
                {
                }; break;
        }
    },
    clearPanel: function () {
        while (document.getElementById('taskOperatingPanel') != null) {
            document.getElementById('taskOperatingPanel').remove();
        }
    },
    drawCarBtns: function () {
        {
            var clearBtnOfObj = function (id) {
                if (document.getElementById(id) == null) { }
                else {
                    var tp = document.getElementById(id);
                    while (tp.children.length > 0) {
                        tp.children[0].remove();
                    }
                }
            }

            var ff2 = function () {
                var divTaskOperatingPanel = document.createElement('div');
                divTaskOperatingPanel.id = 'taskOperatingPanel';

                divTaskOperatingPanel.style.position = 'absolute';
                divTaskOperatingPanel.style.zIndex = '7';
                divTaskOperatingPanel.style.right = '20px';
                divTaskOperatingPanel.style.border = 'none';
                divTaskOperatingPanel.style.width = '5em';
                divTaskOperatingPanel.style.color = 'green';
                //每个子对象1.3em 8px 共2.5个
                divTaskOperatingPanel.style.top = 'calc(50% - 3.25em - 20px)';

                var addItemToTaskOperatingPanle = function (btnName, id, clickF) {
                    var div = document.createElement('div');
                    div.style.width = 'calc(5em - 4px)';
                    div.style.textAlign = 'center';
                    div.style.border = '2px inset #ffc403';
                    div.style.borderRadius = '0.3em';
                    div.style.marginTop = '4px';
                    div.style.marginBottom = '4px';
                    div.style.background = 'rgba(0, 191, 255, 0.6)';
                    div.style.height = '1.3em';
                    div.id = id;

                    var span = document.createElement('span');
                    span.innerText = btnName;
                    div.appendChild(span);
                    //  <span id="carASpan">提升续航</span>

                    div.onclick = function () { clickF(); }
                    divTaskOperatingPanel.appendChild(div);
                }

                addItemToTaskOperatingPanle('宝石', 'useGetDiamondBtn', function () {


                    var tmp_S1 = arguments;
                    clearBtnOfObj('taskOperatingPanel');
                    addItemToTaskOperatingPanle('购买', 'buyDiamondBtn', function () {

                        objMain.ws.send(JSON.stringify({ 'c': 'BuyDiamond', 'pType': objMain.selectObj.obj.userData.index }));
                    });
                    addItemToTaskOperatingPanle('出售', 'sellDiamondBtn', function () {
                        objMain.ws.send(JSON.stringify({ 'c': 'SellDiamond', 'pType': objMain.selectObj.obj.userData.index }));
                    });
                    addItemToTaskOperatingPanle('取消', 'cancleBtn', function () {
                        marketOperate.state = '';
                        marketOperate.refresh();

                    });



                    /*------*/

                });

                document.body.appendChild(divTaskOperatingPanel);
            }

            ff2();
        }

    },
    state: ''
};

var operatePanel =
{
    clearPanel: function () {
        while (document.getElementById('taskOperatingPanel') != null) {
            document.getElementById('taskOperatingPanel').remove();
        }
    },
    refresh: function () {
        operatePanel.clearPanel();
        var clearBtnOfObj = function (id) {
            if (document.getElementById(id) == null) { }
            else {
                var tp = document.getElementById(id);
                while (tp.children.length > 0) {
                    tp.children[0].remove();
                }
            }
        }
        var divTaskOperatingPanel = document.createElement('div');
        divTaskOperatingPanel.id = 'taskOperatingPanel';

        divTaskOperatingPanel.style.position = 'absolute';
        divTaskOperatingPanel.style.zIndex = '7';
        divTaskOperatingPanel.style.right = '20px';
        divTaskOperatingPanel.style.border = 'none';
        divTaskOperatingPanel.style.width = '5em';
        divTaskOperatingPanel.style.color = 'green';
        //每个子对象1.3em 8px 共2.5个
        divTaskOperatingPanel.style.top = 'calc(50% - 3.25em - 20px)';
        var addItemToTaskOperatingPanle = function (btnName, id, clickF) {
            var div = document.createElement('div');
            div.style.width = 'calc(5em - 4px)';
            div.style.textAlign = 'center';
            div.style.border = '2px inset #ffc403';
            div.style.borderRadius = '0.3em';
            div.style.marginTop = '4px';
            div.style.marginBottom = '4px';
            div.style.background = 'rgba(0, 191, 255, 0.6)';
            div.style.height = '1.3em';
            div.id = id;

            var span = document.createElement('span');
            span.innerText = btnName;
            div.appendChild(span);
            //  <span id="carASpan">提升续航</span>

            div.onclick = function () { clickF(); }
            divTaskOperatingPanel.appendChild(div);
        }
        document.body.appendChild(divTaskOperatingPanel);

        var carState = objMain.carState["car"];

        var attackF = function () {
            addItemToTaskOperatingPanle('攻击', 'attackBtn', function () {
                objMain.canSelect = false;
                if (objMain.carState["car"] == 'waitAtBaseStation' || objMain.carState["car"] == 'waitOnRoad') {
                    var selectObj = objMain.selectObj.obj;
                    var customTagIndexKey = selectObj.name.substring(5);
                    if (objMain.othersBasePoint[customTagIndexKey] != undefined) {
                        var fPIndex = objMain.othersBasePoint[customTagIndexKey].fPIndex;
                        objMain.ws.send(JSON.stringify({ 'c': 'Attack', 'TargetOwner': customTagIndexKey, 'Target': fPIndex }));

                    }
                    objMain.selectObj.obj = null;
                    objMain.selectObj.type = '';
                    operatePanel.refresh();
                }
            });

        };
        var magicF = function () {
            if (objMain.driver.driverIndex > 0) {
                addItemToTaskOperatingPanle(objMain.driver.skill1.name, 'skill1Btn', function () {
                    objMain.canSelect = false;
                    if (objMain.carState["car"] == 'waitAtBaseStation' || objMain.carState["car"] == 'waitOnRoad') {
                        var selectObj = objMain.selectObj.obj;
                        var customTagIndexKey = selectObj.name.substring(5);
                        if (objMain.othersBasePoint[customTagIndexKey] != undefined) {
                            var fPIndex = objMain.othersBasePoint[customTagIndexKey].fPIndex;
                            objMain.ws.send(JSON.stringify({ 'c': 'Skill1', 'TargetOwner': customTagIndexKey, 'Target': fPIndex }));

                        }
                        else if (objMain.driver.race == 'devil' && objMain.indexKey == customTagIndexKey) {
                            var fPIndex = objMain.fpIndex;
                            objMain.ws.send(JSON.stringify({ 'c': 'Skill1', 'TargetOwner': customTagIndexKey, 'Target': fPIndex }));
                        }
                        objMain.selectObj.obj = null;
                        objMain.selectObj.type = '';
                        operatePanel.refresh();
                    }
                });
                addItemToTaskOperatingPanle(objMain.driver.skill2.name, 'skill2Btn', function () {
                    objMain.canSelect = false;
                    if (objMain.carState["car"] == 'waitAtBaseStation' || objMain.carState["car"] == 'waitOnRoad') {
                        var selectObj = objMain.selectObj.obj;
                        var customTagIndexKey = selectObj.name.substring(5);
                        if (objMain.othersBasePoint[customTagIndexKey] != undefined) {
                            var fPIndex = objMain.othersBasePoint[customTagIndexKey].fPIndex;
                            objMain.ws.send(JSON.stringify({ 'c': 'Skill2', 'TargetOwner': customTagIndexKey, 'Target': fPIndex }));

                        }
                        else if (objMain.driver.race == 'devil' && objMain.indexKey == customTagIndexKey) {
                            var fPIndex = objMain.fpIndex;
                            objMain.ws.send(JSON.stringify({ 'c': 'Skill2', 'TargetOwner': customTagIndexKey, 'Target': fPIndex }));
                        }
                        objMain.selectObj.obj = null;
                        objMain.selectObj.type = '';
                        operatePanel.refresh();
                    }
                });
            }
        };
        var lookUp = function () {
            addItemToTaskOperatingPanle('查看', 'viewBtn', function () {
                objMain.canSelect = false;
                if (objMain.carState["car"] == 'waitAtBaseStation' || objMain.carState["car"] == 'waitOnRoad') {
                    var selectObj = objMain.selectObj.obj;
                    var animationData =
                    {
                        old: {
                            x: objMain.controls.target.x,
                            y: objMain.controls.target.y,
                            z: objMain.controls.target.z,
                            t: Date.now()
                        },
                        newT:
                        {
                            x: objMain.selectObj.obj.position.x,
                            y: objMain.selectObj.obj.position.y,
                            z: objMain.selectObj.obj.position.z,
                            t: Date.now() + 3000
                        }
                    };
                    objMain.camaraAnimateData = animationData;
                    objMain.selectObj.obj = null;
                    objMain.selectObj.type = '';
                    operatePanel.refresh();
                }
            });
        };
        switch (carState) {
            case 'waitAtBaseStation':
                {
                    switch (objMain.Task.state) {
                        case 'collect':
                            {
                                addItemToTaskOperatingPanle('收集', 'collectBtn', function () {
                                    objMain.canSelect = false;
                                    if (objMain.carState["car"] == 'waitAtBaseStation' || objMain.carState["car"] == 'waitOnRoad') {
                                        var selectObj = objMain.selectObj.obj;
                                        // console.log('selectObj', selectObj.userData.collectPosition.Fp.FastenPositionID);
                                        objMain.ws.send(JSON.stringify({ 'c': 'Collect', 'cType': 'findWork', 'fastenpositionID': selectObj.userData.collectPosition.Fp.FastenPositionID, 'collectIndex': selectObj.userData.collectPosition.collectIndex }));
                                        objMain.selectObj.obj = null;
                                        objMain.selectObj.type = '';
                                        operatePanel.refresh();
                                    }
                                });
                                lookUp();
                            }; break;
                        case 'attack':
                            {
                                attackF();
                                lookUp();
                                //addItemToTaskOperatingPanle('攻击', 'attackBtn', function () {
                                //    objMain.canSelect = false;
                                //    if (objMain.carState["car"] == 'waitAtBaseStation' || objMain.carState["car"] == 'waitOnRoad') {
                                //        var selectObj = objMain.selectObj.obj;
                                //        var customTagIndexKey = selectObj.name.substring(5);
                                //        if (objMain.othersBasePoint[customTagIndexKey] != undefined) {
                                //            var fPIndex = objMain.othersBasePoint[customTagIndexKey].fPIndex;
                                //            objMain.ws.send(JSON.stringify({ 'c': 'Attack', 'TargetOwner': customTagIndexKey, 'Target': fPIndex }));

                                //        }
                                //        objMain.selectObj.obj = null;
                                //        objMain.selectObj.type = '';
                                //        operatePanel.refresh();
                                //    }
                                //});
                            }; break;
                        case 'mile':
                        case 'business':
                        case 'volume':
                        case 'speed':
                            {
                                addItemToTaskOperatingPanle('寻宝', 'promoteBtn', function () {
                                    objMain.canSelect = false;
                                    if (objMain.carState["car"] == 'waitAtBaseStation' || objMain.carState["car"] == 'waitOnRoad') {
                                        objMain.ws.send(JSON.stringify({ 'c': 'Promote', 'pType': objMain.selectObj.type }));
                                        objMain.selectObj.obj = null;
                                        objMain.selectObj.type = '';
                                        operatePanel.refresh();
                                    }
                                });
                                lookUp();
                            }; break;
                        case 'ability':
                            {
                                addItemToTaskOperatingPanle('使用', 'useDiamondBtn', function () {
                                    if (objMain.carState["car"] == 'waitAtBaseStation') {
                                        if (objMain.PromoteDiamondCount[objMain.selectObj.obj.userData.index] > 0) {
                                            objMain.ws.send(JSON.stringify({ 'c': 'Ability', 'pType': objMain.selectObj.obj.userData.index }));
                                        }
                                    }
                                });
                                addItemToTaskOperatingPanle('出售', 'sellDiamondBtn', function () {
                                    if (objMain.carState["car"] == 'waitAtBaseStation') {
                                        objMain.ws.send(JSON.stringify({ 'c': 'SellDiamond', 'pType': objMain.selectObj.obj.userData.index }));
                                    }
                                });
                                addItemToTaskOperatingPanle('购买', 'buyDiamondBtn', function () {
                                    if (objMain.carState["car"] == 'waitAtBaseStation') {
                                        objMain.ws.send(JSON.stringify({ 'c': 'BuyDiamond', 'pType': objMain.selectObj.obj.userData.index }));
                                    }
                                });
                            }; break;
                        case 'setReturn':
                            {
                                if (objMain.driver.driverIndex > 0) { }
                                else {
                                    addItemToTaskOperatingPanle('招募', 'findDriver', function () {
                                        if (objMain.carState["car"] == 'waitAtBaseStation') {
                                            if (objMain.driver.driverIndex < 0) {
                                                driverSys.draw(function (driverIndex) {
                                                    // objMain.ws.send(sendStr);
                                                    alert(driverIndex);
                                                    objMain.ws.send(JSON.stringify({ 'c': 'DriverSelect', 'driverIndex': driverIndex }));
                                                });
                                            }
                                        }
                                    });
                                }
                                lookUp();
                                //if (objMain
                            }; break;
                    }
                }; break;
            case 'waitOnRoad':
                {
                    switch (objMain.Task.state) {
                        case 'collect':
                            {
                                addItemToTaskOperatingPanle('收集', 'collectBtn', function () {
                                    objMain.canSelect = false;
                                    if (objMain.carState["car"] == 'waitAtBaseStation' || objMain.carState["car"] == 'waitOnRoad') {
                                        var selectObj = objMain.selectObj.obj;
                                        // console.log('selectObj', selectObj.userData.collectPosition.Fp.FastenPositionID);
                                        objMain.ws.send(JSON.stringify({ 'c': 'Collect', 'cType': 'findWork', 'fastenpositionID': selectObj.userData.collectPosition.Fp.FastenPositionID, 'collectIndex': selectObj.userData.collectPosition.collectIndex }));
                                        objMain.selectObj.obj = null;
                                        objMain.selectObj.type = '';
                                        operatePanel.refresh();
                                    }
                                });
                                lookUp();
                            }; break;
                        case 'attack':
                            {
                                attackF();
                                magicF();
                                lookUp();
                            }; break;
                        case 'mile':
                        case 'business':
                        case 'volume':
                        case 'speed':
                            {
                                addItemToTaskOperatingPanle('寻宝', 'promoteBtn', function () {
                                    objMain.canSelect = false;
                                    if (objMain.carState["car"] == 'waitAtBaseStation' || objMain.carState["car"] == 'waitOnRoad') {
                                        objMain.ws.send(JSON.stringify({ 'c': 'Promote', 'pType': objMain.selectObj.type }));
                                        objMain.selectObj.obj = null;
                                        objMain.selectObj.type = '';
                                        operatePanel.refresh();
                                    }
                                });
                                lookUp();
                            }; break;
                        case 'setReturn':
                            {
                                if (objMain.driver.driverIndex > 0) {
                                    if (objMain.driver.race == 'devil') {
                                        magicF();
                                    }
                                }
                                lookUp();
                            }; break;
                    }
                    addItemToTaskOperatingPanle('回基地', 'goBackBtn', function () {
                        objMain.canSelect = false;
                        if (objMain.carState["car"] == 'waitOnRoad') {
                            var selectObj = objMain.selectObj.obj;
                            objMain.ws.send(JSON.stringify({ 'c': 'SetCarReturn' }));
                            objMain.selectObj.obj = null;
                            objMain.selectObj.type = '';
                            operatePanel.refresh();
                        }
                    });
                }; break;
        }
    }
};
var ModelOperateF =
{
    f: function (received_obj, config) {
        var manager = new THREE.LoadingManager();
        new THREE.MTLLoader(manager)
            .loadTextOnly(received_obj.Mtl, 'data:image/jpeg;base64,' + received_obj.Img, function (materials) {
                materials.preload();
                // materials.depthTest = false;
                new THREE.OBJLoader(manager)
                    .setMaterials(materials)
                    //.setPath('/Pic/')
                    .loadTextOnly(received_obj.Obj, function (object) {
                        //console.log('o', object);
                        //for (var iOfO = 0; iOfO < object.children.length; iOfO++) {
                        //    if (object.children[iOfO].isMesh) {
                        //        for (var mi = 0; mi < object.children[iOfO].material.length; mi++) {
                        //            object.children[iOfO].material[mi].transparent = true;
                        //            object.children[iOfO].material[mi].opacity = 1;
                        //            object.children[iOfO].material[mi].side = THREE.FrontSide;
                        //            object.children[iOfO].material[mi].color = new THREE.Color(0.45, 0.45, 0.45);
                        //        }
                        //    }
                        //}
                        //console.log('o', object);
                        object.scale.set(0.003, 0.003, 0.003);
                        // object.rotateX(-Math.PI / 2);
                        //  objMain.Model
                        // objMain.rmbModel[field] = object;
                        if (config.transparent != undefined) {
                            for (var iOfO = 0; iOfO < object.children.length; iOfO++) {
                                if (object.children[iOfO].isMesh) {
                                    if (object.children[iOfO].material.isMaterial) {
                                        object.children[iOfO].material.transparent = true;
                                        object.children[iOfO].material.opacity = config.transparent.opacity;
                                    }
                                    else
                                        for (var mi = 0; mi < object.children[iOfO].material.length; mi++) {
                                            object.children[iOfO].material[mi].transparent = true;
                                            object.children[iOfO].material[mi].opacity = config.transparent.opacity;
                                        }
                                }
                            }
                        }
                        if (config.color != undefined) {
                            for (var iOfO = 0; iOfO < object.children.length; iOfO++) {
                                if (object.children[iOfO].isMesh) {
                                    for (var mi = 0; mi < object.children[iOfO].material.length; mi++) {
                                        object.children[iOfO].material[mi].color = new THREE.Color(config.color.r, config.color.g, config.color.b);
                                    }
                                }
                            }
                        }
                        if (config.rotate != undefined) {
                            object.rotateX(config.rotate.x);
                            object.rotateX(config.rotate.y);
                            object.rotateX(config.rotate.z);
                        }
                        if (config.bind != undefined) {
                            config.bind(object);
                        }



                    }, function () { }, function () { });
            });
    }
};

var stateSet =
{
    speed:
    {
        add: function (carId) {
            for (var i = 0; i < 2; i++) {
                var meshCopy = objMain.ModelInput.speed.children[0].clone();
                meshCopy.name = 'fire' + i.toString() + '_' + carId;
                meshCopy.position.x = 77;//97.11
                meshCopy.position.z = 4 - i * 18;
                meshCopy.rotateY(Math.PI / 2 * 3);
                meshCopy.scale.z = 2;//3
                var car = objMain.carGroup.getObjectByName('car_' + carId);
                if (car)
                    car.add(meshCopy);
            }
        },
        Animate: function (carId) {
            //input Is group
            var car = objMain.carGroup.getObjectByName('car_' + carId);
            if (car)
                for (var i = 0; i < 2; i++) {
                    var name = 'fire' + i.toString() + '_' + carId;
                    var fire = car.getObjectByName(name);
                    if (fire) {
                        var percent = (Date.now() % 500) / 500;
                        fire.position.x = 77 + (97.11 - 77) * percent;
                        fire.scale.z = 2 + (3 - 2) * percent;//3
                    }
                }
        },
        clear: function (carId) {
            var car = objMain.carGroup.getObjectByName('car_' + carId);
            if (car)
                for (var i = 0; i < 2; i++) {
                    var name = 'fire' + i.toString() + '_' + carId;
                    var fire = car.getObjectByName(name);
                    if (fire) {
                        car.remove(fire);
                    }
                }
        },
    },
    attck:
    {
        add: function (carId) {
            {
                var meshCopy = objMain.ModelInput.attack.children[0].clone();
                meshCopy.name = 'fist_' + carId;
                meshCopy.position.x = -0;//97.11
                meshCopy.position.y = -30;
                meshCopy.position.z = 0;
                meshCopy.rotateX(Math.PI / 2);
                meshCopy.rotateY(Math.PI);
                meshCopy.scale.set(5, 5, 5);// = 2;//3
                var car = objMain.carGroup.getObjectByName('car_' + carId);
                if (car)
                    car.add(meshCopy);
            }
        },
        Animate: function (carId) {
            //input Is group
            var car = objMain.carGroup.getObjectByName('car_' + carId);
            if (car) {
                var name = 'fist_' + carId;
                var fist = car.getObjectByName(name);
                if (fist) {
                    var percent = (Date.now() % 500) / 500;
                    if (percent < 0.3) {
                        percent = percent / 0.3;
                    }
                    else {
                        percent = (1 - percent) / 0.7;
                    }
                    fist.position.x = -40 + (-35 - (-40)) * percent;
                }
            }
        },
        clear: function (carId) {
            var car = objMain.carGroup.getObjectByName('car_' + carId);
            if (car) {
                var name = 'fist_' + carId;
                var fist = car.getObjectByName(name);
                if (fist) {
                    car.remove(fist);
                }
            }
        },
    },
    defend:
    {
        add: function (carId) {
            {
                var flag = objMain.playerGroup.getChildByName('flag_' + carId);
                if (flag) {
                    var O3d = new THREE.Object3D();
                    O3d.name = 'defender_' + carId;
                    O3d.position.set(flag.position.x, flag.position.y, flag.position.z);

                    for (var i = 0; i < 3; i++) {
                        var meshCopy = objMain.ModelInput.shield.children[0].clone();
                        meshCopy.name = 'shield' + i.toString() + '_' + carId;
                        meshCopy.position.x = 0.8 * Math.cos(i * 2 * Math.PI / 3);//97.11
                        meshCopy.position.y = 0.5;
                        meshCopy.position.z = 0.8 * Math.sin(i * 2 * Math.PI / 3);
                        meshCopy.scale.set(0.003, 0.003, 0.003);
                        meshCopy.rotateX(Math.PI / 2);
                        meshCopy.rotateZ(-Math.PI / 2 + i * 2 * Math.PI / 3);
                        O3d.add(meshCopy);
                    }
                    objMain.shieldGroup.add(O3d);
                }
            }
        },
        Animate: function (carId) {
            //input Is group
            var O3dname = 'defender_' + carId;

            var O3d = objMain.shieldGroup.getObjectByName(O3dname);
            if (O3d) {
                O3d.rotation.y = (Math.sin((Date.now() % 2000) / 2000 * Math.PI * 2) + 1) * Math.PI;
            }
        },
        clear: function (carId) {
            var O3dname = 'defender_' + carId;
            var O3d = objMain.shieldGroup.getObjectByName(O3dname);
            if (O3d) {
                var length = O3d.children.length;
                for (var i = length - 1; i >= 0; i--) {
                    O3d.remove(O3d.children[i]);
                }
                objMain.shieldGroup.remove(O3d);
            }
            //var car = objMain.carGroup.getObjectByName('car_' + carId);
            //if (car) {
            //    var name = 'fist_' + carId;
            //    var fist = car.getObjectByName(name);
            //    if (fist) {
            //        car.remove(fist);
            //    }
            //}
        }
    },
    confusePrepare:
    {
        add: function (carId, animateData) {
            {
                //var flag = objMain.playerGroup.getChildByName('flag_' + carId);
                //if (flag)
                // var animateData = { startX: objMain.controls.target.x, startY: objMain.controls.target.y, startZ: objMain.controls.target.z, start: Date.now(), endX: objMain.controls.target.x + 5, endY: objMain.controls.target.y, endZ: objMain.controls.target.z }
                {
                    var O3d = new THREE.Object3D();
                    O3d.name = 'confusePrepare_' + carId;
                    O3d.position.set(animateData.startX, animateData.startY, animateData.startZ);

                    {
                        var meshCopy = objMain.ModelInput.confusePrepare.children[0].clone();
                        meshCopy.name = 'confusePrepareChild_' + carId;
                        meshCopy.position.x = 0;//97.11
                        meshCopy.position.y = 0;
                        meshCopy.position.z = 0;
                        meshCopy.scale.set(0.05, 0.05, 0.05);
                        meshCopy.rotateX(Math.PI / 2);
                        //meshCopy.rotateZ(-Math.PI / 2 + i * 2 * Math.PI / 3);
                        O3d.add(meshCopy);
                    }
                    O3d.userData.animateData = animateData;
                    objMain.confusePrepareGroup.add(O3d);
                }
            }
        },
        Animate: function (carId) {
            //input Is group
            var O3dname = 'confusePrepare_' + carId;

            var O3d = objMain.confusePrepareGroup.getObjectByName(O3dname);
            if (O3d) {
                var userData = O3d.userData.animateData;
                var percent = ((Date.now() - userData.start) % 3000) / 3000;
                var positionX = userData.startX + percent * (userData.endX - userData.startX);
                var positionY = userData.startY + percent * (userData.endY - userData.startY);
                var positionZ = userData.startZ + percent * (userData.endZ - userData.startZ);
                O3d.position.set(positionX, positionY, positionZ);

                O3d.children[0].position.set(0.1 * Math.sin(percent * 20), 0.06 * Math.sin(percent * 10), 0.08 * Math.sin(percent * 15));
                O3d.children[0].rotation.set((percent * 1.7 % 1) * Math.PI * 2, (percent * 1.1 % 1) * Math.PI * 2, (percent * 1.3 % 1) * Math.PI * 2)

            }
        },
        clear: function (carId) {
            var O3dname = 'confusePrepare_' + carId;
            var O3d = objMain.confusePrepareGroup.getObjectByName(O3dname);
            if (O3d) {
                var length = O3d.children.length;
                for (var i = length - 1; i >= 0; i--) {
                    O3d.remove(O3d.children[i]);
                }
                objMain.confusePrepareGroup.remove(O3d);
            }
            //var car = objMain.carGroup.getObjectByName('car_' + carId);
            //if (car) {
            //    var name = 'fist_' + carId;
            //    var fist = car.getObjectByName(name);
            //    if (fist) {
            //        car.remove(fist);
            //    }
            //}
        }
    },
    confuse:
    {
        add: function (carId) {
            var meshCopy = objMain.ModelInput.confusePrepare.children[0].clone();
            meshCopy.name = 'confuse' + '_' + carId;
            meshCopy.position.y = 18;
            meshCopy.scale.set(5, 5, 5);
            var car = objMain.carGroup.getObjectByName('car_' + carId);
            if (car)
                car.add(meshCopy);
        },
        Animate: function (carId) {
            //input Is group
            var car = objMain.carGroup.getObjectByName('car_' + carId);
            if (car) {
                var name = 'confuse' + '_' + carId;
                var confuseTag = car.getObjectByName(name);
                if (confuseTag) {
                    var t = (Date.now() % 1500 - 750) / 750;
                    var deltaT = (Date.now() % 10000) / 10000;
                    confuseTag.rotation.set(0, Math.abs(t * Math.PI) + deltaT * 2 * Math.PI, 0, 'XYZ')
                }
            }
        },
        clear: function (carId) {
            var car = objMain.carGroup.getObjectByName('car_' + carId);
            if (car) {
                var name = 'confuse' + '_' + carId;
                var confuseTag = car.getObjectByName(name);
                if (confuseTag) {
                    car.remove(confuseTag);
                }
            }
        }
    },
    lostPrepare:
    {
        add: function (carId, animateData) {
            {
                //var flag = objMain.playerGroup.getChildByName('flag_' + carId);
                //if (flag)
                // var animateData = { startX: objMain.controls.target.x, startY: objMain.controls.target.y, startZ: objMain.controls.target.z, start: Date.now(), endX: objMain.controls.target.x + 5, endY: objMain.controls.target.y, endZ: objMain.controls.target.z }
                {
                    var O3d = new THREE.Object3D();
                    O3d.name = 'lostPrepare_' + carId;
                    O3d.position.set(animateData.startX, animateData.startY, animateData.startZ);

                    {
                        var meshCopy = objMain.ModelInput.lostPrepare.children[0].clone();
                        meshCopy.name = 'lostPrepareChild_' + carId;
                        meshCopy.position.x = 0;//97.11
                        meshCopy.position.y = 0;
                        meshCopy.position.z = 0.0;
                        meshCopy.scale.set(0.015, 0.015, 0.015);
                        //meshCopy.rotateX(Math.PI / 2);
                        meshCopy.rotateZ(Math.PI / 2);
                        //meshCopy.rotateZ(-Math.PI / 2 + i * 2 * Math.PI / 3);
                        O3d.add(meshCopy);
                    }
                    O3d.userData.animateData = animateData;
                    objMain.lostPrepareGroup.add(O3d);
                }
            }
        },
        Animate: function (carId) {
            //input Is group
            var O3dname = 'lostPrepare_' + carId;

            var O3d = objMain.lostPrepareGroup.getObjectByName(O3dname);
            if (O3d) {
                var userData = O3d.userData.animateData;
                var percent = ((Date.now() - userData.start) % 3000) / 3000;
                var positionX = userData.startX + percent * (userData.endX - userData.startX);
                var positionY = userData.startY + percent * (userData.endY - userData.startY);
                var positionZ = userData.startZ + percent * (userData.endZ - userData.startZ);
                O3d.position.set(positionX, positionY, positionZ);

                //O3d.children[0].position.set(0.1 * Math.sin(percent * 20), 0.06 * Math.sin(percent * 10), 0.08 * Math.sin(percent * 15));
                O3d.children[0].rotation.set((percent * 2 % 1) * Math.PI * 2, 0, Math.PI / 2)

            }
        },
        clear: function (carId) {
            var O3dname = 'lostPrepare_' + carId;
            var O3d = objMain.lostPrepareGroup.getObjectByName(O3dname);
            if (O3d) {
                var length = O3d.children.length;
                for (var i = length - 1; i >= 0; i--) {
                    O3d.remove(O3d.children[i]);
                }
                objMain.lostPrepareGroup.remove(O3d);
            }
        }
    },
    lost:
    {
        add: function (carId) {
            var meshCopy = objMain.ModelInput.lostPrepare.children[0].clone();
            meshCopy.name = 'lost' + '_' + carId;
            meshCopy.position.y = 35;
            meshCopy.scale.set(1, 1, 1);
            var car = objMain.carGroup.getObjectByName('car_' + carId);
            if (car)
                car.add(meshCopy);
        },
        Animate: function (carId) {
            //input Is group
            var car = objMain.carGroup.getObjectByName('car_' + carId);
            if (car) {
                var name = 'lost' + '_' + carId;;
                var loseTag = car.getObjectByName(name);
                if (loseTag) {
                    var t = (Date.now() % 2500) / 2500;
                    loseTag.rotation.set(0, t * 2 * Math.PI, 0, 'XYZ');
                }
            }
        },
        clear: function (carId) {
            var car = objMain.carGroup.getObjectByName('car_' + carId);
            if (car) {
                var name = 'lost' + '_' + carId;;
                var loseTag = car.getObjectByName(name);
                if (loseTag) {
                    car.remove(loseTag);
                }
            }
        }
    },
    ambusePrepare:
    {
        add: function (carId, animateData) {
            {
                //var flag = objMain.playerGroup.getChildByName('flag_' + carId);
                //if (flag)
                // var animateData = { startX: objMain.controls.target.x, startY: objMain.controls.target.y, startZ: objMain.controls.target.z, start: Date.now(), endX: objMain.controls.target.x + 5, endY: objMain.controls.target.y, endZ: objMain.controls.target.z }
                {
                    var O3d = new THREE.Object3D();
                    O3d.name = 'ambusePrepare_' + carId;
                    O3d.position.set(animateData.startX, animateData.startY, animateData.startZ);

                    {
                        var meshCopy = objMain.ModelInput.ambushPrepare.children[0].clone();
                        meshCopy.name = 'ambusePrepareChild_' + carId;
                        meshCopy.position.x = 0;//97.11
                        meshCopy.position.y = 0;
                        meshCopy.position.z = 0;
                        meshCopy.scale.set(0.005, 0.005, 0.005);
                        //meshCopy.rotateX(Math.PI / 2);
                        //meshCopy.rotateZ(-Math.PI / 2 + i * 2 * Math.PI / 3);
                        O3d.add(meshCopy);
                    }
                    O3d.userData.animateData = animateData;
                    objMain.ambushPrepareGroup.add(O3d);
                }
            }
        },
        Animate: function (carId) {
            //input Is group
            var O3dname = 'ambusePrepare_' + carId;

            var O3d = objMain.ambushPrepareGroup.getObjectByName(O3dname);
            if (O3d) {
                var userData = O3d.userData.animateData;
                var percent = ((Date.now() - userData.start) % 3000) / 3000;
                var positionX = userData.startX + percent * (userData.endX - userData.startX);
                var positionY = userData.startY + percent * (userData.endY - userData.startY);
                var positionZ = userData.startZ + percent * (userData.endZ - userData.startZ);
                O3d.position.set(positionX, positionY, positionZ);

                // O3d.children[0].position.set(0.1 * Math.sin(percent * 20), 0.06 * Math.sin(percent * 10), 0.08 * Math.sin(percent * 15));
                //O3d.children[0].rotation.set((percent * 1.7 % 1) * Math.PI * 2, (percent * 1.1 % 1) * Math.PI * 2, (percent * 1.3 % 1) * Math.PI * 2)

            }
        },
        clear: function (carId) {
            var O3dname = 'ambusePrepare_' + carId;
            var O3d = objMain.ambushPrepareGroup.getObjectByName(O3dname);
            if (O3d) {
                var length = O3d.children.length;
                for (var i = length - 1; i >= 0; i--) {
                    O3d.remove(O3d.children[i]);
                }
                objMain.ambushPrepareGroup.remove(O3d);
            }
            //var car = objMain.carGroup.getObjectByName('car_' + carId);
            //if (car) {
            //    var name = 'fist_' + carId;
            //    var fist = car.getObjectByName(name);
            //    if (fist) {
            //        car.remove(fist);
            //    }
            //}
        }
    },
    control:
    {
        clear: function (carId) {
            stateSet.confusePrepare.clear(carId);
            stateSet.lostPrepare.clear(carId);
            stateSet.ambusePrepare.clear(carId);
        }
    },
    water:
    {
        add: function (targetRoleID, actionRole) {
            {
                this.clear(actionRole);
                var flag = objMain.playerGroup.getChildByName('flag_' + targetRoleID);
                if (flag) {
                    //var O3d = new THREE.Object3D();

                    // O3d.position.set(flag.position.x, flag.position.y, flag.position.z);
                    var waterCopy = objMain.ModelInput.water.clone();

                    for (var i = 0; i < waterCopy.children.length; i++) {
                        waterCopy.children[i].material.transparent = true;
                        waterCopy.children[i].material.opacity = 0.6;
                    }
                    waterCopy.name = 'water_' + actionRole;
                    waterCopy.position.set(flag.position.x, 0, flag.position.z);
                    waterCopy.scale.set(0.15, 0.5, 0.15);
                    waterCopy.userData = { startT: Date.now() };
                    objMain.waterGroup.add(waterCopy);
                }
            }
        },
        Animate: function (actionRole) {
            var name = 'water_' + actionRole;
            var water = objMain.waterGroup.getObjectByName(name);
            if (water) {
                var percent = (Date.now() - water.userData.startT) / 10000;
                if (percent < 1) {
                    //var secondPercent = 
                    var shuilang = water.getObjectByName('shuilang');
                    shuilang.scale.set(1, 1 - percent, 1);
                    var shuizhu = water.getObjectByName('shuizhu');
                    shuizhu.scale.set(1 + Math.pow(percent * 10, 0.66), 1, 1 + Math.pow(percent * 10, 0.66));
                }
                else {
                    objMain.waterGroup.remove(water);
                }
            }
        },
        clear: function (actionRole) {
            var name = 'water_' + actionRole;
            var water = objMain.waterGroup.getObjectByName(name);
            if (water) {
                objMain.waterGroup.remove(water);
            }
        }
    },
    fire:
    {
        particleCount: 400,
        add: function (targetRoleID, actionRole) {
            this.clear(actionRole);
            var particleFire = new fire();
            particleFire.install({ THREE: THREE });

            var geometry1 = new particleFire.Geometry(10, 35, this.particleCount);
            var material1 = new particleFire.Material({ color: 0x800080 });
            var height = window.innerHeight;
            material1.setPerspective(objMain.camera.fov, height);
            var particleFireMesh1 = new THREE.Points(geometry1, material1);
            var flag = objMain.playerGroup.getChildByName('flag_' + targetRoleID);
            if (flag) {
                //  mesh.position.set(flag.position.x, 0, flag.position.z);
                particleFireMesh1.position.set(flag.position.x, 0, flag.position.z);
                particleFireMesh1.name = 'fire_' + actionRole;
                particleFireMesh1.userData = { startT: Date.now() };
                objMain.fireGroup.add(particleFireMesh1);
            }
        },
        Animate: function (actionRole) {
            var name = 'fire_' + actionRole;
            var fire = objMain.fireGroup.getObjectByName(name);
            if (fire) {
                fire.material.update(Date.now() % 2000 / 2000 * 20);
                //particleFireMesh1.material.update(delta);
                //particleFireMesh2.material.update(delta);
                var percent = (Date.now() - fire.userData.startT) / 10000;
                if (percent < 1) {
                    var car = objMain.carGroup.getObjectByName('car_' + actionRole);
                    if (car) {
                        var deltaX = car.position.x - fire.position.x;
                        var deltaZ = car.position.z - fire.position.z;

                        for (let i = this.particleCount - this.particleCount / 4; i < this.particleCount; i++) {
                            var positionPercent = (i - (this.particleCount - this.particleCount / 4)) / (this.particleCount / 4);

                            fire.geometry.attributes.position.array[i * 3 + 0] = positionPercent * deltaX;
                            fire.geometry.attributes.position.array[i * 3 + 1] = (positionPercent * positionPercent - positionPercent) * -100;
                            fire.geometry.attributes.position.array[i * 3 + 2] = positionPercent * deltaZ;

                        }
                        fire.geometry.attributes.position.needsUpdate = true;
                    }
                }
                else {
                    objMain.fireGroup.remove(fire);
                }
            }
        },
        clear: function (actionRole) {
            var name = 'fire_' + actionRole;
            var fire = objMain.fireGroup.getObjectByName(name);
            if (fire) {
                objMain.fireGroup.remove(fire);
            }
        }
    },
    lightning:
    {
        add: function (targetRoleID, actionRole) {
            this.clear(actionRole);
            let rayParams = {

                sourceOffset: new THREE.Vector3(),
                destOffset: new THREE.Vector3(),
                radius0: 0.07,
                radius1: 0.08,
                minRadius: 0.05,
                maxIterations: 7,
                isEternal: true,

                timeScale: 0.7,

                propagationTimeFactor: 0.05,
                vanishingTimeFactor: 0.95,
                subrayPeriod: 3.5,
                subrayDutyCycle: 0.6,
                maxSubrayRecursion: 3,
                ramification: 7,
                recursionProbability: 0.6,

                roughness: 0.85,
                straightness: 0.8

            };
            let lightningStrike = new THREE.LightningStrike(rayParams);

            let lightningMaterial = new THREE.MeshBasicMaterial({ color: new THREE.Color(0xb0fffe) });
            let lightningStrikeMesh = new THREE.Mesh(lightningStrike, lightningMaterial);
            lightningStrikeMesh.name = 'lightning_' + actionRole;

            let flag = objMain.playerGroup.getObjectByName('flag_' + targetRoleID);
            if (flag) {
                lightningStrikeMesh.position.set(flag.position.x, 0, flag.position.z);
                lightningStrikeMesh.userData = { startT: Date.now() };
                objMain.lightningGroup.add(lightningStrikeMesh);
            }
        },
        Animate: function (actionRole) {
            let name = 'lightning_' + actionRole;
            var lightningStrikeMesh = objMain.lightningGroup.getObjectByName(name);
            if (lightningStrikeMesh) {
                var percent = (Date.now() - lightningStrikeMesh.userData.startT) / 10000;
                if (percent < 1) {
                    //objMain.carGroup.getObjectByName('car_'+objMain.indexKey).position
                    var car = objMain.carGroup.getObjectByName('car_' + actionRole);
                    var deltaX = car.position.x - lightningStrikeMesh.position.x;
                    var deltaZ = car.position.z - lightningStrikeMesh.position.z;

                    lightningStrikeMesh.geometry.rayParameters.sourceOffset.copy(new THREE.Vector3(deltaX, 10, deltaZ));
                    lightningStrikeMesh.geometry.rayParameters.sourceOffset.y += 1;
                    lightningStrikeMesh.geometry.rayParameters.destOffset.copy(new THREE.Vector3(Math.sin(percent * Math.PI * 5) * 3.3, 0, Math.cos(percent * Math.PI * 1.5)));
                    lightningStrikeMesh.geometry.rayParameters.destOffset.y -= 1;
                    lightningStrikeMesh.geometry.update(Date.now() / 2000);
                }
                else {
                    objMain.lightningGroup.remove(lightningStrikeMesh);
                }
            }
        },
        clear: function (actionRole) {
            let name = 'lightning_' + actionRole;
            var lightningStrikeMesh = objMain.lightningGroup.getObjectByName(name);
            if (lightningStrikeMesh) {
                objMain.lightningGroup.remove(lightningStrikeMesh);
            }
        }
    }
}
var DirectionOperator =
{
    data: null,
    show: function (received_obj) {
        DirectionOperator.data = received_obj;
        objMain.mainF.removeF.clearGroup(objMain.directionGroup);
        var newDirectionModle = objMain.ModelInput.direction.clone();
        newDirectionModle.rotateX(-Math.PI / 2);
        newDirectionModle.scale.set(0.4, 0.4, 0.4);//(Math.PI / 2);
        newDirectionModle.position.set(DirectionOperator.data.positionX, -0.1, -DirectionOperator.data.positionY);

        objMain.directionGroup.add(newDirectionModle);
        for (var i = 0; i < DirectionOperator.data.direction.length; i++) {
            var newArrow = objMain.ModelInput.directionArrow.clone();
            newArrow.scale.set(0.03, 0.03, 0.03);//(Math.PI / 2);
            newArrow.position.set(DirectionOperator.data.positionX, -0.1, -DirectionOperator.data.positionY);
            newArrow.rotation.y = DirectionOperator.data.direction[i];
            objMain.directionGroup.add(newArrow);

        }
    }
};

var BuildingModelObj =
{
    f: function (received_obj) {
        if (received_obj.existed) {
            var amodel = received_obj.amodel;
            BuildingModelObj.copy(amodel, received_obj);
        }
        else {
            if (objMain.buildingGroup.getObjectByName(received_obj.modelID) == undefined) {
                var manager = new THREE.LoadingManager();
                new THREE.MTLLoader(manager)
                    .loadTextOnly(received_obj.mtlText, 'data:image/jpeg;base64,' + received_obj.imageBase64, function (materials) {
                        materials.preload();
                        // materials.depthTest = false;
                        new THREE.OBJLoader(manager)
                            .setMaterials(materials)
                            //.setPath('/Pic/')
                            .loadTextOnly(received_obj.objText, function (object) {
                                var amodel = received_obj.amodel;
                                objMain.buildingModel[amodel] = object;
                                BuildingModelObj.copy(amodel, received_obj);
                            }, function () { }, function () { });
                    });
            }
        }
    },
    copy: function (amodel, received_obj) {
        if (objMain.buildingModel[amodel] == undefined) {

        }
        else {
            if (objMain.buildingGroup.getObjectByName(received_obj.modelID) == undefined) {
                var obj = objMain.buildingModel[amodel].clone();
                obj.name = received_obj.modelID;
                obj.position.set(received_obj.x, received_obj.y, received_obj.z);
                obj.rotation.set(0, received_obj.rotatey, 0, 'XYZ');
                objMain.buildingGroup.add(obj);
            }
        }
    }
};
//////////
/*
 * 手柄类，此游戏只支持单手柄操作。
 */

/////////////
/*
 * 复数类
 */
function Complex(R, I) {
    if (isNaN(R) || isNaN(I)) { throw new TypeError('Complex params require Number'); }
    this.r = R;
    this.i = I;
}
// 加法
Complex.prototype.add = function (that) {
    return new Complex(this.r + that.r, this.i + that.i);
};
// 负运算
Complex.prototype.neg = function () {
    return new Complex(-this.r, -this.i);
};
// 乘法
Complex.prototype.multiply = function (that) {
    if (this.r === that.r && this.i + that.i === 0) {
        return this.r * this.r + this.i * this.i
    }
    return new Complex(this.r * that.r - this.i * that.i, this.r * that.i + this.i * that.r);
};
// 除法
Complex.prototype.divide = function (that) {
    var a = this.r;
    var b = this.i;
    var c = that.r;
    var d = that.i;
    return new Complex((a * c + b * d) / (c * c + d * d), (b * c - a * d) / (c * c + d * d));
};
// 模长
Complex.prototype.mo = function () {
    return Math.sqrt(this.r * this.r + this.i * this.i);
};
//变为角度
Complex.prototype.toAngle = function () {

    if (this.r > 1e-6) {
        var angle = Math.atan(this.i / this.r);
        angle = (angle + Math.PI * 4) % (Math.PI * 2);
        return angle;
    }
    else if (this.r < -1e-6) {
        var angle = Math.atan(this.i / this.r);
        angle = (angle + Math.PI * 3) % (Math.PI * 2);
        return angle;
    }
    else if (this.i > 0) {
        return Math.PI / 2;
    }
    else if (this.i < 0) {
        return Math.PI * 3 / 2;
    }
    else {
        throw 'this Complex can not change to angle';
    }
    return Math.sqrt(this.r * this.r + this.i * this.i);
};
Complex.prototype.toOne = function () {
    var m = this.mo();
    this.r /= m, this.i /= m;
    //return Math.sqrt(this.r * this.r + this.i * this.i);
};
Complex.prototype.isZero = function () {
    return this.mo() < 1e-4;
}
Complex.prototype.toString = function () {
    return "{" + this.r + "," + this.i + "}";
};
// 判断两个复数相等
Complex.prototype.equal = function (that) {
    return that !== null && that.constructor === Complex && this.r === that.r && this.i === that.i;
};
Complex.ZERO = new Complex(0, 0);
Complex.ONE = new Complex(1, 0);
Complex.I = new Complex(0, 1);
// 从普通字符串解析为复数
Complex.parse = function (s) {
    try {
        var execres = Complex.parseRegExp.exec(s);
        return new Complex(parseFloat(execres[1]), parseFloat(execres[2]));
    } catch (e) {
        throw new TypeError("Can't parse '" + s + "'to a complex");
    }
};
Complex.parseRegExp = /^\{([\d\s]+[^,]*),([\d\s]+[^}]*)\}$/;
window.c = Complex;
// console.log(/^\{([\d\s]+[^,]*),([\d\s]+[^}]*)\}$/.exec('{2,3}'));
// 示例代码 

////////////

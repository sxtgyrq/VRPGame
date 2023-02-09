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
    debug: (function () {
        if (window.location.hostname == 'www.nyrq123.com') {
            return false;
        }
        else { return true; }
    })(),
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
    buildingGroup: null,
    buildingShowGroup: null,
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
                var start = new THREE.Vector3(MercatorGetXbyLongitude(fp.Longitude), MercatorGetZbyHeight(fp.Height), -MercatorGetYbyLatitude(fp.Latitde));
                var end = new THREE.Vector3(MercatorGetXbyLongitude(fp.positionLongitudeOnRoad), MercatorGetZbyHeight(fp.Height), -MercatorGetYbyLatitude(fp.positionLatitudeOnRoad));
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
            },
            clearNearObj: function (x, z, group) {
                var startIndex = group.children.length - 1;
                for (var i = startIndex; i >= 0; i--) {
                    if ((group.children[i].position.x - x) * (group.children[i].position.x - x) + (group.children[i].position.z - z) * (group.children[i].position.z - z) < 99 * 99)
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
        drawPanelOfDetail: function (modelDetail) {
            //  var modelDetail = { "c": "modelDetail", "x": 97873.8828125, "y": 0.0, "z": -35051.9609375, "locked": false, "dmState": 0, "bussinessAddress": "", "Content": "指引", "author": "1MhoP61wXyV5uCAZk36JFFQfV95mzfLFdw", "amState": 0, "modelName": "n4a4c065aa4794388a6b27f8a28c11b62", "createTime": "2022-03-04 10:34:13" }
            var lengthOfObjs = objMain.groupOfOperatePanle.children.length;
            for (var i = lengthOfObjs - 1; i >= 0; i--) {
                objMain.groupOfOperatePanle.remove(objMain.groupOfOperatePanle.children[i]);
            }

            var element = document.createElement('div');
            var table = document.createElement('table');
            table.border = "1";
            table.style.backgroundColor = "yellowgreen";
            {
                {
                    var tr = document.createElement('tr');
                    var th = document.createElement('th');
                    var td = document.createElement('td');
                    th.innerText = "锁定";
                    td.innerText = modelDetail['locked'] ? "是" : "否";
                    th.style.border = "3px solid red";
                    td.style.border = "3px solid red";
                    tr.appendChild(th);
                    tr.appendChild(td);
                    table.appendChild(tr);
                }
                {
                    var tr = document.createElement('tr');
                    var th = document.createElement('th');
                    var td = document.createElement('td');
                    th.innerText = "个体状态";
                    switch (modelDetail['dmState']) {
                        case 0:
                            {
                                td.innerText = "没有使用";
                            }; break;
                        case 1:
                            {
                                td.innerText = "正在使用";
                            }; break;
                    }
                    th.style.border = "3px solid red";
                    td.style.border = "3px solid red";
                    tr.appendChild(th);
                    tr.appendChild(td);
                    table.appendChild(tr);
                }
                {
                    var tr = document.createElement('tr');
                    var th = document.createElement('th');
                    var td = document.createElement('td');
                    th.innerText = "业务地址";
                    td.innerText = modelDetail['bussinessAddress'];
                    th.style.border = "3px solid red";
                    td.style.border = "3px solid red";
                    tr.appendChild(th);
                    tr.appendChild(td);
                    table.appendChild(tr);
                }
                {
                    var tr = document.createElement('tr');
                    var th = document.createElement('th');
                    var td = document.createElement('td');
                    th.innerText = "模型类型";
                    td.innerText = modelDetail['Content'];
                    th.style.border = "3px solid red";
                    td.style.border = "3px solid red";
                    tr.appendChild(th);
                    tr.appendChild(td);
                    table.appendChild(tr);
                }
                {
                    var tr = document.createElement('tr');
                    var th = document.createElement('th');
                    var td = document.createElement('td');
                    th.innerText = "建造者";
                    td.innerText = modelDetail['author'];
                    th.style.border = "3px solid red";
                    td.style.border = "3px solid red";
                    tr.appendChild(th);
                    tr.appendChild(td);
                    table.appendChild(tr);
                }
                {
                    var tr = document.createElement('tr');
                    var th = document.createElement('th');
                    var td = document.createElement('td');
                    th.innerText = "模型状态";
                    switch (modelDetail['amState']) {
                        case 0:
                            {
                                td.innerText = "未通过";
                            }; break;
                        case 1:
                            {
                                td.innerText = "审核OK";
                            }; break;
                    }
                    th.style.border = "3px solid red";
                    td.style.border = "3px solid red";
                    tr.appendChild(th);
                    tr.appendChild(td);
                    table.appendChild(tr);
                }
                {
                    var tr = document.createElement('tr');
                    var th = document.createElement('th');
                    var td = document.createElement('td');
                    th.innerText = "模型名称";
                    td.innerText = modelDetail['modelName'];
                    th.style.border = "3px solid red";
                    td.style.border = "3px solid red";
                    tr.appendChild(th);
                    tr.appendChild(td);
                    table.appendChild(tr);
                }
                {
                    var tr = document.createElement('tr');
                    var th = document.createElement('th');
                    var td = document.createElement('td');
                    th.innerText = "创建时间";
                    td.innerText = modelDetail['createTime'];
                    th.style.border = "3px solid red";
                    td.style.border = "3px solid red";
                    tr.appendChild(th);
                    tr.appendChild(td);
                    table.appendChild(tr);
                }
            }
            //Object.keys(modelDetail).forEach(function (key) {

            //    //console.log(key, modelDetail[key]);
            //    var tr = document.createElement('tr');
            //    var th = document.createElement('th');
            //    var td = document.createElement('td');
            //    th.innerText = key;
            //    td.innerText = modelDetail[key];
            //    tr.appendChild(th);
            //    tr.appendChild(td);
            //    table.appendChild(tr);
            //});  
            element.appendChild(table);

            var object = new THREE.CSS2DObject(element);
            object.position.set(modelDetail.x, 0, modelDetail.z);

            objMain.groupOfOperatePanle.add(object);



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
                        backgroundData['main'] = cubeTexture;
                        objMain.scene.background = backgroundData['main'];
                    }; break;
                default:
                    {
                        var cubeTextureLoader = new THREE.CubeTextureLoader();
                        cubeTextureLoader.setPath('Pic/' + background.path + '/');
                        var cubeTexture = cubeTextureLoader.load([
                            "px.jpg", "nx.jpg",
                            "py.jpg", "ny.jpg",
                            "pz.jpg", "nz.jpg"
                        ]);
                        backgroundData['main'] = cubeTexture;
                        objMain.scene.background = backgroundData['main'];
                    }; break;
            }
        },
        backgroundData: {}
    },
    rightAndDuty:
    {
        data: {},
        update: function () { }
    },
    dealWithReceivedObj: function (received_obj, evt, received_msg) {
        switch (received_obj.c) {
            case 'SetHash':
                {
                    //  { "c": "SetHash", "hash": "d75845a7b891986477998d904b6e5e0c" }
                    var ss = prompt('对信息进行签名', received_obj.hash);
                    objMain.ws.send(ss);
                    set3DHtml();
                    objMain.state = 'OnLine';
                }; break;
            case 'SingleRoadPathData':
                {
                    // MapData.meshPoints.push(received_obj.meshPoints);
                    var basePoint = received_obj.basePoint;
                    for (var i = 0; i < received_obj.meshPoints.length; i += 12) {
                        var itemData = [
                            (received_obj.meshPoints[i + 0] + basePoint[0]) / 1000000,
                            (received_obj.meshPoints[i + 1] + basePoint[1]) / 1000000,
                            (received_obj.meshPoints[i + 2] + basePoint[2]) / 1000000,
                            (received_obj.meshPoints[i + 3] + basePoint[0]) / 1000000,
                            (received_obj.meshPoints[i + 4] + basePoint[1]) / 1000000,
                            (received_obj.meshPoints[i + 5] + basePoint[2]) / 1000000,
                            (received_obj.meshPoints[i + 6] + basePoint[0]) / 1000000,
                            (received_obj.meshPoints[i + 7] + basePoint[1]) / 1000000,
                            (received_obj.meshPoints[i + 8] + basePoint[2]) / 1000000,
                            (received_obj.meshPoints[i + 9] + basePoint[0]) / 1000000,
                            (received_obj.meshPoints[i + 10] + basePoint[1]) / 1000000,
                            (received_obj.meshPoints[i + 11] + basePoint[2]) / 1000000
                        ];
                        //var itemData = [
                        //    (received_obj.meshPoints[i + 0] + basePoint[0]) / 1000000,
                        //    (received_obj.meshPoints[i + 1] + basePoint[1]) / 1000000,
                        //    (received_obj.meshPoints[i + 2] + basePoint[0]) / 1000000,
                        //    (received_obj.meshPoints[i + 3] + basePoint[1]) / 1000000,
                        //    (received_obj.meshPoints[i + 4] + basePoint[0]) / 1000000,
                        //    (received_obj.meshPoints[i + 5] + basePoint[1]) / 1000000,
                        //    (received_obj.meshPoints[i + 6] + basePoint[0]) / 1000000,
                        //    (received_obj.meshPoints[i + 7] + basePoint[1]) / 1000000,
                        //];
                        MapData.meshPoints.push(itemData);
                        // MapData.meshPoints.push(received_obj.meshPoints[i]);
                    }

                    var drawRoadInfomation = function () {

                        objMain.mainF.removeF.clearGroup(objMain.roadGroup);
                        //  objMain.F.clearGroup(
                        var obj = MapData.meshPoints;

                        var positions = [];
                        var colors = [];
                        for (var i = 0; i < obj.length; i++) {
                            positions.push(
                                MercatorGetXbyLongitude(obj[i][0]), MercatorGetXbyLongitude(obj[i][2]), -MercatorGetYbyLatitude(obj[i][1]),
                                MercatorGetXbyLongitude(obj[i][3]), MercatorGetXbyLongitude(obj[i][5]), -MercatorGetYbyLatitude(obj[i][4]),
                                MercatorGetXbyLongitude(obj[i][6]), MercatorGetXbyLongitude(obj[i][8]), -MercatorGetYbyLatitude(obj[i][7]),
                                MercatorGetXbyLongitude(obj[i][6]), MercatorGetXbyLongitude(obj[i][8]), -MercatorGetYbyLatitude(obj[i][7]),
                                MercatorGetXbyLongitude(obj[i][9]), MercatorGetXbyLongitude(obj[i][11]), -MercatorGetYbyLatitude(obj[i][10]),
                                MercatorGetXbyLongitude(obj[i][0]), MercatorGetXbyLongitude(obj[i][2]), -MercatorGetYbyLatitude(obj[i][1]),

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
                            x: received_obj.mctX / 256,
                            y: objMain.controls.target.y,
                            z: 0 - received_obj.mctY / 256,
                            t: Date.now() + 3000,

                        }
                    };
                    objMain.camaraAnimateData = animationData;
                    objMain.scene.getObjectByName('axesHelper').position.set(received_obj.mctX / 256, 0, 0 - received_obj.mctY / 256);
                }; break;
            case 'AddModel':
                {
                    if (objMain.buildingGroup.children.length > 0) {
                        objMain.mainF.removeF.clearGroup(objMain.buildingGroup);
                    }
                    ModelOperateF.f(received_obj);

                }; break;
            case 'EditModel':
                {
                    if (objMain.buildingGroup.children.length > 0) {
                        objMain.mainF.removeF.clearGroup(objMain.buildingGroup);
                    }
                    var obj = objMain.buildingShowGroup.getObjectByName(received_obj.id);
                    objMain.buildingShowGroup.remove(obj);
                    objMain.buildingGroup.add(obj);
                }; break;
            case 'SetState':
                {
                    if (received_obj.state == 'roadCross') {
                        objMain.gamePadState = 'road';
                        $.notify('道路选择模式！');
                    }
                    else if (received_obj.state == 'modelEdit') {
                        objMain.gamePadState = 'shop';
                        $.notify('商店选择模式！');
                    }
                    // else if (received_obj.state == '') { }
                }; break;
            case 'DrawModel':
                {
                    if (objMain.buildingGroup.getObjectByName(received_obj.id) != undefined) { }
                    else if (objMain.buildingShowGroup.getObjectByName(received_obj.id) != undefined) {
                        ModelOperateF.update(received_obj);
                    }
                    else {
                        ModelOperateF.f2(received_obj);
                    }
                }; break;
            case 'ShowModelTypes':
                {
                    objMain.modelTypes = received_obj.modelTypes;
                    var c = document.getElementById('modelTypes');
                    c.innerHTML = '';
                    for (var i = 0; i < objMain.modelTypes.length; i += 2) {
                        var op = document.createElement('option');
                        op.value = objMain.modelTypes[i];
                        op.innerText = objMain.modelTypes[i + 1];
                        c.appendChild(op);
                    }
                }; break;
            case 'InputAddress':
                {
                    var ss = prompt('InputAddress', '');
                    document.getElementById('BTCAddress').innerText = ss;
                    objMain.ws.send(ss);
                }; break;
            case 'modelDetail':
                {
                    console.log('modelDetail', received_msg);
                    objMain.mainF.drawPanelOfDetail(received_obj);
                }; break;
            case 'DownloadModel':
                {
                    var url = "";
                    if (objMain.debug)
                        url = 'http://127.0.0.1:21001/file?amid=' + received_obj.amID;
                    else
                        url = 'https://www.nyrq123.com/websocket' + window.location.pathname.split('/')[1] + 'editor/file?amid=' + received_obj.amID;
                    window.open(url, '_blank');
                }; break;
            case 'ShowMsg':
                {
                    $.notify(received_obj.Msg);
                }; break;
            case 'SetBackgroundScene':
                {
                    setScenseFromData(received_obj.r, received_obj.name);
                }; break;
            case 'GetHeightAtPositionResult':
                {
                    if (objMain.buildingGroup.children.length > 0) {
                        objMain.buildingGroup.children[0].position.y = MercatorGetZbyHeight(received_obj.height);
                    }
                }; break;
            case 'chargingItem':
                {
                    if (received_obj.r.chargingType == 'alipay') {
                        received_obj.r.chargingType = '支付宝';
                    }
                    else if (received_obj.r.chargingType == 'wechat') {
                        received_obj.r.chargingType = '微信打赏';
                    }
                    var tt = document.getElementById('chargingTable');
                    if (tt != null) {
                        tt.children[0].children[received_obj.row + 1].children[0].innerText = received_obj.r.chargingDatetime;
                        tt.children[0].children[received_obj.row + 1].children[1].innerText = received_obj.r.chargingword;
                        tt.children[0].children[received_obj.row + 1].children[2].innerText = received_obj.r.bindWordAddr;
                        tt.children[0].children[received_obj.row + 1].children[3].innerText = received_obj.r.chargingType;
                        tt.children[0].children[received_obj.row + 1].children[4].innerText = received_obj.r.chargingMoney;
                        //var itemHtml = `<tr>
                        //    <th>${received_obj.r.chargingDatetime}</th>
                        //    <th>${received_obj.r.chargingword}</th>
                        //    <th>${received_obj.r.bindWordAddr}</th>
                        //    <th>${received_obj.r.chargingType}</th>
                        //    <th>${received_obj.r.chargingMoney}</th>
                        //</tr>`;
                    }
                }; break;
            case 'LookForTaskCopyResult':
                {
                    document.getElementById('taskAddrForSearch').value = received_obj.addr;
                    document.getElementById('jsonResultForSearchShow').value = received_obj.json;
                }; break;
        }
    },

    camaraAnimateData: null,
    axesHelper: null,
    gamePadState: 'road',
    gamePadKeyState: {},
    editingState: 'add',
    closestObjName: '',
    useAddNew: false,
    modelTypes: [],
    defaultCube: null,
    pageIndex: 0
};
var startA = function () {
    var connected = false;
    var wsConnect = '';
    if (objMain.debug)
        wsConnect = 'ws://127.0.0.1:21001/editor';
    else
        wsConnect = 'wss://www.nyrq123.com/websocket' + window.location.pathname.split('/')[1] + 'editor/editor';
    var ws = new WebSocket(wsConnect);
    ws.onopen = function () {
        {
            //var session = '';
            //if (sessionStorage['session'] == undefined) {

            //}
            //else {
            //    session = sessionStorage['session'];
            //}
            //ws.send(JSON.stringify({ c: 'CheckSession', session: session }));
            objMain.ws.send('');
        }
        //   alert("数据发送中...");
    };
    ws.onmessage = function (evt) {
        var received_msg = evt.data;
        console.log(evt.data);
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
            for (var i = 0; i < objMain.buildingGroup.children.length; i++) {
                /*
                 * 初始化汽车的大小
                 */
                //var scale = Math.sin(((Date.now() % 2000) / 2000) * Math.PI * 2) * 0.1 + 1;
                //if (objMain.buildingGroup.children[i].isGroup) {
                //    objMain.buildingGroup.children[i].scale.set(scale, scale, scale);
                //} 
            }
            objMain.closestObjName = '';
            if (objMain.buildingGroup.children.length == 0) {
                var minLength = 100000000;
                // objMain.closestObj = null;
                for (var i = 0; i < objMain.buildingShowGroup.children.length; i++) {
                    var p1 = objMain.controls.target;
                    var p2 = objMain.buildingShowGroup.children[i].position;
                    var length = Math.sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.z - p2.z) * (p1.z - p2.z));
                    if (length < minLength) {
                        objMain.closestObjName = objMain.buildingShowGroup.children[i].name;
                        minLength = length;
                    }
                    /*
                     * 初始化汽车的大小
                     */
                    //var scale = Math.sin(((Date.now() % 2000) / 2000) * Math.PI * 2) * 0.1 + 1;
                    //if (objMain.buildingGroup.children[i].isGroup) {
                    //    objMain.buildingGroup.children[i].scale.set(scale, scale, scale);
                    //} 
                }
            }
            if (objMain.closestObjName != '') {
                var scale = Math.sin(((Date.now() % 2000) / 2000) * Math.PI * 2) * 0.05 + 1.05;
                objMain.buildingShowGroup.getObjectByName(objMain.closestObjName).scale.set(scale, scale, scale);
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
                        }
                    }
                }
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

    objMain.defaultCube = cubeTexture;
    objMain.scene.background = objMain.defaultCube;

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

    objMain.buildingShowGroup = new THREE.Group();
    objMain.scene.add(objMain.buildingShowGroup);

    objMain.axesHelper = new THREE.AxesHelper(20);
    objMain.axesHelper.name = 'axesHelper';
    objMain.scene.add(objMain.axesHelper);
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
        objMain.controls.maxPolarAngle = Math.PI - Math.PI / 36;
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
    f: function (received_obj) {
        var manager = new THREE.LoadingManager();
        new THREE.MTLLoader(manager)
            .loadTextOnly(received_obj.aModel.mtlText, 'data:image/jpeg;base64,' + received_obj.aModel.imageBase64, function (materials) {
                materials.preload();
                // materials.depthTest = false;
                new THREE.OBJLoader(manager)
                    .setMaterials(materials)
                    //.setPath('/Pic/')
                    .loadTextOnly(received_obj.aModel.objText, function (object) {
                        object.position.set(objMain.controls.target.x, objMain.controls.target.y, objMain.controls.target.z);
                        object.name = received_obj.id;
                        objMain.buildingGroup.add(object);
                    }, function () { }, function () { });
            });
    },
    update: function (received_obj) {
        var object = objMain.buildingShowGroup.getObjectByName(received_obj.id);
        object.position.set(received_obj.x, received_obj.y, received_obj.z);
        object.rotation.set(0, received_obj.r, 0, 'XYZ');
    },
    f2: function (received_obj) {
        var manager = new THREE.LoadingManager();
        new THREE.MTLLoader(manager)
            .loadTextOnly(received_obj.aModel.mtlText, 'data:image/jpeg;base64,' + received_obj.aModel.imageBase64, function (materials) {
                materials.preload();
                // materials.depthTest = false;
                new THREE.OBJLoader(manager)
                    .setMaterials(materials)
                    //.setPath('/Pic/')
                    .loadTextOnly(received_obj.aModel.objText, function (object) {
                        object.position.set(received_obj.x, received_obj.y, received_obj.z);
                        object.rotation.set(0, received_obj.r, 0, 'XYZ');
                        object.name = received_obj.id;
                        objMain.buildingShowGroup.add(object);
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

var drawObjInFrontPage = function () {
    if (objMain.useAddNew) {
        if (objMain.buildingGroup.children.length > 0) {
            objMain.mainF.removeF.clearGroup(objMain.buildingGroup);
        }
        var model =
        {
            mtlText: uploadObj.mtl,
            imageBase64: uploadObj.imgBase64,
            objText: uploadObj.obj
        };
        var received_obj =
        {
            aModel: model
        };
        ModelOperateF.f(received_obj);
    }
}
var sendNewModelToServer = function () { };
var uploadObj =
{
    obj: '',
    mtl: '',
    imgBase64: '',
    modelTypes: '',
    modelName: '',
    setValue: function (objInput, mtlInput, base64Input, modelTypes, modelName) {
        uploadObj.obj = objInput;
        uploadObj.mtl = mtlInput;
        uploadObj.imgBase64 = base64Input;
        uploadObj.modelTypes = modelTypes;
        if (modelName != null && modelName != '' && modelName != undefined) {
            uploadObj.modelName = modelName;
        }
    },
    objNew: function () {
        return [uploadObj.obj, uploadObj.mtl, uploadObj.imgBase64, uploadObj.modelTypes, uploadObj.modelName];
    }
    //uploadObj
};

var setScense = function () {
    if (window.localStorage['crossName'] != undefined) {
        var cubeTextureLoader = new THREE.CubeTextureLoader();
        //https://www.nyrq123.com/websockettaiyuaneditor/upload
        cubeTextureLoader.setPath('https://www.nyrq123.com/websockettaiyuaneditor/img/' + window.localStorage['crossName'] + '/');
        var suffix = '?rm=' + Date.now();
        var cubeTexture = cubeTextureLoader.load([
            'px.jpg' + suffix, 'nx.jpg' + suffix,
            'py.jpg' + suffix, 'ny.jpg' + suffix,
            'pz.jpg' + suffix, 'nz.jpg' + suffix
        ]);
        objMain.scene.background = cubeTexture;
    }
}
var setScenseFromData = function (r, name) {
    //if (window.localStorage.px != undefined)
    {
        if (r.hasValue) {
            var cubeTextureLoader = new THREE.CubeTextureLoader();
            cubeTextureLoader.setPath('');
            //var cubeTexture = cubeTextureLoader.load([
            //    "xi_r.jpg", "dong_r.jpg",
            //    "ding_r.jpg", "di_r.jpg",
            //    "nan_r.jpg", "bei_r.jpg"
            //]);
            var cubeTexture = cubeTextureLoader.load([
                r.px, r.nx,
                r.py, r.ny,
                r.pz, r.nz
            ]);
            objMain.scene.background = cubeTexture;

            if (r.crossState == 0) {
                $.notify('路口没有被使用');
            }
            else {
                $.notify('路口使用中');
            }
        }
        else {
            objMain.scene.background = objMain.defaultCube;
        }
        window.localStorage['crossName'] = name;
    }
}
var uploadBackground = function () {
    // var image
    if (window.localStorage.px != undefined) {
        var backgroundData =
        {
            'c': 'SetBG',
        };
        var json = JSON.stringify(backgroundData);
        objMain.ws.send(json);
    }
    //var json = JSON.stringify({ c: 'ViewAngle', x1: objMain.camera.position.x, y1: -objMain.camera.position.z, x2: objMain.controls.target.x, y2: -objMain.controls.target.z });
    //objMain.ws.send(json);
}
var showBackground = function () {
    if (window.localStorage.px != undefined) {
        var backgroundData =
        {
            'c': 'showBackground',
        };
        var json = JSON.stringify(backgroundData);
        objMain.ws.send(json);
    }
}
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

var Decrypt_90021457 = function (k, array) {
    //array = [];
    const p = 90006799;
    const a = 0;
    const b = 7;
    const q = 90021457;
    const x = 29106958;
    const y = 23443935;
    const pHalf = 45003399;

    function ex_gcd_0(a, b) {
        //var ret, tmp;
        if (b == 0) {
            var x = 1;
            var y = 0;
            return [a, x, y];
        }
        else {
            // ret = ex_gcd_0(b, a % b, out x, out y);
            var ret, tmp;
            var calR = ex_gcd_0(b, a % b);
            ret = calR[0];
            var x = calR[1];
            var y = calR[2];
            tmp = x;
            x = y;
            y = tmp - parseInt(a / b) * y;
            return [ret, x, y];
        }
    }
    function ex_gcd_1(a, b) {
        a = a % b;
        if (a < 0) {
            a += b;
        }
        var x, y;
        var r = ex_gcd_0(a, b);
        x = r[1];
        // y = r[2];
        if (x > 0) {
            return x;
        }
        else {
            return b + x;
        }
    }
    function getDoubleP(bigIntegers) {
        var x = bigIntegers[0];
        var y = bigIntegers[1];
        {
            //   var s = ((3 * x * x + Secp256k1.a) * (Inverse.ex_gcd((2 * y) % Secp256k1.p, Secp256k1.p))) % Secp256k1.p;
            var s = (((3 * ((x * x) % p) + a) % p) * (ex_gcd_1((2 * y) % p, p))) % p;
            var Xr = (s * s - 2 * x) % p;
            while (Xr < 0) {
                Xr += p;
            }
            //Xr = Xr % p;
            var Yr = (s * (x - Xr) - y) % p;
            while (Yr < 0) {
                Yr += p;
            }
            return [Xr, Yr, false];
        }
    }

    function pointPlus(point_P, point_Q) {
        //返回结果为[x,y,false/true]
        if (point_P[0] == point_Q[0]) {
            if (point_P[1] == point_Q[1]) {
                return getDoubleP(point_P);
            }
            else {
                return [null, null, true];
            }
        }
        else {
            //var isZero = false;
            var s = ((point_P[1] - point_Q[1]) % p) * ((ex_gcd_1((point_P[0] - point_Q[0] + p) % p, p)) % p);
            s = s % p;
            while (s < 0) {
                s += p;
            }
            var Xr = ((s * s) % p - (point_P[0] + point_Q[0]) % p) % p;
            while (Xr < 0) {
                Xr += p;
            }
            var Yr = ((s * (point_P[0] - Xr)) % p - point_P[1]) % p;

            while (Yr < 0) {
                Yr += p;
            }
            return [Xr, Yr, false];
        }
    }

    function get(Integers64) {
        var result = [];
        while (Integers64 != 0) {
            if (Integers64 % 2 == 0) {
                result.push(true);
            }
            else {
                result.push(false);
            }
            Integers64 = parseInt(Integers64 / 2);
        }
        return result;
    }

    function getMulValue(rValue, baseP) {
        if (rValue > 0) {
            var result = null;
            var r = get(rValue);
            var isZero = false;
            for (var i = 0; i < r.length; i++) {
                //if (i == 6) {
                //    var x = 0;
                //    x++;
                //}
                if (!r[i]) {
                    if (baseP == null) {
                    }
                    else {
                        if (result == null) {
                            result = baseP;
                        }
                        else {
                            var plusResult = pointPlus(result, baseP);
                            result = [plusResult[0], plusResult[1]];
                            isZero = plusResult[2];

                            //console.log(i + '_' + 'result', result);
                        }
                    }
                }
                var doubleResult = getDoubleP(baseP);
                baseP = [doubleResult[0], doubleResult[1]];
                // console.log(i + '_' + 'result', baseP);
            }
            return result;
        }
        else if (rValue == 0) {
            return [null, null, true];
        }
        else {
            throw ("privateKey的值不能为0和负数");
        }
    }
    //array.length
    var returnResult = [];
    for (var i = 0; i < array.length; i += 5) {
        var rQ = [array[i], array[i + 1]];
        var MplusrQ = [array[i + 2], array[i + 3]];
        var delta = array[i + 4];
        var krp = getMulValue(q - k, rQ);
        var result = pointPlus(MplusrQ, krp);
        returnResult.push((result[0] + delta) % p - pHalf)
    }
    return returnResult;
};
var DecryptAnimationData = function () {

};


var testDecrypt_90021457 = function () {
    var a = [75466798, 81406970, 21619754, 29165962, 76169975, 84142573, 50967975, 63015491, 77365864, 32932916, 47307348, 9651629, 78914493, 78866865, 81087706, 25367942, 63165591, 64169215, 31396010, 89119520, 4782279, 26364734, 12827316, 70599708, 73495823, 49000881, 53608810, 55608229, 68500461, 26294481, 658128, 72564170, 39016320, 21739804, 72185398, 29718781, 65713360, 42322059, 25568395, 29275174, 54676908, 11819431, 14827494, 25167159, 59084040, 13266611, 17451125, 17228646, 28705285, 31518149, 47541565, 8908869, 60485590, 36925560, 40662912, 55635922, 74094528, 79504856, 45770840, 67018897, 5283665, 47981645, 17201376, 53267806, 27812340, 72001512, 35895865, 60152992, 80308793, 41989419, 27566156, 23356260, 53832179, 5999552, 40625404, 38635588, 40734375, 24045215, 79228516, 51841636, 38019306, 12811372, 71807766, 73310784, 54160133, 26090117, 67856328, 4714332, 68939384, 37859329, 2447097, 25704349, 32597850, 35286153, 70281648, 65357715, 11764504, 4760044, 63603826, 1415689, 82051465, 78797940, 53434301, 77835940, 52691023, 83787596, 77804395, 17726814, 24803158, 58973814, 88079038, 2805306, 75701, 3014744, 37061813, 15407248, 56973939, 41718059, 43422100, 73061922, 46808378, 26930071, 83884697, 66144530, 5036281, 13177407, 32037307, 8103280, 52059927, 4054535, 79867907, 27848200, 45095063, 55193341, 84457648, 87117008, 21217563, 81691640, 53565932, 63218793, 1494915, 15391095, 47518576, 65794240, 8344377, 67412346, 74072144, 65702913, 48697779, 14354403, 2711207, 41691133, 77551547, 75872589, 68943862, 10244919, 41765821, 69731933, 59429570, 38684002, 57222601, 24039665, 49826167, 82571793, 39198172, 67258829, 7897845, 42021461, 77337642, 70699286, 35246441, 86190669, 44961877, 63699619, 59795338, 85991943, 16517213, 26197314, 37199520, 45083805, 22788260, 71684560, 66451590, 32787723, 59098629, 4665246, 11993949, 88597189, 23823083, 2997028];

    var b = [70083688, 53966462, 45003278, 45003247, 45003573, 45003586, 45003634, 45003668, 45003592, 45003649, 45003681, 45003595, 45003651, 45003684, 45003558, 45003592, 45003623, 45003658, 45003645, 45003718];

    var k = 85036360;

    Decrypt_90021457(k, a);

    //var p = 90006799;
    //var s = 83810483;
    //var point_P = [78102886, 15773670];
    //var Xr = 56552480;
    //var Yr = ((s * (point_P[0] - Xr)) % p - point_P[1]) % p;
}

var outPutPath = function (aIndex) {
    var outPut = '';
    var c = objMain.carsAnimateData['car_' + objMain.indexKey]
    // var aIndex = 1;
    for (var i = 0; i < c.current.animateData[aIndex].initialData.animateData.length; i++) {
        outPut += c.current.animateData[aIndex].initialData.animateData[i].x0;
        outPut += ',';
        outPut += c.current.animateData[aIndex].initialData.animateData[i].y0;
        outPut += '      ';
        outPut += c.current.animateData[aIndex].initialData.animateData[i].x1; outPut += ',';
        outPut += c.current.animateData[aIndex].initialData.animateData[i].y1; outPut += '    '
    }
    console.log('outPut', outPut);
    return outPut;
}

var calHash = function (a) {
    const q = 90021457;
    var primeNumbers = [5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97];
    var primeCount = primeNumbers.length;

    var pIndex = a % primeCount;

    var a0 = 1;
    var aArray = [];
    var pArray = [];
    for (var i = 0; i < 50; i++) {
        aArray.push(a0);
        a0 = (a0 * a) % q;
        var pv = primeNumbers[(pIndex + i) % primeCount];
        pArray.push(pv);
    }
    var sum = 0;
    for (var i = 0; i < 50; i++) {
        sum += (pArray[i] * aArray[i]);
        sum = sum % q;
    }
    return sum % q;
}

//var outPut = function (a) {
//    //var a = 30739615;
//    for (var i = 0; i < 100; i++) {
//        r = testHash(a);
//        console.log(a, r);
//        a = r;
//    }
//}
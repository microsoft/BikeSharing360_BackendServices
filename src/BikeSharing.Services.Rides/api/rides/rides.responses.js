'use strict';


const rideTypeFromModel = function (rm) {
    return rm.eventId ? "Event" : "Custom";
}

const rideModelToRideOutput = function(rm) {
    const data =  {
        id: rm.id,
        rideType: rideTypeFromModel(rm),
        duration: rm.duration,
        bikeId: rm.bikeId,
        start: rm.start,
        stop: rm.stop,
        from: rm.fromStation.name,
        fromStation: rm.fromStation
    }


    if (rm.toStation) {
        data.to = rm.toStation.name;
        data.toStation = rm.toStation;
    }

    if (rm.eventId) {
        data.eventId = rm.eventId;
        data.eventType = rm.eventType;
        data.name = rm.eventName;
    }

    if (rm.geoDistance) {
        data.distance = rm.geoDistance;
    }


    return data;
}

const rideModelToRideWithBikeOutput = function (rm) {
    var data = rideModelToRideOutput(rm);
    data.bike = rm.bike;
    data.userId = rm.userId;
    return data;
}


module.exports = {
    rideModelToRideOutput,
    rideModelToRideWithBikeOutput
};
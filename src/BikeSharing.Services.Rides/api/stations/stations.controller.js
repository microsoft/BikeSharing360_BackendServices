'use strict';

const Station = require(process.cwd() + '/models/station')();
const Bike = require(process.cwd() + '/models/bike')();
const Ride = require(process.cwd() + '/models/ride')();
const sequelize = require(process.cwd() + '/db/db.sequelize.js').sequelize();
const StationResponses = require('./stations.responses');

let checkout = function (req, res, next) {

    //from url
    let stationId = req.params.id;

    //payload
    let endStationId = req.params.endStationId;
    let userId = req.params.userId;

    let eventId = (req.params.event) ? req.params.event.id : null;
    let eventName = (req.params.event) ? req.params.event.name : null;
    let eventType = (req.params.event) ? req.params.event.type : null;
    
    let pr = Bike.findOne({
        where: { stationId: stationId }
    })

    pr.then(bike => {
        if (!bike) {
            res.send(404, null);
            next();
            return;
        }

        bike.updateAttributes({
            stationId: null
        })
            .then(function (updatedBike) {
                Ride.create({
                    start: new Date(),
                    bikeId: updatedBike.id,
                    userId: userId,
                    eventType: eventType,
                    eventId: eventId,
                    eventName: eventName,
                    startStationId: stationId,
                    endStationId: endStationId
                })
                    .then(function (newRide) {
                        res.send(200, newRide.id);
                        next();
                    })
                    .catch(function (createRideError) {
                        res.send(createRideError);
                        next();
                    });
            })
            .catch(function (updateBikeError) {
                res.send(e);
                next();
            });

    });
    pr.catch(e => {
        res.send(e);
        next();
    });
};

let info = function (req, res, next) {
    let id = req.params.id;

    let pr = Station.findById(id, {
        include: [{
            model: Bike,
            as: 'bikes'
        }]
    });
    pr.then(result => {
        res.send(StationResponses.stationModelToStationOutput(result));
        next();
    });
    pr.catch(e => {
        res.send(e);
        next();
    });
}

let byTenant = function (req, res, next) {
    let tenantId = req.params.id;
    let from = parseInt(req.query.from || 0);
    let size = parseInt(req.query.size || 20);

    Station.findAndCountAll({
        offset: from * size,
        limit: size,
        distinct: true,
        include: [{
            model: Bike,
            as: 'bikes'
        }]
    }).then(function (result) {
        var stations = result.rows.map(function (station) {
            return StationResponses.stationModelToStationOutput(station);
        });

        res.setHeader('total', result.count);
        res.send(stations);
        next();
    }).catch(e => {
        res.send(e);
        next();
    });
}

let nearestLocation = function (req, res, next) {
    let latitude = req.query.latitude || 0;
    let longitude = req.query.longitude || 0;
    let count = req.query.count || 10;

    sequelize.query(`exec [dbo].[pStationsNear] @latitude = ${latitude}, @longitude = ${longitude}, @size = ${count}`, { type: sequelize.QueryTypes.SELECT })
        .then(results => {
            res.send(results);
            next();
        })
        .catch(e => {
            res.send(e);
            next();
        });
}

module.exports = {
    checkout,
    info,
    nearestLocation,
    byTenant
};


'use strict';

const Ride = require(process.cwd() + '/models/ride')();
const Bike = require(process.cwd() + '/models/bike')();
const Station = require(process.cwd() + '/models/station')();
const RideResponses = require('./rides.responses');
var Sequelize = require('sequelize');

let getAll = function (req, res, next) {
    let userid = req.params.userId;
    let from = parseInt(req.query.from || 0);
    let size = parseInt(req.query.size || 20);

    return Ride.findAndCountAll({
        order: [['start', 'DESC']],
        distinct: true,
        include: [{ model: Station, as: 'fromStation' },
                  { model: Station, as: 'toStation' }],
        offset: from * size,
        limit: size
    })
        .then(r => {
            res.setHeader('total', r.count);
            res.send(r.rows.map(RideResponses.rideModelToRideOutput));
            next();
        })
        .catch(e => {
            res.send(e);
            next();
        });
};

let routesByUser = function (req, res, next) {
    let userid = req.params.userId;
    let from = parseInt(req.query.from || 0);
    let size = parseInt(req.query.size || 20);
    
    return Ride.findAndCountAll({
        order: [['start', 'DESC']],
        include: [{ model: Station, as: 'fromStation' },
            { model: Station, as: 'toStation' }],
        offset: from * size,
        limit: size,
        distinct: true,
        where: {
            userId: userid,
            stop: {
                $ne: null
            }
        }
    })
        .then(r => {
            res.setHeader('total', r.count);
            res.send(r.rows.map(RideResponses.rideModelToRideOutput));
            next();
        })
        .catch(e => {
            res.send(e);
            next();
        });
};

let info = function (req, res, next) {
    let id = req.params.id;
    Ride.findById(id).then(r => {
        res.send(r);
        next();
    }).catch(e => {
        res.send(e);
        next();
    });
}

let routesByUserAndBike = function (req, res, next) {
    let rides = req.params;
    let promises = [];
    for (let i = 0; i < rides.length; i++) {
        let ride = rides[i];
        let promise = getRoutesByUserAndBike(ride.userId, ride.bikeId).then(function (r) {
            if (r != null) {
                return RideResponses.rideModelToRideWithBikeOutput(r);
            }
        });
        promises.push(promise);
    }
    return Sequelize.Promise.all(promises).then(function (results) {
        results = results.filter(r => r != null);
        res.send(results);
        next();
    })
    .catch(e => {
        res.send(e);
        next();
    });
};

function getRoutesByUserAndBike(userId, bikeId){
    return Ride.findOne({
        order: [['start', 'DESC']],
        include: [
            { model: Station, as: 'fromStation' },
            { model: Station, as: 'toStation' }
            ,{
                model: Bike,
                as: 'bike',
                where: {
                    id: bikeId
                },
            }
        ],
        where: {
            userId: userId,
            bikeId: bikeId
        }
    });
}

module.exports = {
    getAll: getAll,
    routesByUser: routesByUser,
    info: info,
    routesByUserAndBike: routesByUserAndBike
};
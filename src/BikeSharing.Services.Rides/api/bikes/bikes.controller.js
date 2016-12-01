'use strict';

const Bike = require(process.cwd() + '/models/bike')();
const Ride = require(process.cwd() + '/models/ride')();
const RidePosition = require(process.cwd() + '/models/ridePosition')();

const sequelize = require(process.cwd() + '/db/db.sequelize.js').sequelize();

let getBikesByUser = function (req, res, next) {
    let userId = req.params.userId;

    Ride.findAll({
        attributes: ['bikeId'],
        where: {
            userId: userId,
            stop: null
        },
        //include: [{
        //    model: Bike,
        //    as: 'bike'
        //}]
    }).then(function (bikes) {
        if (!bikes.length) {
            res.send(404, null);
            next();
            return;
        }

        res.send(bikes);
        next();
    }).catch(e => {
        res.send(e);
        next();
    });
};

let locateBike = function (req, res, next) {
    let bikeId = req.params.bikeId;
    
    RidePosition.findOne({
        attributes: ['longitude', 'latitude'],
        order: [['ts', 'DESC']],
        include: [{
            model: Ride,
            as: 'ride',
            where: {
                bikeId: bikeId
            },
        }]
    }).then(function (position) {
        if (!position) {
            res.send(404, null);
            next();
            return;
        }

        res.send(
            {
                longitude: position.longitude,
                latitude: position.latitude
            });
        next();
    }).catch(e => {
        res.send(e);
        next();
    });
};

let locateBikebyTime = function (req, res, next) {
        res.send(
            {
                longitude: -74.005711,
                latitude: 40.713078
            });
        next();
};

let getBikeById = function (req, res, next) {
    let bikeId = req.params.bikeId;
    let pr = Bike.findById(bikeId);
    pr.then(r => {
        if (!r) {
            res.send(404, null);
            next();
            return;
        }

        res.send(r);
        next();
    });
    pr.catch(e => {
        res.send(e);
        next();
    });
}

module.exports = {
    locateBikebyTime: locateBikebyTime,
    getBikesByUser: getBikesByUser,
    locateBike: locateBike,
    getBikeById: getBikeById
};


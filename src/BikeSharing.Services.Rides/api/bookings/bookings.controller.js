'use strict';

const Ride = require(process.cwd() + '/models/ride')();
const Station = require(process.cwd() + '/models/station')();
const Bike = require(process.cwd() + '/models/bike')();
const RideResponses = require('../rides/rides.responses');

let calculateDueDate = function (date) {
    let d = new Date(date.getTime()); 
    d.setMinutes(d.getMinutes() + 30);
    return d;
};

let byId = function (req, res, next) {
    let id = req.params.id;
    let from = parseInt(req.query.from || 0);
    let size = parseInt(req.query.size || 20);

    return Ride.findOne({
        where: { 'id': id },
        order: [['start', 'DESC']],
        include: [
            {
                model: Station,
                as: 'fromStation',
                include: [{ model: Bike, as: 'bikes' }]
            },
            {
                model: Station,
                as: 'toStation',
                include: [{ model: Bike, as: 'bikes' }]
            }]
    })
        .then(ride => {
            let rideResponse = RideResponses.rideModelToRideOutput(ride);
            var booking = {
                id: rideResponse.id,
                bikeId: rideResponse.bikeId,
                eventId: rideResponse.eventId,
                rideType: rideResponse.rideType,
                fromStation: rideResponse.fromStation,
                toStation: rideResponse.toStation,
                registrationDate: rideResponse.start,
                dueDate: calculateDueDate(rideResponse.start)
            };

            booking.toStation.occupied = booking.toStation.bikes.length;
            booking.fromStation.occupied = booking.fromStation.bikes.length;

            res.send(booking);
            next();
        })
        .catch(e => {
            res.send(e);
            next();
        });
};

module.exports = {
    byId: byId
};


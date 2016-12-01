'use strict';

var Sequelize = require('sequelize');
var Ride;

module.exports = function (sequelize) {
    if (Ride) {
        return Ride;
    }

    Ride = sequelize.define('Ride', {
        id: {
            type: Sequelize.INTEGER,
            autoIncrement: true,
            primaryKey: true,
            field: 'Id'
        },
        duration: {
            type: Sequelize.INTEGER,
            allowNull: true
        },
        start: {
            type: Sequelize.DATE,
            allowNull: true
        },
        stop: {
            type: Sequelize.DATE,
            allowNull: true
        },
        bikeId: {
            type: Sequelize.INTEGER,
            allowNull: false
        },
        userId: {
            type: Sequelize.INTEGER,
            allowNull: false
        },
        eventType: {
            type: Sequelize.INTEGER,
            allowNull: true
        },
        eventId: {
            type: Sequelize.INTEGER,
            allowNull: true
        },
        eventName: {
            type: Sequelize.STRING('512'),
            allowNull: true
        },
        geoDistance: {
            type: Sequelize.INTEGER,
            allowNull: true
        }
    },
        {
            tableName: 'rides',
            schema: 'dbo',
            timestamps: false,
            classMethods: {
                associate: function (models) {
                    Ride.belongsTo(models.Station, { foreignKey: 'startStationId', as: 'fromStation' });
                    Ride.belongsTo(models.Station, { foreignKey: 'endStationId', as: 'toStation' });
                    Ride.belongsTo(models.Bike, { foreignKey: 'bikeId', as: 'bike' });
                    Ride.hasMany(models.RidePosition, { foreignKey: 'rideId', as: 'positions' });
                }
            }
        }
    );
    
    return Ride;
};
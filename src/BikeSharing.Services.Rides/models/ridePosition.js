'use strict';

var Sequelize = require('sequelize');
var RidePosition;

module.exports = function (sequelize) {
    if (RidePosition) {
        return RidePosition;
    }

    RidePosition = sequelize.define('RidePosition', {
        id: {
            type: Sequelize.INTEGER,
            autoIncrement: true,
            primaryKey: true
        },
        //rideId: {
        //    type: Sequelize.INTEGER,
        //    allowNull: false
        //},
        latitude: Sequelize.INTEGER,
        longitude: Sequelize.INTEGER,
        ts: Sequelize.DATE
    },
        {
            tableName: 'ridePositions',
            schema: 'dbo',
            timestamps: false,
            classMethods: {
                associate: function (models) {
                    models.RidePosition.belongsTo(models.Ride, { foreignKey: 'rideId', as: 'ride' });
                }
            }
        }
    );

    return RidePosition;
};
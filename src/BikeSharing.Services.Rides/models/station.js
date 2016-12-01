'use strict';

var Sequelize = require('sequelize');

var Station;

module.exports = function (sequelize) {
    if (Station) {
        return Station;
    }

    Station = sequelize.define('Station', {
        id: {
            type: Sequelize.INTEGER,
            autoIncrement: true,
            primaryKey: true
        },
        name: {
            type: Sequelize.STRING('64'),
            allowNull: false
        },
        latitude: {
            type: Sequelize.DECIMAL(18, 10),
            allowNull: false
        },
        longitude: {
            type: Sequelize.DECIMAL(18, 10),
            allowNull: false
        },
        slots: {
            type: Sequelize.DECIMAL(4, 0),
            allowNull: false
        }
    },
        {
            tableName: 'stations',
            schema: 'dbo',
            timestamps: false,
            classMethods: {
                associate: function (models) {
                    Station.hasMany(models.Bike, { foreignKey: 'stationId', as: 'bikes' });
                }
            }
        }
    );

    return Station;
};
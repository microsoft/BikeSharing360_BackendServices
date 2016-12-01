'use strict';

var Sequelize = require('sequelize');
var Bike;

module.exports = function (sequelize) {
    if (Bike) {
        return Bike;
    }

    Bike = sequelize.define('Bike', {
        id: {
            type: Sequelize.INTEGER,
            autoIncrement: false,
            primaryKey: true
        },
        serialNumber: Sequelize.STRING(16),
        inCirculationSince: Sequelize.DATE,
        stationId: Sequelize.INTEGER,
    },
        {
            tableName: 'Bikes',
            schema: 'dbo',
            timestamps: false,
            classMethods: {
                associate: function (models) {
                    Bike.belongsTo(models.Station, { foreignKey: 'stationId', as: 'station' });
                }
            }
        }
    );

    return Bike;
};
'use strict';

const stationModelToStationOutput = function (station) {
    return {
        id: station.id,
        name: station.name,
        longitude: station.longitude,
        latitude: station.latitude,
        slots: station.slots,
        occupied: station.bikes.length
    };
}

module.exports = {
    stationModelToStationOutput
};
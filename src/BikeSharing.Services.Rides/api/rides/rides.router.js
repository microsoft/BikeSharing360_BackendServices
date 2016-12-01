'use strict';

const config = require(process.cwd() + '/config').server;

const controller = require('./rides.controller');

const routes = {
    all: config.path + '/rides',
    info: config.path + '/rides/:id',
    byUser: config.path + '/rides/user/:userId',
    byUserAndBike: config.path + '/users/rides/last'
};

module.exports = app => {
    app.get(routes.all, controller.getAll);
    app.get(routes.info, controller.info);
    app.get(routes.byUser, controller.routesByUser);
    app.post(routes.byUserAndBike, controller.routesByUserAndBike);
};
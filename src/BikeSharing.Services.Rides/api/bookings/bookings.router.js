'use strict';

const config = require(process.cwd() + '/config').server;

const controller = require('./bookings.controller');

const routes = {
    byId: config.path + '/bookings/:id',
};

module.exports = app => {
    app.get(routes.byId, controller.byId);
};
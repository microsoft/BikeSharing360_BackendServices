'use strict';

const config = require(process.cwd() + '/config').server;

const controller = require('./stations.controller');

const routes = {
    info: config.path + '/stations/:id',
    checkout: config.path + '/stations/:id/checkout',
    nearestLocation: config.path + '/stations/nearto',
    byTenant: config.path + '/stations/tenant/:id'
};

module.exports = app => {
    app.get(routes.nearestLocation, controller.nearestLocation);
    app.put(routes.checkout, controller.checkout);
    app.get(routes.info, controller.info);
    app.get(routes.byTenant, controller.byTenant);
};


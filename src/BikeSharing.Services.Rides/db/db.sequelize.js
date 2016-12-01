'use strict';

let config = require('../config/server.config').db;
let Sequelize = require('sequelize');
var glob = require('glob');

var sequelize;

module.exports = {
    connect: function () {
        sequelize = new Sequelize(config.database, config.userName, config.password, config.options);

        var files = glob.sync(process.cwd() + '/models/*.js');
        var models = [];
        files.forEach(function (f) {
            models.push(require(f)(sequelize));
        });

        // Add relations
        models.forEach(function (m) {
            if (!m.associate) {
                return;
            }
            m.associate(sequelize.models);
        });

        // Connect to DB
        return sequelize.sync().then(function () {
            console.log('DB connection established.');
        }).catch(function (err) {
            console.log(err);
            throw err;
        });
    },

    sequelize: function () {
        return sequelize;
    }
};

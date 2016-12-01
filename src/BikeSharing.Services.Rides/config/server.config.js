'use strict';

const config = {
    port: process.env.port || 8000,
    path: 'api',
    name: 'BikeSharingRides',
    db: {
        userName: process.env.databaseUsername || '',
        password: process.env.databasePassword || '',
        database: process.env.database || 'bikesharing-services-rides',
        options: {
            host: process.env.databaseServer || 'localhost',
            dialect: 'mssql',
            dialectOptions: {
                encrypt: true
            },
            pool: {
                max: 5,
                min: 0,
                idle: 10000
            }
        }
    }
};

if (process.env.serverInstance) {
    config.db.options.dialectOptions.instanceName = process.env.serverInstance;
}

module.exports = config;
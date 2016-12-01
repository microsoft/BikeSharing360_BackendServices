var restify = require('restify');
var config = require('./config/');
var db = require('./db/db.sequelize');
var glob = require('glob');

var app = restify.createServer({ name: config.server.name });
var port = process.env.port || config.server.port || 8080;

// Middlewares 
app.use(restify.fullResponse());
app.use(restify.bodyParser());

app.use(restify.queryParser());
app.use(restify.authorizationParser());
app.pre(restify.pre.sanitizePath());
app.pre(restify.pre.pause());

db.connect().then(function () {
    // Import al controllers in /api/**
    var modules = glob.sync('./api/**/*.router.*');
    modules.forEach(function (m) { return require(m)(app); });

    // Configure port
    app.listen(port, function (err) {
        if (err) {
            console.log('Error on port: ', port);
            return;
        }
        console.log('Listening on port: ', port);
    });
});
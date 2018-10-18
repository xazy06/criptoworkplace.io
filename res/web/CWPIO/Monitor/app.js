'use strict';

const pg = require('pg');
const conString = 'postgres://cwp:5678plkjhgjhsad97tgih87rtfb@35.197.207.175/cwp_dev';

pg.connect(conString, function (err, client, done) {
    if (err) {
        return console.error('error fetching client from pool', err);
    }
    client.query('SELECT name, age FROM users;', [], function (err, result) {
        done()
        if (err) {
            // Передача ошибки в обработчик express
            return next(err)
        }
        res.json(result.rows)
    })
})
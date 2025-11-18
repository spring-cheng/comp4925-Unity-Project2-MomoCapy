const { ObjectId } = require('mongodb');
const database = include('databaseConnectionMongoDB');

const mongodb_database = process.env.REMOTE_MONGODB_DATABASE;
const users = database.db(mongodb_database).collection('users');

/**
 * Middleware: Require user to be logged in.
 * Redirects to /login if not authenticated.
 */
function isLoggedIn(req, res, next) {
  if (!req.session.user) {
    return res.redirect('/login');
  }
  next();
}

/**
 * Middleware: Require user to be logged in (API version).
 * Sends 400 JSON error instead of redirect.
 */
function requireAuth(req, res, next) {
  if (!req.session.user) {
    return res.status(400).json({ error: 'You must be logged in to perform this action.' });
  }
  next();
}

module.exports = {
  isLoggedIn,
  requireAuth,
};

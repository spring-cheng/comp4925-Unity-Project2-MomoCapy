import 'dotenv/config';
import express from 'express';
import session from 'express-session';
import MongoStore from 'connect-mongo';
import helmet from 'helmet';
import path from 'path';
import bcrypt from 'bcryptjs';
import database from './databaseConnectionMongoDB.js';

const port = process.env.PORT || 5000;

const app = express();

// Middleware
app.use(helmet({
  contentSecurityPolicy: {
    directives: {
      defaultSrc: ["'self'"],
      styleSrc: ["'self'", "'unsafe-inline'"], // for inline CSS
      scriptSrc: ["'self'"],
      imgSrc: ["'self'", "data:", "https://res.cloudinary.com"], // Allow Cloudinary images
      fontSrc: ["'self'"],
      connectSrc: ["'self'"],
      mediaSrc: ["'self'"],
      objectSrc: ["'none'"],
      childSrc: ["'self'"],
      frameSrc: ["'self'"],
      workerSrc: ["'self'"],
      manifestSrc: ["'self'"]
    }
  }
}));
app.use(express.urlencoded({ extended: false }));
app.use(express.json()); // support JSON bodies

const mongoUser = process.env.REMOTE_MONGODB_USER;
const mongoPass = process.env.REMOTE_MONGODB_PASSWORD;
const mongoHost = process.env.REMOTE_MONGODB_HOST;
const mongoDb = process.env.REMOTE_MONGODB_DATABASE;
const mongodb_database = process.env.REMOTE_MONGODB_DATABASE;
const users = database.db(mongodb_database).collection('users');

const mongoUri = `mongodb+srv://${mongoUser}:${mongoPass}@${mongoHost}/${mongoDb}?retryWrites=true&w=majority`;

app.use(
  session({
    secret: process.env.SESSION_SECRET || 'superSecretKey',
    resave: false,
    saveUninitialized: false,
    store: MongoStore.create({
      mongoUrl: mongoUri,
      dbName: process.env.REMOTE_MONGODB_DATABASE,
      collectionName: 'sessions',
    }),
    cookie: {
      httpOnly: true,
      secure: process.env.NODE_ENV === 'production', 
      sameSite: process.env.NODE_ENV === 'production' ? 'none' : 'lax',
      maxAge: 1000 * 60 * 60 * 24, 
    },
  })
);

// middleware
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Basic routes
app.get('/', (req, res) => {
  res.send('Hello from Express app');
});

app.get('/', (req, res) => {
  res.send("Hello from Express app");
});

app.post('/register', async (req, res) => {
  const { username, password } = req.body;

  if (!username || !password)
    return res.status(400).send("Missing fields");

  const exists = await users.findOne({ username });

  if (exists)
    return res.status(400).send("User already exists");

  const hashed = await bcrypt.hash(password, 10);

  await users.insertOne({
    username,
    password: hashed,
  });

  res.send("Welcome! Your account has been created.");
});

app.post('/login', async (req, res) => {
  const { username, password } = req.body;

  const user = await users.findOne({ username });
  if (!user) return res.status(400).send("User not found");

  const match = await bcrypt.compare(password, user.password);
  if (!match) return res.status(400).send("Incorrect password");

  req.session.user = username;

  res.send("You are now logged in.");
});

app.post('/score', async (req, res) => {
  try {
    if (!req.session.user) {
      return res.status(401).json({ error: "Not logged in." });
    }

    const { savedCats, missedBoxes, won } = req.body;

    if (savedCats == null || missedBoxes == null || won == null) {
      return res.status(400).json({ error: "Missing score fields." });
    }

    const scoresCollection = database
      .db(process.env.REMOTE_MONGODB_DATABASE)
      .collection("scores");

    const entry = {
      username: req.session.user,
      savedCats,
      missedBoxes,
      won,
      createdAt: new Date()
    };

    await scoresCollection.insertOne(entry);

    res.json({ success: true, message: "Score saved successfully!" });
  } catch (err) {
    console.error("Score error:", err);
    res.status(500).json({ error: "Server error saving score" });
  }
});

// 404 handler
app.use((req, res) => {
  res.status(404).send('Not Found');
});

// Error handler
app.use((err, req, res, next) => {
  console.error(err);
  res.status(500).send('Internal Server Error');
});

// Start server
app.listen(port, () => {
  console.log(`Server listening on http://localhost:${port}`);
});

export default app;
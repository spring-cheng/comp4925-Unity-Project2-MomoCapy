import 'dotenv/config';
import { MongoClient } from 'mongodb';

const mongodb_host = process.env.REMOTE_MONGODB_HOST;
const mongodb_user = process.env.REMOTE_MONGODB_USER;
const mongodb_password = process.env.REMOTE_MONGODB_PASSWORD;

const atlasURI = `mongodb+srv://${mongodb_user}:${mongodb_password}@${mongodb_host}/?retryWrites=true`;

const database = new MongoClient(atlasURI);

export default database;

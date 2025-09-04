const express = require('express');
const cors = require('cors');
const fs = require('fs');
const path = require('path');
const { v4: uuidv4 } = require('uuid');

const app = express();
const PORT = process.env.PORT || 4000;

app.use(cors());
app.use(express.json());

// Simple in-memory user for mocked auth
const MOCK_USER = {
  id: 'user-1',
  email: 'test@example.com',
  name: 'Test User',
  password: 'password123' // mocked only — do not use in prod
};

// read nodes once (small JSON)
const nodesFile = path.join(__dirname, 'data', 'nodes.json');
let nodes = [];
try {
  nodes = JSON.parse(fs.readFileSync(nodesFile, 'utf8'));
} catch (err) {
  console.error('Failed to read nodes.json', err);
}

// POST /api/v1/auth/login (mock)
app.post('/api/v1/auth/login', (req, res) => {
  const { email, password } = req.body || {};
  // simple validation
  if (!email || !password) {
    return res.status(400).json({ message: 'email and password required' });
  }

  if (email === MOCK_USER.email && password === MOCK_USER.password) {
    // return mocked token and user (no password)
    const token = uuidv4();
    const user = { id: MOCK_USER.id, email: MOCK_USER.email, name: MOCK_USER.name };
    return res.json({ token, user });
  }

  return res.status(401).json({ message: 'invalid credentials' });
});

// GET /api/v1/nodes
app.get('/api/v1/nodes', (req, res) => {
  // simulate small delay
  setTimeout(() => res.json(nodes), 250);
});

// health
app.get('/health', (req, res) => res.json({ status: 'ok' }));

app.listen(PORT, () => {
  console.log(`Mock backend running on http://localhost:${PORT}`);
});

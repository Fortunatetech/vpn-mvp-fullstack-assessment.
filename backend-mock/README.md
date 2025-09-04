Mock backend for VPN MVP
1. Install:
   cd backend-mock
   npm install

2. Run:
   npm start
   # server runs on http://localhost:4000

3. Endpoints:
   POST /api/v1/auth/login  -> { email, password }  (use test@example.com / password123)
   GET  /api/v1/nodes        -> returns nodes array
   GET  /health

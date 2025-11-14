# Sprint 1 — Frontend Setup and Authentication Pages

## Goal

Set up the React application structure and implement authentication-related pages and logic.  
Users should be able to:

- Register a new account
- Log in and log out
- Maintain session state
- View and edit their profile

## Tasks

### Setup

- Install dependencies:
  - `axios`, `react-router-dom`, `bootstrap`, `Playwright`
  - `react-query` or custom hooks for data fetching (optional)
- Configure base API URL for backend communication

### Authentication Pages

1. **Register Page**
   - Form with name, email, password
   - POST to `/user/register`
   - Show success or error messages

2. **Login Page**
   - Form with email and password
   - POST to `/user/login`
   - On success, store session token (e.g., in `localStorage` or context)
   - Redirect to home

3. **Logout Functionality**
   - Clear stored token and call `/user/logout`

4. **Profile Page**
   - GET `/user/profile` to load data
   - PUT `/user/profile` to update name, bio, or profile picture
   - Show “Unauthorized” message if not logged in

### Routing

- Set up routes: `/login`, `/register`, `/profile`, `/home`
- Protect routes (redirect to login if unauthenticated)

### Testing — Frontend Behavior

- Registration and login works
- Session persists across page reloads
- Unauthorized users are redirected correctly
- Profile editing updates UI and backend data
- Use Playwright for testing

### Deliverables

- React app connected to backend auth endpoints  
- Working login, register, logout, and profile pages  
- Session management implemented  

### Estimated Duration

~1 week  

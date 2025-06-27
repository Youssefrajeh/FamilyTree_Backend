# FamilyTree
# Family Tree Web Application

A full-stack web application for creating and managing family trees with user authentication, interactive visualization, and collaborative family data management.

## 🏗️ Architecture

- **Backend**: ASP.NET Core Web API (hosted on Render)
- **Frontend**: Vue.js 3 + Quasar Framework (hosted on Netlify)
- **Database**: PostgreSQL
- **Authentication**: JWT tokens
- **Visualization**: D3.js for interactive family tree

## 📁 Project Structure

```
FamilyTree/
├── backend/                 # ASP.NET Core Web API
│   ├── FamilyTreeAPI/
│   ├── Models/
│   ├── Controllers/
│   ├── Services/
│   └── Data/
├── frontend/                # Vue.js + Quasar
│   ├── src/
│   ├── public/
│   └── quasar.config.js
├── database/                # Database scripts
└── docs/                   # Documentation
```

## 🚀 Features

### ✅ User Authentication
- Register/Login with email and password
- JWT token-based authentication
- Family tree association per user

### ✅ Family Information Management
- Personal details (name, DOB, gender)
- Family relationships (parents, spouse, children)
- Profile photo upload
- Interactive family member linking

### ✅ Tree Visualization
- Auto-generated family tree from oldest ancestor
- Interactive D3.js visualization (zoom, pan, expand/collapse)
- Responsive design for all devices
- Real-time updates

### ✅ Backend API
- RESTful API endpoints
- PostgreSQL database integration
- Image upload handling
- Secure authentication

## 🔧 Setup Instructions

### Backend Setup
1. Navigate to `backend/` directory
2. Install dependencies: `dotnet restore`
3. Update connection string in `appsettings.json`
4. Run migrations: `dotnet ef database update`
5. Start API: `dotnet run`

### Frontend Setup
1. Navigate to `frontend/` directory
2. Install dependencies: `npm install`
3. Update API URL in environment files
4. Start development server: `quasar dev`

## 🌐 Deployment

### Backend (Render)
- Configured for automatic deployment from Git
- Environment variables for database connection
- Production-ready build configuration

### Frontend (Netlify)
- Static site deployment
- Environment variables for API endpoints
- Custom domain support

## 📋 API Endpoints

- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `GET /api/user/{id}` - Get user details
- `POST /api/user` - Create/update user info
- `GET /api/tree/{rootId}` - Get family tree data
- `POST /api/upload` - Upload profile images

## 🔐 Security Features

- JWT token authentication
- Password hashing with bcrypt
- CORS configuration
- Environment variable protection
- SQL injection prevention

## 📱 Responsive Design

- Mobile-first approach
- Touch-friendly tree navigation
- Adaptive layouts for all screen sizes
- Progressive Web App capabilities 
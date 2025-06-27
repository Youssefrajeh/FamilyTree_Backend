# 🌳 Family Tree Web Application - Complete Setup Guide

## 📋 Project Overview

This is a complete full-stack family tree management application built with:

### 🖥️ Backend (ASP.NET Core Web API)
- **Framework**: ASP.NET Core 8.0 Web API
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: JWT Bearer tokens
- **Password Hashing**: BCrypt
- **Hosting**: Render.com
- **Features**: User management, family member CRUD, relationship management, tree generation

### 🌐 Frontend (Vue.js + Quasar)
- **Framework**: Vue.js 3 with Composition API
- **UI Library**: Quasar Framework
- **State Management**: Pinia
- **HTTP Client**: Axios
- **Visualization**: D3.js for family tree rendering
- **Hosting**: Netlify
- **Features**: Authentication, family management, interactive tree visualization

## 🚀 Quick Start

### Prerequisites
- Node.js 18+ and npm
- .NET 8.0 SDK
- PostgreSQL (for local development)
- Git

### 1. Clone and Setup Backend

```bash
# Navigate to backend directory
cd backend/FamilyTreeAPI/FamilyTreeAPI

# Restore packages
dotnet restore

# Update database connection string in appsettings.json
# Create local PostgreSQL database: familytree_db

# Run migrations
dotnet ef database update

# Start the API
dotnet run
```

The API will be available at `https://localhost:7297` with Swagger UI at the root.

### 2. Setup Frontend

```bash
# Navigate to frontend directory
cd quasar-project

# Install dependencies
npm install

# Start development server
npm run dev
```

The frontend will be available at `http://localhost:9000`.

## 🏗️ Project Structure

```
FamilyTree/
├── README.md                           # Main project documentation
├── SETUP_GUIDE.md                     # This setup guide
├── netlify.toml                        # Netlify deployment config
├── backend/
│   ├── render.yaml                     # Render deployment config
│   └── FamilyTreeAPI/
│       └── FamilyTreeAPI/
│           ├── Controllers/            # API controllers
│           │   ├── AuthController.cs
│           │   ├── FamilyMembersController.cs
│           │   └── FamilyTreeController.cs
│           ├── Data/                   # Database context
│           │   └── FamilyTreeContext.cs
│           ├── DTOs/                   # Data transfer objects
│           │   ├── AuthDTOs.cs
│           │   └── FamilyMemberDTOs.cs
│           ├── Models/                 # Database models
│           │   ├── User.cs
│           │   ├── FamilyMember.cs
│           │   └── Spouse.cs
│           ├── Services/               # Business logic
│           │   ├── AuthService.cs
│           │   └── FamilyTreeService.cs
│           ├── Program.cs              # App configuration
│           ├── appsettings.json        # Development config
│           └── appsettings.Production.json # Production config
├── quasar-project/                     # Frontend application
│   ├── src/
│   │   ├── components/                 # Vue components
│   │   ├── layouts/                    # Layout components
│   │   ├── pages/                      # Page components
│   │   │   └── LoginPage.vue
│   │   ├── router/                     # Vue Router config
│   │   ├── services/                   # API services
│   │   │   └── api.js
│   │   ├── stores/                     # Pinia stores
│   │   │   ├── auth.js
│   │   │   └── familyTree.js
│   │   └── App.vue
│   ├── public/                         # Static assets
│   ├── package.json
│   └── quasar.config.js               # Quasar configuration
└── docs/
    └── DEPLOYMENT.md                   # Deployment guide
```

## 🔧 Configuration

### Backend Configuration

1. **Database Setup** (`appsettings.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=familytree_db;Username=postgres;Password=your_password"
  },
  "Jwt": {
    "Key": "your-super-secret-jwt-key-that-should-be-at-least-32-characters-long",
    "Issuer": "FamilyTreeAPI",
    "Audience": "FamilyTreeApp"
  }
}
```

2. **Create Local Database**:
```sql
CREATE DATABASE familytree_db;
```

### Frontend Configuration

Update API URL in `src/services/api.js`:
```javascript
export const api = axios.create({
  baseURL: 'https://localhost:7297/api', // For local development
  // baseURL: 'https://your-render-app.onrender.com/api', // For production
})
```

## 📊 Database Schema

### Tables
- **Users**: ASP.NET Identity users with first/last names
- **FamilyMembers**: Core family member data with relationships
- **Spouses**: Marriage relationships between family members

### Key Relationships
- Self-referencing parent-child relationships
- Many-to-many spouse relationships
- User ownership of family data

## 🔐 API Endpoints

### Authentication
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/verify-token` - Token validation
- `GET /api/auth/me` - Get current user

### Family Members
- `GET /api/familymembers` - Get all family members
- `POST /api/familymembers` - Create family member
- `PUT /api/familymembers/{id}` - Update family member
- `DELETE /api/familymembers/{id}` - Delete family member
- `GET /api/familymembers/search` - Search family members

### Family Tree
- `GET /api/familytree/{rootId}` - Get family tree from root
- `GET /api/familytree/roots` - Get potential tree roots
- `GET /api/familytree/statistics` - Get family statistics

## 🎨 Frontend Features

### Pages & Components
- **Authentication**: Login and registration forms
- **Dashboard**: Family overview and statistics
- **Family Management**: Add, edit, delete family members
- **Tree Visualization**: Interactive D3.js family tree
- **Profile Management**: User settings and preferences

### State Management (Pinia)
- **Auth Store**: User authentication state
- **Family Tree Store**: Family data and operations

## 🚀 Deployment

Follow the comprehensive deployment guide in `docs/DEPLOYMENT.md` for:

1. **Backend to Render**: Automatic PostgreSQL provisioning and API deployment
2. **Frontend to Netlify**: Static site deployment with environment configuration

## 🛠️ Development Workflow

### Backend Development
```bash
# Run with hot reload
dotnet watch run

# Run database migrations
dotnet ef migrations add MigrationName
dotnet ef database update

# Run tests (if added)
dotnet test
```

### Frontend Development
```bash
# Development server
npm run dev

# Build for production
npm run build:spa

# Lint and format
npm run lint
npm run format
```

## 🔍 Testing

### Backend Testing
- Use Swagger UI at `https://localhost:7297` for API testing
- Test authentication flow with registration and login
- Verify CRUD operations for family members

### Frontend Testing
- Test user registration and login flows
- Verify family member management functionality
- Test responsive design on different screen sizes

## 📚 Key Features Implemented

### ✅ User Authentication
- Secure registration and login with JWT tokens
- Password validation and hashing
- Token-based API authorization

### ✅ Family Management
- Complete CRUD operations for family members
- Parent-child relationship management
- Spouse relationship handling
- Profile image support

### ✅ Tree Visualization
- Automatic tree generation from oldest ancestor
- Interactive D3.js visualization
- Family statistics and insights
- Responsive tree rendering

### ✅ Security
- JWT token authentication
- CORS configuration
- Input validation and sanitization
- SQL injection prevention

### ✅ Production Ready
- Environment-based configuration
- Deployment automation
- Error handling and logging
- Performance optimizations

## 🆘 Troubleshooting

### Common Issues

1. **Database Connection Failed**:
   - Verify PostgreSQL is running
   - Check connection string in appsettings.json
   - Ensure database exists

2. **API 401 Unauthorized**:
   - Check JWT token in localStorage
   - Verify token hasn't expired
   - Ensure API is running

3. **Frontend Can't Connect to API**:
   - Verify API URL in api.js
   - Check CORS configuration
   - Ensure both frontend and backend are running

4. **Build Failures**:
   - Clear node_modules and reinstall: `rm -rf node_modules package-lock.json && npm install`
   - Clear .NET cache: `dotnet clean && dotnet restore`

## 🎯 Next Steps

### Potential Enhancements
1. **Image Upload**: Implement file upload for profile photos
2. **Advanced Visualization**: Add more tree layouts and zoom controls
3. **Export Features**: PDF/image export of family trees
4. **Mobile App**: React Native or Flutter mobile version
5. **Social Features**: Family sharing and collaboration
6. **Data Import**: Import from GEDCOM files
7. **Advanced Search**: Full-text search and filtering
8. **Notifications**: Email notifications for family updates

### Performance Optimizations
1. **Caching**: Implement Redis caching for frequently accessed data
2. **Pagination**: Add pagination for large family lists
3. **Lazy Loading**: Implement lazy loading for tree nodes
4. **Image Optimization**: Add image compression and CDN

## 📞 Support

For issues or questions:
1. Check the troubleshooting section above
2. Review the deployment guide in `docs/DEPLOYMENT.md`
3. Check API documentation via Swagger UI
4. Review console logs for error details

## 🏆 Congratulations!

You now have a complete, production-ready family tree management application! The system includes secure authentication, comprehensive family data management, interactive visualization, and is ready for deployment to modern cloud platforms. 
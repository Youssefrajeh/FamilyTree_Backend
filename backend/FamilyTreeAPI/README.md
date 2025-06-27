# Family Tree API

ASP.NET Core Web API for the Family Tree Management Application.

## Features

- User authentication with JWT tokens
- Family member management
- Family tree visualization data
- PostgreSQL database integration
- Swagger API documentation

## Local Development

### Prerequisites
- .NET 8.0 SDK
- PostgreSQL (optional - falls back to in-memory database)

### Setup
1. Clone the repository
2. Navigate to the project directory: `cd FamilyTreeAPI`
3. Install dependencies: `dotnet restore`
4. Update connection string in `appsettings.json` (optional)
5. Run the application: `dotnet run`

### API Documentation
- Development: http://localhost:5000 (Swagger UI at root)
- Production: https://your-api-url.onrender.com/api-docs

## Deployment on Render

### Automatic Deployment (Recommended)
1. Connect your GitHub repository to Render
2. Use the `render.yaml` file for automatic configuration
3. Render will automatically:
   - Create a PostgreSQL database
   - Deploy the API
   - Set up environment variables

### Manual Deployment
1. Create a new Web Service on Render
2. Connect your GitHub repository
3. Set the following environment variables:
   - `ASPNETCORE_ENVIRONMENT`: Production
   - `ASPNETCORE_URLS`: http://0.0.0.0:$PORT
   - `DATABASE_URL`: (from PostgreSQL database)
   - `JWT__KEY`: (32+ character secret key)
   - `JWT__ISSUER`: FamilyTreeAPI
   - `JWT__AUDIENCE`: FamilyTreeApp

4. Build Command: `dotnet build`
5. Start Command: `dotnet run --urls http://0.0.0.0:$PORT`

### Database Setup
- Create a PostgreSQL database on Render
- The application will automatically run migrations on startup
- Connection string will be provided via `DATABASE_URL` environment variable

## API Endpoints

### Authentication
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login

### Family Members
- `GET /api/familymembers` - Get all family members
- `POST /api/familymembers` - Create new family member
- `PUT /api/familymembers/{id}` - Update family member
- `DELETE /api/familymembers/{id}` - Delete family member

### Health Check
- `GET /health` - API health status

## Environment Variables

| Variable | Description | Required |
|----------|-------------|----------|
| `DATABASE_URL` | PostgreSQL connection string | Yes (Production) |
| `JWT__KEY` | JWT signing key (32+ chars) | Yes |
| `JWT__ISSUER` | JWT issuer | No (default: FamilyTreeAPI) |
| `JWT__AUDIENCE` | JWT audience | No (default: FamilyTreeApp) |
| `ASPNETCORE_ENVIRONMENT` | Environment name | No (default: Production) |

## CORS Configuration

The API is configured to allow requests from:
- Localhost (development)
- Netlify domains (production)
- Custom domains (configurable)

## Security Features

- JWT token authentication
- Password hashing with BCrypt
- CORS protection
- Security headers
- SQL injection prevention 
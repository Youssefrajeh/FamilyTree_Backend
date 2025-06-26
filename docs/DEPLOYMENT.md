# Family Tree Application - Deployment Guide

## 🚀 Overview

This guide will walk you through deploying your Family Tree application to:
- **Backend**: Render.com (ASP.NET Core Web API + PostgreSQL)
- **Frontend**: Netlify (Vue.js + Quasar Framework)

## 📋 Prerequisites

Before deploying, ensure you have:
- A GitHub repository with your code
- A Render.com account
- A Netlify account
- All code committed and pushed to your main branch

## 🖥️ Backend Deployment (Render)

### Step 1: Prepare Your Repository

1. Ensure your backend code is in the `backend/FamilyTreeAPI/FamilyTreeAPI/` directory
2. Verify that `render.yaml` is in the root of your repository
3. Make sure `appsettings.Production.json` is configured

### Step 2: Deploy to Render

1. **Connect Repository**:
   - Go to [Render Dashboard](https://dashboard.render.com)
   - Click "New" → "Blueprint"
   - Connect your GitHub repository
   - Select your repository

2. **Configure Environment Variables**:
   Render will automatically create the required environment variables based on your `render.yaml` file:
   - `JWT_SECRET_KEY` (auto-generated)
   - Database connection variables (auto-configured)

3. **Deploy**:
   - Click "Create New Blueprint"
   - Render will automatically provision the PostgreSQL database and deploy your API
   - Wait for the deployment to complete (usually 5-10 minutes)

### Step 3: Verify Backend Deployment

1. Once deployed, note your API URL (e.g., `https://your-app-name.onrender.com`)
2. Test the API by visiting: `https://your-app-name.onrender.com` (should show Swagger UI)
3. Test authentication endpoints to ensure they're working

## 🌐 Frontend Deployment (Netlify)

### Step 1: Update API URL

1. Update the `netlify.toml` file with your actual Render API URL:
   ```toml
   [context.production.environment]
   VUE_APP_API_URL = "https://your-actual-render-app.onrender.com/api"
   ```

### Step 2: Deploy to Netlify

1. **Connect Repository**:
   - Go to [Netlify Dashboard](https://app.netlify.com)
   - Click "New site from Git"
   - Choose GitHub and select your repository

2. **Configure Build Settings**:
   - Build command: `npm ci && npm run build:spa`
   - Publish directory: `quasar-project/dist/spa`
   - Base directory: `quasar-project/`

3. **Deploy**:
   - Click "Deploy site"
   - Netlify will build and deploy your frontend
   - Wait for deployment to complete (usually 2-5 minutes)

### Step 3: Configure Custom Domain (Optional)

1. In Netlify dashboard, go to "Domain management"
2. Add your custom domain
3. Configure DNS settings as instructed by Netlify
4. Enable HTTPS (automatically handled by Netlify)

## 🔧 Environment Configuration

### Backend Environment Variables (Render)

| Variable | Description | Auto-configured |
|----------|-------------|-----------------|
| `ASPNETCORE_ENVIRONMENT` | Set to "Production" | ✅ |
| `ASPNETCORE_URLS` | Bind to 0.0.0.0:$PORT | ✅ |
| `JWT_SECRET_KEY` | JWT signing key | ✅ |
| `PGHOST` | PostgreSQL host | ✅ |
| `PGPORT` | PostgreSQL port | ✅ |
| `PGDATABASE` | PostgreSQL database name | ✅ |
| `PGUSER` | PostgreSQL username | ✅ |
| `PGPASSWORD` | PostgreSQL password | ✅ |

### Frontend Environment Variables (Netlify)

| Variable | Description | Value |
|----------|-------------|-------|
| `VUE_APP_API_URL` | Backend API URL | `https://your-render-app.onrender.com/api` |

## 🗄️ Database Setup

The PostgreSQL database is automatically:
1. Created by Render
2. Connected to your application
3. Migrated on first startup (see `Program.cs`)

### Manual Database Operations (if needed)

If you need to run migrations manually:

```bash
# Connect to your deployed app via Render shell
cd backend/FamilyTreeAPI/FamilyTreeAPI
dotnet ef database update
```

## 🔐 Security Considerations

### Production Security Checklist

- [ ] JWT secret key is auto-generated and secure
- [ ] Database credentials are auto-generated
- [ ] HTTPS is enabled (automatic with Render/Netlify)
- [ ] CORS is properly configured for your frontend domain
- [ ] Environment variables are properly set

### Post-Deployment Security Updates

1. **Update CORS Origins** in `Program.cs`:
   ```csharp
   builder.Services.AddCors(options =>
   {
       options.AddPolicy("AllowSpecificOrigins", policy =>
       {
           policy.WithOrigins("https://your-netlify-domain.netlify.app")
                 .AllowAnyMethod()
                 .AllowAnyHeader();
       });
   });
   ```

2. **Use specific CORS policy** instead of "AllowAll":
   ```csharp
   app.UseCors("AllowSpecificOrigins");
   ```

## 📊 Monitoring and Maintenance

### Health Checks

Monitor your application health:
- **Backend**: `https://your-render-app.onrender.com/health`
- **Frontend**: Your Netlify domain should load the application

### Logs

- **Render**: View logs in the Render dashboard
- **Netlify**: View build logs and function logs in Netlify dashboard

### Updates

To deploy updates:
1. **Backend**: Push changes to your main branch - Render auto-deploys
2. **Frontend**: Push changes to your main branch - Netlify auto-deploys

## 🚨 Troubleshooting

### Common Issues

1. **Backend won't start**:
   - Check Render logs for errors
   - Verify all environment variables are set
   - Ensure database connection is working

2. **Frontend can't connect to API**:
   - Verify API URL in environment variables
   - Check CORS configuration
   - Ensure API is deployed and running

3. **Database connection errors**:
   - Check PostgreSQL service status in Render
   - Verify connection string format
   - Check firewall/network settings

4. **Authentication not working**:
   - Verify JWT secret key is set
   - Check token expiration settings
   - Ensure HTTPS is enabled

### Getting Help

- Check [Render Documentation](https://render.com/docs)
- Check [Netlify Documentation](https://docs.netlify.com)
- Review application logs for specific error messages

## ✅ Deployment Checklist

### Pre-deployment
- [ ] Code is committed and pushed to GitHub
- [ ] Backend builds successfully locally
- [ ] Frontend builds successfully locally
- [ ] All environment variables are configured
- [ ] Database migrations are ready

### During deployment
- [ ] Render service deploys successfully
- [ ] Database is created and connected
- [ ] API endpoints are accessible
- [ ] Netlify site builds successfully
- [ ] Frontend connects to backend API

### Post-deployment
- [ ] Test user registration and login
- [ ] Test family member CRUD operations
- [ ] Test family tree visualization
- [ ] Verify all features work in production
- [ ] Configure custom domain (if desired)
- [ ] Set up monitoring and alerts

## 🎉 Success!

Once deployed, your Family Tree application will be accessible at:
- **API**: `https://your-render-app.onrender.com`
- **App**: `https://your-netlify-site.netlify.app`

Users can now register, login, and start building their family trees! 
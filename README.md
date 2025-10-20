# Notika Identity Email System

A comprehensive ASP.NET Core 9.0 web application that provides a complete email management system with user authentication, role-based authorization, and messaging capabilities.

## 🚀 Features

### Authentication & Authorization
- **User Registration & Login**: Complete user registration with email verification
- **JWT Token Authentication**: Secure token-based authentication system
- **Google OAuth Integration**: Social login with Google accounts
- **Role-Based Access Control**: Comprehensive role management system
- **Email Activation**: Email verification system with activation codes
- **Password Management**: Secure password hashing and change functionality

### Messaging System
- **Internal Messaging**: Send and receive messages between users
- **Message Categories**: Organize messages with custom categories
- **Inbox & Sent Box**: Separate views for received and sent messages
- **Message Status Tracking**: Read/unread message status
- **Message Details**: Detailed message viewing with sender/receiver information

### User Management
- **Profile Management**: Complete user profile editing capabilities
- **User Administration**: Admin panel for user management
- **Role Assignment**: Assign and manage user roles
- **User Status Control**: Activate/deactivate user accounts

### Additional Features
- **Comment System**: User comment functionality
- **Notification System**: User notification management
- **Responsive Design**: Modern UI with Notika admin template
- **Error Handling**: Custom error pages (401, 403, 404)
- **Email Integration**: SMTP email sending capabilities

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Core Identity + JWT Bearer
- **UI Framework**: MVC with Razor Views
- **Email**: MailKit for SMTP email functionality
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap
- **Template**: Notika Admin Template

## 📋 Prerequisites

Before running this application, ensure you have the following installed:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/)

## ⚙️ Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/emredurak-dev/NotikaIdentityEmail.git
cd NotikaIdentityEmail
```

### 2. Database Configuration
1. Update the connection string in `Context/EmailContext.cs`:
```csharp
optionsBuilder.UseSqlServer("YOUR_CONNECTION_STRING_HERE");
```

2. Run Entity Framework migrations:
```bash
dotnet ef database update
```

### 3. Configuration Setup

#### JWT Settings (`appsettings.json`)
```json
{
  "JwtSettingsKey": {
    "Key": "YOUR_JWT_SECRET_KEY_HERE_MINIMUM_32_CHARACTERS_LONG",
    "Issuer": "localhost",
    "Audience": "localhost",
    "ExpireMinutes": 5
  }
}
```

#### Google OAuth Setup (`Program.cs`)
```csharp
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = "YOUR_GOOGLE_CLIENT_ID_HERE";
    googleOptions.ClientSecret = "YOUR_GOOGLE_CLIENT_SECRET_HERE";
    googleOptions.CallbackPath = "/signin-google";
});
```

#### Email Configuration (`Controllers/RegisterController.cs`)
Update the SMTP settings for email functionality:
```csharp
client.Connect("YOUR_SMTP_SERVER_HERE", 587, false);
client.Authenticate("YOUR_EMAIL_ADDRESS_HERE", "YOUR_EMAIL_PASSWORD_HERE");
```

### 4. Run the Application
```bash
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

## 🗂️ Project Structure

```
NotikaIdentityEmail/
├── Controllers/           # MVC Controllers
│   ├── ActivationController.cs
│   ├── CategoryController.cs
│   ├── CommentController.cs
│   ├── HomeController.cs
│   ├── LoginController.cs
│   ├── MessageController.cs
│   ├── ProfileController.cs
│   ├── RegisterController.cs
│   └── RoleController.cs
├── Entities/             # Data Models
│   ├── AppUser.cs
│   ├── Category.cs
│   ├── Comment.cs
│   ├── Message.cs
│   └── Notification.cs
├── Models/               # ViewModels and DTOs
│   ├── IdentityModels/
│   ├── JwtModels/
│   └── MessageViewModels/
├── Views/                # Razor Views
│   ├── Shared/
│   ├── Home/
│   ├── Login/
│   ├── Message/
│   └── ...
├── Context/              # Database Context
│   └── EmailContext.cs
├── ViewComponents/       # Reusable UI Components
└── wwwroot/             # Static Files
    ├── css/
    ├── js/
    └── lib/
```

## 🔐 Security Features

- **JWT Token Security**: Secure token-based authentication
- **Password Hashing**: ASP.NET Core Identity password hashing
- **Email Verification**: Account activation via email
- **Role-Based Authorization**: Fine-grained access control
- **HTTPS Enforcement**: Secure communication
- **Cookie Security**: HttpOnly, Secure, SameSite cookie settings

## 📧 Email System

The application includes a comprehensive email system:

- **User Registration Emails**: Automatic activation code emails
- **Internal Messaging**: User-to-user messaging system
- **Email Categories**: Organized message categorization
- **SMTP Integration**: Configurable SMTP email sending

## 🎨 User Interface

- **Modern Design**: Clean and professional interface
- **Responsive Layout**: Mobile-friendly design
- **Admin Dashboard**: Comprehensive admin panel
- **User Dashboard**: Personalized user experience
- **Component-Based**: Reusable view components

## 🔧 API Endpoints

### Authentication
- `POST /Login/UserLogin` - User login
- `POST /Register/CreateUser` - User registration
- `GET /Login/ExternalLogin` - Google OAuth login

### Messaging
- `GET /Message/Inbox` - Get user inbox
- `GET /Message/Sendbox` - Get user sent messages
- `POST /Message/ComposeMessage` - Send new message
- `GET /Message/MessageDetail/{id}` - Get message details

### User Management
- `GET /Profile/EditProfile` - Get user profile
- `POST /Profile/EditProfile` - Update user profile
- `GET /Role/UserList` - Get all users (Admin)
- `POST /Role/AssignRole` - Assign roles to users

## 🚀 Deployment

### Production Deployment
1. Update connection strings for production database
2. Configure production SMTP settings
3. Set up Google OAuth credentials for production domain
4. Update JWT settings for production
5. Deploy to your preferred hosting platform (Azure, AWS, etc.)

### Environment Variables
For production deployment, consider using environment variables for sensitive configuration:
- `ConnectionStrings__DefaultConnection`
- `JwtSettingsKey__Key`
- `Google__ClientId`
- `Google__ClientSecret`

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 🔄 Version History

- **v1.0.0** - Initial release with core functionality
  - User authentication and authorization
  - Messaging system
  - Role management
  - Email integration

---

**Note**: Remember to replace all placeholder values (YOUR_*_HERE) with your actual configuration values before running the application.

---

# Screenshots
<img width="951" height="656" alt="Screenshot 2025-10-20 145344" src="https://github.com/user-attachments/assets/a2dcea36-6026-43a5-8269-0a414de37e1b" />
<img width="1351" height="629" alt="Screenshot 2025-10-20 143348" src="https://github.com/user-attachments/assets/720738a4-6325-42ad-86a5-bbb6ea835cc9" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 143315" src="https://github.com/user-attachments/assets/765842c0-2f17-4429-9dda-6206c4b0c402" />
<img width="1600" height="221" alt="Screenshot 2025-10-20 143215" src="https://github.com/user-attachments/assets/9b5feaad-c748-44af-8ccb-d0fbbd2921c1" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 143122" src="https://github.com/user-attachments/assets/878afb41-4582-4fbd-b0df-ad4b138e4b63" />
<img width="184" height="133" alt="Screenshot 2025-10-20 143116" src="https://github.com/user-attachments/assets/26df20f4-9711-440a-b846-c4945d84d0b8" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 143036" src="https://github.com/user-attachments/assets/b9617823-6f55-49a9-8cea-30bad22737f7" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 143015" src="https://github.com/user-attachments/assets/5e03335a-b401-462f-a513-3e8093367e66" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 142909" src="https://github.com/user-attachments/assets/2cc3d19e-201c-4146-9fd0-b60749c50369" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 142859" src="https://github.com/user-attachments/assets/8dc4c521-1f08-4e10-be4c-e08ef4d851a9" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 142844" src="https://github.com/user-attachments/assets/534ce9d3-b923-4e3e-99ca-e1909b6696dc" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 142837" src="https://github.com/user-attachments/assets/d65423eb-6839-45f2-9d9b-6fbe725fae3a" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 142821" src="https://github.com/user-attachments/assets/787d8b30-2fea-41bd-8f57-648b4b7ae0b5" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 142753" src="https://github.com/user-attachments/assets/97a3e953-3704-4602-9907-a4a92a1f1a1b" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 142731" src="https://github.com/user-attachments/assets/a6b0fd2c-b7c2-4c21-b86a-b9aeed63e84d" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 142724" src="https://github.com/user-attachments/assets/a9a61064-84b3-4e65-a2d2-3315e5d78570" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 142528" src="https://github.com/user-attachments/assets/9b1903b5-4537-410c-bf6c-57f1913390db" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 142502" src="https://github.com/user-attachments/assets/6d902c2c-f7bc-4a6c-9845-5b66e505ea57" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 142455" src="https://github.com/user-attachments/assets/5f4627e8-d1d2-489a-a536-b70444749f3d" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 142450" src="https://github.com/user-attachments/assets/c5241f5b-507a-4ad9-9dff-c0798b9da581" />
<img width="1919" height="1079" alt="Screenshot 2025-10-20 142439" src="https://github.com/user-attachments/assets/46254206-5580-44c3-9c20-605da709680a" />


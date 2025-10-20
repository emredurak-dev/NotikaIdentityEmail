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
git clone https://github.com/yourusername/NotikaIdentityEmail.git
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

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

If you encounter any issues or have questions:

1. Check the [Issues](https://github.com/yourusername/NotikaIdentityEmail/issues) page
2. Create a new issue with detailed information
3. Contact the development team

## 🔄 Version History

- **v1.0.0** - Initial release with core functionality
  - User authentication and authorization
  - Messaging system
  - Role management
  - Email integration

## 📞 Contact

- **Developer**: Your Name
- **Email**: your.email@example.com
- **GitHub**: [@yourusername](https://github.com/yourusername)

---

**Note**: Remember to replace all placeholder values (YOUR_*_HERE) with your actual configuration values before running the application.

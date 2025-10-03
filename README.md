# Gift of the Givers - Disaster Alleviation Foundation Web Application

## 📋 Project Overview

The **Gift of the Givers Web Application** is a comprehensive ASP.NET Core web platform designed to facilitate disaster relief operations, donation management, and volunteer coordination for the Disaster Alleviation Foundation. This system streamlines the process of managing donations, allocating resources, and coordinating volunteer efforts during emergency situations.

---

## 🎯 Purpose

This application serves as a centralized platform for:
- **Managing monetary and goods donations**
- **Coordinating disaster relief efforts**
- **Tracking inventory and allocations**
- **Volunteer management and task assignment**
- **Providing transparency in disaster relief operations**

---

## 🏗️ System Architecture

### Technology Stack
- **Backend**: ASP.NET Core 8.0 MVC
- **Frontend**: Razor Pages, Bootstrap, jQuery
- **Database**: Entity Framework Core with SQL Server
- **Authentication**: ASP.NET Core Identity
- **Deployment**: Azure DevOps compatible

---

## 📁 Project Structure

```
St10405518_GiftOfTheGiversWeb/
├── Areas/
│   └── Identity/                 # Authentication & Authorization
│       └── Pages/
│           ├── Account/
│           │   ├── Login.cshtml
│           │   ├── Register.cshtml
│           │   └── Logout.cshtml
├── Controllers/                  # MVC Controllers
│   ├── HomeController.cs
│   ├── DisastersController.cs
│   ├── GoodsDonationsController.cs
│   ├── MoneyDonationsController.cs
│   ├── VolunteerManagementController.cs
│   └── AdminVolunteerController.cs
├── Models/                       # Data Models
│   ├── Disaster.cs
│   ├── GoodsDonation.cs
│   ├── MoneyDonation.cs
│   ├── Volunteer.cs
│   ├── GoodsAllocation.cs
│   ├── MoneyAllocation.cs
│   └── ViewModels/               # View Models
├── Data/                         # Database Context
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs
├── Views/                        # Razor Views
│   ├── Home/
│   ├── Disasters/
│   ├── GoodsDonations/
│   ├── MoneyDonations/
│   └── Shared/
├── Migrations/                   # Database Migrations
├── wwwroot/                      # Static Files
│   ├── css/
│   ├── js/
│   ├── images/
│   └── lib/
└── Properties/                   # Configuration
```

---

## 🚀 Core Features

### 1. **User Management & Authentication**
- User registration and login system
- Role-based access control
- Secure password management
- User profile management

### 2. **Disaster Management**
- Create and manage disaster records
- Track disaster status and impact
- Monitor ongoing relief operations
- Historical disaster data

### 3. **Donation Management**
#### Monetary Donations
- Record cash donations
- Track donation sources
- Generate donation receipts
- Financial reporting

#### Goods Donations
- Inventory management for donated goods
- Categorization of donated items
- Stock level monitoring
- Goods tracking system

### 4. **Allocation System**
- Resource allocation to disasters
- Real-time inventory tracking
- Allocation history and reporting
- Resource utilization analytics

### 5. **Volunteer Management**
- Volunteer registration and profiling
- Skills and availability tracking
- Task assignment system
- Volunteer performance monitoring

### 6. **Administrative Dashboard**
- Real-time analytics and reporting
- System monitoring
- User management
- Configuration settings

---

## 🗃️ Database Models

### Core Entities:

1. **Disaster**
   - Disaster ID, Name, Description
   - Location, Start/End Date
   - Severity Level, Status
   - Affected Areas

2. **MoneyDonation**
   - Donation ID, Amount
   - Donor Information
   - Donation Date, IsAnonymous
   - Allocation Status

3. **GoodsDonation**
   - Donation ID, Item Description
   - Category, Quantity
   - Donor Information
   - Donation Date

4. **Volunteer**
   - Volunteer ID, Personal Details
   - Skills, Availability
   - Assignment History
   - Contact Information

5. **Allocation Records**
   - Allocation ID, Resource Type
   - Quantity Allocated, Disaster ID
   - Allocation Date, Status

---

## ⚙️ Installation & Setup

### Prerequisites
- .NET 8.0 SDK
- SQL Server 2019 or later
- Visual Studio 2022 or VS Code
- Git


### Admin Login Details:
Username: admin@giftofthegivers.org
password: Admin123!


### Installation Steps

1. **Clone the Repository**
   ```bash
   git clone https://dev.azure.com/ST10405518/GiftOfTheGivers-WebApp/_git/GiftOfTheGivers-WebApp
   cd St10405518_GiftOfTheGiversWeb
   ```

2. **Database Setup**
   ```bash
   # Update connection string in appsettings.json
   # Run database migrations
   dotnet ef database update
   ```

3. **Run the Application**
   ```bash
   dotnet run
   # or
   dotnet watch run
   ```

4. **Access the Application**
   - Open browser to: `https://localhost:7000`
   - Default admin credentials (if seeded)

### Configuration

Update `appsettings.json` with your environment settings:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=GiftOfTheGivers;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

---

## 🔧 Development

### Building the Project
```bash
dotnet build
```

### Running Tests
```bash
dotnet test
```

### Database Migrations
```bash
# Create new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

### Code First Approach
The application uses Entity Framework Code First approach with automatic migrations for database management.

---

## 🎨 User Interface

### Design Features
- **Responsive Design**: Bootstrap-based layout
- **Modern UI**: Clean and intuitive interface
- **Accessibility**: WCAG compliant components
- **Mobile-Friendly**: Optimized for all devices

### Key Pages
- **Home**: Dashboard and overview
- **Disasters**: Disaster management interface
- **Donations**: Donation recording and tracking
- **Volunteers**: Volunteer management portal
- **Reports**: Analytics and reporting

---

## 🔒 Security Features

- ASP.NET Core Identity for authentication
- Role-based authorization
- Secure password hashing
- CSRF protection
- XSS prevention
- SQL injection protection
- Secure headers configuration

---

## 📊 Reporting & Analytics

### Available Reports
- Donation summaries (monetary and goods)
- Disaster impact reports
- Volunteer activity reports
- Resource allocation summaries
- Financial tracking reports

---

## 🚀 Deployment

### Azure DevOps Pipeline
The project includes configuration for CI/CD through Azure DevOps.

### Manual Deployment
1. Build release version: `dotnet publish -c Release`
2. Deploy to IIS or Azure App Service
3. Configure production database
4. Update environment variables

---

## 🤝 Contributing

### Development Workflow
1. Fork the repository
2. Create feature branch: `git checkout -b feature/AmazingFeature`
3. Commit changes: `git commit -m 'Add AmazingFeature'`
4. Push to branch: `git push origin feature/AmazingFeature`
5. Open Pull Request

### Code Standards
- Follow ASP.NET Core best practices
- Use meaningful commit messages
- Include XML documentation for public APIs
- Write unit tests for new features

---

## 📝 License

This project is developed for educational purposes as part of academic coursework.

---

## 👥 Development Team

- **Student**: ST10405518
- **Institution**: IIE Varsity College
- **Course**: BSc in Computer and Information Sciences in Application Development

---

## 📞 Support & Contact

For technical support or questions regarding this application, please contact the development team through the institution's academic channels.

---

## 🔄 Version History

- **Version 1.0** (Initial Release)
  - Core donation management system
  - Basic disaster tracking
  - Volunteer management
  - Administrative dashboard

---

## 🎯 Future Enhancements

- [ ] Mobile application companion
- [ ] Real-time notifications
- [ ] Advanced analytics dashboard
- [ ] Integration with payment gateways
- [ ] Multi-language support
- [ ] API for third-party integrations
- [ ] Advanced reporting with data visualization
- [ ] Automated email notifications
- [ ] Document generation for receipts and reports

---

*Last Updated: October 2024*

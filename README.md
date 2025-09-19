# DVLD - Driving and Vehicle Licensing API

![.NET](https://img.shields.io/badge/.NET-8-blue.svg) ![EF Core](https://img.shields.io/badge/EF%20Core-8-lightgrey.svg) ![MS SQL Server](https://img.shields.io/badge/MS%20SQL%20Server-DB-red.svg) ![JWT](https://img.shields.io/badge/Authentication-JWT-orange.svg)

A comprehensive and secure RESTful API for a Driving and Vehicle License Department (DVLD) system. This project provides a robust backend solution for managing the entire lifecycle of driver licensing, from initial application to renewal and administrative actions. It is built with a focus on security, performance, and maintainability using modern .NET technologies.

## 🚀 Live Demo

You can view a live demo of the front-end application that consumes this API here:
[https://full-stack-website-react-asp-net-eight.vercel.app/user/userLicense](https://full-stack-website-react-asp-net-eight.vercel.app/user/userLicense)

---
## About The Project

This API is designed to handle all core operations of a driver licensing authority. It manages personal records, license applications, various types of driving tests, and different license classes, all while ensuring data integrity and secure access. The system is based on the detailed requirements for a DVLD project provided by Programming Advices.

### Core Services 📜

* **People Management:** Full CRUD (Create, Read, Update, Delete) operations for individuals in the system, ensuring no duplicate national IDs exist.
* **Application Management:** Handles various application types, including:
    * New Driving License (First Time)
    * License Renewal
    * Replacement for Lost or Damaged License
    * International License Issuance
* **Multi-Class Licensing:** Supports a wide range of license categories, such as motorcycles, personal cars, commercial vehicles, buses, and trucks, each with its own fees, validity period, and age requirements.
* **Test & Appointment System:** Manages the three-step testing process (Vision, Written, and Practical), including appointment scheduling, fee collection, and result tracking.
* **License Management:** Complete control over licenses, including issuance, renewal, detainment, and release functionalities.
* **User Administration:** Securely manage system users, their personal information, and access permissions (RBAC).

### Technical Features 🚀

* **Secure Authentication:** Implemented using **JSON Web Tokens (JWT)** with support for **Role-Based Access Control (RBAC)** and **refresh tokens** to ensure secure and persistent user sessions.
* **Robust Architecture:** Utilizes the **Repository and Unit of Work patterns** for a clean, decoupled data access layer with **Entity Framework Core**.
* **Performance Optimized:** Includes response **caching** to reduce database load, **rate limiting** to prevent API abuse, and **health check endpoints** for easy monitoring and container orchestration.
* **Integrated Notifications:** Employs **SendGrid** for sending transactional emails and **Twilio** for critical SMS alerts to users.
* **Structured Logging:** Uses **Serilog** for detailed and structured application logging, enabling efficient diagnostics and monitoring.

---

## Technologies Used

* **Framework:** ASP.NET Core 8
* **Database:** Microsoft SQL Server
* **ORM:** Entity Framework Core 8
* **Authentication:** JWT (JSON Web Tokens)
* **Logging:** Serilog
* **Email Service:** SendGrid
* **SMS Service:** Twilio

---

## Getting Started

To get a local copy up and running, follow these simple steps.

### Prerequisites

* .NET 8 SDK
* Microsoft SQL Server
* A code editor like Visual Studio or VS Code

### Installation

1.  **Clone the repo**
    ```sh
    git clone [https://github.com/your_username/your_project_name.git](https://github.com/your_username/your_project_name.git)
    ```
2.  **Navigate to the project directory**
    ```sh
    cd your_project_name
    ```
3.  **Configure User Secrets or `appsettings.json`**
    Update your `appsettings.Development.json` or use .NET User Secrets to provide the necessary configuration values:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Your_SQL_Server_Connection_String"
      },
      "JwtSettings": {
        "Key": "Your_Super_Secret_Key_For_JWT",
        "Issuer": "Your_API_Issuer",
        "Audience": "Your_API_Audience",
        "DurationInMinutes": 60
      },
      "SendGrid": {
        "ApiKey": "Your_SendGrid_API_Key"
      },
      "Twilio": {
        "AccountSID": "Your_Twilio_Account_SID",
        "AuthToken": "Your_Twilio_Auth_Token"
      }
    }
    ```
4.  **Apply Database Migrations**
    Run the following command to create and seed the database.
    ```sh
    dotnet ef database update
    ```
5.  **Run the application**
    ```sh
    dotnet run
    ```
The API will be available at `https://localhost:5001` or `http://localhost:5000`.

---

## API Endpoints Overview

Here is a complete list of the available API endpoints, grouped by functionality.

### Account

| Method | Endpoint                                               | Description                                        |
| :----- | :----------------------------------------------------- | :------------------------------------------------- |
| `POST` | `/api/Account/Register`                                | Register a new user.                               |
| `POST` | `/api/Account/Login`                                   | Log in to get a JWT.                               |
| `POST` | `/api/Account/RegisterWithRefreshToken`                | Register a new user and get a refresh token.       |
| `POST` | `/api/Account/LoginWithRefreshToken`                   | Log in and get a refresh token.                    |
| `POST` | `/api/Account/RefreshToken`                            | Get a new access token using a refresh token.      |
| `POST` | `/api/Account/RevokeToken`                             | Revoke a user's refresh token.                     |
| `POST` | `/api/Account/RegisterWithEmailConfirmation`           | Register a new user with email confirmation.       |
| `GET`  | `/api/Account/VerifyEmail`                             | Verify a user's email address.                     |
| `POST` | `/api/Account/LoginWithEmailConfirmation`              | Log in for an email-verified account.              |
| `POST` | `/api/Account/LoginWithEmailConfirmationWithRefreshToken` | Log in with email verification and get refresh token. |
| `POST` | `/api/Account/ForgotPassword`                          | Initiate the password reset process.               |
| `POST` | `/api/Account/ResetPassword`                           | Reset a user's password.                           |

### Applicants

| Method   | Endpoint                                        | Description                                     |
| :------- | :---------------------------------------------- | :---------------------------------------------- |
| `GET`    | `/api/Applicants/GetAllApplicants`              | Get a list of all applicants.                   |
| `POST`   | `/api/Applicants`                               | Create a new applicant.                         |
| `DELETE` | `/api/Applicants/{id}`                          | Delete an applicant by their ID.                |
| `PUT`    | `/api/Applicants/{id}`                          | Update an applicant by their ID.                |
| `DELETE` | `/api/Applicants/by-nationalno/{nationalNo}`    | Delete an applicant by their National No.       |
| `GET`    | `/api/Applicants/GetPaged`                      | Get a paginated list of applicants.             |
| `GET`    | `/api/Applicants/GetApplicantByApplicantId/{id}`| Get an applicant by their ID.                   |
| `GET`    | `/api/Applicants/GetApplicantByNationalNo/{nationalNo}` | Get an applicant by their National No.      |
| `GET`    | `/api/Applicants/GetApplicantByUserId/{userId}` | Get an applicant by their User ID.              |
| `GET`    | `/api/Applicants/{id}/fullname`                 | Get the full name of an applicant by ID.        |
| `GET`    | `/api/Applicants/GetUserIdByApplicantId/{id}`   | Get User ID from Applicant ID.                  |
| `GET`    | `/api/Applicants/GetUserIdByNationalNo/{nationalNo}` | Get User ID from National No.               |
| `GET`    | `/api/Applicants/IsNationalNoExist/{nationalNo}`| Check if a National No already exists.          |
| `GET`    | `/api/Applicants/{id}/details`                  | Get detailed information for an applicant by ID.|
| `GET`    | `/api/Applicants/details/by-nationalno/{nationalNo}` | Get detailed info for an applicant by National No.|
| `GET`    | `/api/Applicants/GetApplicantIdByUserId/{userId}` | Get Applicant ID from User ID.                  |
| `GET`    | `/api/Applicants/GetUserProfile/{applicantId}`  | Get a user's profile by their Applicant ID.     |
| `PUT`    | `/api/Applicants/UpdateUserProfile/{id}`        | Update a user's profile.                        |

### Applications

| Method   | Endpoint                                                              | Description                                                      |
| :------- | :-------------------------------------------------------------------- | :--------------------------------------------------------------- |
| `GET`    | `/api/Applications`                                                   | Get all applications.                                            |
| `GET`    | `/api/Applications/ApplicantApplicationByNationalNo`                  | Get applications for an applicant by National No.                |
| `GET`    | `/api/Applications/ApplicantApplicationById/{applicantId}`            | Get applications for an applicant by Applicant ID.               |
| `GET`    | `/api/Applications/GetApplicationById/{id}`                           | Get a specific application by its ID.                            |
| `GET`    | `/api/Applications/status`                                            | Get applications filtered by status.                             |
| `DELETE` | `/api/Applications/{id}`                                              | Delete an application by its ID.                                 |
| `PUT`    | `/api/Applications/{id}`                                              | Update an application by its ID.                                 |
| `PUT`    | `/api/Applications/Accept/{applicationId}`                            | Mark an application as accepted.                                 |
| `PUT`    | `/api/Applications/Reject/{applicationId}`                            | Mark an application as rejected.                                 |
| `GET`    | `/api/Applications/GetAllApplicationTypes`                            | Get all available application types.                             |
| `GET`    | `/api/Applications/GetApplicationTypeById/{id}`                       | Get a specific application type by its ID.                       |
| `PUT`    | `/api/Applications/UpdateApplicationType/{id}`                        | Update an application type.                                      |
| `POST`   | `/api/Applications/ApplyForNewLocalDrivingLicense`                    | Submit a new application for a local driving license.            |
| `GET`    | `/api/Applications/GetAllLocalAppsLicense`                            | Get all local driving license applications.                      |
| `GET`    | `/api/Applications/GetLocalAppLicenseByIdAsync/{id}`                  | Get a local license application by its ID.                       |
| `GET`    | `/api/Applications/GetAllLocalAppLicensesByNationalNoAsync`           | Get all local license applications by National No.               |
| `GET`    | `/api/Applications/GetAllLocalAppLicensesByApplicantIdAsync/{applicantId}` | Get all local license applications by Applicant ID.             |
| `POST`   | `/api/Applications/ScheduleVisionTest`                                | Schedule a vision test for an application.                       |
| `POST`   | `/api/Applications/ApplyForNewInternationalLicenseApplication/{applicantId}` | Apply for a new international license.                       |
| `GET`    | `/api/Applications/GetAllInternationalLicenseApps`                    | Get all international license applications.                      |
| `GET`    | `/api/Applications/GetInternationalLicenseAppByIdAsync/{id}`          | Get an international license application by ID.                  |
| `GET`    | `/api/Applications/GetAllInternationalLicenseAppsWithsByNationalNoAsync` | Get all international license applications by National No.   |
| `GET`    | `/api/Applications/GetAllInternationalLicenseAppByApplicantIdAsync/{applicantId}` | Get all international license applications by Applicant ID.|
| `POST`   | `/api/Applications/ApplyForRenewLicenseApplicantion/{licenseId}`      | Apply to renew an existing license.                              |
| `GET`    | `/api/Applications/GetAllRenewLicenseApps`                            | Get all license renewal applications.                            |
| `POST`   | `/api/Applications/ApplyForReplacementDamagedLicenseApplicationAsync/{licenseId}` | Apply to replace a damaged license.                      |
| `POST`   | `/api/Applications/ApplyForReplacementLostLicenseApplicationAsync/{licenseId}` | Apply to replace a lost license.                         |
| `POST`   | `/api/Applications/ApplyForReleaseLicenseApplication/{licenseId}`     | Apply to release a detained license.                             |


### Drivers

| Method | Endpoint                                        | Description                             |
| :----- | :---------------------------------------------- | :-------------------------------------- |
| `GET`  | `/Api/Drivers`                                  | Get all drivers.                        |
| `POST` | `/Api/Drivers`                                  | Create a new driver from an applicant.  |
| `GET`  | `/Api/Drivers/{id}`                             | Get a driver by their ID.               |
| `GET`  | `/Api/Drivers/GetDriverByApplicantId/{applicantId}` | Get a driver by their Applicant ID.     |
| `GET`  | `/Api/Drivers/GetDriverByApplicantNationalNo`   | Get a driver by their National No.      |
| `GET`  | `/Api/Drivers/IsApplicantDriver/{applicantId}`  | Check if an applicant is already a driver. |

### Licenses

| Method   | Endpoint                                                  | Description                                  |
| :------- | :-------------------------------------------------------- | :------------------------------------------- |
| `GET`    | `/Api/Licenses/GetAllLicenses`                            | Get all licenses.                            |
| `POST`   | `/Api/Licenses/IssueLicenseFirstTime`                     | Issue a license for the first time.          |
| `GET`    | `/Api/Licenses/GetLicensesByApplicantId/{applicantId}`    | Get licenses by Applicant ID.                |
| `GET`    | `/Api/Licenses/GetAllLicenseClasses`                      | Get all available license classes.           |
| `GET`    | `/Api/Licenses/GetLicensesByDriverId/{driverId}`          | Get licenses by Driver ID.                   |
| `GET`    | `/Api/Licenses/GetLicenseByLicenseId/{licenseId}`         | Get a license by its ID.                     |
| `GET`    | `/Api/Licenses/GetLicensesByNationalNo`                   | Get licenses by National No.                 |
| `POST`   | `/Api/Licenses/IssueInternationalLicesnse`                | Issue a new international license.           |
| `GET`    | `/Api/Licenses/GetInternationalLicensesByApplicantId/{applicantId}` | Get international licenses by Applicant ID. |
| `POST`   | `/Api/Licenses/RenewLicense`                              | Renew an existing license.                   |
| `POST`   | `/Api/Licenses/ReplaceForDamagedLicense`                  | Replace a damaged license.                   |
| `POST`   | `/Api/Licenses/ReplaceForLostLicense`                     | Replace a lost license.                      |
| `POST`   | `/Api/Licenses/DetainLicense`                             | Detain a license.                            |
| `GET`    | `/Api/Licenses/GetAllDetainedLicenses`                    | Get all detained licenses.                   |
| `POST`   | `/Api/Licenses/ReleaseLicense/{applicationId}`            | Release a detained license.                  |

### Tests

| Method | Endpoint                                                | Description                                    |
| :----- | :------------------------------------------------------ | :--------------------------------------------- |
| `GET`  | `/api/Tests`                                            | Get all tests.                                 |
| `GET`  | `/api/Tests/GetAllTestAppoinmentByApplicantId/{applicantId}` | Get all test appointments for an applicant. |
| `GET`  | `/api/Tests/{Id}`                                       | Get a test appointment by its ID.              |
| `POST` | `/api/Tests/ScheduleWrittenTest`                        | Schedule a written test for an application.    |
| `POST` | `/api/Tests/SchedulePracticalTest`                      | Schedule a practical test for an application.  |
| `PUT`  | `/api/Tests/UpdateTestAppointment`                      | Update a test appointment.                     |
| `POST` | `/api/Tests/TakeTest`                                   | Record the result of a completed test.         |
| `GET`  | `/api/Tests/GetTestResultByTestAppoinmentId/{TestAppoinmentId}` | Get the result of a specific test.      |


### Users

| Method   | Endpoint                          | Description                             |
| :------- | :-------------------------------- | :-------------------------------------- |
| `GET`    | `/Api/Users/GetAllUsers`          | Get all users.                          |
| `GET`    | `/Api/Users/GetUser/{userID}`     | Get a user by their ID.                 |
| `GET`    | `/Api/Users/GetUserByEmail/{email}` | Get a user by their email address.      |
| `GET`    | `/Api/Users/GetRolesOfUser/{userId}` | Get the roles for a specific user.     |
| `POST`   | `/Api/Users/LockUnLock/{id}`      | Lock or unlock a user account.          |
| `POST`   | `/Api/Users/AddUser`              | Add a new user.                         |
| `PUT`    | `/Api/Users/UpdateUser`           | Update an existing user.                |
| `DELETE` | `/Api/Users/DeleteUser/{userID}`  | Delete a user by their ID.              |
| `GET`    | `/Api/Users/GetRolesForManaging/{userId}` | Get roles for managing a user.  |
| `POST`   | `/Api/Users/ManageUserRoles`      | Add or remove roles for a user.         |

---

## Acknowledgments

* This project's requirements and business logic are based on the **"Full Project in C# - Project 1"** document by **Mohammed Abu-Hadhoud** from **ProgrammingAdvices.com**.

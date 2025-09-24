# 🛒 MyEcomApp - E-Commerce Application

## 📌 Overview
This project is a **simple E-Commerce system** built with:
- **Backend:** .NET Core Web API  
- **Frontend:** Angular (with Atomic design structure)  
- **Database:** Microsoft SQL Server (Entity Framework Core + Stored Procedures)  
- **Authentication:** JWT with Refresh Token mechanism  
- **Testing:** Unit tests (xUnit), Integration tests, and Jest for frontend  

---

## ⚙️ Backend (MyEcomApi)

### ✅ Features
- **Architecture:** Clean MVC with layered structure (Entities, DbContext, Repositories, Services, Controllers).
- **Entities:**
  - **Product**
    - Category  
    - Product Code (Unique, e.g., `P01`, `P02`, ...)  
    - Name  
    - Image (stored in local storage, `/wwwroot/uploads/`)  
    - Price (decimal)  
    - Minimum Quantity  
    - Discount Rate (%)  
  - **User**
    - Username (Unique)  
    - Password (Hashed)  
    - Email (Unique)  
    - Last Login Time  

- **Authentication:**
  - JWT authentication for all API endpoints.  
  - Refresh token mechanism for expired tokens.  

- **Database:**
  - **SQL Server** with EF Core migrations.  
  - Stored procedures for selected operations.  
  - Provided `.bak` file and SQL script for setup.  

- **Testing:**
  - Unit tests for services and repositories.  
  - Integration tests for controllers and database.  

---

## 🎨 Frontend (MyEcomApp - Angular)

### ✅ Features
- **Architecture:** Atomic design pattern (auth, core, product, shared modules).  
- **Authentication:**
  - Login & Register forms with JWT authentication.  
  - Token stored in `localStorage`.  
- **Product Module:**
  - Product List with responsive design.  
  - Product Card component (image, title, description, price, rating, add-to-cart).  
- **Shared Components:**
  - Navbar  
  - Footer  

### 📂 Project Structure
```

src/app/
├─ auth/
│   ├─ login/
│   ├─ register/
│   ├─ auth-routing.module.ts
│   └─ auth.module.ts
├─ core/
│   ├─ auth.ts / product.ts
│   ├─ core.module.ts
├─ product/
│   ├─ product-card/
│   └─ product-list/
├─ shared/
│   ├─ navbar/
│   ├─ footer/
│   └─ shared.module.ts
├─ app.routes.ts
└─ main.ts

````

- **State Management:** RxJS observables for handling auth state and product data.  
- **Styling:** SCSS global styles + component-based CSS/SCSS.  

---

## 🧪 Testing

### Backend
- Unit tests with **xUnit**  
- Integration tests with in-memory DB  

### Frontend
- Component testing with **Jest**  
---

## 🚀 Getting Started

### 1️⃣ Backend
```bash
cd MyEcomApi
dotnet restore
dotnet ef database update   # apply migrations
dotnet run
````

Backend will run on:

* `http://localhost:5015` (HTTP)
* `https://localhost:7030` (HTTPS)

### 2️⃣ Frontend

```bash
cd MyEcomApp
npm install
ng serve -o
```

Frontend will run on:

* `http://localhost:4200`

---

## 📦 Database

* File: `MyEcomDb.bak` (SQL Server backup)
---

## 📚 Technologies Used

* **Backend:** .NET 8, EF Core, SQL Server, xUnit
* **Frontend:** Angular 17, RxJS, SCSS, Jest, Cypress
* **Auth:** JWT, Refresh Tokens

---

## ✨ Future Improvements

* Shopping cart & checkout.
* Admin panel for product management.
* Advanced search and filtering.

---

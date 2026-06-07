# GrpcCrudSample - .NET 9 gRPC CRUD Sample
 پروژه نمونه برای پیاده‌سازی CRUD با استفاده از *gRPC* و *Protocol Buffers* در *ASP.NET Core 9* است.

# مدل
- Id
- Name
- LastName
- PersonalCode
- BirthDay

# ساختار پروژه
- `GrpcCrudSample.Api` : سرویس gRPC
- `GrpcCrudSample.Client` : کنسول کلاینت برای تست متدهای CRUD

# تکنولوژی‌ها
- .NET 9
- ASP.NET Core gRPC
- Protocol Buffers
- In-Memory Repository

# متدهای gRPC
- `GetAll`
- `GetById`
- `Create`
- `Update`
- `Delete`

# نحوه اجرا

# 1) اجرای سرویس
dotnet restore
dotnet build
dotnet run --project GrpcCrudSample.Api
```

سرویس روی آدرس زیر اجرا می‌شود:
- `https://localhost:7243`

## 2) اجرای کلاینت
در یک ترمینال جدید:

dotnet run --project GrpcCrudSample.Client
```

# # نکات مهم زمان اجرا
- از `Protocol Buffers` به‌جای REST استفاده شده است.
- ساختار پروژه تمیز و قابل توسعه است.
- اعتبارسنجی اولیه برای ورودی‌ها انجام شده است.
- برای سادگی از `InMemory Repository` استفاده شده، اما به‌راحتی می‌توان آن را با EF Core/SQL Server جایگزین کرد.

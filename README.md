# قالب Modular Monolith مع Domain-Driven Design (DDD) في ASP.NET Core (.NET 9)

قالب احترافي عالي الجودة (Enterprise-Grade Template) مبني وفق معمارية **المونوليث المجزأ (Modular Monolith)** ومبادئ **التصميم الموجه بالمجال (Domain-Driven Design - DDD)** و **Clean Architecture** في بيئة **.NET 9**.

---

## 🌟 أبرز مميزات القالب

1. **معمارية Modular Monolith حقيقية**:
   - تقسيم النظام إلى سياقات محددة (Bounded Contexts) منفصلة بالكامل (`Users`, `Orders`).
   - كل موديول يمتلك طبقاته الخاصة (`Domain`, `Application`, `Infrastructure`, `Presentation`).
   - منع الوصول المباشر إلى بيانات أو كيانات موديول آخر.

2. **تواصل مفصول بين الموديولات (Decoupled Inter-Module Communication)**:
   - التواصل متزامن عبر **العقود العامة فقط (`Module.Contracts`)**.
   - التواصل غير المتزامن عبر **أحداث التكامل (`Integration Events`)** وناقل أحداث داخلي خفيف (`IEventBus`) دون أي ترابط مباشر.

3. **تطبيق صارم لمبادئ DDD و Clean Architecture**:
   - **Rich Domain Models**: كبسلة كاملة للبيانات ومنع النماذج الضعيفة (Anemic Models)، مع استخدام دوال المصنع (`Factory Methods`) والمحددات الخاصة (`private setters`).
   - **Strongly-Typed IDs**: منع هوس المتغيرات البدائية (`UserId`, `OrderId`, `CustomerId`).
   - **Value Objects**: كائنات قيم ذات سلوك وتحقق ذاتي ومقارنة بالقيمة (`Money`, `Address`, `Email`, `FirstName`, `LastName`).
   - **Domain Events**: إدارة الأحداث داخل النطاق وتفريغها تلقائياً عند حفظ التغييرات عبر `DomainEventsDispatcherInterceptor`.
   - **Result Pattern**: نمط النتيجة (`Result`, `Result<T>`, `Error`) للتعامل مع نتائج الأعمال دون إطلاق استثناءات تؤثر على الأداء (`throw new Exception`).

4. **طبقة بنية تحتية مرنة وقابلة للتوسع**:
   - عزل قواعد البيانات داخل نفس السيرفر عبر مخططات مستقلة (`schemas: users, orders`).
   - تتبع تلقائي للتدقيق (`AuditableEntityInterceptor`: CreatedAtUtc, CreatedBy, LastModifiedAtUtc).
   - دعم كامل للحذف اللطيف (`ISoftDeletable`).
   - معالجة مركزية للأخطاء متوافقة مع معيار RFC 7807 (`ProblemDetails`).

5. **خطوط أنابيب CQRS (MediatR Behaviors)**:
   - `ValidationBehavior`: تدقيق آلي للمدخلات باستخدام FluentValidation وجمع كافة الأخطاء.
   - `LoggingBehavior`: تسجيل احترافي للأوامر والاستعلامات وقياس زمن التنفيذ وتحديد العمليات البطيئة.

6. **اختبارات المعمارية الصارمة (Architecture Tests)**:
   - فحص آلي باستخدام `NetArchTest.Rules` يضمن عدم قيام أي موديول باختراق حدود موديول آخر، والتأكد من نقاء طبقة الـ `Domain`.

---

## 📂 هيكلة المجلدات والمشاريع (Solution Structure)

```text
DDDTemplate/
├── src/
│   ├── BuildingBlocks/                     # اللبنات الأساسية المشتركة الخالية من منطق الأعمال
│   │   ├── BuildingBlocks.Domain/          # Entity, AggregateRoot, ValueObject, StronglyTypedId, Result
│   │   ├── BuildingBlocks.Application/     # CQRS (ICommand, IQuery), Behaviors, EventBus Interfaces
│   │   ├── BuildingBlocks.Infrastructure/  # Interceptors, In-Memory EventBus, Specifications
│   │   └── BuildingBlocks.Presentation/    # ApiController Base, GlobalExceptionHandler (ProblemDetails)
│   │
│   ├── Modules/                            # الموديولات الوظيفية (Bounded Contexts)
│   │   ├── Users/
│   │   │   ├── Users.Contracts/            # عقود الموديول العامة وأحداث التكامل (مثل UserRegisteredIntegrationEvent)
│   │   │   ├── Users.Domain/               # User Aggregate Root, Email, FirstName, LastName, UserRegisteredDomainEvent
│   │   │   ├── Users.Application/          # RegisterUser, GetUserById (Commands, Queries, Validators)
│   │   │   ├── Users.Infrastructure/       # UsersDbContext (Schema: users), UserRepository, EF Configurations
│   │   │   └── Users.Presentation/         # UsersController (REST API Endpoints)
│   │   │
│   │   └── Orders/
│   │       ├── Orders.Contracts/           # عقود موديول الطلبات وأحداث التكامل
│   │       ├── Orders.Domain/              # Order, OrderItem, Customer, Money, Address, OrderStatus
│   │       ├── Orders.Application/         # CreateOrder, CancelOrder, GetOrderById + UserRegisteredIntegrationEventHandler
│   │       ├── Orders.Infrastructure/      # OrdersDbContext (Schema: orders), OrderRepository, EF Configurations
│   │       └── Orders.Presentation/        # OrdersController (REST API Endpoints)
│   │
│   └── Bootstrapper/
│       └── DDDTemplate.Api/                # نقطة الانطلاق الرئيسية (Host) وتشغيل السيرفر و Swagger و Serilog
│
└── tests/
    ├── ArchitectureTests/                  # اختبارات عزل الموديولات وقواعد Clean Architecture
    ├── BuildingBlocks.UnitTests/           # اختبارات الـ Result Pattern و الـ Value Objects
    ├── Modules/
    │   ├── Users.UnitTests/                # اختبارات نطاق وقواعد أعمال موديول المستخدمين
    │   └── Orders.UnitTests/               # اختبارات نطاق وقواعد أعمال موديول الطلبات
```

---

## 🚀 كيفية التشغيل (How to Run)

### 1. تشغيل الاختبارات الآلية (Architecture & Unit Tests)
```bash
dotnet test DDDTemplate.slnx --logger "console;verbosity=normal"
```

### 2. تشغيل الـ Web API
```bash
dotnet run --project src/Bootstrapper/DDDTemplate.Api/DDDTemplate.Api.csproj
```
بعد التشغيل، افتح المتصفح على:
- توثيق الـ API (Swagger): `https://localhost:5001/swagger` أو `http://localhost:5000/swagger`
- فحص الحالة (Health Check): `http://localhost:5000/health`

---

## 🔄 كيفية إضافة موديول جديد (Adding a New Module)

لإضافة سياق عمل جديد (مثال: `Invoicing` أو `Notifications`) في 4 خطوات بسيطة:

1. **إنشاء مجلد الموديول ومجاريعه**:
   - `Invoicing.Contracts` (يحوي DTOs وأحداث التكامل العامة).
   - `Invoicing.Domain` (يرث من `BuildingBlocks.Domain`).
   - `Invoicing.Application` (يرث من `BuildingBlocks.Application` و `Invoicing.Domain`).
   - `Invoicing.Infrastructure` (يرث من `BuildingBlocks.Infrastructure` و `Invoicing.Application`).
   - `Invoicing.Presentation` (يرث من `BuildingBlocks.Presentation` و `Invoicing.Application`).

2. **عزل قاعدة البيانات**:
   - في `InvoicingDbContext`، اضبط الـ Schema الخاص بالموديول:
     ```csharp
     modelBuilder.HasDefaultSchema("invoicing");
     ```

3. **التواصل مع الموديولات الأخرى**:
   - استمع لأحداث الموديولات الأخرى بتنفيذ:
     ```csharp
     public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedIntegrationEvent>
     ```
   - إذا رغب موديول آخر بمعرفة أحداثك، انشرها عبر `IEventBus.PublishAsync(...)`.

4. **التسجيل في الـ Bootstrapper (`Program.cs`)**:
   ```csharp
   builder.Services.AddInvoicingPresentation();
   builder.Services.AddInvoicingInfrastructure(builder.Configuration);
   ```

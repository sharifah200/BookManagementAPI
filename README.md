# Book Management API

واجهة برمجة تطبيقات RESTful لإدارة مجموعة من الكتب باستخدام ASP.NET Core

## المميزات

- ✅ إدارة كاملة للكتب (إنشاء، قراءة، تحديث، حذف)
- ✅ نظام مصادقة وتفويض باستخدام JWT
- ✅ تقسيم الصفحات والبحث
- ✅ توثيق API باستخدام Swagger
- ✅ معمارية نظيفة مع Repository Pattern
- ✅ قاعدة بيانات SQLite
- ✅ تسجيل مفصل للأحداث
- ✅ دعم CORS

## متطلبات التشغيل

- .NET 8.0 SDK
- Visual Studio 2022 / VS Code

## كيفية التشغيل

1. **استنساخ المشروع**
   ```bash
   git clone <repository-url>
   cd BookManagementAPI
   ```

2. **استعادة الحزم**
   ```bash
   dotnet restore
   ```

3. **تشغيل المشروع**
   ```bash
   dotnet run
   ```

4. **الوصول إلى Swagger**
   - افتح المتصفح واذهب إلى: `https://localhost:5001`

## نقاط النهاية (API Endpoints)

### المصادقة
- `POST /api/auth/register` - تسجيل مستخدم جديد
- `POST /api/auth/login` - تسجيل الدخول

### الكتب
- `GET /api/books` - جلب جميع الكتب (مع تقسيم الصفحات)
- `GET /api/books/{id}` - جلب كتاب محدد
- `POST /api/books` - إضافة كتاب جديد (يتطلب مصادقة)
- `PUT /api/books/{id}` - تحديث كتاب (يتطلب مصادقة)
- `DELETE /api/books/{id}` - حذف كتاب (يتطلب مصادقة)
- `GET /api/books/search` - البحث في الكتب
- `GET /api/books/author/{author}` - جلب كتب مؤلف محدد

## المعمارية

```
BookManagementAPI/
├── Controllers/          # تحكمات API
├── Models/              # نماذج البيانات
├── DTOs/                # كائنات نقل البيانات
├── Data/                # سياق قاعدة البيانات
├── Repositories/        # طبقة الوصول للبيانات
└── Program.cs           # نقطة دخول التطبيق
```

## أمثلة الاستخدام

### تسجيل مستخدم جديد
```json
POST /api/auth/register
{
  "userName": "testuser",
  "email": "test@example.com",
  "password": "Test123!"
}
```

### تسجيل الدخول
```json
POST /api/auth/login
{
  "userName": "testuser",
  "password": "Test123!"
}
```

### إضافة كتاب جديد
```json
POST /api/books
Authorization: Bearer <your-jwt-token>
{
  "title": "The Great Gatsby",
  "author": "F. Scott Fitzgerald",
  "publishedDate": "1925-04-10",
  "numberOfPages": 180
}
```

### جلب الكتب مع التقسيم
```
GET /api/books?pageNumber=1&pageSize=10&searchTerm=gatsby
```

## الإعدادات

تأكد من تحديث `appsettings.json` بالإعدادات المناسبة:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=books.db"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!",
    "Issuer": "BookManagementAPI",
    "Audience": "BookManagementAPI"
  }
}
```

## التطوير

للمساهمة في المشروع:

1. أنشئ فرع جديد للميزة
2. اكتب الكود مع التوثيق المناسب
3. اختبر التغييرات
4. أرسل Pull Request

## الترخيص

هذا المشروع مُرخص تحت رخصة MIT.

# BookManagementAPI

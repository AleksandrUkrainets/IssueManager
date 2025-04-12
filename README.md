# 🛠️ IssueManager

**IssueManager** — это веб-приложение, реализованное на ASP.NET Core с использованием Onion архитектуры, предназначенное для управления **issues** (проблемами, запросами, предложениями) в репозиториях GitHub, GitLab и Bitbucket. Пользователи могут авторизоваться через OAuth2, а затем выполнять операции с issues в своих репозиториях через унифицированный API.

---

## 📌 Основные возможности

- OAuth2-аутентификация через GitHub и GitLab
- Получение JWT токена на основе access_token провайдеров
- Управление issues (создание, обновление, удаление) в GitHub и GitLab
- Поддержка хранения зашифрованных токенов в PostgreSQL
- Использование AutoMapper и Refit для интеграции с внешними API
- Логгирование через Serilog
- Swagger UI с авторизацией через JWT

---

## 🧱 Архитектура

Проект следует **Onion Architecture** и разделён на следующие слои:

- **Presentation** — Web API, Middleware, контроллеры
- **Application** — бизнес-логика, DTO, интерфейсы сервисов
- **Domain** — бизнес-модели и интерфейсы провайдеров
- **Infrastructure** — реализация API-провайдеров (GitHub, GitLab), сервисы
- **Persistence** — реализация доступа к данным (EF Core, PostgreSQL)

---

## 🚀 Установка и запуск

### 🔧 Требования

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL (локально или в Railway.app)
- Visual Studio или Rider (или любой другой IDE)
- GitHub OAuth App (client_id и client_secret)
- GitLab OAuth App (опционально)

### 🧪 Локальный запуск

1. **Клонируйте репозиторий:**
   ```bash
   git clone https://github.com/your-user/IssueManager.git
   cd IssueManager
   ```

2. **Настройте переменные окружения:**
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "Jwt:Key" "<your-jwt-secret-key>"
   dotnet user-secrets set "OAuth:Providers:github:ClientId" "<your-client-id>"
   dotnet user-secrets set "OAuth:Providers:github:ClientSecret" "<your-client-secret>"
   dotnet user-secrets set "ConnectionStrings:DefaultConnectionString" "Host=localhost;Database=IssueManagerDb;Username=postgres;Password=yourpassword"
   ```

3. **Примените миграции:**
   ```bash
   dotnet ef database update --project IssueManager.Persistance
   ```

4. **Запустите приложение:**
   ```bash
   dotnet run --project IssueManager.Presentation
   ```

---

## 📬 Эндпоинты API

> Все эндпоинты защищены JWT. Авторизуйтесь через `/api/oauth/signIn` и используйте полученный токен в `Authorization: Bearer {token}`

### 🔐 OAuth

| Метод | Путь                         | Описание                        |
|-------|------------------------------|----------------------------------|
| GET   | `/api/oauth/authUrl?provider=github` | Получить ссылку на авторизацию |
| GET   | `/api/oauth/signIn?code=...&state=github` | Завершить OAuth и получить JWT |

---

### 🐛 Работа с issues

> Требуется заголовок `Authorization: Bearer {JWT}`

| Метод | Путь                  | Описание                   |
|-------|------------------------|-----------------------------|
| POST  | `/api/issues/create`   | Создать issue             |
| PUT   | `/api/issues/update`   | Обновить issue            |
| DELETE| `/api/issues/delete`   | Удалить issue             |

Пример запроса:
```json
POST /api/issues/create
{
  "repo": "username/repo",
  "title": "Bug found",
  "body": "Steps to reproduce..."
}
```

---

## 🔍 Swagger

Swagger UI доступен по адресу:

- `https://localhost:5001/swagger`

Поддерживает JWT авторизацию.

---

## 📝 Логгирование

Вся активность пользователей и ошибки логируются в файлы:
- `Logs/log-YYYY-MM-DD.txt`
- Серилог конфигурирован в `Program.cs`

---

## 📦 Технологии

- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL
- AutoMapper
- Refit
- Serilog
- Swagger (Swashbuckle)

---

## 🧩 Планы на будущее

- Поддержка Bitbucket
- UI-интерфейс на Blazor или React
- Поддержка меток, комментариев, pull-request'ов

---

## 🤝 Лицензия

Проект распространяется под лицензией MIT. См. [LICENSE](./LICENSE) для подробностей.

# AspNet.IdentityJwtRefreshExample

# Описание
Пример реализации аутентификации и авторизации с использованием ASP.NET Core Identity + JWT (Access Token + Refresh Token).

В проекте используется аутентификация через JWT токен с использованием Refresh Token.

* Реализовано:
- [x] Регистрация пользователя
- [x] Логин пользователя
- [x] Генерация Access Token
- [x] Генерация Refresh Token
- [x] Ротация Refresh Token
- [x] Обновление пары токенов (Access + Refresh)
- [x] Logout через revoke Refresh Token
- [x] Авторизация по ролям через [Authorize(Roles = "")]
- [x] Эндпоинт получения Claims текущего пользователя
- [x] Работа JWT в Swagger

Identity используется для:

* создания пользователей
* хеширования пароля
* управления ролями

JWT используется:
* Access Token - короткоживущий токен (1 минута)
* Refresh Token - для получения новой пары токенов (3 минуты)

Refresh Token:
* хранится в БД
* ротируется при обновлении
* становится недействительным после начала новой сессии или обновлении пары токенов

## Стек
* ASP.NET Core 10.0
* ASP.NET Core Identity
* Microsoft.AspNetCore.Authentication.JwtBearer
* EF Core (Code First)
* MS SQL

## JWT Конфигурация
```json
 "Jwt": {
   "SecretKey": "superSecretKey__@123456789123456",
   "Issuer": "https://localhost.5001",
   "Audience": "https://localhost:5001",
   "TokenValidityInMinutes": 1,
   "RefreshTokenValidityInMinutes": 3
 }
```
Для демонстрации:
* Access Token живёт - 1 минуту
* Refresh Token живёт - 3 минуты

В течение срока жизни Refresh Token можно получать новые пары токенов (при каждом обновлении пары токенов происходит ротация).

## Миграции
Миграции применяются автоматически при запуске приложения
```c#
  await db.Database.MigrateAsync();
```

Дополнительно выполняется начальная инициализация для таблиц ролей и пользователей: 
* создаются роли `User`, `Admin`
* создается пользователь с ролью `Admin`
  * email: `admin@example.com`
  * password: `Admin123!`

## Поток работы токенов
* Новая сессия:
```
POST /api/account/login →
  AccessToken (1 минута)
  RefreshToken (3 минуты)
```
* срок AccessToken истек:
```
 POST /api/account/refresh-token →
    Новый AccessToken
    Новый RefreshToken (старый становится недействительным)
```
* Logout:
```
POST /api/account/logout
```
Refresh Token пользователя сбрасывается.

## AccountController
Пример тестовых данных:
### POST /api/account/register
```json
{
  "firstName": "UserFirstName",
  "lastName": "UserLastName",
  "userName": "user.name",
  "email": "user@example.com",
  "password": "Password123!"
}
```
### POST /api/account/login
```json
{
  "email": "user@example.com",
  "password": "Password123!"
}
```
Ответ:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh-token"
}
```
### POST /api/account/logout
Требует авторизацию пользователя.
Сбрасывает Refresh Token текущему пользователю.
### GET /api/account/auth-user
Требует авторизацию пользователя.
Возвращает Claims текущего пользователя (из JWT).

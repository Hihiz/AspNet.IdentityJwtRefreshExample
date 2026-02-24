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
